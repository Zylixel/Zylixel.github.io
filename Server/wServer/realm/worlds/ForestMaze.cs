namespace wServer.realm.worlds
{
    public class ForestMaze : World
    {
        public ForestMaze()
        {
            Name = "Forest Maze";
            ClientWorldName = "dungeons.Forest_Maze";
            Background = 0;
            Difficulty = 1;
            AllowTeleport = true;
        }

        protected override void Init()
        {
            LoadMap("wServer.realm.worlds.maps.forestmaze.jm", MapType.Json);
        }
    }
}
