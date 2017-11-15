#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using db;
using MySql.Data.MySqlClient;
using wServer.logic;
using wServer.networking;
using wServer.networking.svrPackets;
using wServer.realm.entities.merchant;
using wServer.realm.entities.player;
using wServer.realm.setpieces;
using wServer.realm.worlds;

#endregion

namespace wServer.realm.commands
{
    internal class posCmd : Command
    {
        public posCmd()
            : base("p", 1)
        {
        }

        protected override bool Process(Player player, RealmTime time, string[] args)
        {
            player.SendInfo("X: " + (int)player.X + " - Y: " + (int)player.Y);
            return true;
        }
    }

    internal class BanCommand : Command
    {
        public BanCommand() : 
            base("ban", 1)
        {
        }

        protected override bool Process(Player player, RealmTime time, string[] args)
        {
            var p = player.Manager.FindPlayer(args[1]);
            if (p == null)
            {
                player.SendError("Player not found");
                return false;
            }
            player.Manager.Database.DoActionAsync(db =>
            {
                var cmd = db.CreateQuery();
                cmd.CommandText = "UPDATE accounts SET banned=1 WHERE id=@accId;";
                cmd.Parameters.AddWithValue("@accId", p.AccountId);
                cmd.ExecuteNonQuery();
            });
            return true;
        }
    }


    internal class AddWorldCommand : Command
    {
        public AddWorldCommand()
            : base("addworld", 1)
        {
        }

        protected override bool Process(Player player, RealmTime time, string[] args)
        {
            Task.Factory.StartNew(() => GameWorld.AutoName(1, true)).ContinueWith(_ => player.Manager.AddWorld(_.Result), TaskScheduler.Default);
            return true;
        }
    }


    internal class SpawnCommand : Command
    {
        public SpawnCommand()
            : base("spawn", 1)
        {
        }


        protected override bool Process(Player player, RealmTime time, string[] args)
        {
            if (player.Owner is Vault || player.Owner is PetYard || player.Owner is Nexus || player.Owner is Market || player.Owner is ClothBazaar)
            {
                player.SendInfo("You cant Spawn in this world.");
                return false;
            }
            int num;
            if (args.Length > 0 && int.TryParse(args[0], out num)) //multi
            {
                string name = string.Join(" ", args.Skip(1).ToArray());
                ushort objType;
                //creates a new case insensitive dictionary based on the XmlDatas
                Dictionary<string, ushort> icdatas = new Dictionary<string, ushort>(
                    player.Manager.GameData.IdToObjectType,
                    StringComparer.OrdinalIgnoreCase);
                if (!icdatas.TryGetValue(name, out objType) ||
                    !player.Manager.GameData.ObjectDescs.ContainsKey(objType))
                {
                    player.SendInfo("Unknown entity!");
                    return false;
                }
                int c = int.Parse(args[0]);
                if  (c > 25)
                {
                    player.SendError("Maximum spawn count is set to 25!");
                    return false;
                }
                for (int i = 0; i < num; i++)
                {
                    Entity entity = Entity.Resolve(player.Manager, objType);
                    entity.Move(player.X, player.Y);
                    player.Owner.EnterWorld(entity);
                }
                player.SendInfo("Success!");
            }
            else
            {
                string name = string.Join(" ", args);
                ushort objType;
                //creates a new case insensitive dictionary based on the XmlDatas
                Dictionary<string, ushort> icdatas = new Dictionary<string, ushort>(
                    player.Manager.GameData.IdToObjectType,
                    StringComparer.OrdinalIgnoreCase);
                if (!icdatas.TryGetValue(name, out objType) ||
                    !player.Manager.GameData.ObjectDescs.ContainsKey(objType))
                {
                    player.SendHelp("Usage: /spawn <entityname>");
                    return false;
                }
                Entity entity = Entity.Resolve(player.Manager, objType);
                entity.Move(player.X, player.Y);
                player.Owner.EnterWorld(entity);
            }
            return true;
        }
    }

    internal class AddEffCommand : Command
    {
        public AddEffCommand()
            : base("addeff", 1)
        {
        }

