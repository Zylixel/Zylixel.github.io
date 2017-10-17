#region

using wServer.networking.cliPackets;

#endregion

namespace wServer.networking.handlers
{
    internal class GotoAckHandler : PacketHandlerBase<GotoAckPacket>
    {
        public override PacketID Id
        {
            get { return PacketID.GOTOACK; }
        }

        protected override void HandlePacket(Client client, GotoAckPacket packet)
        {
            //TODO: Implement something.
        }
    }
}