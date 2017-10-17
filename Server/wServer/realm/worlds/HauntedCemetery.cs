namespace wServer.realm.worlds
{
    public class HauntedCemetery : World
    {
        public HauntedCemetery()
        {
            Name = "Haunted Cemetery";
            ClientWorldName = "Haunted Cemetery";
            Background = 0;
            Difficulty = 2;
            AllowTeleport = true;
        }
        public override bool NeedsPortalKey => true;

        protected override void Init()
        {
            LoadMap("wServer.realm.worlds.maps.ceme1.jm", MapType.Json);
        }
    }
}