        protected override bool Process(Player player, RealmTime time, string[] args)
        {
            if (args.Length == 0)
            {
                player.SendHelp("Usage: /addeff <Effectname or Effectnumber>");
                return false;
            }
            try
            {
                player.ApplyConditionEffect(new ConditionEffect
                {
                    Effect = (ConditionEffectIndex)Enum.Parse(typeof(ConditionEffectIndex), args[0].Trim(), true),
                    DurationMS = -1
                });
                {
                    player.SendInfo("Success!");
                }
            }
            catch
            {
                player.SendError("Invalid effect!");
                return false;
            }
            return true;
        }
    }

    internal class RemoveEffCommand : Command
    {
        public RemoveEffCommand()
            : base("remeff", 1)
        {
        }

        protected override bool Process(Player player, RealmTime time, string[] args)
        {
            if (args.Length == 0)
            {
                player.SendHelp("Usage: /remeff <Effectname or Effectnumber>");
                return false;
            }
            try
            {
                player.ApplyConditionEffect(new ConditionEffect
                {
                    Effect = (ConditionEffectIndex)Enum.Parse(typeof(ConditionEffectIndex), args[0].Trim(), true),
                    DurationMS = 0
                });
                player.SendInfo("Success!");
            }
            catch
            {
                player.SendError("Invalid effect!");
                return false;
            }
            return true;
        }
    }

    internal class GiveCommand : Command
    {
        public GiveCommand()
            : base("give", 0) //Todo Fix this
        {
        }

        protected override bool Process(Player player, RealmTime time, string[] args)
        {
            if (args.Length == 0)
            {
                player.SendHelp("Usage: /give <Itemname>");
                return false;
            }

            string name = string.Join(" ", args.ToArray()).Trim();
            ushort objType;


            if (player.Client.Account.Rank < 2)
                if (!(name == "Death Arena Key" | name == "Marble Seal" | name == "Shatters Key"))
                {
                    player.SendError("No Permission! You may only give 'Death Arena Key' and 'Marble Seal'");
                    return false;
                }

            //creates a new case insensitive dictionary based on the XmlDatas
                Dictionary<string, ushort> icdatas = new Dictionary<string, ushort>(player.Manager.GameData.IdToObjectType,
                StringComparer.OrdinalIgnoreCase);

            if (!icdatas.TryGetValue(name, out objType))
            {
                player.SendError("Unknown type!");
                return false;
            }
            if (!player.Manager.GameData.Items[objType].Secret || player.Client.Account.Rank >= 4)
            {
                for (int i = 0; i < player.Inventory.Length; i++)
                    if (player.Inventory[i] == null)
                    {
                        player.Inventory[i] = player.Manager.GameData.Items[objType];
                        player.UpdateCount++;
                        player.SaveToCharacter();
                        player.SendInfo("Success!");
                        break;
                    }
            }
            else
            {
                player.SendError("Item cannot be given!");
                return false;
            }
            return true;
        }
    }


    internal class TpCommand : Command
    {
        public TpCommand()
            : base("tp", 1)
        {
        }

        protected override bool Process(Player player, RealmTime time, string[] args)
        {
            if (args.Length == 0 || args.Length == 1)
            {
                player.SendHelp("Usage: /tp <X coordinate> <Y coordinate>");
            }
            else
            {
                int x, y;
                try
                {
                    x = int.Parse(args[0]);
                    y = int.Parse(args[1]);
                }
                catch
                {
                    player.SendError("Invalid coordinates!");
                    return false;
                }
                player.Move(x + 0.5f, y + 0.5f);
                if (player.Pet != null)
                    player.Pet.Move(x + 0.5f, y + 0.5f);
                player.UpdateCount++;
                player.Owner.BroadcastPacket(new GotoPacket
                {
                    ObjectId = player.Id,
                    Position = new Position
                    {
                        X = player.X,
                        Y = player.Y
                    }
                }, null);
            }
            return true;
        }
    }

    class KillAll : Command
    {
        public KillAll() : base("killAll", 2) { }
        
        protected override bool Process(Player player, RealmTime time, string[] args)
        {
            var iterations = 0;
            var lastKilled = -1;
            var killed = 0;

            var mobName = args.Aggregate((s, a) => string.Concat(s, " ", a));
            while (killed != lastKilled)
            {
                lastKilled = killed;
                foreach (var i in player.Owner.Enemies.Values.Where(e =>
                    e.ObjectDesc?.ObjectId != null && e.ObjectDesc.ObjectId.ContainsIgnoreCase(mobName)))
                {
                    i.Death(time);
                    killed++;
                }
                if (++iterations >= 5)
                    break;
            }

            player.SendInfo($"{killed} enemy killed!");
            return true;
        }
    }

