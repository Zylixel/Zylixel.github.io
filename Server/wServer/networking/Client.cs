#region

using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading.Tasks;
using wServer.networking.cliPackets;
using wServer.networking.svrPackets;
using wServer.realm;
using wServer.realm.entities.player;

#endregion

namespace wServer.networking
{
    public enum ProtocalStage
    {
        Connected,
        Handshaked,
        Ready,
        Disconnected
    }

    public class Client : IDisposable
    {
        public const string ServerVersion = "27.7.X2";
        private bool _disposed;

        public uint UpdateAckCount = 0;

        private NetworkHandler _handler;

        public Client(RealmManager manager, Socket skt)
        {
            Socket = skt;
            Manager = manager;
            ReceiveKey =
                new RC4(new byte[] {0x31, 0x1f, 0x80, 0x69, 0x14, 0x51, 0xc7, 0x1d, 0x09, 0xa1, 0x3a, 0x2a, 0x6e});
            SendKey = new RC4(new byte[] {0x72, 0xc5, 0x58, 0x3c, 0xaf, 0xb6, 0x81, 0x89, 0x95, 0xcd, 0xd7, 0x4b, 0x80});
            BeginProcess();
        }

        public RC4 ReceiveKey { get; private set; }
        public RC4 SendKey { get; private set; }
        public RealmManager Manager { get; private set; }

        public int Id { get; internal set; }

        public Socket Socket { get; internal set; }

        public Char Character { get; internal set; }

        public Account Account { get; internal set; }

        public ProtocalStage Stage { get; internal set; }

        public Player Player { get; internal set; }

        public wRandom Random { get; internal set; }
        public string ConnectedBuild { get; internal set; }
        public int TargetWorld { get; internal set; }

        public void BeginProcess()
        {
            Console.WriteLine($"Received client @ {Socket.RemoteEndPoint}.");
            _handler = new NetworkHandler(this, Socket);
            _handler.BeginHandling();
        }

        public void SendPacket(Packet pkt)
        {
            _handler?.SendPacket(pkt);
        }

        public void SendPackets(IEnumerable<Packet> pkts)
        {
            _handler?.SendPackets(pkts);
        }

        public bool IsReady()
        {
            if (Stage == ProtocalStage.Disconnected)
                return false;
            return Stage != ProtocalStage.Ready || (Player != null && (Player == null || Player.Owner != null));
        }

        internal void ProcessPacket(Packet pkt)
        {
            try
            {
                if (pkt.Id == (PacketID) 255) return;
                IPacketHandler handler;
                if (!PacketHandlers.Handlers.TryGetValue(pkt.Id, out handler))
                    Console.WriteLine($"Unhandled packet '{pkt.Id}'.");
                else
                    handler.Handle(this, (ClientPacket) pkt);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error when handling packet '{pkt}'...", e);
                Disconnect();
            }
        }

        public void Disconnect()
        {
            try
            {
                if (Stage == ProtocalStage.Disconnected) return;
                Stage = ProtocalStage.Disconnected;
                if (Account != null)
                    DisconnectFromRealm();

                Socket.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public Task Save()
        {
            return Manager.Database.DoActionAsync(db =>
            {
                try
                {
                    string w = null;
                    if (Player != null)
                    {
                        Player.SaveToCharacter();
                        if (Player.Owner != null)
                        {
                            if (Player.Owner.Id == -6) return;
                            w = Player.Owner.Name;
                        }
                    }

                    if (Character != null)
                    {
                        if (w != null) db.UpdateLastSeen(Account.AccountId, Character.CharacterId, w);
                        db.SaveCharacter(Account, Character);
                    }

                    db.UnlockAccount(Account);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("SaveException", ex);
                }
            });
        }

        //Following must execute, network loop will discard disconnected client, so logic loop
        private void DisconnectFromRealm()
        {
            Manager.Logic.AddPendingAction(t =>
            {
                Save();
                Manager.Disconnect(this);
            }, PendingPriority.Destruction);
        }

        public void Reconnect(ReconnectPacket pkt)
        {
            Manager.Logic.AddPendingAction(t =>
            {
                Save();
                SendPacket(pkt);
            }, PendingPriority.Destruction);
        }

        public void Dispose()
        {
            if (_disposed) return;
            _handler = null;
            ReceiveKey = null;
            SendKey = null;
            Manager = null;
            Socket = null;
            Character = null;
            Account = null;
            Player?.Dispose();
            Player = null;
            Random = null;
            ConnectedBuild = null;
            _disposed = true;
        }
    }
}