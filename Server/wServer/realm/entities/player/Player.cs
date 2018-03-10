#region

using db;
using System;
using System.Collections.Generic;
using System.Linq;
using wServer.logic;
using wServer.networking;
using wServer.networking.cliPackets;
using wServer.networking.svrPackets;
using FailurePacket = wServer.networking.svrPackets.FailurePacket;

#endregion

namespace wServer.realm.entities.player
{
    internal interface IPlayer
    {
        void Damage(int dmg, Entity chr, bool forgiveHealthViolation);
        bool IsVisibleToEnemy();
    }

    public static class ComparableExtension
    {
        public static bool InRange<T>(this T value, T from, T to) where T : IComparable<T>
        {
            return value.CompareTo(from) >= 1 && value.CompareTo(to) <= -1;
        }
    }

    public partial class Player : Character, IContainer, IPlayer
    {
        public int _buyCooldown = 0;
        private bool _dying;

        private Item[] _inventory;

        private float _hpRegenCounter;
        private float _mpRegenCounter;
        private bool _resurrecting;

        public int healthViolation = 0;

        public int checkForDex = 0;
        public int lastShootTime = Environment.TickCount;
        public int shootCounter = 0;

        private byte[,] _tiles;
        private int _pingSerial;
        private SetTypeSkin _setTypeSkin;

