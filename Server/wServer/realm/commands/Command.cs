#region

using System;
using System.Collections.Generic;
using log4net;
using wServer.realm.entities.player;
using wServer.networking.svrPackets;
using wServer.realm.worlds;
using wServer.networking;


#endregion

namespace wServer.realm.commands
{
    public abstract class Command
    {
        protected static readonly ILog log = LogManager.GetLogger(typeof (Command));

        public Command(string name, int permLevel = 0)
        {
            CommandName = name;
            PermissionLevel = permLevel;
        }

        public string CommandName { get; private set; }
        public int PermissionLevel { get; private set; }

        protected abstract bool Process(Player player, RealmTime time, string[] args);

        private static int GetPermissionLevel(Player player)
        {
            if (player.Client.Account.Rank == 3)
                return 1;
            return 0;
        }


        public bool HasPermission(Player player)
        {
            if (GetPermissionLevel(player) < PermissionLevel)
                return false;
            return true;
        }

        public bool Execute(Player player, RealmTime time, string args)
        {
            if (!HasPermission(player))
            {
                player.SendError("No permission!");
                return false;
            }

            try
            {
                string[] a = args.Split(' ');
                return Process(player, time, a);
            }
            catch (Exception ex)
            {
                log.Error("Error when executing the command.", ex);
                player.SendError("Error when executing the command.");
                return false;
            }
        }
    }

    public class CommandManager
    {
        private static readonly ILog log = LogManager.GetLogger(typeof (CommandManager));

        private readonly Dictionary<string, Command> cmds;

        private RealmManager manager;

        public CommandManager(RealmManager manager)
        {
            this.manager = manager;
            cmds = new Dictionary<string, Command>(StringComparer.InvariantCultureIgnoreCase);
            Type t = typeof (Command);
            foreach (Type i in t.Assembly.GetTypes())
                if (t.IsAssignableFrom(i) && i != t)
                {
                    Command instance = (Command) Activator.CreateInstance(i);
                    cmds.Add(instance.CommandName, instance);
                }
        }

        public IDictionary<string, Command> Commands
        {
            get { return cmds; }
        }

        public bool Execute(Player player, RealmTime time, string text)
        {
            int index = text.IndexOf(' ');
            string cmd = text.Substring(1, index == -1 ? text.Length - 1 : index - 1);
            string args = index == -1 ? "" : text.Substring(index + 1);

            Command command;
            if (!cmds.TryGetValue(cmd, out command))
            {
                player.SendError("Unknown command!");
                return false;
            }
            log.InfoFormat("[Command] <{0}> {1}", player.Name, text);
            return command.Execute(player, time, args);
        }
    }
    internal class Godland : Command
    {
        public Godland()
            : base("gland", 0)
        {
        }

        protected override bool Process(Player player, RealmTime time, string[] args)
        {
            if (!(player.Owner is GameWorld))
            {
                player.SendError("That command can't be used here!");
                return false;
            }
            else
            {
                player.Move(1000 + 0.5f, 1000 + 0.5f);
                if (player.Pet != null)
                    player.Pet.Move(1000 + 0.5f, 1000 + 0.5f);
                player.UpdateCount++;
                player.ApplyConditionEffect(new ConditionEffect
                {
                    Effect = ConditionEffectIndex.Invincible,
                    DurationMS = 2000
                });
                player.Owner.BroadcastPacket(new GotoPacket
                {
                    ObjectId = player.Id,
                    Position = new Position
                    {
                        X = player.X,
                        Y = player.Y
                    }
                }, null);
                player.SendInfo("Success!");
                return true;
            }
        }
    }
    internal class TpMarket : Command
    {
        public TpMarket()
            : base("Market", 0)
        {
        }

        protected override bool Process(Player player, RealmTime time, string[] args)
        {
            player.Client.Reconnect(new ReconnectPacket
            {
                Host = "",
                Port = 2050,
                GameId = -15,
                Name = "Market",
                Key = Empty<byte>.Array,
            });
            return true;
        }
    }

    internal class ZysRealm : Command
    {
        public ZysRealm()
            : base("Realm", 0)
        {
        }

        protected override bool Process(Player player, RealmTime time, string[] args)
        {
            player.Client.Reconnect(new ReconnectPacket
            {
                GameId = 1,
                Host = "",
                Name = "Realm",
                Key = Empty<byte>.Array,
                Port = 2050
            });
            return true;
        }
    }
    internal class Realm : Command
    {
        public Realm()
            : base("NRealm", 1)
        {
        }

        protected override bool Process(Player player, RealmTime time, string[] args)
        {
            foreach (KeyValuePair<string, Client> i in player.Manager.Clients)
            {
                {
                    Packet pkt;
                    {
                        pkt = new ReconnectPacket
                        {
                            GameId = 1,
                            Host = "",
                            Name = "Market",
                            Key = Empty<byte>.Array,
                            Port = 2050
                        };
                        player.SendInfo("Sending you to Nicks Realm!");
                    }

                    i.Value.SendPacket(pkt);

                    return true;
                }
            }
            player.SendError(string.Format("Player '{0}' could not be found!", args));
            return false;
        }
    }
}