    class WhosAlive : Command
    {
        public WhosAlive() : base("WhosAlive", 1) { }

        protected override bool Process(Player player, RealmTime time, string[] args)
        {
            var mobName = args.Aggregate((s, a) => string.Concat(s, " ", a));
            {
                foreach (var i in player.Owner.Enemies.Values.Where(e =>
                    e.ObjectDesc?.ObjectId != null && e.ObjectDesc.ObjectId.ContainsIgnoreCase(mobName)))
                player.SendInfo($"{i.ObjectDesc.ObjectId} is Alive");
            }
            return true;
        }
    }

    internal class Kick : Command
    {
        public Kick()
            : base("kick", 1)
        {
        }

        protected override bool Process(Player player, RealmTime time, string[] args)
        {
            if (args.Length == 0)
            {
                player.SendHelp("Usage: /kick <playername>");
                return false;
            }
            try
            {
                foreach (KeyValuePair<int, Player> i in player.Owner.Players)
                {
                    if (i.Value.Name.ToLower() == args[0].ToLower().Trim())
                    {
                        player.SendInfo("Player Disconnected");
                        i.Value.Client.Disconnect();
                    }
                }
            }
            catch
            {
                player.SendError("Cannot kick!");
                return false;
            }
            return true;
        }
    }

    internal class Mute : Command
    {
        public Mute()
            : base("mute", 1)
        {
        }

        protected override bool Process(Player player, RealmTime time, string[] args)
        {
            if (args.Length == 0)
            {
                player.SendHelp("Usage: /mute <playername>");
                return false;
            }
            try
            {
                foreach (KeyValuePair<int, Player> i in player.Owner.Players)
                {
                    if (i.Value.Name.ToLower() == args[0].ToLower().Trim())
                    {
                        i.Value.Muted = true;
                        i.Value.Manager.Database.DoActionAsync(db => db.MuteAccount(i.Value.AccountId));
                        player.SendInfo("Player Muted.");
                    }
                }
            }
            catch
            {
                player.SendError("Cannot mute!");
                return false;
            }
            return true;
        }
    }

    internal class Max : Command
    {
        public Max()
            : base("max", 1)
        {
        }

        protected override bool Process(Player player, RealmTime time, string[] args)
        {
            try
            {
                player.Stats[0] = player.ObjectDesc.MaxHitPoints;
                player.Stats[1] = player.ObjectDesc.MaxMagicPoints;
                player.Stats[2] = player.ObjectDesc.MaxAttack;
                player.Stats[3] = player.ObjectDesc.MaxDefense;
                player.Stats[4] = player.ObjectDesc.MaxSpeed;
                player.Stats[5] = player.ObjectDesc.MaxHpRegen;
                player.Stats[6] = player.ObjectDesc.MaxMpRegen;
                player.Stats[7] = player.ObjectDesc.MaxDexterity;
                player.SaveToCharacter();
                player.Client.Save();
                player.UpdateCount++;
                player.SendInfo("Success");
            }
            catch
            {
                player.SendError("Error while maxing stats");
                return false;
            }
            return true;
        }
    }

    internal class UnMute : Command
    {
        public UnMute()
            : base("unmute", 1)
        {
        }

        protected override bool Process(Player player, RealmTime time, string[] args)
        {
            if (args.Length == 0)
            {
                player.SendHelp("Usage: /unmute <playername>");
                return false;
            }
            try
            {
                foreach (KeyValuePair<int, Player> i in player.Owner.Players)
                {
                    if (i.Value.Name.ToLower() == args[0].ToLower().Trim())
                    {
                        i.Value.Muted = true;
                        i.Value.Manager.Database.DoActionAsync(db => db.UnmuteAccount(i.Value.AccountId));
                        player.SendInfo("Player Unmuted.");
                    }
                }
            }
            catch
            {
                player.SendError("Cannot unmute!");
                return false;
            }
            return true;
        }
    }

    internal class OryxSay : Command
    {
        public OryxSay()
            : base("osay", 2)
        {
        }

