namespace wServer.networking.svrPackets
{
    public class UnlockedSkinPacket : ServerPacket
    {
        public int SkinId { get; set; }

        public override PacketID Id
        {
            get { return PacketID.RESKIN2; }
        }

        public override Packet CreateInstance()
        {
            return new UnlockedSkinPacket();
        }

        protected override void Read(Client client, NReader rdr)
        {
            SkinId = rdr.ReadInt32();
        }

        protected override void Write(Client client, NWriter wtr)
        {
            wtr.Write(SkinId);
        }
    }
}
