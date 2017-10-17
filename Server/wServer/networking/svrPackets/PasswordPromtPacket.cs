namespace wServer.networking.svrPackets
{
    public class PasswordPromtPacket : ServerPacket
    {
        public const int SignIn = 2;
        public const int SendEmailAndSignIn = 3;
        public const int Register = 4;

        public int CleanPasswordStatus { get; set; }

        public override PacketID Id
        {
            get { return PacketID.PASSWORDPROMPT; }
        }

        public override Packet CreateInstance()
        {
            return new PasswordPromtPacket();
        }

        protected override void Read(Client client, NReader rdr)
        {
            CleanPasswordStatus = rdr.ReadInt32();
        }

        protected override void Write(Client client, NWriter wtr)
        {
            wtr.Write(CleanPasswordStatus);
        }
    }
}
