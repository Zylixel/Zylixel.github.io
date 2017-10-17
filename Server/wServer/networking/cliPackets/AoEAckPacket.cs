namespace wServer.networking.cliPackets
{
    public class AoeAckPacket : ClientPacket
    {
        public int Time { get; set; }
        public Position Position { get; set; }

        public override PacketID Id
        {
            get { return PacketID.AOEACK; }
        }

        public override Packet CreateInstance()
        {
            return new AoeAckPacket();
        }

        protected override void Read(Client psr, NReader rdr)
        {
            Time = rdr.ReadInt32();
            Position = Position.Read(psr, rdr);
        }

        protected override void Write(Client psr, NWriter wtr)
        {
            wtr.Write(Time);
            Position.Write(psr, wtr);
        }
    }
}