        protected override bool Process(Player player, RealmTime time, string[] args)
        {
            if (args.Length == 0)
            {
                player.SendHelp("Usage: /oryxsay <saytext>");
                return false;
            }
            string saytext = string.Join(" ", args);
            player.SendEnemy("Oryx the Mad God", saytext);
            return true;
        }
    }

    internal class SWhoCommand : Command //get all players from all worlds (this may become too large!)
    {
        public SWhoCommand()
            : base("swho", 1)
        {
        }

        protected override bool Process(Player player, RealmTime time, string[] args)
        {
            StringBuilder sb = new StringBuilder("All conplayers: ");

            foreach (KeyValuePair<int, World> w in player.Manager.Worlds)
            {
                World world = w.Value;
                if (w.Key != 0)
                {
                    Player[] copy = world.Players.Values.ToArray();
                    if (copy.Length != 0)
                    {
                        for (int i = 0; i < copy.Length; i++)
                        {
                            sb.Append(copy[i].Name);
                            sb.Append(", ");
                        }
                    }
                }
            }
            string fixedString = sb.ToString().TrimEnd(',', ' '); //clean up trailing ", "s

            player.SendInfo(fixedString);
            return true;
        }
    }

    internal class Announcement : Command
    {
        public Announcement()
            : base("announce", 1)
        {
        }

        protected override bool Process(Player player, RealmTime time, string[] args)
        {
            if (args.Length == 0)
            {
                player.SendHelp("Usage: /announce <saytext>");
                return false;
            }
            string saytext = string.Join(" ", args);

            foreach (Client i in player.Manager.Clients.Values)
            {
                i.SendPacket(new TextPacket
                {
                    BubbleTime = 0,
                    Stars = -1,
                    Name = "@ANNOUNCEMENT",
                    Text = " " + saytext
                });
            }
            return true;
        }
    }

    internal class Summon : Command
    {
        public Summon()
            : base("summon", 1)
        {
        }

        protected override bool Process(Player player, RealmTime time, string[] args)
        {
            if (player.Owner is Vault || player.Owner is PetYard)
            {
                player.SendInfo("You cant summon in this world.");
                return false;
            }
            foreach (KeyValuePair<string, Client> i in player.Manager.Clients)
            {
                if (i.Value.Player.Name.EqualsIgnoreCase(args[0]))
                {
                    Packet pkt;
                    if (i.Value.Player.Owner == player.Owner)
                    {
                        i.Value.Player.Move(player.X, player.Y);
                        pkt = new GotoPacket
                        {
                            ObjectId = i.Value.Player.Id,
                            Position = new Position(player.X, player.Y)
                        };
                        i.Value.Player.UpdateCount++;
                        player.SendInfo("Player summoned!");
                    }
                    else
                    {
                        pkt = new ReconnectPacket
                        {
                            GameId = player.Owner.Id,
                            Host = "",
                            IsFromArena = false,
                            Key = player.Owner.PortalKey,
                            KeyTime = -1,
                            Name = player.Owner.Name,
                            Port = -1
                        };
                        player.SendInfo("Player will connect to you now!");
                    }

                    i.Value.SendPacket(pkt);

                    return true;
                }
            }
            player.SendError(string.Format("Player '{0}' could not be found!", args));
            return false;
        }
    }

    internal class KillPlayerCommand : Command
    {
        public KillPlayerCommand()
            : base("kill", 3)
        {
        }

        protected override bool Process(Player player, RealmTime time, string[] args)
        {
            foreach (Client i in player.Manager.Clients.Values)
            {
                if (i.Account.Name.EqualsIgnoreCase(args[0]))
                {
                    i.Player.HP = 0;
                    i.Player.Death("Admin");
                    player.SendInfo("Player killed!");
                    return true;
                }
            }
            player.SendError(string.Format("Player '{0}' could not be found!", args));
            return false;
        }
    }

    internal class PetSizeCommand : Command
    {
        public PetSizeCommand()
            : base("PetSize", 2)
        {
        }

