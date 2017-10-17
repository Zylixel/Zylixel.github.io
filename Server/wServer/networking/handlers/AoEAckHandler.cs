#region

using wServer.networking.cliPackets;

#endregion

namespace wServer.networking.handlers
{
    internal class AoeAckHandler : PacketHandlerBase<AoeAckPacket>
    {
        public override PacketID Id => PacketID.AOEACK;

        protected override void HandlePacket(Client client, AoeAckPacket packet)
        {
            //TODO: Implement something.
        }
    }
}