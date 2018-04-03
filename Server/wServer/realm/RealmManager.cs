#region

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using db;
using db.data;
using wServer.logic;
using wServer.networking;
using wServer.realm.commands;
using wServer.realm.entities.merchant;
using wServer.realm.entities.player;
using wServer.realm.worlds;
using CheckConfig = wServer.logic.CheckConfig;

#endregion

namespace wServer.realm
{
    public enum PendingPriority
    {
        Emergent,
        Destruction,
        Networking,
        Normal,
        Creation
    }

    public struct RealmTime
    {
        public int thisTickCounts { get; set; }

        public int thisTickTimes { get; set; }

        public long tickCount { get; set; }

        public long tickTimes { get; set; }
    }

    public class RealmManager
    {
        public const int MAX_REALM_PLAYERS = 20;

        public static List<string> Realms = new List<string>(44)
        {
            "Zylixel's Realm"
        };

        public static List<string> CurrentPortalNames = new List<string>();

        private Thread logic;
        private Thread network;
        private int nextClientId;

        private int nextWorldId;

        private readonly ConcurrentDictionary<string, Vault> vaults;

        public RealmManager(int maxClients, int tps)
        {
            MaxClients = maxClients;
            TPS = tps;
            Clients = new ConcurrentDictionary<string, Client>();
            Worlds = new ConcurrentDictionary<int, World>();
            GuildHalls = new ConcurrentDictionary<string, GuildHall>();
            LastWorld = new ConcurrentDictionary<string, World>();
            vaults = new ConcurrentDictionary<string, Vault>();
            Random = new Random();
        }

        public ConcurrentDictionary<string, Client> Clients { get; }
        public ConcurrentDictionary<int, World> Worlds { get; }
        public ConcurrentDictionary<string, GuildHall> GuildHalls { get; }
        public ConcurrentDictionary<string, World> LastWorld { get; }

        public Random Random { get; }

        public BehaviorDb Behaviors { get; private set; }

        public ChatManager Chat { get; private set; }

        public CommandManager Commands { get; private set; }

        public XmlData GameData { get; private set; }

        public string InstanceId { get; private set; }

        public LogicTicker Logic { get; private set; }

        public static int CurrentWorldId { get; private set; }
        public static int CurrentCourtId { get; private set; }

        public int MaxClients { get; }

        public RealmPortalMonitor Monitor { get; private set; }

        public NetworkTicker Network { get; private set; }
        public DatabaseTicker Database { get; private set; }

        public bool Terminating { get; private set; }

        public int TPS { get; }

        public World AddWorld(int id, World world)
        {
            if (world.Manager != null)
                throw new InvalidOperationException("World already added.");
            world.Id = id;
            Worlds[id] = world;
            OnWorldAdded(world);
            return world;
        }

        public XmlData GetData()
        {
            return GameData;
        }

        public World AddWorld(World world)
        {
            if (world.Manager != null)
                throw new InvalidOperationException("World already added.");
            world.Id = Interlocked.Increment(ref nextWorldId);
            Worlds[world.Id] = world;
            OnWorldAdded(world);
            return world;
        }

        public void CloseWorld(World world)
        {
            Monitor.WorldRemoved(world);
        }

        public async void Disconnect(Client client)
        {
            if (client == null) return;
            Client dummy;
            client.Disconnect("RealmManager Disconnect");
            await client.Save();
            while (!Clients.TryRemove(client.Account.AccountId, out dummy) &&
                   Clients.ContainsKey(client.Account.AccountId)) ;
            client.Dispose();
        }

        public Player FindPlayer(string name)
        {
            if (name.Split(' ').Length > 1)
                name = name.Split(' ')[1];
            return (from i in Worlds
                where i.Key != 0
                from e in i.Value.Players
                where string.Equals(e.Value.Client.Account.Name, name, StringComparison.CurrentCultureIgnoreCase)
                select e.Value).FirstOrDefault();
        }

        public Player FindPlayerRough(string name)
        {
            Player dummy;
            foreach (var i in Worlds)
                if (i.Key != 0)
                    if ((dummy = i.Value.GetUniqueNamedPlayerRough(name)) != null)
                        return dummy;
            return null;
        }

        public World GetWorld(int id)
        {
            World ret;
            if (!Worlds.TryGetValue(id, out ret)) return null;
            if (ret.Id == 0) return null;
            return ret;
        }