        protected override bool Process(Player player, RealmTime time, string[] args)
        {
            try
            {
                if (args.Length < 2)
                {
                    player.SendHelp("Use /petsize <Player> <Pet Size>");
                    return false;
                }
                if (Convert.ToInt32(args[1]) <= 0 || Convert.ToInt32(args[1]) > 1000)
                {
                    player.SendHelp("Make sure your Pet Size is an integer above -1 and below 1001");
                    return false;
                }
                foreach (Client i in player.Manager.Clients.Values)
                {
                    if (i.Account.Name.EqualsIgnoreCase(args[0]))
                    {
                        i.Player.Pet.Size = int.Parse(args[1]);
                        i.Player.UpdateCount++;
                        i.Player.SendInfo(player.Name + " changed your pets size to " + args[1]);
                        player.SendInfo("Success!");
                        using (Database db = new Database())
                            db.UpdatePetSize(Convert.ToInt32(i.Player.AccountId), i.Player.Pet.PetId, Convert.ToInt32(args[1]));
                        return true;
                    }
                }
                {
                    player.SendError("Cannot Find Account");
                }
            }
            catch(Exception ex)
            {
                player.SendError("Error!");
                log.Error(ex);
                return false;
            }
            return true;
        }
    }

    internal class RestartCommand : Command
    {
        public RestartCommand()
            : base("restart", 3)
        {
        }

        protected override bool Process(Player player, RealmTime time, string[] args)
        {
            try
            {
                foreach (Client i in player.Manager.Clients.Values)
                {
                    {
                        i.SendPacket(new TextPacket
                        {
                            BubbleTime = 0,
                            Stars = -1,
                            Name = "@ANNOUNCEMENT",
                            Text = "Server restarting soon. Please be ready to disconnect. Estimated server down time: 30 Seconds - 1 Minute"
                        });
                    }
                }
            }
            catch
            {
                player.SendError("Cannot say that in announcement!");
                return false;
            }
            return true;
        }
    }

    internal class TqCommand : Command
    {
        public TqCommand()
            : base("tq", 1)
        {
        }

        protected override bool Process(Player player, RealmTime time, string[] args)
        {
            if (player.Quest == null)
            {
                player.SendError("Player does not have a quest!");
                return false;
            }
            player.Move(player.Quest.X + 0.5f, player.Quest.Y + 0.5f);
            if (player.Pet != null)
                player.Pet.Move(player.Quest.X + 0.5f, player.Quest.Y + 0.5f);
            player.UpdateCount++;
            player.Owner.BroadcastPacket(new GotoPacket
            {
                ObjectId = player.Id,
                Position = new Position
                {
                    X = player.Quest.X,
                    Y = player.Quest.Y
                }
            }, null);
            player.SendInfo("Success!");
            return true;
        }
    }

    internal class LevelCommand : Command
    {
        public LevelCommand()
            : base("level", 2)
        {
        }

        protected override bool Process(Player player, RealmTime time, string[] args)
        {
            try
            {
                if (args.Length == 0)
                {
                    player.SendHelp("Use /level <ammount>");
                    return false;
                }
                if (args.Length == 1)
                {
                    player.Client.Character.Level = int.Parse(args[0]);
                    player.Client.Player.Level = int.Parse(args[0]);
                    player.UpdateCount++;
                    player.SendInfo("Success!");
                }
            }
            catch
            {
                player.SendError("Error!");
                return false;
            }
            return true;
        }
    }

    internal class SetCommand : Command
    {
        public SetCommand()
            : base("setStat", 1)
        {
        }

