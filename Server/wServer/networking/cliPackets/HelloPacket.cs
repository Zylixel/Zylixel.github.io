namespace wServer.networking.cliPackets
{
    public class HelloPacket : ClientPacket
    {
        public string BuildVersion { get; set; }
        public int GameId { get; set; }
        public string Guid { get; set; }
        public string Password { get; set; }
        public string Secret { get; set; }
        public int Randomint1 { get; set; }
        public int KeyTime { get; set; }
        public byte[] Key { get; set; }
        public byte[] MapInfo { get; set; }
        public string Obf1 { get; set; }
        public string Obf2 { get; set; }
        public string Obf3 { get; set; }
        public string Obf4 { get; set; }
        public string Obf5 { get; set; }

        public override PacketID Id
        {
            get { return PacketID.HELLO; }
        }

        public override Packet CreateInstance()
        {
            return new HelloPacket();
        }

        protected override void Read(Client psr, NReader rdr)
        {
            BuildVersion = rdr.ReadUTF();
            GameId = rdr.ReadInt32();
            Guid = Rsa.Instance.Decrypt(rdr.ReadUTF());
            rdr.ReadInt32();
            Password = Rsa.Instance.Decrypt(rdr.ReadUTF());
            Randomint1 = rdr.ReadInt32();
            Secret = rdr.ReadUTF();
            KeyTime = rdr.ReadInt32();
            Key = rdr.ReadBytes(rdr.ReadInt16());
            MapInfo = rdr.ReadBytes(rdr.ReadInt32());
            Obf1 = rdr.ReadUTF();
            Obf2 = rdr.ReadUTF();
            Obf3 = rdr.ReadUTF();
            Obf4 = rdr.ReadUTF();
            Obf5 = rdr.ReadUTF();
        }

        protected override void Write(Client psr, NWriter wtr)
        {
            wtr.WriteUTF(BuildVersion);
            wtr.Write(GameId);
            wtr.Write(0);
            wtr.WriteUTF(Rsa.Instance.Encrypt(Guid));
            wtr.Write(Randomint1);
            wtr.WriteUTF(Rsa.Instance.Encrypt(Password));
            wtr.WriteUTF(Secret);
            wtr.Write(KeyTime);
            wtr.Write((ushort)Key.Length);
            wtr.Write(Key);
            wtr.Write(MapInfo.Length);
            wtr.Write(MapInfo);
            wtr.WriteUTF(Obf1);
            wtr.WriteUTF(Obf2);
            wtr.WriteUTF(Obf3);
            wtr.WriteUTF(Obf4);
            wtr.WriteUTF(Obf5);
        }
    }
}