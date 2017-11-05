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

        protected override bool Process(Player player, RealmTime time, string[] args)
        {
            player.Client.Reconnect(new ReconnectPacket
            {
                Host = "",
                Port = Program.Settings.GetValue<int>("port"),
                GameId = World.TUT_ID,
                Name = "Tutorial",
                Key = Empty<byte>.Array
            });
            return true;
        }
    }

    internal class TradeCommand : Command
    {
        public TradeCommand()
            : base("trade")
        {
        }

        protected override bool Process(Player player, RealmTime time, string[] args)
        {
            if(string.IsNullOrWhiteSpace(args[0]))
            {
                player.SendInfo("Usage: /trade <player name>");
                return false;
            }
            player.RequestTrade(time, new RequestTradePacket
            {
                Name = args[0]
            });
            return true;
        }
    }


    internal class WhoCommand : Command
    {
        public WhoCommand()
            : base("who")
        {
        }

        protected override bool Process(Player player, RealmTime time, string[] args)
        {
            StringBuilder sb = new StringBuilder("Players online: ");
            Player[] copy = player.Owner.Players.Values.ToArray();
            for (int i = 0; i < copy.Length; i++)
            {
                if (i != 0) sb.Append(", ");
                sb.Append(copy[i].Name);
            }

            player.SendInfo(sb.ToString());
            return true;
        }
    }

    internal class ServerCommand : Command
    {
        public ServerCommand()
            : base("server")
        {
        }

        protected override bool Process(Player player, RealmTime time, string[] args)
        {
            player.SendInfo(player.Owner.Name);
            return true;
        }
    }

    internal class PauseCommand : Command
    {
        public PauseCommand()
            : base("pause")
        {
        }

        protected override bool Process(Player player, RealmTime time, string[] args)
        {
            if (player.HasConditionEffect(ConditionEffectIndex.Paused))
            {
                player.ApplyConditionEffect(new ConditionEffect
                {
                    Effect = ConditionEffectIndex.Paused,
                    DurationMS = 0
                });
                player.SendInfo("Game resumed.");
            }
            else
            {
                foreach (Enemy i in player.Owner.EnemiesCollision.HitTest(player.X, player.Y, 8).OfType<Enemy>())
                {
                    if (i.ObjectDesc.Enemy)
                    {
                        player.SendInfo("Not safe to pause.");
                        return false;
                    }
                }
                player.ApplyConditionEffect(new ConditionEffect
                {
                    Effect = ConditionEffectIndex.Paused,
                    DurationMS = -1
                });
                player.SendInfo("Game paused.");
            }
            return true;
        }
    }

    internal class TeleportCommand : Command
    {
        public TeleportCommand()
            : base("teleport")
        {
        }

        protected override bool Process(Player player, RealmTime time, string[] args)
        {
            try
            {
                if (String.Equals(player.Name.ToLower(), args[0].ToLower()))
                {
                    player.SendInfo("You are already at yourself, and always will be!");
                    return false;
                }

                foreach (KeyValuePair<int, Player> i in player.Owner.Players)
                {
                    if (i.Value.Name.ToLower() == args[0].ToLower().Trim())
                    {
                        player.Teleport(time, new TeleportPacket
                        {
                            ObjectId = i.Value.Id
                        });
                        return true;
                    }
                }
                player.SendInfo($"Cannot teleport, {args[0].Trim()} not found!");
            }
            catch
            {
                player.SendHelp("Usage: /teleport <player name>");
            }
            return false;
        }
    }

    class TellCommand : Command
    {
        public TellCommand() : base("tell") { }

        protected override bool Process(Player player, RealmTime time, string[] args)
        {
            if (!player.NameChosen)
            {
                player.SendError("Choose a name!");
                return false;
            }
            if (args.Length < 2)
            {
                player.SendError("Usage: /tell <player name> <text>");
                return false;
            }

            string playername = args[0].Trim();
            string msg = string.Join(" ", args, 1, args.Length - 1);

            if (string.Equals(player.Name.ToLower(), playername.ToLower()))
            {
                player.SendInfo("Quit telling yourself!");
                return false;
            }

            /*if (playername.ToLower() == "muledump")
            {
                if (msg.ToLower() == "private muledump")
                {
                    player.Client.SendPacket(new TextPacket //echo to self
                    {
                        ObjectId = player.Id,
                        BubbleTime = 10,
                        Stars = player.Stars,
                        Name = player.Name,
                        Recipient = "Muledump",
                        Text = msg.ToSafeText(),
                        CleanText = ""
                    });

                    player.Manager.Database.DoActionAsync(db =>
                    {
                        var cmd = db.CreateQuery();
                        cmd.CommandText = "UPDATE accounts SET publicMuledump=0 WHERE id=@accId;";
                        cmd.Parameters.AddWithValue("@accId", player.AccountId);
                        cmd.ExecuteNonQuery();
                        player.Client.SendPacket(new TextPacket
                        {
                            ObjectId = -1,
                            BubbleTime = 10,
                            Stars = 70,
                            Name = "Muledump",
                            Recipient = player.Name,
                            Text = "Your muledump is now hidden, only you can view it now.",
                            CleanText = ""
                        });
                    });
                }
                else if (msg.ToLower() == "public muledump")
                {
                    player.Client.SendPacket(new TextPacket //echo to self
                    {
                        ObjectId = player.Id,
                        BubbleTime = 10,
                        Stars = player.Stars,
                        Name = player.Name,
                        Recipient = "Muledump",
                        Text = msg.ToSafeText(),
                        CleanText = ""
                    });
                    player.Manager.Database.DoActionAsync(db =>
                    {
                        var cmd = db.CreateQuery();
                        cmd.CommandText = "UPDATE accounts SET publicMuledump=1 WHERE id=@accId;";
                        cmd.Parameters.AddWithValue("@accId", player.AccountId);
                        cmd.ExecuteNonQuery();

                        player.Client.SendPacket(new TextPacket
                        {
                            ObjectId = -1,
                            BubbleTime = 10,
                            Stars = 70,
                            Name = "Muledump",
                            Recipient = player.Name,
                            Text = "Your muledump is now public, anyone can view it now.",
                            CleanText = ""
                        });
                    });
                }
                else
                {
                    player.Client.SendPacket(new TextPacket //echo to self
                    {
                        ObjectId = player.Id,
                        BubbleTime = 10,
                        Stars = player.Stars,
                        Name = player.Name,
                        Recipient = "Muledump",
                        Text = msg.ToSafeText(),
                        CleanText = ""
                    });

                    player.Client.SendPacket(new TextPacket
                    {
                        ObjectId = -1,
                        BubbleTime = 10,
                        Stars = 70,
                        Name = "Muledump",
                        Recipient = player.Name,
                        Text = "U WOT M8, 1v1 IN THE GARAGE!!!!111111oneoneoneeleven",
                        CleanText = ""
                    });
                }
                return true;
            }*/

            foreach (var i in player.Manager.Clients.Values)
            {
                if (i.Account.NameChosen && i.Account.Name.EqualsIgnoreCase(playername))
                {
                    player.Client.SendPacket(new TextPacket //echo to self
                    {
                        ObjectId = player.Id,
                        BubbleTime = 10,
                        Stars = player.Stars,
                        Name = player.Name,
                        Recipient = i.Account.Name,
                        Text = msg.ToSafeText(),
                        CleanText = ""
                    });

                    i.SendPacket(new TextPacket //echo to /tell player
                    {
                        ObjectId = i.Player.Owner.Id == player.Owner.Id ? player.Id : -1,
                        BubbleTime = 10,
                        Stars = player.Stars,
                        Name = player.Name,
                        Recipient = i.Account.Name,
                        Text = msg.ToSafeText(),
                        CleanText = ""
                    });
                    return true;
                }
            }
            player.SendError($"{playername} not found.");
            return false;
        }
    }
    internal class TpMarket : Command
    {
        public TpMarket()
            : base("Market")
        {
        }

        protected override bool Process(Player player, RealmTime time, string[] args)
        {
            player.Client.Reconnect(new ReconnectPacket
            {
                Host = "",
                Port = 2050,
                GameId = World.FMARKET,
                Name = "Market",
                Key = Empty<byte>.Array
            });
            return true;
        }
    }
    internal class TpRealm : Command
    {
        public TpRealm()
            : base("Realm")
        {
        }

        protected override bool Process(Player player, RealmTime time, string[] args)
        {
            log.Error("Trying to connect to: " + RealmManager.CurrentWorldId);
            player.Client.Reconnect(new ReconnectPacket
            {
                Host = "",
                Port = 2050,
                GameId = RealmManager.CurrentWorldId,
                Name = "Zy's Realm",
                Key = Empty<byte>.Array
            });
            return true;
        }
    }
    internal class Godland : Command
    {
        public Godland()
            : base("gland")
        {
        }

        protected override bool Process(Player player, RealmTime time, string[] args)
        {
            if (!(player.Owner is GameWorld))
            {
                player.SendError("That command can't be used here!");
                return false;
            }
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