        public void Initialize()
        {
            if (CheckConfig.IsDebugOn())
                Console.WriteLine("Initializing Realm Manager...");
            GameData = new XmlData();
            Behaviors = new BehaviorDb(this);
            GeneratorCache.Init();
            MerchantLists.InitMerchatLists(GameData);
            AddWorld(World.NEXUS_ID, Worlds[0] = new Nexus());
            AddWorld(World.MARKET, new ClothBazaar());
            AddWorld(World.TUT_ID, new Tutorial(true));
            AddWorld(World.FMARKET, new Market());
            Monitor = new RealmPortalMonitor(this);
            Task.Factory.StartNew(() => GameWorld.AutoName(1, true))
                .ContinueWith(_ => AddWorld(_.Result), TaskScheduler.Default);
            Chat = new ChatManager(this);
            Commands = new CommandManager(this);
            if (CheckConfig.IsDebugOn())
                Console.WriteLine("Realm Manager initialized.");
        }

        public Vault PlayerVault(Client processor)
        {
            Vault v;
            if (!vaults.TryGetValue(processor.Account.AccountId, out v))
                vaults.TryAdd(processor.Account.AccountId, v = (Vault) AddWorld(new Vault(false, processor)));
            else
                v.Reload(processor);
            return v;
        }

        public bool RemoveVault(string accountId)
        {
            Vault dummy;
            return vaults.TryRemove(accountId, out dummy);
        }

