#region

using wServer.networking.cliPackets;

#endregion

namespace wServer.networking.handlers
{
    internal class PongHandler : PacketHandlerBase<PongPacket>
    {
        public override PacketID Id => PacketID.PONG;

        protected override void HandlePacket(Client client, PongPacket packet)
        {
            client.Player.Pong(packet.Time, packet);
        }
    }
}