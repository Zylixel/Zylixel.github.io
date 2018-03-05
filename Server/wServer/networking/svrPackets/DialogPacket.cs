namespace wServer.networking.svrPackets
{
    public class DialogPacket : ServerPacket
    {
        public string Title { get; set; }
        public string Description { get; set; }

        public override PacketID Id
        {
            get { return PacketID.DIALOG; }
        }

        public override Packet CreateInstance()
        {
            return new DialogPacket();
        }

        protected override void Read(Client psr, NReader rdr)
        {
            Title = rdr.ReadUTF();
            Description = rdr.ReadUTF();
        }

        protected override void Write(Client psr, NWriter wtr)
        {
            wtr.WriteUTF(Title);
            wtr.WriteUTF(Description);
        }
    }
}