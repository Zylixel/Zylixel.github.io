namespace wServer.networking.cliPackets
{
    public class PetCommandPacket : ClientPacket
    {
        public const int FollowPet = 1;
        public const int UnfollowPet = 2;
        public const int ReleasePet = 3;

        public int CommandId { get; set; }
        public uint PetId { get; set; }

        public override PacketID Id
        {
            get { return PacketID.PETCOMMAND; }
        }

        public override Packet CreateInstance()
        {
            return new PetCommandPacket();
        }

        protected override void Read(Client client, NReader rdr)
        {
            CommandId = rdr.ReadByte();
            PetId = (uint)rdr.ReadInt32();
        }

        protected override void Write(Client client, NWriter wtr)
        {
            wtr.Write((byte)CommandId);
            wtr.Write((int)PetId);
        }
    }
}
