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
            : base("p", 4)
        {
        }

        protected override Tuple<bool, string> Process(Player player, RealmTime time, string[] args)
        {
            player.SendInfo("X: " + (int)player.X + " - Y: " + (int)player.Y);
            return Tuple.Create(true, "");
        }
    }

    internal class BanCommand : Command
    {
        public BanCommand() : 
            base("ban", 3)
        {
        }

        protected override Tuple<bool, string> Process(Player player, RealmTime time, string[] args)
        {
            var p = player.Manager.FindPlayer(args[0]);
            if (p == null)
                return Tuple.Create(false, "Player not found");

            player.Manager.Database.DoActionAsync(db =>
            {
                var cmd = db.CreateQuery();
                cmd.CommandText = "UPDATE accounts SET banned=1 WHERE id=@accId;";
                cmd.Parameters.AddWithValue("@accId", p.AccountId);
                cmd.ExecuteNonQuery();
            });
            return Tuple.Create(true, "Success");
        }
    }

    internal class AddWorldCommand : Command
    {
        public AddWorldCommand()
            : base("addworld", 4)
        {
        }

        protected override Tuple<bool, string> Process(Player player, RealmTime time, string[] args)
        {
            Task.Factory.StartNew(() => GameWorld.AutoName(1, true)).ContinueWith(_ => player.Manager.AddWorld(_.Result), TaskScheduler.Default);
            return Tuple.Create(true, "Success");
        }
    }


    internal class SpawnCommand : Command
    {
        public SpawnCommand() : base("spawn", 1) {}
        
        protected override Tuple<bool, string> Process(Player player, RealmTime time, string[] args)
        {
            if (player.Owner is Vault || player.Owner is PetYard || player.Owner is Nexus || player.Owner is Market || player.Owner is ClothBazaar || player.Owner is GuildHall)
                return Tuple.Create(false, "You cannot spawn in this world.");

            int num;
            #region multi
            if (args.Length > 0 && int.TryParse(args[0], out num)) //multi
            {
                string name = string.Join(" ", args.Skip(1).ToArray());
                ushort objType;
                //creates a new case insensitive dictionary based on the XmlDatas
                Dictionary<string, ushort> icdatas = new Dictionary<string, ushort>(
                    player.Manager.GameData.IdToObjectType,
                    StringComparer.OrdinalIgnoreCase);
                if (!icdatas.TryGetValue(name, out objType) || !player.Manager.GameData.ObjectDescs.ContainsKey(objType))
                    return Tuple.Create(false, "Unknown Entity.");
                int c = int.Parse(args[0]);
                if (c > 25)
                    return Tuple.Create(false, "Maximum spawn count is set to 25.");

                for (int i = 0; i < num; i++)
                {
                    Entity entity = Entity.Resolve(player.Manager, objType);
                    name = entity.Name;
                    entity.Move(player.X, player.Y);
                    player.Owner.Timers.Add(new WorldTimer(5 * 1000, (world, RealmTime) => player.Owner?.EnterWorld(entity)));
                }
                player.Manager.Chat.Say(player, "Spawning " + c + " " + name + " in 5 seconds...");
            }
            #endregion
            #region single
            else
            {
                string name = string.Join(" ", args);
                ushort objType;
                //creates a new case insensitive dictionary based on the XmlDatas
                Dictionary<string, ushort> icdatas = new Dictionary<string, ushort>(
                    player.Manager.GameData.IdToObjectType,
                    StringComparer.OrdinalIgnoreCase);
                if (!icdatas.TryGetValue(name, out objType) || !player.Manager.GameData.ObjectDescs.ContainsKey(objType))
                    return Tuple.Create(false, "Unknown Entity.");

                Entity entity = Entity.Resolve(player.Manager, objType);
                name = entity.Name;
                entity.Move(player.X, player.Y);
                player.Owner.Timers.Add(new WorldTimer(5 * 1000, (world, RealmTime) => player.Owner?.EnterWorld(entity)));
                player.Manager.Chat.Say(player, "Spawning " + name + " in 5 seconds...");
                #endregion
            }
            return Tuple.Create(true, "Success");
        }
    }

    class SizeCommand : Command
    {
        public SizeCommand() : base("size", 2) { }

        protected override Tuple<bool, string> Process(Player player, RealmTime time, string[] args)
        {
            if (string.IsNullOrEmpty(args[0]))
                return Tuple.Create(false, "Usage: / size < positive integer >.Using 0 will restore the default size for the sprite.");

            var size = Utils.FromString(args[0]);
            if (size < 5 && size != 0 || size > 500)
                return Tuple.Create(false, $"Invalid size. Size needs to be within the range: {5}-{500}. Use 0 to reset size to default.");
            
            player.Size = size;
            
            return Tuple.Create(true, "Success");
        }
    }

    internal class AddEffCommand : Command
    {
        public AddEffCommand() : base("addeff", 2) {}

        protected override Tuple<bool, string> Process(Player player, RealmTime time, string[] args)
        {
            if (args.Length == 0)
                return Tuple.Create(false, "Usage: /addeff <Effectname or Effectnumber>");

            try
            {
                player.ApplyConditionEffect(new ConditionEffect
                {
                    Effect = (ConditionEffectIndex)Enum.Parse(typeof(ConditionEffectIndex), args[0].Trim(), true),
                    DurationMS = -1
                });
            }
            catch
            {
                return Tuple.Create(false, "Invalid effect!");
            }
            return Tuple.Create(true, "Success");
        }
    }

    internal class RemoveEffCommand : Command
    {
        public RemoveEffCommand() : base("remeff", 2) {}

        protected override Tuple<bool, string> Process(Player player, RealmTime time, string[] args)
        {
            if (args.Length == 0)
                return Tuple.Create(false, "Usage: /remeff <Effectname or Effectnumber>");

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
                return Tuple.Create(false, "Invalid effect!");
            }
            return Tuple.Create(true, "Success");
        }
    }

    internal class GiveCommand : Command
    {
        public GiveCommand() : base("give", 2) {}

        protected override Tuple<bool, string> Process(Player player, RealmTime time, string[] args)
        {
            {
                if (args.Length == 0)
                    return Tuple.Create(false, "Usage: /give <Itemname>");

                string name = string.Join(" ", args.ToArray()).Trim();
                ushort objType;
                
                //creates a new case insensitive dictionary based on the XmlDatas
                Dictionary<string, ushort> icdatas = new Dictionary<string, ushort>(player.Manager.GameData.IdToObjectType,
                StringComparer.OrdinalIgnoreCase);

                if (!icdatas.TryGetValue(name, out objType))
                    return Tuple.Create(false, "Unknown type!");

                if (!player.Manager.GameData.Items[objType].Secret || player.Client.Account.Rank >= 3)
                {
                    for (int i = 4; i < player.Inventory.Length; i++)
                        if (player.Inventory[i] == null)
                        {
                            player.Inventory[i] = player.Manager.CreateSerial(player.Manager.GameData.Items[objType], player.Owner.Name.Replace("'", ""), true);
                            player.UpdateCount++;
                            player.SaveToCharacter();
                            break;
                        }
                }
                else
                    return Tuple.Create(false, "Item cannot be given!");
            }
            return Tuple.Create(true, "Success");
        }
    }

    internal class GivePlayerCommand : Command
    {
        public GivePlayerCommand() : base("giveplayer", 4) { }

        protected override Tuple<bool, string> Process(Player player, RealmTime time, string[] args)
        {
            {
                if (args.Length <= 1)
                    return Tuple.Create(false, "Usage: /giveplayer <Player> <Itemname>");

                string playerName = args[0];
                ushort objType = (ushort)Convert.ToInt32(args[1]);

                //creates a new case insensitive dictionary based on the XmlDatas
                Dictionary<string, ushort> icdatas = new Dictionary<string, ushort>(player.Manager.GameData.IdToObjectType,
                StringComparer.OrdinalIgnoreCase);

                foreach (Client i in player.Manager.Clients.Values)
                {
                    if (i.Account.Name.EqualsIgnoreCase(playerName))
                    {
                        if (!i.Manager.GameData.Items[objType].Secret || i.Account.Rank >= 3)
                        {
                            for (int j = 4; j < player.Inventory.Length; j++)
                                if (i.Player.Inventory[j] == null)
                                {
                                    i.Player.Inventory[j] = player.Manager.CreateSerial(player.Manager.GameData.Items[objType], player.Owner.Name.Replace("'", ""), true);
                                    i.Player.UpdateCount++;
                                    i.Player.SaveToCharacter();
                                    return Tuple.Create(true, "Success");
                                }
                        }
                        return Tuple.Create(false, "Item cannot be given!");
                    }
                }
                return Tuple.Create(false, "Player could no be found!");
            }
        }
    }


    internal class TpCommand : Command
    {
        public TpCommand() : base("tp", 4) {}

        protected override Tuple<bool, string> Process(Player player, RealmTime time, string[] args)
        {
            if (args.Length == 0 || args.Length == 1)
                return Tuple.Create(false, "Usage: /tp <X coordinate> <Y coordinate>");

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
                    return Tuple.Create(false, "Invalid coordinates!");
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
            return Tuple.Create(true, "Success");
        }
    }

    class KillAll : Command
    {
        public KillAll() : base("killAll", 2) { }
        
        protected override Tuple<bool, string> Process(Player player, RealmTime time, string[] args)
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
            
            return Tuple.Create(true, $"{killed} enemy killed!");
        }
    }

    class WhosAlive : Command
    {
        public WhosAlive() : base("WhosAlive", 4) { }

        protected override Tuple<bool, string> Process(Player player, RealmTime time, string[] args)
        {
            var mobName = args.Aggregate((s, a) => string.Concat(s, " ", a));
            {
                foreach (var i in player.Owner.Enemies.Values.Where(e =>
                    e.ObjectDesc?.ObjectId != null && e.ObjectDesc.ObjectId.ContainsIgnoreCase(mobName)))
                player.SendInfo($"{i.ObjectDesc.ObjectId} is Alive");
            }
            return Tuple.Create(true, "Success");
        }
    }

    internal class Kick : Command
    {
        public Kick()
            : base("kick", 2)
        {
        }

        protected override Tuple<bool, string> Process(Player player, RealmTime time, string[] args)
        {
            if (args.Length == 0)
                return Tuple.Create(false, "Usage: /kick <playername>");

            try
            {
                foreach (KeyValuePair<int, Player> i in player.Owner.Players)
                {
                    if (i.Value.Name.ToLower() == args[0].ToLower().Trim())
                        i.Value.Client.Disconnect(Client.DisconnectReason.KICKED);
                }
            }
            catch
            {
                return Tuple.Create(false, "Cannot kick!");
            }
            return Tuple.Create(true, "Player Disconnected");
        }
    }

    internal class Mute : Command
    {
        public Mute()
            : base("mute", 2)
        {
        }

        protected override Tuple<bool, string> Process(Player player, RealmTime time, string[] args)
        {
            if (args.Length == 0)
                return Tuple.Create(false, "Usage: /mute <playername>");

            try
            {
                foreach (KeyValuePair<int, Player> i in player.Owner.Players)
                {
                    if (i.Value.Name.ToLower() == args[0].ToLower().Trim())
                    {
                        i.Value.Muted = true;
                        i.Value.Manager.Database.DoActionAsync(db => db.MuteAccount(i.Value.AccountId));
                    }
                }
            }
            catch
            {
                return Tuple.Create(false, "Cannot mute!");
            }
            return Tuple.Create(true, "Player Muted.");
        }
    }

    internal class Max : Command
    {
        public Max() : base("max", 2) {}

        protected override Tuple<bool, string> Process(Player player, RealmTime time, string[] args)
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
                player.Client.Save();
                player.UpdateCount++;
            }
            catch
            {
                return Tuple.Create(false, "Error while maxing stats");
            }
            return Tuple.Create(true, "Success");
        }
    }

    internal class UnMute : Command
    {
        public UnMute() : base("unmute", 2) {}

        protected override Tuple<bool, string> Process(Player player, RealmTime time, string[] args)
        {
            if (args.Length == 0)
                return Tuple.Create(false, "Usage: /unmute <playername>");

            try
            {
                foreach (KeyValuePair<int, Player> i in player.Owner.Players)
                {
                    if (i.Value.Name.ToLower() == args[0].ToLower().Trim())
                    {
                        i.Value.Muted = true;
                        i.Value.Manager.Database.DoActionAsync(db => db.UnmuteAccount(i.Value.AccountId));
                    }
                }
            }
            catch
            {
                return Tuple.Create(false, "Cannot unmute!");
            }
            return Tuple.Create(true, "Player Unmuted.");
        }
    }

    internal class SWhoCommand : Command //get all players from all worlds (this may become too large!)
    {
        public SWhoCommand() : base("online") {}

        protected override Tuple<bool, string> Process(Player player, RealmTime time, string[] args)
        {
            StringBuilder sb = new StringBuilder("All connected players: ");

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
            return Tuple.Create(true, fixedString);
        }
    }

    internal class Announcement : Command
    {
        public Announcement() : base("announce", 3) {}

        protected override Tuple<bool, string> Process(Player player, RealmTime time, string[] args)
        {
            if (args.Length == 0)
                return Tuple.Create(false, "Usage: /announce <saytext>");

            string saytext = string.Join(" ", args);
            player.Manager.Chat.Announce(saytext);
            return Tuple.Create(true, "Success");
        }
    }

    internal class Summon : Command
    {
        public Summon() : base("summon", 3) {}

        protected override Tuple<bool, string> Process(Player player, RealmTime time, string[] args)
        {
            if (player.Owner is Vault || player.Owner is PetYard)
                return Tuple.Create(false, "You cant summon in this world.");

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
                    }

                    i.Value.SendPacket(pkt);
                    return Tuple.Create(true, "Player summoned!");
                }
            }
            return Tuple.Create(false, $"Player '{args[0]}' could not be found!");
        }
    }

    internal class PetSizeCommand : Command
    {
        public PetSizeCommand() : base("PetSize", 4) {}

        protected override Tuple<bool, string> Process(Player player, RealmTime time, string[] args)
        {
            if (args.Length < 2)
                return Tuple.Create(false, "Use /petsize <Player> <Pet Size>");

            if (Convert.ToInt32(args[1]) < 0 || Convert.ToInt32(args[1]) > 1000)
                return Tuple.Create(false, "Make sure your Pet Size is a positve integer below 1000");

            foreach (Client i in player.Manager.Clients.Values)
            {
                if (i.Account.Name.EqualsIgnoreCase(args[0]))
                {
                    i.Player.Pet.Size = int.Parse(args[1]);
                    i.Player.UpdateCount++;
                    i.Player.SendInfo(player.Name + " changed your pets size to " + args[1]);
                    using (Database db = new Database())
                        db.UpdatePetSize(Convert.ToInt32(i.Player.AccountId), i.Player.Pet.PetId, Convert.ToInt32(args[1]));
                    return Tuple.Create(true, "Success");
                }
            }
            {
                return Tuple.Create(false, "Cannot Find Account");
            }
        }
    }

    internal class RestartCommand : Command
    {
        public RestartCommand() : base("restart", 4) {}

        protected override Tuple<bool, string> Process(Player player, RealmTime time, string[] args)
        {
            player.Owner.Timers.Add(new WorldTimer(10 * 1000, (world, RealmTime) => {
                Program.wServerShutdown = true;
            }));
            player.Manager.Chat.Announce("Server restarting soon. You will be disconnected in 10 seconds");
            return Tuple.Create(true, "Success");
        }
    }
    
    internal class TqCommand : Command
    {
        private Position calculateDistance(Player player)
        {
            float radius = 50.5f;
            int angle = new Random().Next(1, 360);
            float newX = player.Quest.X + radius * (float)Math.Cos(angle);
            float newY = player.Quest.Y + radius * (float)Math.Sin(angle);
            return new Position { X = newX, Y = newY };
        }

        public TqCommand() : base("tq") {}

        protected override Tuple<bool, string> Process(Player player, RealmTime time, string[] args)
        {
            if (player.Quest == null)
                return Tuple.Create(false, "Player does not have a quest!");

            if (player.Quest == null)
                return Tuple.Create(false, "Player does not have a quest!");
            Position Pos = new Position{ X = -1, Y = -1 };
            while (!player.Validate(Pos.X, Pos.Y))
            {
                Pos = calculateDistance(player);
            }
            player.Move(Pos.X, Pos.Y);
            if (player.Pet != null)
                player.Pet.Move(Pos.X, Pos.Y);
            player.UpdateCount++;
            player.Owner.BroadcastPacket(new GotoPacket
            {
                ObjectId = player.Id,
                Position = new Position
                {
                    X = Pos.X,
                    Y = Pos.Y
                }
            }, null);
            return Tuple.Create(true, "Success");
        }
    }

    internal class LevelCommand : Command
    {
        public LevelCommand() : base("level", 4) {}

        protected override Tuple<bool, string> Process(Player player, RealmTime time, string[] args)
        {
            try
            {
                if (args.Length == 0)
                    return Tuple.Create(false, "Use /level <ammount>");

                if (args.Length == 1)
                {
                    player.Client.Character.Level = int.Parse(args[0]);
                    player.Client.Player.Level = int.Parse(args[0]);
                    player.UpdateCount++;
                }
            }
            catch
            {
                return Tuple.Create(false, "Error!");
            }
            return Tuple.Create(true, "Success");
        }
    }

    internal class SetCommand : Command
    {
        public SetCommand()
            : base("setStat", 2)
        {
        }

        protected override Tuple<bool, string> Process(Player player, RealmTime time, string[] args)
        {
            if (args.Length == 2)
            {
                try
                {
                    string stat = args[0].ToLower();
                    int amount = int.Parse(args[1]);
                    if (amount > 500 && stat != "health" && stat != "mana" && stat != "hp" && stat != "mp")
                        return Tuple.Create(false, "That's a bit excessive... Keep it under 500");

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
                        case "defense":
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
                            return Tuple.Create(false, "Invalid Stat");
                    }
                    player.SaveToCharacter();
                    player.Client.Save();
                    player.UpdateCount++;
                }
                catch
                {
                    return Tuple.Create(false, "Error while setting stat");
                }
                return Tuple.Create(true, "Success");
            }
            if (args.Length == 3)
            {
                if (player.Client.Account.Rank < 3)
                    return Tuple.Create(false, "Only higher ranked admins can set other players stats");

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
                                case "defense":
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
                                    return Tuple.Create(false, "Invalid Stat");
                            }
                            i.Player.SaveToCharacter();
                            i.Player.Client.Save();
                            i.Player.UpdateCount++;
                            player.SendInfo("Success");
                        }
                        catch
                        {
                            return Tuple.Create(false, "Error while setting stat");
                        }
                        return Tuple.Create(true, "Success");
                    }
                }
                return Tuple.Create(false, "Player could not be found!");
            }
            player.SendHelp("Usage: /setStat <Stat> <Amount>");
            player.SendHelp("or");
            return Tuple.Create(false, "Usage: /setStat <Player> <Stat> <Amount>");
        }
    }

    internal class SetpieceCommand : Command
    {
        public SetpieceCommand()
            : base("setpiece", 4)
        {
        }

        protected override Tuple<bool, string> Process(Player player, RealmTime time, string[] args)
        {
            ISetPiece piece = (ISetPiece)Activator.CreateInstance(Type.GetType(
                "wServer.realm.setpieces." + args[0], true, true));
            piece.RenderSetPiece(player.Owner, new IntPoint((int)player.X + 1, (int)player.Y + 1));
            return Tuple.Create(true, "Success");
        }
    }

    internal class ListCommands : Command
    {
        public ListCommands() : base("commands") { }

        protected override Tuple<bool, string> Process(Player player, RealmTime time, string[] args)
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
            
            return Tuple.Create(true, sb.ToString());
        }
    }

    internal class GodCommand : Command
    {
        public GodCommand()
        : base("god", 2)
        {
        }

        protected override Tuple<bool, string> Process(Player player, RealmTime time, string[] args)
        {
            if (player.HasConditionEffect(ConditionEffectIndex.Invincible))
            {
                player.ApplyConditionEffect(new ConditionEffect
                {
                    Effect = ConditionEffectIndex.Invincible,
                    DurationMS = 0
                });
                return Tuple.Create(true, "Godmode Off");
            }
            else
            {
                player.ApplyConditionEffect(new ConditionEffect
                {
                    Effect = ConditionEffectIndex.Invincible,
                    DurationMS = -1
                });
                return Tuple.Create(true, "Godmode On");
            }
        }
    }

    internal class AccIdCommand : Command
    {
        public AccIdCommand()
            : base("accid", 2) { }

        protected override Tuple<bool, string> Process(Player player, RealmTime time, string[] args)
        {
            if (string.IsNullOrEmpty(args[0]))
            {
                return Tuple.Create(false, "Usage: /accid <player>");
            }
            var plr = player.Manager.FindPlayer(args[0]);
            return Tuple.Create(true, "Account ID of " + plr.Name + " : " + plr.AccountId);
        }
    }

    internal class CloseRealmCmd : Command
    {
        public CloseRealmCmd()
            : base("closerealm", 4)
        {
        }
        protected override Tuple<bool, string> Process(Player player, RealmTime time, string[] args)
        {
            if (player.Owner is GameWorld)
            {
                var gw = player.Owner as GameWorld;
                gw.Overseer.InitCloseRealm();
                return Tuple.Create(true, "Success");
            }
            return Tuple.Create(false, "Cannot use outside of the realm");
        }
    }

    internal class Sell : Command
    {
        public Sell() : base("Sell") {}
        protected override Tuple<bool, string> Process(Player player, RealmTime time, string[] args)
        {

            using (var db = new Database())
            {
                if (args.Length < 2)
                    return Tuple.Create(false, "Usage: /sell <slot> <price>");
                
                int slot = Convert.ToInt32(args[0]) + 3;
                if (Convert.ToInt32(args[0]) > 8 || Convert.ToInt32(args[0]) < 1)
                    return Tuple.Create(false, "Slot Number Invalid, please only choose items in slot 1-8");

                if (Convert.ToInt32(args[1]) < 0)
                    return Tuple.Create(false, "Selling price must be a positive integer");

                Item item = player.Inventory[slot];
                if (item.Secret || item.Soulbound)
                    return Tuple.Create(false, $"Cannot Sell {item.ObjectId} because it is soulbound or secret");

                if (Merchant.checkItem(player.SerialConvert(player.Inventory[slot])))
                {
                    MySqlCommand cmd = db.CreateQuery();
                    cmd.CommandText = "INSERT INTO market(itemID, fame, serialid) VALUES(@itemID, @fame, @serialid)";
                    cmd.Parameters.AddWithValue("@itemID", Convert.ToInt32(item.ObjectType));
                    cmd.Parameters.AddWithValue("@fame", args[1]);
                    cmd.Parameters.AddWithValue("@serialid", item.serialId);
                    cmd.ExecuteNonQuery();
                    player.Inventory[slot] = null;
                    player.Client.Save();
                    player.UpdateCount++;
                    MerchantLists.AddItem(item, Convert.ToInt32(args[1])); //Adds to runtime
                    return Tuple.Create(true, "Success");
                }
                return Tuple.Create(false, $"Cannot Sell {item.ObjectId}");
            }
        }
    }
    
    internal class VisitCommand : Command
    {
        public VisitCommand() : base("visit", 3) {}

        protected override Tuple<bool, string> Process(Player player, RealmTime time, string[] args)
        {
            foreach (KeyValuePair<string, Client> i in player.Manager.Clients)
            {
                if (i.Value.Player.Owner is PetYard || i.Value.Player.Owner is Vault) //Use this if you don't want people visiting those worlds
                    return Tuple.Create(false, $"You cant visit players in world: {i.Value.Player.Owner}");

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
                    }
                    return Tuple.Create(true, "You are visiting " + i.Value.Player.Owner.Id);
                }
            }
            return Tuple.Create(false, $"Player '{args[0]}' could not be found!");
        }
    }

    class LinkCommand : Command
    {
        public LinkCommand() : base("link", 2) { }

        protected override Tuple<bool, string> Process(Player player, RealmTime time, string[] args)
        {
            var world = player.Owner;
            if (world.Id < 0)
                return Tuple.Create(false, "You cannot link this world.");

            if (!player.Manager.Monitor.AddPortal(world))
                return Tuple.Create(false, "Link already exists.");

            return Tuple.Create(true, "World Linked");
        }
    }
    class UnLinkCommand : Command
    {
        public UnLinkCommand() : base("unlink", 2) { }

        protected override Tuple<bool, string> Process(Player player, RealmTime time, string[] args)
        {
            var world = player.Owner;
            if (world.Id < 0)
                return Tuple.Create(false, "You cannot unlink this world.");
            
            if (!player.Manager.Monitor.RemovePortal(player.Owner))
                return Tuple.Create(false, "Link not found.");
            else
                return Tuple.Create(true, "Link removed.");
        }
    }
}