namespace wServer.networking.cliPackets
{
    public class LeaveArenaPacket : ClientPacket
    {
        public int Li { get; set; }

        public override PacketID Id
        {
            get { return PacketID.LEAVEARENA; }
        }

        public override Packet CreateInstance()
        {
            return new LeaveArenaPacket();
        }

        protected override void Read(Client client, NReader rdr)
        {
            Li = rdr.ReadInt32();
        }

        protected override void Write(Client client, NWriter wtr)
        {
            wtr.Write(Li);
        }
    }
}