        public bool RemoveWorld(World world)
        {
            if (world.Manager == null)
                throw new InvalidOperationException("World is not added.");
            World dummy;
            if (Worlds.TryRemove(world.Id, out dummy))
            {
                try
                {
                    OnWorldRemoved(world);
                    world.Dispose();
                    GC.Collect();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
                return true;
            }
            return false;
        }

        public void Run()
        {
            if (CheckConfig.IsDebugOn())
                Console.WriteLine("Starting Realm Manager...");
            Network = new NetworkTicker(this);
            Logic = new LogicTicker(this);
            Database = new DatabaseTicker();
            network = new Thread(Network.TickLoop)
            {
                Name = "Network",
                CurrentCulture = CultureInfo.InvariantCulture
            };
            logic = new Thread(Logic.TickLoop)
            {
                Name = "Logic",
                CurrentCulture = CultureInfo.InvariantCulture
            };
            //Start logic loop first
            logic.Start();
            network.Start();
            if (CheckConfig.IsDebugOn())
                Console.WriteLine("Realm Manager started.");
        }

        public void Stop()
        {
            if (CheckConfig.IsDebugOn())
                Console.WriteLine("Stopping Realm Manager...");
            Terminating = true;
            var saveAccountUnlock = new List<Client>();
            foreach (var c in Clients.Values)
            {
                saveAccountUnlock.Add(c);
                c.Disconnect("Server Closing");
            }
            //To prevent a buggy Account in use.
            using (var db = new Database())
            {
                foreach (var c in saveAccountUnlock)
                    db.UnlockAccount(c.Account);
            }
            GameData.Dispose();
            logic.Join();
            network.Join();
            if (CheckConfig.IsDebugOn())
                Console.WriteLine("Realm Manager stopped.");
        }

        public bool TryConnect(Client psr)
        {
            var acc = psr.Account;
            if (Clients.Count >= MaxClients)
                return false;
            if (acc.Banned)
                return false;
            psr.Id = Interlocked.Increment(ref nextClientId);
            Client dummy;
            if (Clients.ContainsKey(acc.AccountId))
                if (!Clients[acc.AccountId].Socket.Connected)
                    Clients.TryRemove(acc.AccountId, out dummy);
            var ret = Clients.TryAdd(psr.Account.AccountId, psr);
            if (!ret)
                Program.writeError($"Returning {ret} whilst adding a client in TryConnect");
            return ret;
        }
        
        private void OnWorldAdded(World world)
        {
            if (world.Manager == null)
                world.Manager = this;
            if (world is GameWorld)
            {
                Monitor.WorldAdded(world);
                CurrentWorldId = world.Id;
            }
            if (world is CourtOfBereavement)
                CurrentCourtId = world.Id;
            if (CheckConfig.IsDebugOn())
                Console.WriteLine("World {0}({1}) added.", world.Id, world.Name);
        }

        private void OnWorldRemoved(World world)
        {
            world.Manager = null;
            if (world is GameWorld)
                Monitor.WorldRemoved(world);
            if (CheckConfig.IsDebugOn())
                Console.WriteLine("World {0}({1}) removed.", world.Id, world.Name);
        }

        public void ProtectFromOryx()
        {
            foreach (Client i in Clients.Values)
            { 
                i.Player.ApplyConditionEffect(new ConditionEffect
                {
                    Effect = ConditionEffectIndex.Invulnerable,
                    DurationMS = 10 * 1000
                });
                if (!(i.Player.Owner is GameWorld))
                    i.Player.SendInfo("Oryx is summoning players to his castle. You have been givin Invulnerablility for 10 seconds.");
            }
        }

        public Item CreateSerial(OldItem item, string DroppedIn, bool soulbound = false, bool insert = true, int _id = -1)
        {
            int id;
            if (insert && _id == -1)
                using (var db = new Database())
                {
                    id = db.GetNextSerialId();
                }
            else
                id = _id;
            Item ret = new Item
            {
                serialId = id,
                ObjectType = item.ObjectType,
                firstUser = -1,
                currentUser = -1,
                droppedIn = DroppedIn,
                Soulbound = soulbound,
                banned = 0,
                ObjectId = item.ObjectId,
                SlotType = item.SlotType,
                Tier = item.Tier,
                Description = item.Description,
                RateOfFire = item.RateOfFire,
                Usable = item.Usable,
                BagType = item.BagType,
                MpCost = item.MpCost,
                FameBonus = item.FameBonus,
                NumProjectiles = item.NumProjectiles,
                ArcGap = item.ArcGap,
                Consumable = item.Consumable,
                Potion = item.Potion,
                DisplayId = item.DisplayId,
                SuccessorId = item.SuccessorId,
                Cooldown = item.Cooldown,
                Resurrects = item.Resurrects,
                Texture1 = item.Texture1,
                Texture2 = item.Texture2,
                Secret = item.Secret,
                IsBackpack = item.IsBackpack,
                Rarity = item.Rarity,
                Family = item.Family,
                Class = item.Class,
                Doses = item.Doses,
                StatsBoost = item.StatsBoost,
                ActivateEffects = item.ActivateEffects,
                Projectiles = item.Projectiles,
                MpEndCost = item.MpEndCost,
                Timer = item.Timer,
                XpBooster = item.XpBooster,
                LootDropBooster = item.LootDropBooster,
                LootTierBooster = item.LootTierBooster,
                SetType = item.SetType,
                BrokenResurrect = item.BrokenResurrect,
                NotBrokenResurrect = item.NotBrokenResurrect,
                MantleResurrect = item.MantleResurrect,
                MpGiveBack = item.MpGiveBack,
                Treasure = item.Treasure,
                Maxy = item.Maxy,
                FeedPower = item.FeedPower
            };
            if (insert)
                using (var db = new Database())
                {
                    db.InsertSerial(ret);
                }
            return ret;
        }

        public Item[] CreateSerial(OldItem[] items, string DroppedIn, bool soulbound = false, bool insert = true)
        {
            List<Item> ret = new List<Item>();
            int id;
            foreach (var item in items)
            {
                if (insert)
                    using (var db = new Database())
                    {
                        id = db.GetNextSerialId();
                    }
                else
                    id = -1;
                ret.Add(new Item
                {
                    serialId = id,
                    ObjectType = item.ObjectType,
                    firstUser = 0,
                    currentUser = 0,
                    Soulbound = soulbound,
                    droppedIn = DroppedIn,
                    banned = 0,
                    ObjectId = item.ObjectId,
                    SlotType = item.SlotType,
                    Tier = item.Tier,
                    Description = item.Description,
                    RateOfFire = item.RateOfFire,
                    Usable = item.Usable,
                    BagType = item.BagType,
                    MpCost = item.MpCost,
                    FameBonus = item.FameBonus,
                    NumProjectiles = item.NumProjectiles,
                    ArcGap = item.ArcGap,
                    Consumable = item.Consumable,
                    Potion = item.Potion,
                    DisplayId = item.DisplayId,
                    SuccessorId = item.SuccessorId,
                    Cooldown = item.Cooldown,
                    Resurrects = item.Resurrects,
                    Texture1 = item.Texture1,
                    Texture2 = item.Texture2,
                    Secret = item.Secret,
                    IsBackpack = item.IsBackpack,
                    Rarity = item.Rarity,
                    Family = item.Family,
                    Class = item.Class,
                    Doses = item.Doses,
                    StatsBoost = item.StatsBoost,
                    ActivateEffects = item.ActivateEffects,
                    Projectiles = item.Projectiles,
                    MpEndCost = item.MpEndCost,
                    Timer = item.Timer,
                    XpBooster = item.XpBooster,
                    LootDropBooster = item.LootDropBooster,
                    LootTierBooster = item.LootTierBooster,
                    SetType = item.SetType,
                    BrokenResurrect = item.BrokenResurrect,
                    NotBrokenResurrect = item.NotBrokenResurrect,
                    MantleResurrect = item.MantleResurrect,
                    MpGiveBack = item.MpGiveBack,
                    Treasure = item.Treasure,
                    Maxy = item.Maxy,
                    FeedPower = item.FeedPower
                });
                if (insert)
                    using (var db = new Database())
                    {
                        db.InsertSerial(ret.LastOrDefault());
                    }
            }
            return ret.ToArray();
        }
    }

    

    public class TimeEventArgs : EventArgs
    {
        public TimeEventArgs(RealmTime time)
        {
            Time = time;
        }

        public RealmTime Time { get; }
    }
}