        protected override bool Process(Player player, RealmTime time, string[] args)
        {
            if (args.Length == 2)
            {
                try
                {
                    string stat = args[0].ToLower();
                    int amount = int.Parse(args[1]);
                    if (amount > 500)
                    {
                        player.SendError("That's a bit excessive... Keep it under 500");
                        return false;
                    }
                    switch (stat)
                    {
                        case "health":
                        case "hp":
                            player.Stats[0] = amount;
                            break;
                        case "mana":
                        case "mp":
                            player.Stats[1] = amount;
                            break;
                        case "atk":
                        case "attack":
                            player.Stats[2] = amount;
                            break;
                        case "def":
                        case "defence":
                            player.Stats[3] = amount;
                            break;
                        case "spd":
                        case "speed":
                            player.Stats[4] = amount;
                            break;
                        case "vit":
                        case "vitality":
                            player.Stats[5] = amount;
                            break;
                        case "wis":
                        case "wisdom":
                            player.Stats[6] = amount;
                            break;
                        case "dex":
                        case "dexterity":
                            player.Stats[7] = amount;
                            break;
                        default:
                            player.SendError("Invalid Stat");
                            player.SendHelp("Stats: Health, Mana, Attack, Defence, Speed, Vitality, Wisdom, Dexterity");
                            player.SendHelp("Shortcuts: Hp, Mp, Atk, Def, Spd, Vit, Wis, Dex");
                            return false;
                    }
                    player.SaveToCharacter();
                    player.Client.Save();
                    player.UpdateCount++;
                    player.SendInfo("Success");
                }
                catch
                {
                    player.SendError("Error while setting stat");
                    return false;
                }
                return true;
            }
            if (args.Length == 3)
            {
                if (player.Client.Account.Rank < 5)
                {
                    player.SendError("Only higher ranked admins can set other players stats");
                    return false;
                }
                foreach (Client i in player.Manager.Clients.Values)
                {
                    if (i.Account.Name.EqualsIgnoreCase(args[0]))
                    {
                        try
                        {
                            string stat = args[1].ToLower();
                            int amount = int.Parse(args[2]);
                            switch (stat)
                            {
                                case "health":
                                case "hp":
                                    i.Player.Stats[0] = amount;
                                    break;
                                case "mana":
                                case "mp":
                                    i.Player.Stats[1] = amount;
                                    break;
                                case "atk":
                                case "attack":
                                    i.Player.Stats[2] = amount;
                                    break;
                                case "def":
                                case "defence":
                                    i.Player.Stats[3] = amount;
                                    break;
                                case "spd":
                                case "speed":
                                    i.Player.Stats[4] = amount;
                                    break;
                                case "vit":
                                case "vitality":
                                    i.Player.Stats[5] = amount;
                                    break;
                                case "wis":
                                case "wisdom":
                                    i.Player.Stats[6] = amount;
                                    break;
                                case "dex":
                                case "dexterity":
                                    i.Player.Stats[7] = amount;
                                    break;
                                default:
                                    player.SendError("Invalid Stat");
                                    player.SendHelp("Stats: Health, Mana, Attack, Defence, Speed, Vitality, Wisdom, Dexterity");
                                    player.SendHelp("Shortcuts: Hp, Mp, Atk, Def, Spd, Vit, Wis, Dex");
                                    return false;
                            }
                            i.Player.SaveToCharacter();
                            i.Player.Client.Save();
                            i.Player.UpdateCount++;
                            player.SendInfo("Success");
                        }
                        catch
                        {
                            player.SendError("Error while setting stat");
                            return false;
                        }
                        return true;
                    }
                }
                player.SendError("Player could not be found!");
                return false;
            }
            player.SendHelp("Usage: /setStat <Stat> <Amount>");
            player.SendHelp("or");
            player.SendHelp("Usage: /setStat <Player> <Stat> <Amount>");
            player.SendHelp("Shortcuts: Hp, Mp, Atk, Def, Spd, Vit, Wis, Dex");
            return false;
        }
    }

    internal class SetpieceCommand : Command
    {
        public SetpieceCommand()
            : base("setpiece", 2)
        {
        }

        protected override bool Process(Player player, RealmTime time, string[] args)
        {
            ISetPiece piece = (ISetPiece)Activator.CreateInstance(Type.GetType(
                "wServer.realm.setpieces." + args[0], true, true));
            piece.RenderSetPiece(player.Owner, new IntPoint((int)player.X + 1, (int)player.Y + 1));
            return true;
        }
    }

    internal class ListCommands : Command
    {
        public ListCommands() : base("commands", permLevel: 1) { }

        protected override bool Process(Player player, RealmTime time, string[] args)
        {
            Dictionary<string, Command> cmds = new Dictionary<string, Command>();
            Type t = typeof(Command);
            foreach (Type i in t.Assembly.GetTypes())
                if (t.IsAssignableFrom(i) && i != t)
                {
                    Command instance = (Command)Activator.CreateInstance(i);
                    cmds.Add(instance.CommandName, instance);
                }
            StringBuilder sb = new StringBuilder("");
            Command[] copy = cmds.Values.ToArray();
            for (int i = 0; i < copy.Length; i++)
            {
                if (i != 0) sb.Append(", ");
                sb.Append(copy[i].CommandName);
            }

            player.SendInfo(sb.ToString());
            return true;
        }
    }
    internal class GodCommand : Command
    {
        public GodCommand()
        : base("god", 2)
        {
        }