        public Player(RealmManager manager, Client psr)
            : base(manager, (ushort)psr.Character.ObjectType, psr.Random)
        {
            try
            {
                Client = psr;
                Manager = psr.Manager;
                StatsManager = new StatsManager(this, psr.Random.CurrentSeed);
                Name = psr.Account.Name;
                AccountId = psr.Account.AccountId;
                FameCounter = new FameCounter(this);
                Tokens = psr.Account.FortuneTokens;
                HpPotionPrice = 5;
                MpPotionPrice = 5;

                Level = psr.Character.Level == 0 ? 1 : psr.Character.Level;
                Experience = psr.Character.Exp;
                ExperienceGoal = GetExpGoal(Level);
                Stars = GetStars();
                Texture1 = psr.Character.Tex1;
                Texture2 = psr.Character.Tex2;
                Credits = psr.Account.Credits;
                NameChosen = psr.Account.NameChosen;
                CurrentFame = psr.Account.Stats.Fame;
                Fame = psr.Character.CurrentFame;
                XpBoosted = psr.Character.XpBoosted;
                XpBoostTimeLeft = psr.Character.XpTimer;
                _xpFreeTimer = XpBoostTimeLeft != -1.0;
                LootDropBoostTimeLeft = psr.Character.LDTimer;
                _lootDropBoostFreeTimer = LootDropBoost;
                LootTierBoostTimeLeft = psr.Character.LTTimer;
                _lootTierBoostFreeTimer = LootTierBoost;
                var state =
                    psr.Account.Stats.ClassStates.SingleOrDefault(_ => Utils.FromString(_.ObjectType) == ObjectType);
                FameGoal = GetFameGoal(state?.BestFame ?? 0);
                Glowing = IsUserInLegends();
                Guild = GuildManager.Add(this, psr.Account.Guild);
                HP = psr.Character.HitPoints <= 0 ? psr.Character.MaxHitPoints : psr.Character.HitPoints;
                Mp = psr.Character.MagicPoints;
                ConditionEffects = 0;
                OxygenBar = 100;
                HasBackpack = psr.Character.HasBackpack == 1;
                PlayerSkin = Client.Account.OwnedSkins.Contains(Client.Character.Skin) ? Client.Character.Skin : 0;
                HealthPotions = psr.Character.HealthStackCount < 0 ? 0 : psr.Character.HealthStackCount;
                MagicPotions = psr.Character.MagicStackCount < 0 ? 0 : psr.Character.MagicStackCount;

                Locked = psr.Account.Locked ?? new List<string>();
                Ignored = psr.Account.Ignored ?? new List<string>();
                try
                {
                    Manager.Database.DoActionAsync(db =>
                    {
                        Locked = db.GetLockeds(AccountId);
                        Ignored = db.GetIgnoreds(AccountId);
                        Muted = db.IsMuted(AccountId);
                        DailyQuest = psr.Account.DailyQuest;
                    });
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }

                if (HasBackpack)
                {
                    var inv =
                        psr.Character.Equipment.Select(
                            _ =>
                                _ == -1
                                    ? null
                                    : (Manager.GameData.Items.ContainsKey((ushort)_) ? Manager.GameData.Items[(ushort)_] : null))
                            .ToArray();
                    var backpack =
                        psr.Character.Backpack.Select(
                            _ =>
                                _ == -1
                                    ? null
                                    : (Manager.GameData.Items.ContainsKey((ushort)_) ? Manager.GameData.Items[(ushort)_] : null))
                            .ToArray();

                    Inventory = inv.Concat(backpack).ToArray();
                    var xElement = Manager.GameData.ObjectTypeToElement[ObjectType].Element("SlotTypes");
                    if (xElement != null)
                    {
                        var slotTypes =
                            Utils.FromCommaSepString32(
                                xElement.Value);
                        Array.Resize(ref slotTypes, 20);
                        SlotTypes = slotTypes;
                    }
                }
                else
                {
                    Inventory =
                        psr.Character.Equipment.Select(
                            _ =>
                                _ == -1
                                    ? null
                                    : (Manager.GameData.Items.ContainsKey((ushort)_) ? Manager.GameData.Items[(ushort)_] : null))
                            .ToArray();
                    var xElement = Manager.GameData.ObjectTypeToElement[ObjectType].Element("SlotTypes");
                    if (xElement != null)
                        SlotTypes =
                            Utils.FromCommaSepString32(
                                xElement.Value);
                }
                Stats = new[]
                {
                    psr.Character.MaxHitPoints,
                    psr.Character.MaxMagicPoints,
                    psr.Character.Attack,
                    psr.Character.Defense,
                    psr.Character.Speed,
                    psr.Character.HpRegen,
                    psr.Character.MpRegen,
                    psr.Character.Dexterity
                };

                Pet = null;

                for (var i = 0; i < SlotTypes.Length; i++)
                    if (SlotTypes[i] == 0) SlotTypes[i] = 10;

                if (Client.Account.Rank >= 3) return;
                for (var i = 0; i < 4; i++)
                    if (Inventory[i]?.SlotType != SlotTypes[i])
                        Inventory[i] = null;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        ~Player()
        {
            WorldInstance = null;
            Quest = null;
        }

        //Stats
        public string AccountId { get; }

        public int[] Boost { get; private set; }

        public Client Client { get; }

        public int Credits { get; set; }
        public int Tokens { get; set; }
        public int CurrentFame { get; set; }

        public int Experience { get; set; }
        public int ExperienceGoal { get; set; }

        public int Fame { get; set; }

        public FameCounter FameCounter { get; }

        public QuestItem DailyQuest { get; set; }

        public int FameGoal { get; set; }

        public bool Glowing { get; set; }

        public bool HasBackpack { get; set; }

        public int HealthPotions { get; set; }

        public List<string> Ignored { get; set; }

        public bool Invited { get; set; }
        public bool Muted { get; set; }

        public int Level { get; set; }

        public List<string> Locked { get; set; }

        public bool LootDropBoost
        {
            get { return LootDropBoostTimeLeft > 0; }
            set { LootDropBoostTimeLeft = value ? LootDropBoostTimeLeft : 0.0f; }
        }
        public float LootDropBoostTimeLeft { get; set; }

        public bool LootTierBoost
        {
            get { return LootTierBoostTimeLeft > 0; }
            set { LootTierBoostTimeLeft = value ? LootTierBoostTimeLeft : 0.0f; }
        }
        public float LootTierBoostTimeLeft { get; set; }

        public bool XpBoosted { get; set; }
        public float XpBoostTimeLeft { get; set; }

        public int MagicPotions { get; set; }

        public ushort HpPotionPrice { get; set; }
        public ushort MpPotionPrice { get; set; }

        public bool HpFirstPurchaseTime { get; set; }
        public bool MpFirstPurchaseTime { get; set; }

        public new RealmManager Manager { get; }

        public int MaxHp { get; set; }

        public int MaxMp { get; set; }

        public int Mp { get; set; }

        public bool NameChosen { get; set; }

        public int OxygenBar { get; set; }

        public Pet Pet { get; set; }

        public int PlayerSkin { get; set; }

        public int Stars { get; set; }

        public int[] Stats { get; }

        public StatsManager StatsManager { get; }

        public int Texture1 { get; set; }

        public int Texture2 { get; set; }

        public Item[] Inventory
        {
            get { return _inventory; }
            set { _inventory = value; }
        }

        public GuildManager Guild { get; set; }

        public int[] SlotTypes { get; set; }
        public int MaximumHp { get; private set; }
        public ushort Dmg { get; private set; }
        public int AshCooldown { get; private set; }
        
        public bool isInvincible()
        {
            if (HasConditionEffect(ConditionEffectIndex.Paused) || HasConditionEffect(ConditionEffectIndex.Stasis) || HasConditionEffect(ConditionEffectIndex.Invincible))
                return true;
            return false;
        }
        
        public void Damage(int dmg, Entity chr, bool forgiveHealthViolation = false)
        {
            if (forgiveHealthViolation)
                healthViolation = 0; // If the player recieves a damage function then the server has taken it's course, and we forvie the player
            if (CheckMantleResurrect())
               return;

            for (int i = 0; i < 4; i++)
            {
                Item item = Inventory[i];
                if (item == null || !item.MpGiveBack) continue;
                {
                    Mp += dmg / 3;
                    break;
                }
            }

            try
            {
                if (HasConditionEffect(ConditionEffectIndex.Paused) ||
                    HasConditionEffect(ConditionEffectIndex.Stasis) ||
                    HasConditionEffect(ConditionEffectIndex.Invincible))
                    return;

                var _oldHP = HP;

                dmg = (int)StatsManager.GetDefenseDamage(dmg, false);
                if (!HasConditionEffect(ConditionEffectIndex.Invulnerable))
                    HP -= dmg;

                if (_oldHP <= HP && dmg > 0)
                    Client.Disconnect();

                UpdateCount++;

                Owner.BroadcastPacket(new DamagePacket
                {
                    TargetId = Id,
                    Effects = 0,
                    Damage = (ushort)dmg,
                    Killed = HP <= 0,
                    BulletId = 0,
                    ObjectId = chr.Id
                }, this);
                SaveToCharacter();

                if (_oldHP <= HP && dmg > 0)
                    Client.Disconnect();

                if (HP <= 0)
                    Death(chr.ObjectDesc.DisplayId, chr.ObjectDesc);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error while processing playerDamage: ", e);
            }
        }

        protected override void ExportStats(IDictionary<StatsType, object> stats)
        {
            base.ExportStats(stats);
            stats[StatsType.AccountId] = AccountId;
            stats[StatsType.Name] = Name;

            stats[StatsType.Experience] = Experience - GetLevelExp(Level);
            stats[StatsType.ExperienceGoal] = ExperienceGoal;
            stats[StatsType.Level] = Level;

            stats[StatsType.CurrentFame] = CurrentFame;
            stats[StatsType.Fame] = Fame;
            stats[StatsType.FameGoal] = FameGoal;
            stats[StatsType.Stars] = Stars;

            stats[StatsType.Guild] = Guild[AccountId].Name;
            stats[StatsType.GuildRank] = Guild[AccountId].Rank;

            stats[StatsType.Credits] = Credits;
            stats[StatsType.Tokens] = Tokens;
            stats[StatsType.NameChosen] = NameChosen ? 1 : 0;
            stats[StatsType.Texture1] = Texture1;
            stats[StatsType.Texture2] = Texture2;

            if (Glowing)
                stats[StatsType.Glowing] = 1;

            stats[StatsType.Hp] = HP;
            stats[StatsType.Mp] = Mp;

            stats[StatsType.Inventory0] = Inventory[0]?.ObjectType ?? -1;
            stats[StatsType.Inventory1] = Inventory[1]?.ObjectType ?? -1;
            stats[StatsType.Inventory2] = Inventory[2]?.ObjectType ?? -1;
            stats[StatsType.Inventory3] = Inventory[3]?.ObjectType ?? -1;
            stats[StatsType.Inventory4] = Inventory[4]?.ObjectType ?? -1;
            stats[StatsType.Inventory5] = Inventory[5]?.ObjectType ?? -1;
            stats[StatsType.Inventory6] = Inventory[6]?.ObjectType ?? -1;
            stats[StatsType.Inventory7] = Inventory[7]?.ObjectType ?? -1;
            stats[StatsType.Inventory8] = Inventory[8]?.ObjectType ?? -1;
            stats[StatsType.Inventory9] = Inventory[9]?.ObjectType ?? -1;
            stats[StatsType.Inventory10] = Inventory[10]?.ObjectType ?? -1;
            stats[StatsType.Inventory11] = Inventory[11]?.ObjectType ?? -1;

            if (Boost == null) CalcBoost();

            if (Boost != null)
            {
                stats[StatsType.MaximumHp] = Stats[0] + Boost[0];
                stats[StatsType.MaximumMp] = Stats[1] + Boost[1];
                stats[StatsType.Attack] = Stats[2] + Boost[2];
                stats[StatsType.Defense] = Stats[3] + Boost[3];
                stats[StatsType.Speed] = Stats[4] + Boost[4];
                stats[StatsType.Vitality] = Stats[5] + Boost[5];
                stats[StatsType.Wisdom] = Stats[6] + Boost[6];
                stats[StatsType.Dexterity] = Stats[7] + Boost[7];

                stats[StatsType.HPBoost] = Boost[0];
                stats[StatsType.MPBoost] = Boost[1];
                stats[StatsType.AttackBonus] = Boost[2];
                stats[StatsType.DefenseBonus] = Boost[3];
                stats[StatsType.SpeedBonus] = Boost[4];
                stats[StatsType.VitalityBonus] = Boost[5];
                stats[StatsType.WisdomBonus] = Boost[6];
                stats[StatsType.DexterityBonus] = Boost[7];
            }

            stats[StatsType.Size] = _setTypeSkin?.Size ?? Size;
            stats[StatsType.HasBackpack] = HasBackpack.GetHashCode();

            stats[StatsType.Backpack0] = HasBackpack ? (Inventory[12]?.ObjectType ?? -1) : -1;
            stats[StatsType.Backpack1] = HasBackpack ? (Inventory[13]?.ObjectType ?? -1) : -1;
            stats[StatsType.Backpack2] = HasBackpack ? (Inventory[14]?.ObjectType ?? -1) : -1;
            stats[StatsType.Backpack3] = HasBackpack ? (Inventory[15]?.ObjectType ?? -1) : -1;
            stats[StatsType.Backpack4] = HasBackpack ? (Inventory[16]?.ObjectType ?? -1) : -1;
            stats[StatsType.Backpack5] = HasBackpack ? (Inventory[17]?.ObjectType ?? -1) : -1;
            stats[StatsType.Backpack6] = HasBackpack ? (Inventory[18]?.ObjectType ?? -1) : -1;
            stats[StatsType.Backpack7] = HasBackpack ? (Inventory[19]?.ObjectType ?? -1) : -1;

            stats[StatsType.Skin] = _setTypeSkin?.SkinType ?? PlayerSkin;
            stats[StatsType.HealStackCount] = HealthPotions;
            stats[StatsType.MagicStackCount] = MagicPotions;

            if (Owner != null && Owner.Name == "Ocean Trench")
                stats[StatsType.OxygenBar] = OxygenBar;

            stats[StatsType.XpBoosterActive] = XpBoosted ? 1 : 0;
            stats[StatsType.XpBoosterTime] = (int)XpBoostTimeLeft;
            stats[StatsType.LootDropBoostTimer] = (int)LootDropBoostTimeLeft;
            stats[StatsType.LootTierBoostTimer] = (int)LootTierBoostTimeLeft;
        }

        public void CalcBoost()
        {
            CheckSetTypeSkin();
            if (Boost == null) Boost = new int[12];
            else
                for (var i = 0; i < Boost.Length; i++) Boost[i] = 0;
            for (var i = 0; i < 4; i++)
            {
                if (Inventory.Length < i || Inventory.Length == 0) return;
                if (Inventory[i] == null) continue;
                foreach (var pair in Inventory[i].StatsBoost)
                {
                    if (pair.Key == StatsType.MaximumHp) Boost[0] += pair.Value;
                    if (pair.Key == StatsType.MaximumMp) Boost[1] += pair.Value;
                    if (pair.Key == StatsType.Attack) Boost[2] += pair.Value;
                    if (pair.Key == StatsType.Defense) Boost[3] += pair.Value;
                    if (pair.Key == StatsType.Speed) Boost[4] += pair.Value;
                    if (pair.Key == StatsType.Vitality) Boost[5] += pair.Value;
                    if (pair.Key == StatsType.Wisdom) Boost[6] += pair.Value;
                    if (pair.Key == StatsType.Dexterity) Boost[7] += pair.Value;
                }
            }

            if (_setTypeBoosts == null) return;
            for (var i = 0; i < 8; i++)
                Boost[i] += _setTypeBoosts[i];
        }

        public bool CompareName(string name)
        {
            var rn = name.ToLower();
            return rn.Split(' ')[0].StartsWith("[") || Name.Split(' ').Length == 1
                ? Name.ToLower().StartsWith(rn)
                : Name.Split(' ')[1].ToLower().StartsWith(rn);
        }

        private void announceDeath(string killer)
        {
            int maxed = (from i in Manager.GameData.ObjectTypeToElement[ObjectType].Elements("LevelIncrease") let xElement = Manager.GameData.ObjectTypeToElement[ObjectType].Element(i.Value) where xElement != null let limit = int.Parse(xElement.Attribute("max").Value) let idx = StatsManager.StatsNameToIndex(i.Value) where Stats[idx] >= limit select limit).Count();
            string _class = Manager.GameData.ObjectTypeToId[(ushort)Client.Character.ObjectType];
            bool firstBorn;
            string finalFame = Client.Character.FameStats.CalculateTotal(Manager.GameData, Client.Account, Client.Character, Client.Character.CurrentFame, out firstBorn).ToString();
            string deathMessage;
            
            if (maxed > 0)
            {
                deathMessage = (Name + ", the " + maxed.ToString() + "/8 " + _class + " has died to " + killer + " with " + finalFame + " fame");
            }
            else
            {
                deathMessage = (Name + ", the level " + Level + " " + _class + " has died to " + killer);
            }

            long pGuild = Client.Account.Guild.Id;

            if (maxed >= 6 || Fame >= 1000)
            {

                foreach (var w in Manager.Worlds.Values)
                    foreach (var p in w.Players.Values)
                        p.SendHelp(deathMessage);

            }
            else if (pGuild > 0)
            {
                foreach (var i in Owner.Players.Values)
                    if (i.Client.Account.Guild.Id != pGuild)
                        i.SendInfo(deathMessage);

                foreach (var w in Manager.Worlds.Values)
                    foreach (var p in w.Players.Values)
                        if (p.Client.Account.Guild.Id == pGuild)
                            p.SendHelp(deathMessage);
            }
            else
            {
                foreach (var i in Owner.Players.Values)
                    i.SendInfo(deathMessage);
            }
        }

        public void Death(string killer, ObjectDesc desc = null)
        {
            if (_dying) return;
            _dying = true;

            switch (Owner.Name)
            {
                case "Arena":
                    {
                        HP = Client.Character.MaxHitPoints;
                        Client.Reconnect(new ReconnectPacket
                        {
                            Host = "",
                            Port = Program.Settings.GetValue<int>("port"),
                            GameId = World.NEXUS_ID,
                            Name = "Nexus",
                            Key = Empty<byte>.Array
                        });
                        return;
                    }
            }

            if (CheckBrokenAmulet(MathsUtils.GenerateProb(100)) || CheckNotBrokenAmulet())
                return;

            if (Client.Stage == ProtocalStage.Disconnected || _resurrecting)
                return;
            if (CheckResurrection())
                return;

            if (Client.Character.Dead)
            {
                Client.Disconnect();
                return;
            }

            GenerateGravestone();

            if (desc != null)
                killer = desc.DisplayId;
            switch (killer)
            {
                case "":
                case "Unknown":
                    Client.Disconnect();
                    return;
                default:
                    if (desc != null)
                        announceDeath(desc.ObjectId);
                    else
                        announceDeath(killer);
                    break;
            }
            if (Client.Account.Rank > 1)
            {
                Client.Disconnect();
                return;
            }
            try
            {
                Manager.Database.DoActionAsync(db =>
                {
                    Client.Character.Dead = true;
                    SaveToCharacter();
                    db.SaveCharacter(Client.Account, Client.Character);
                    db.Death(Manager.GameData, Client.Account, Client.Character, killer);
                });
                if (Owner.Id != World.TEST_ID)
                {
                    Client.SendPacket(new DeathPacket
                    {
                        AccountId = AccountId,
                        CharId = Client.Character.CharacterId,
                        Killer = killer,
                        Obf0 = -1,
                        Obf1 = -1
                    });
                    Owner.Timers.Add(new WorldTimer(1000, (w, t) => Client.Disconnect()));
                    Owner.LeaveWorld(this);
                }
                else
                    Client.Disconnect();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private bool CheckBrokenAmulet(bool doesRevive)
        {
            for (int i = 0; i < 4; i++)
            {
                Item item = Inventory[i];

                if (item == null || !item.BrokenResurrect) continue;

                if (doesRevive)
                {
                    HP = Stats[0] + Stats[0];
                    MP = Stats[1] + Stats[1];
                    Inventory[i] = null;
                    foreach (Player player in Owner.Players.Values)
                        player.SendInfo($"{Name}'s {item.ObjectId} successfully saved him from death");

                    ApplyConditionEffect(new ConditionEffect
                    {
                        Effect = ConditionEffectIndex.Invulnerable,
                        DurationMS = 100000000
                    });

                    Client.Reconnect(new ReconnectPacket
                    {
                        Host = "",
                        Port = 2050,
                        GameId = World.NEXUS_ID,
                        Name = "Nexus",
                        Key = Empty<byte>.Array
                    });
                    _newbieTime += 1000;

                    _resurrecting = true;
                    return true;
                }
                foreach (Player player in Owner.Players.Values)
                    player.SendInfo($"{Name}'s {item.ObjectId} unfortunately did not save him from death");
            }
            return false;
        }

        private bool CheckNotBrokenAmulet()
        {
            for (int i = 0; i < 11; i++)
            {
                Item item = Inventory[i];

                if (item == null || !item.NotBrokenResurrect) continue;
                
                HP = Stats[0] + Stats[0];
                MP = Stats[1] + Stats[1];
                foreach (Player player in Owner.Players.Values)
                    player.SendInfo($"{Name}'s {item.ObjectId} successfully saved him from death... No surprise since hes a cheater");

                ApplyConditionEffect(new ConditionEffect
                {
                    Effect = ConditionEffectIndex.Invulnerable,
                    DurationMS = -1
                });

                Client.Reconnect(new ReconnectPacket
                {
                    Host = "",
                    Port = 2050,
                    GameId = World.NEXUS_ID,
                    Name = "Nexus",
                    Key = Empty<byte>.Array
                });
                _newbieTime += 1000;

                _resurrecting = true;
                return true;
            }
            return false;
        }

        private bool CheckMantleResurrect()
        {
            for (int i = 0; i < 4; i++)
            {
                Item item = Inventory[i];

                if (item == null || !item.MantleResurrect) continue;

                if (_pendantReady)
                {

                    if (HP < MaxHp / 5)
                    {
                        HP = MaxHp / 5;

                        ApplyConditionEffect(new ConditionEffect
                        {
                            Effect = ConditionEffectIndex.Stasis,
                            DurationMS = 1500

                        });
                        ApplyConditionEffect(new ConditionEffect
                        {
                            Effect = ConditionEffectIndex.Paralyzed,
                            DurationMS = 1500
                        });
                        ApplyConditionEffect(new ConditionEffect

                        {
                            Effect = ConditionEffectIndex.Stunned,
                            DurationMS = 1500

                        });
                        _pendantReady = false;
                        return true;
                    }
                }
            }
            return false;
        }

        public void GivePet(PetItem petInfo)
        {
            Pet = new Pet(Manager, petInfo, this);
            Pet.Move(X, Y);
            Owner.EnterWorld(Pet);
        }

        public override bool HitByProjectile(Projectile projectile, RealmTime time)
        {
            if (projectile.ProjectileOwner is Player ||
                HasConditionEffect(ConditionEffectIndex.Paused) ||
                HasConditionEffect(ConditionEffectIndex.Stasis) ||
                HasConditionEffect(ConditionEffectIndex.Invincible))
                return false;

            return base.HitByProjectile(projectile, time);
        }


        public override void Init(World owner)
        {
            WorldInstance = owner;
            var rand = new Random();
            int x, y;
            do
            {
                x = rand.Next(0, owner.Map.Width);
                y = rand.Next(0, owner.Map.Height);
            } while (owner.Map[x, y].Region != TileRegion.Spawn);
            Move(x + 0.5f, y + 0.5f);
            _tiles = new byte[owner.Map.Width, owner.Map.Height];
            SetNewbiePeriod();
            base.Init(owner);

            if (Client.Character.Pet != null)
                GivePet(Client.Character.Pet);

            if (owner.Id == World.NEXUS_ID || owner.Name == "Vault")
            {
                Client.SendPacket(new GlobalNotificationPacket
                {
                    Type = 0,
                    Text = Client.Account.Gifts.Count > 0 ? "giftChestOccupied" : "giftChestEmpty"
                });
            }

            SendAccountList(Locked, AccountListPacket.LockedListId);
            SendAccountList(Ignored, AccountListPacket.IgnoredListId);

            WorldTimer[] accTimer = {null};
            owner.Timers.Add(accTimer[0] = new WorldTimer(5000, (w, t) =>
            {
                Manager.Database.DoActionAsync(db =>
                {
                    if (Client?.Account == null) return;
                    Client.Account = db.GetAccount(AccountId, Manager.GameData);
                    Credits = Client.Account.Credits;
                    CurrentFame = Client.Account.Stats.Fame;
                    Tokens = Client.Account.FortuneTokens;
                    accTimer[0].Reset();
                    Manager.Logic.AddPendingAction(_ => w.Timers.Add(accTimer[0]), PendingPriority.Creation);
                });
            }));

            WorldTimer[] pingTimer = {null};
            owner.Timers.Add(pingTimer[0] = new WorldTimer(PingPeriod, (w, t) =>
            {
                Client.SendPacket(new PingPacket { Serial = _pingSerial++ });
                pingTimer[0].Reset();
                Manager.Logic.AddPendingAction(_ => w.Timers.Add(pingTimer[0]), PendingPriority.Creation);
            }));
            Manager.Database.DoActionAsync(db =>
            {
                db.UpdateLastSeen(Client.Account.AccountId, Client.Character.CharacterId, owner.Name);
                db.LockAccount(Client.Account);
            });

            if (Client.Account.IsGuestAccount)
            {
                owner.Timers.Add(new WorldTimer(1000, (w, t) => Client.Disconnect()));
                Client.SendPacket(new FailurePacket
                {
                    ErrorId = 8,
                    ErrorDescription = "Registration needed."
                });
                Client.SendPacket(new PasswordPromtPacket
                {
                    CleanPasswordStatus = PasswordPromtPacket.Register
                });
                return;
            }

            if (!Client.Account.VerifiedEmail && Program.Verify)
            {
                Client.SendPacket(new VerifyEmailDialogPacket());
                owner.Timers.Add(new WorldTimer(1000, (w, t) => Client.Disconnect()));
                return;
            }
            CheckSetTypeSkin();
        }

        public void SaveToCharacter()
        {
            var chr = Client.Character;
            chr.Exp = Experience;
            chr.Level = Level;
            chr.Tex1 = Texture1;
            chr.Tex2 = Texture2;
            chr.Pet = Pet?.Info;
            chr.CurrentFame = Fame;
            chr.HitPoints = HP;
            chr.MagicPoints = Mp;
            switch (Inventory.Length)
            {
                case 12:
                    chr.Equipment = Inventory.Select(_ => _?.ObjectType ?? -1).ToArray();
                    break;
                case 20:
                    var equip = Inventory.Select(_ => _?.ObjectType ?? -1).ToArray();
                    var backpack = new int[8];
                    Array.Copy(equip, 12, backpack, 0, 8);
                    Array.Resize(ref equip, 12);
                    chr.Equipment = equip;
                    chr.Backpack = backpack;
                    break;
            }
            chr.MaxHitPoints = Stats[0];
            chr.MaxMagicPoints = Stats[1];
            chr.Attack = Stats[2];
            chr.Defense = Stats[3];
            chr.Speed = Stats[4];
            chr.HpRegen = Stats[5];
            chr.MpRegen = Stats[6];
            chr.Dexterity = Stats[7];
            chr.HealthStackCount = HealthPotions;
            chr.MagicStackCount = MagicPotions;
            chr.HasBackpack = HasBackpack.GetHashCode();
            chr.Skin = PlayerSkin;
            chr.XpBoosted = XpBoosted;
            chr.XpTimer = (int)XpBoostTimeLeft;
            chr.LDTimer = (int)LootDropBoostTimeLeft;
            chr.LTTimer = (int)LootTierBoostTimeLeft;
        }

        public void Teleport(RealmTime time, TeleportPacket packet)
        {
            var obj = Client.Player.Owner.GetEntity(packet.ObjectId);
            try
            {
                if (obj == null) return;
                if (!TpCooledDown())
                {
                    SendError("Player.teleportCoolDown");
                    return;
                }
                if (obj.HasConditionEffect(ConditionEffectIndex.Invisible))
                {
                    SendError("server.no_teleport_to_invisible");
                    return;
                }
                if (obj.HasConditionEffect(ConditionEffectIndex.Paused))
                {
                    SendError("server.no_teleport_to_paused");
                    return;
                }
                var player = obj as Player;
                if (player != null && !player.NameChosen)
                {
                    SendError("server.teleport_needs_name");
                    return;
                }
                if (obj.Id == Id)
                {
                    SendError("server.teleport_to_self");
                    return;
                }
                if (player.Client.Account.Rank < 1)
                        if (!Owner.AllowTeleport)
                        {
                            SendError(GetLanguageString("server.no_teleport_in_realm", new KeyValuePair<string, object>("realm", Owner.Name)));
                            return;
                        }

                SetTpDisabledPeriod();
                Move(obj.X, obj.Y);
                Pet?.Move(obj.X, obj.X);
                FameCounter.Teleport();
                SetNewbiePeriod();
                ApplyConditionEffect(new ConditionEffect
                {
                    Effect = ConditionEffectIndex.Invulnerable,
                    DurationMS = 5 * 1000
                });
                UpdateCount++;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                SendError("player.cannotTeleportTo");
                return;
            }
            Owner.BroadcastPacket(new GotoPacket
            {
                ObjectId = Id,
                Position = new Position
                {
                    X = X,
                    Y = Y
                }
            }, null);
            Owner.BroadcastPacket(new ShowEffectPacket
            {
                EffectType = EffectType.Teleport,
                TargetId = Id,
                PosA = new Position
                {
                    X = X,
                    Y = Y
                },
                Color = new ARGB(0xFFFFFFFF)
            }, null);
        }

        public override void Tick(RealmTime time)
        {
            try
            {
                if (Manager.Clients.Count(_ => _.Value.Id == Client.Id) == 0)
                {
                    if (Owner != null)
                        Owner.LeaveWorld(this);
                    else
                        WorldInstance.LeaveWorld(this);
                    Manager.Database.DoActionAsync(db => db.UnlockAccount(Client.Account));
                    return;
                }
                if (Client.Stage == ProtocalStage.Disconnected || (!Client.Account.VerifiedEmail && Program.Verify))
                {
                    if (Owner != null)
                        Owner.LeaveWorld(this);
                    else
                        WorldInstance.LeaveWorld(this);
                    Manager.Database.DoActionAsync(db => db.UnlockAccount(Client.Account));
                    return;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            if (Stats != null && Boost != null)
            {
                MaxHp = Stats[0] + Boost[0];
                MaxMp = Stats[1] + Boost[1];
            }

            if (!KeepAlive(time)) return;

            if (HP == MaxHp) _pendantReady = true;          

            if (Boost == null) CalcBoost();

            TradeHandler?.Tick(time);
            HandleRegen(time);
            HandleQuest(time);
            HandleEffects(time);
            HandleGround(time);
            HandleBoosts();

            FameCounter.Tick(time);

            // Bug/Hack Fixes
            if (Mp < 0) Mp = 0;
            if (_buyCooldown > 0)
                _buyCooldown--;
            
            if (checkForDex >= 5)
            {
                SendError("Kicked for dexterity hacks or lag");
                Client.Save();
                Client.Disconnect();
                foreach (var i in Manager.Clients.Values)
                    if (i.Account.Rank > 0)
                        i.Player.SendError($"Player {Client.Player.Name} has just been kicked for dexterity hacks");
            }
            
            if (healthViolation >= 5)
            {
                SendError("Kicked for god hacks");
                Client.Save();
                Client.Disconnect();
                foreach (var i in Manager.Clients.Values)
                    if (i.Account.Rank > 0)
                        i.Player.SendError($"Player {Client.Player.Name} has just been kicked for god hacks");
            }

            try
            {
                if (Owner != null)
                {
                    SendUpdate(time);
                    if (!Owner.IsPassable((int)X, (int)Y) && Owner.Name != "The Other Side")
                    {
                        Console.WriteLine($"Player {Name} No-Clipped at position: {X}, {Y}");
                        Client.Player.SendError("Our server detected that you were Out-Of-Bounds.");
                        Client.Reconnect(new ReconnectPacket
                        {
                            Host = "",
                            Port = Program.Settings.GetValue<int>("port"),
                            GameId = World.NEXUS_ID,
                            Name = "Nexus",
                            Key = Empty<byte>.Array
                        });
                    }
                }
            }

            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            try
            {
                SendNewTick(time);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            if (HP < 0 && !_dying)
            {
                Client.Player.SendError("Woooooah there cowboy! you almost died to 'Unknown' Luckily thats a dumb way to die so I'm not gonna let that happen!");
                Client.Reconnect(new ReconnectPacket
                {
                    Host = "",
                    Port = Program.Settings.GetValue<int>("port"),
                    GameId = World.NEXUS_ID,
                    Name = "Nexus",
                    Key = Empty<byte>.Array
                });
                return;
            }
            
            base.Tick(time);
        }

        private bool CheckResurrection()
        {
            for (var i = 0; i < 4; i++)
            {
                var item = Inventory[i];
                if (item == null || !item.Resurrects) continue;

                HP = Stats[0] + Stats[0];
                Mp = Stats[1] + Stats[1];
                Inventory[i] = null;
                Owner.BroadcastPacket(new TextPacket
                {
                    BubbleTime = 0,
                    Stars = -1,
                    Name = "",
                    Text = $"{Name}'s {item.ObjectId} breaks and he disappears"
                }, null);
                Client.Reconnect(new ReconnectPacket
                {
                    Host = "",
                    Port = Program.Settings.GetValue<int>("port"),
                    GameId = World.NEXUS_ID,
                    Name = "Nexus",
                    Key = Empty<byte>.Array
                });

                _resurrecting = true;
                return true;
            }
            return false;
        }

        private void GenerateGravestone()
        {
            var maxed = (from i in Manager.GameData.ObjectTypeToElement[ObjectType].Elements("LevelIncrease") let xElement = Manager.GameData.ObjectTypeToElement[ObjectType].Element(i.Value) where xElement != null let limit = int.Parse(xElement.Attribute("max").Value) let idx = StatsManager.StatsNameToIndex(i.Value) where Stats[idx] >= limit select limit).Count();

            ushort objType;
            int? time;
            switch (maxed)
            {
                case 8:
                    objType = 0x0735;
                    time = null;
                    break;

                case 7:
                    objType = 0x0734;
                    time = null;
                    break;

                case 6:
                    objType = 0x072b;
                    time = null;
                    break;

                case 5:
                    objType = 0x072a;
                    time = null;
                    break;

                case 4:
                    objType = 0x0729;
                    time = null;
                    break;

                case 3:
                    objType = 0x0728;
                    time = null;
                    break;

                case 2:
                    objType = 0x0727;
                    time = null;
                    break;

                case 1:
                    objType = 0x0726;
                    time = null;
                    break;

                default:
                    if (Level <= 1)
                    {
                        objType = 0x0723;
                        time = 30 * 1000;
                    }
                    else if (Level < 20)
                    {
                        objType = 0x0724;
                        time = 60 * 1000;
                    }
                    else
                    {
                        objType = 0x0725;
                        time = 5 * 60 * 1000;
                    }
                    break;
            }
            var obj = new StaticObject(Manager, objType, time, true, time != null, false);
            obj.Move(X, Y);
            obj.Name = Name;
            Owner.EnterWorld(obj);
        }

        private void HandleRegen(RealmTime time)
        {
            if (HP == Stats[0] + Boost[0] || !CanHpRegen())
                _hpRegenCounter = 0;
            else
            {
                _hpRegenCounter += StatsManager.GetHPRegen() * time.thisTickTimes / 1000f;
                var regen = (int)_hpRegenCounter;
                if (regen > 0)
                {
                    HP = Math.Min(Stats[0] + Boost[0], HP + regen);
                    _hpRegenCounter -= regen;
                    UpdateCount++;
                }
            }

            if (Mp == Stats[1] + Boost[1] || !CanMpRegen())
                _mpRegenCounter = 0;
            else
            {
                _mpRegenCounter += StatsManager.GetMPRegen() * time.thisTickTimes / 1000f;
                var regen = (int)_mpRegenCounter;
                if (regen <= 0) return;
                Mp = Math.Min(Stats[1] + Boost[1], Mp + regen);
                _mpRegenCounter -= regen;
                UpdateCount++;
            }
        }

        public new void Dispose()
        {
            _tiles = null;
            Guild.Remove(this);
        }
    }
}
