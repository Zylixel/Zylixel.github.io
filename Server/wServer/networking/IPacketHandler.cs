#region

using System;
using System.Collections.Generic;
using wServer.networking.cliPackets;
using wServer.realm;
using FailurePacket = wServer.networking.svrPackets.FailurePacket;

#endregion

namespace wServer.networking
{
    internal interface IPacketHandler
    {
        PacketID Id { get; }
        void Handle(Client client, ClientPacket packet);
    }

    internal abstract class PacketHandlerBase<T> : IPacketHandler where T : ClientPacket
    {
        private Client _client;

        public abstract PacketID Id { get; }

        public void Handle(Client client, ClientPacket packet)
        {
            _client = client;
            HandlePacket(client, (T) packet);
            if (client.Player != null)
                client.Player.isLagging = false;
        }

        public RealmManager Manager { get { return _client.Manager; } }
        public Client Client { get { return _client; } }

        protected abstract void HandlePacket(Client client, T packet);

        protected void SendFailure(string text)
        {
            _client.SendPacket(new FailurePacket {ErrorId = 0, ErrorDescription = text});
        }
    }

    internal class PacketHandlers
    {
        public static Dictionary<PacketID, IPacketHandler> Handlers = new Dictionary<PacketID, IPacketHandler>();

        static PacketHandlers()
        {
            foreach (Type i in typeof (Packet).Assembly.GetTypes())
            {
                if (typeof (IPacketHandler).IsAssignableFrom(i) &&
                    !i.IsAbstract && !i.IsInterface)
                {
                    IPacketHandler pkt = (IPacketHandler) Activator.CreateInstance(i);
                    Handlers.Add(pkt.Id, pkt);
                }
            }
        }
    }
}