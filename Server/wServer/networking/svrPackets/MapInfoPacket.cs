namespace wServer.networking.svrPackets
{
    public class MapInfoPacket : ServerPacket
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public string Name { get; set; }
        public string ClientWorldName { get; set; }
        public int Difficulty { get; set; }
        public uint Seed { get; set; }
        public int Background { get; set; }
        public bool AllowTeleport { get; set; }
        public bool ShowDisplays { get; set; }
        public string[] ClientXml { get; set; }
        public string[] ExtraXml { get; set; }

        public override PacketID Id
        {
            get { return PacketID.MAPINFO; }
        }

        public override Packet CreateInstance()
        {
            return new MapInfoPacket();
        }

        protected override void Read(Client psr, NReader rdr)
        {
            Width = rdr.ReadInt32();
            Height = rdr.ReadInt32();
            Name = rdr.ReadUTF();
            ClientWorldName = rdr.ReadUTF();
            Seed = rdr.ReadUInt32();
            Background = rdr.ReadInt32();
            Difficulty = rdr.ReadInt32();
            AllowTeleport = rdr.ReadBoolean();
            ShowDisplays = rdr.ReadBoolean();

            ClientXml = new string[rdr.ReadInt16()];
            for (int i = 0; i < ClientXml.Length; i++)
                ClientXml[i] = rdr.ReadUTF();

            ExtraXml = new string[rdr.ReadInt16()];
            for (int i = 0; i < ExtraXml.Length; i++)
                ExtraXml[i] = rdr.ReadUTF();
        }

        protected override void Write(Client psr, NWriter wtr)
        {
            wtr.Write(Width);
            wtr.Write(Height);
            wtr.WriteUTF(Name);
            wtr.WriteUTF(ClientWorldName);
            wtr.Write(Seed);
            wtr.Write(Background);
            wtr.Write(Difficulty);
            wtr.Write(AllowTeleport);
            wtr.Write(ShowDisplays);

            wtr.Write((ushort) ClientXml.Length);
            foreach (string i in ClientXml)
                wtr.Write32UTF(i);

            wtr.Write((ushort) ExtraXml.Length);
            foreach (string i in ExtraXml)
                wtr.Write32UTF(i);
        }
    }
}