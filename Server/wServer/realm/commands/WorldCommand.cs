#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using wServer.networking.cliPackets;
using wServer.networking.svrPackets;
using wServer.realm.entities;
using wServer.realm.entities.player;
using wServer.realm.worlds;

#endregion

namespace wServer.realm.commands
{
    internal class TutorialCommand : Command
    {
        public TutorialCommand()
            : base("tutorial")
        {
        }

        protected override Tuple<bool, string>Process (Player player, RealmTime time, string[] args)
        {
            player.Client.Reconnect(new ReconnectPacket
            {
                Host = "",
                Port = Program.Settings.GetValue<int>("port"),
                GameId = World.TUT_ID,
                Name = "Tutorial",
                Key = Empty<byte>.Array
            });
            return Tuple.Create(true, "Success");
        }
    }

    internal class TradeCommand : Command
    {
        public TradeCommand()
            : base("trade")
        {
        }

        protected override Tuple<bool, string>Process (Player player, RealmTime time, string[] args)
        {
            if(string.IsNullOrWhiteSpace(args[0]))
                return Tuple.Create(false, "Usage: /trade <player name>");

            player.RequestTrade(time, new RequestTradePacket
            {
                Name = args[0]
            });
            return Tuple.Create(true, "");
        }
    }


    internal class WhoCommand : Command
    {
        public WhoCommand()
            : base("who")
        {
        }

        protected override Tuple<bool, string>Process (Player player, RealmTime time, string[] args)
        {
            StringBuilder sb = new StringBuilder("Players online: ");
            Player[] copy = player.Owner.Players.Values.ToArray();
            for (int i = 0; i < copy.Length; i++)
            {
                if (i != 0) sb.Append(", ");
                sb.Append(copy[i].Name);
            }
            return Tuple.Create(true, sb.ToString());
        }
    }

    internal class ServerCommand : Command
    {
        public ServerCommand() : base("server") {}

        protected override Tuple<bool, string>Process (Player player, RealmTime time, string[] args)
        {
            player.SendInfo(player.Owner.Name);
            return Tuple.Create(true, "Success");
        }
    }

    internal class PauseCommand : Command
    {
        public PauseCommand()
            : base("pause")
        {
        }

        protected override Tuple<bool, string> Process (Player player, RealmTime time, string[] args)
        {
            if (player.HasConditionEffect(ConditionEffectIndex.Paused))
            {
                player.ApplyConditionEffect(new ConditionEffect
                {
                    Effect = ConditionEffectIndex.Paused,
                    DurationMS = 0
                });
                return Tuple.Create(true, "Game resumed.");
            }
            else
            {
                foreach (Enemy i in player.Owner.EnemiesCollision.HitTest(player.X, player.Y, 8).OfType<Enemy>())
                {
                    if (i.ObjectDesc.Enemy)
                        return Tuple.Create(false, "Not safe to pause.");
                }
                player.ApplyConditionEffect(new ConditionEffect
                {
                    Effect = ConditionEffectIndex.Paused,
                    DurationMS = -1
                });
                return Tuple.Create(true, "Game paused.");
            }
        }
    }

    internal class TeleportCommand : Command
    {
        public TeleportCommand()
            : base("teleport")
        {
        }

        protected override Tuple<bool, string> Process (Player player, RealmTime time, string[] args)
        {
            try
            {
                if (String.Equals(player.Name.ToLower(), args[0].ToLower()))
                {
                    return Tuple.Create(false, "You are already at yourself, and always will be!");
                }

                foreach (KeyValuePair<int, Player> i in player.Owner.Players)
                {
                    if (i.Value.Name.ToLower() == args[0].ToLower().Trim())
                    {
                        player.Teleport(time, new TeleportPacket
                        {
                            ObjectId = i.Value.Id
                        });
                        return Tuple.Create(true, "Success");
                    }
                }
                return Tuple.Create(false, "Cannot teleport, {args[0].Trim()} not found!");
            }
            catch
            {
                return Tuple.Create(false, "Usage: /teleport <player name>");
            }

        }
    }

    class TellCommand : Command
    {
        public TellCommand() : base("tell") { }

        protected override Tuple<bool, string> Process (Player player, RealmTime time, string[] args)
        {
            if (!player.NameChosen)
                return Tuple.Create(false, "Choose a name!");

            if (args.Length < 2)
                return Tuple.Create(false, "Usage: /tell <player name> <text>");

            string playername = args[0].Trim();
            string msg = string.Join(" ", args, 1, args.Length - 1);

            if (string.Equals(player.Name.ToLower(), playername.ToLower()))
                return Tuple.Create(false, "Quit telling yourself!");

            foreach (var i in player.Manager.Clients.Values)
            {
                if (i.Account.NameChosen && i.Account.Name.EqualsIgnoreCase(playername))
                {
                    player.SendStarText("*To: " + i.Player.Name, player.Stars, msg.ToSafeText());
                    i.Player.SendStarText("*" + player.Name, player.Stars, msg.ToSafeText());
                    
                    return Tuple.Create(true, "Success");
                }
            }
            return Tuple.Create(false, $"{playername} not found.");
        }
    }
    internal class TpMarket : Command
    {
        public TpMarket()
            : base("Market")
        {
        }

        protected override Tuple<bool, string>Process (Player player, RealmTime time, string[] args)
        {
            player.Client.Reconnect(new ReconnectPacket
            {
                Host = "",
                Port = 2050,
                GameId = World.FMARKET,
                Name = "Market",
                Key = Empty<byte>.Array
            });
            return Tuple.Create(true, "Success");
        }
    }
    internal class TpRealm : Command
    {
        public TpRealm()
            : base("Realm")
        {
        }

        protected override Tuple<bool, string>Process (Player player, RealmTime time, string[] args)
        {
            player.Client.Reconnect(new ReconnectPacket
            {
                Host = "",
                Port = 2050,
                GameId = RealmManager.CurrentWorldId,
                Name = "Frostz's Realm",
                Key = Empty<byte>.Array
            });
            return Tuple.Create(true, "Success");
        }
    }

    internal class TpCourt : Command
    {
        public TpCourt() : base("Court") {}

        protected override Tuple<bool, string>Process (Player player, RealmTime time, string[] args)
        {
            if (CourtOfBereavement._waitforplayers)
            {
                player.Client.Reconnect(new ReconnectPacket
                {
                    Host = "",
                    Port = 2050,
                    GameId = RealmManager.CurrentCourtId,
                    Name = "Court of Bereavement",
                    Key = Empty<byte>.Array
                });
                return Tuple.Create(true, "Success");
            }
            return Tuple.Create(false, "No Court available to join!");
        }
    }

    internal class GetSerial : Command
    {
        public GetSerial() : base("Serial") {}

        protected override Tuple<bool, string>Process (Player player, RealmTime time, string[] args)
        {
            return Tuple.Create(true, "Serial Number is " + player.Inventory[Convert.ToInt32(args[0])].serialId);
        }
    }

    internal class Godland : Command
    {
        public Godland() : base("gland") {}

        protected override Tuple<bool, string>Process (Player player, RealmTime time, string[] args)
        {
            if (!(player.Owner is GameWorld))
                return Tuple.Create(false, "That command can't be used here!");

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
            return Tuple.Create(true, "Success!");
        }
    }
}