        protected override bool Process(Player player, RealmTime time, string[] args)
        {
            if (player.HasConditionEffect(ConditionEffectIndex.Invincible))
            {
                player.ApplyConditionEffect(new ConditionEffect
                {
                    Effect = ConditionEffectIndex.Invincible,
                    DurationMS = 0
                });
                player.SendInfo("Godmode Off");
            }
            else
            {
                player.ApplyConditionEffect(new ConditionEffect
                {
                    Effect = ConditionEffectIndex.Invincible,
                    DurationMS = -1
                });
                player.SendInfo("Godmode On");
            }
            return true;
        }
    }
    internal class StarsCommand : Command
    {
        public StarsCommand()
        : base("stars", 2)
        {
        }

        protected override bool Process(Player player, RealmTime time, string[] args)
        {
            try
            {
                if (args.Length == 0)
                {
                    player.SendHelp("Use /stars <ammount>");
                }
                else if (args.Length == 1)
                {
                    player.Client.Player.Stars = int.Parse(args[0]);
                    player.UpdateCount++;
                    player.SendInfo("Success Setting Stars!");
                }
            }
            catch
            {
                player.SendError("Error!");
            }
            return true;
        }
    }
    internal class CFameCommand : Command
    {
        public CFameCommand()
        : base("cfame", 5)
        {
        }

        protected override bool Process(Player player, RealmTime time, string[] args)
        {
            if (args[0] == "")
            {
                player.SendHelp("Usage: /cfame <Fame Amount>");
                return false;
            }
            try
            {
                int newFame = Convert.ToInt32(args[0]);
                int newXP = Convert.ToInt32(newFame + "000");
                player.Fame = newFame;
                player.Experience = newXP;
                player.SaveToCharacter();
                player.Client.Save();
                player.UpdateCount++;
                player.SendInfo("Updated Character Fame To: " + newFame);
            }
            catch
            {
                player.SendInfo("Error Setting Fame");
                return false;
            }
            return true;
        }
    }
    internal class AccIdCommand : Command
    {
        public AccIdCommand()
            : base("accid", 1) { }

        protected override bool Process(Player player, RealmTime time, string[] args)
        {
            if (string.IsNullOrEmpty(args[0]))
            {
                player.SendHelp("Usage: /accid <player>");
                return false;
            }
            var plr = player.Manager.FindPlayer(args[0]);
            player.SendInfo("Account ID of " + plr.Name + " : " + plr.AccountId);
            return true;
        }
    }
    internal class CloseRealmCmd : Command
    {
        public CloseRealmCmd()
            : base("closerealm", 1)
        {
        }
        protected override bool Process(Player player, RealmTime time, string[] args)
        {
            if (player.Owner is GameWorld)
            {
                var gw = player.Owner as GameWorld;
                gw.Overseer.InitCloseRealm();
                return true;
            }
            return false;
        }
    }

    internal class ForceUp : Command
    {
        public ForceUp()
            : base("AdminForceUp", 3)
        {
        }
        protected override bool Process(Player player, RealmTime time, string[] args)
        {
            player.TryUpgrade("force");
            return true;
        }
    }

