#region

using System;
using System.Linq;
using System.Text;
using db;
using wServer.networking.cliPackets;
using wServer.networking.svrPackets;
using wServer.realm;
using wServer.realm.worlds;
using FailurePacket = wServer.networking.svrPackets.FailurePacket;

#endregion

namespace wServer.networking.handlers
{
    internal class HelloHandler : PacketHandlerBase<HelloPacket>
    {
        public override PacketID Id => PacketID.HELLO;

        protected override void HandlePacket(Client client, HelloPacket packet)
        {
            if (Client.ServerVersion != packet.BuildVersion)
            {
                client.SendPacket(new FailurePacket
                {
                    ErrorId = 0,
                    ErrorDescription = "server.update_client"
                });
                client.SendPacket(new FailurePacket
                {
                    ErrorId = 4,
                    ErrorDescription = Client.ServerVersion
                });
                client.Disconnect("Incorrect Version");
                return;
            }
            client.Manager.Database.DoActionAsync(db =>
            {
                if ((client.Account = db.Verify(packet.Guid, packet.Password, Manager.GameData)) == null)
                {
                    Program.writeNotable(@"Account not verified.");
                    client.Account = Database.CreateGuestAccount(packet.Guid);

                    if (client.Account == null)
                    {
                        Program.writeNotable(@"Account is null!");
                        client.SendPacket(new FailurePacket
                        {
                            ErrorDescription = "Invalid account."
                        });
                        client.Disconnect("Invalid account");
                        return;
                    }
                }
                if (!client.Account.IsGuestAccount)
                {
                    int? timeout = null;
                    
                    if (!IsWhiteListed(client.Account.Rank))
                    {
                        client.SendPacket(new FailurePacket
                        {
                            ErrorId = 0,
                            ErrorDescription = "You are not whitelisted!"
                        });
                        client.Disconnect("Whitelist");
                        return;
                    }
                    if (db.CheckAccountInUse(client.Account, ref timeout))
                    {
                        if (timeout == null)
                        {
                            client.SendPacket(new FailurePacket
                            {
                                ErrorId = 0,
                                ErrorDescription = "Account in use."
                            });
                        }
                        else
                        {
                            client.SendPacket(new FailurePacket
                            {
                                ErrorId = 0,
                                ErrorDescription = "Account in use. (" + timeout + " seconds until timeout.)"
                            });
                        }
                        client.Disconnect("Account in use");
                        return;
                    }
                }
                Console.WriteLine(@"Client trying to connect!");
                client.ConnectedBuild = packet.BuildVersion;
                if (!client.Manager.TryConnect(client))
                {
                    client.Account = null;
                    client.SendPacket(new FailurePacket
                    {
                        ErrorDescription = "Failed to connect."
                    });
                    client.Disconnect("Failed to connect");
                    Program.writeError("Client Failed to connect.");
                }
                else
                {
                    if (packet.GameId == World.NEXUS_LIMBO) packet.GameId = World.NEXUS_ID;
                    World world = client.Manager.GetWorld(packet.GameId);
                    if (world == null && packet.GameId == World.TUT_ID) world = client.Manager.AddWorld(new Tutorial(false));
                    if (world == null)
                    {
                        client.SendPacket(new FailurePacket
                        {
                            ErrorId = 1,
                            ErrorDescription = "Invalid world."
                        });
                        client.Disconnect("Invalid World");
                        return;
                    }
                    if (world.NeedsPortalKey)
                    {
                        if (!world.PortalKey.SequenceEqual(packet.Key))
                        {
                            client.SendPacket(new FailurePacket
                            {
                                ErrorId = 1,
                                ErrorDescription = "Invalid Portal Key"
                            });
                            client.Disconnect("Invalid Portal Key");
                            return;
                        }
                        if (world.PortalKeyExpired)
                        {
                            client.SendPacket(new FailurePacket
                            {
                                ErrorId = 1,
                                ErrorDescription = "Portal key expired."
                            });
                            client.Disconnect("Expired Portal Key");
                            return;
                        }
                    }
                    Console.WriteLine(@"Client joined world " + world.Id);
                    if (packet.MapInfo.Length > 0 || world.Id == -6) //Test World
                        (world as Test).LoadJson(Encoding.Default.GetString(packet.MapInfo));
                    if (world.IsLimbo)
                        world = world.GetInstance(client);
                    client.Random = new wRandom(world.Seed);
                    client.TargetWorld = world.Id;
                    client.SendPacket(new MapInfoPacket
                    {
                        Width = world.Map.Width,
                        Height = world.Map.Height,
                        Name = world.Name,
                        Seed = world.Seed,
                        ClientWorldName = world.ClientWorldName,
                        Difficulty = world.Difficulty,
                        Background = world.Background,
                        AllowTeleport = world.AllowTeleport,
                        ShowDisplays = world.ShowDisplays,
                        ClientXml = world.ClientXml,
                        ExtraXml = Manager.GameData.AdditionXml
                    });
                    client.Stage = ProtocalStage.Handshaked;
                }
            });
        }

        private bool IsWhiteListed(int rank)
        {
            if (Program.WhiteList)
            {
                if (rank > 4) return true;
                return false;
            }
            return true;
        }
    }
}
