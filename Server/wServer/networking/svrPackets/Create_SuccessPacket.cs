namespace wServer.networking.svrPackets
{
    public class CreateSuccessPacket : ServerPacket
    {
        public int ObjectId { get; set; }
        public int CharacterId { get; set; }

        public override PacketID Id
        {
            get { return PacketID.CREATE_SUCCESS; }
        }

        public override Packet CreateInstance()
        {
            return new CreateSuccessPacket();
        }

        protected override void Read(Client psr, NReader rdr)
        {
            ObjectId = rdr.ReadInt32();
            CharacterId = rdr.ReadInt32();
        }

        protected override void Write(Client psr, NWriter wtr)
        {
            wtr.Write(ObjectId);
            wtr.Write(CharacterId);
        }
    }
}