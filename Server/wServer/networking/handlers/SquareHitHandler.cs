#region

using wServer.networking.cliPackets;

#endregion

namespace wServer.networking.handlers
{
    internal class SquareHitHandler : PacketHandlerBase<SquareHitPacket>
    {
        public override PacketID Id => PacketID.SQUAREHIT;

        protected override void HandlePacket(Client client, SquareHitPacket packet)
        {
            //TODO: Implement something
        }
    }
}