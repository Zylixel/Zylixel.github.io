#region

using wServer.networking.cliPackets;

#endregion

namespace wServer.networking.handlers
{
    internal class UpdateAckHandler : PacketHandlerBase<UpdateAckPacket>
    {
        public override PacketID Id => PacketID.UPDATEACK;

        protected override void HandlePacket(Client client, UpdateAckPacket packet)
        {
            client.Player.UpdatesReceived++;
        }
    }
}