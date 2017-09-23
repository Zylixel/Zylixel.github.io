namespace wServer.realm.worlds
{
    public class WoodlandLabyrinth : World
    {
        public WoodlandLabyrinth()
        {
            Name = "Woodland Labyrinth";
            ClientWorldName = "Woodland Labyrinth";
            Background = 0;
            Dungeon = true;
            Difficulty = 5;
            AllowTeleport = true;
        }

        public override bool NeedsPortalKey => true;

        protected override void Init()
        {
            LoadMap("wServer.realm.worlds.maps.woodlandlabyrinth.jm", MapType.Json);
        }
    }
}