    internal class Sell : Command
    {
        public Sell()
            : base("Sell")
        {
        }
        protected override bool Process(Player player, RealmTime time, string[] args)
        {
            {
                player.SendError("Selling is currently Disabled");
                return false;
            }
            using (var db = new Database())
            {
                if (args.Length < 2)
                {
                    player.SendError("Usage: /sell <slot> <price>");
                    return false;
                }
                if (Convert.ToInt32(args[0]) > 8 || Convert.ToInt32(args[0]) < 1)
                {
                    player.SendError("Slot Number Invalid, please only choose items in slot 1-8");
                    return false;
                }
                if (Convert.ToInt32(args[1]) < 0)
                {
                    player.SendError("Do you know how an economy works?");
                    return false;
                }
                int slot = Convert.ToInt32(args[0]) + 3;
                int itemID = player.Inventory[slot].ObjectType;
                string checker = player.Manager.GameData.ObjectTypeToId[player.Inventory[slot].ObjectType];

                if (!checker.Contains("Egg"))
                    if (!checker.Contains("Skin"))
                        if (!checker.Contains("Cloth"))
                            if (!checker.Contains("Dye"))
                                if (!checker.Contains("Tincture"))
                                    if (!checker.Contains("Effusion"))
                                        if (!checker.Contains("Elixir"))
                                            if (!checker.Contains("Tarot"))
                                                if (!checker.Contains("Gunball"))
                                                        {

                                                    
                                                    MySqlCommand cmd = db.CreateQuery();
                                                    cmd.CommandText = "INSERT INTO market(itemID, fame, id) VALUES(@itemID, @fame, @playerID)";
                                                    cmd.Parameters.AddWithValue("@itemID", Convert.ToInt32(itemID));
                                                    cmd.Parameters.AddWithValue("@fame", args[1]);
                                                    var plr = player.Manager.FindPlayer(player.Name);
                                                    cmd.Parameters.AddWithValue("@playerID", plr.AccountId);
                                                    try
                                                    {
                                                        cmd.ExecuteNonQuery();
                                                        player.Inventory[slot] = null;
                                                        player.SaveToCharacter();
                                                        player.Client.Save();
                                                        player.UpdateCount++;
                                                        if (logic.CheckConfig.IsDebugOn())
                                                            log.Error("Requesting Update for Item | " + itemID);
                                                        Merchants.RefreshMerchants = itemID;
                                                        Merchants.RefreshMerchantsCooldown = 100;
                                                    }
                                                    catch (Exception e)
                                                    {
                                                        Console.WriteLine("[" + DateTime.Now.ToString("h:mm:ss tt") + "] " + e);
                                                    }
                                                    return true;
                                                }
            }
        player.SendError("You cannot sell this item!");
        return false;

        }
        }

    internal class ReviveCommand : Command
    {
        public ReviveCommand() : base("revive", 1) { }
        protected override bool Process(Player player, RealmTime time, string[] args)
        {
            if (string.IsNullOrEmpty(args[0]))
            {
                player.SendHelp("Usage: /revive <accId> <fame>");
                return false;
            }
            player.Manager.Database.DoActionAsync(db =>
            {
                var cmd = db.CreateQuery();
                cmd.CommandText = "UPDATE characters SET dead=0 WHERE accId=@accId AND fame=@base";
                cmd.Parameters.AddWithValue("@base", args[1]);
                cmd.Parameters.AddWithValue("@accId", args[0]);
                if (cmd.ExecuteNonQuery() == 0)
                {
                    player.SendInfo("Could not revive. Make sure you wrote it right.");
                }
                else
                    player.SendInfo("Character Successfully Revived");
            });
            return true;
        }
    }
    internal class VisitCommand : Command
    {
        public VisitCommand()
            : base("visit", 2)
        {
        }

        protected override bool Process(Player player, RealmTime time, string[] args)
        {
            foreach (KeyValuePair<string, Client> i in player.Manager.Clients)
                if (i.Value.Player.Owner is PetYard)
                {
                    player.SendInfo("You cant visit players in that world.");
                    return false;
                }
            foreach (KeyValuePair<string, Client> i in player.Manager.Clients)
            {
                if (i.Value.Player.Name.EqualsIgnoreCase(args[0]))
                {
                    Packet pkt;
                    if (i.Value.Player.Owner == player.Owner)
                    {
                        player.Move(i.Value.Player.X, i.Value.Player.Y);
                        pkt = new GotoPacket
                        {
                            ObjectId = player.Id,
                            Position = new Position(i.Value.Player.X, i.Value.Player.Y)
                        };
                        i.Value.Player.UpdateCount++;
                        player.SendInfo("Player already in world.");
                    }
                    else
                    {
                        player.Client.Reconnect(new ReconnectPacket
                        {
                            GameId = i.Value.Player.Owner.Id,
                            Host = "",
                            IsFromArena = false,
                            Key = Empty<byte>.Array,
                            KeyTime = -1,
                            Name = i.Value.Player.Owner.Name,
                            Port = -1
                        });
                        player.SendInfo("You are visiting " + i.Value.Player.Owner.Id);
                    }


                    return true;

                }
            }
            player.SendError(string.Format("Player '{0}' could not be found!", args));
            return false;
        }
    }
}
