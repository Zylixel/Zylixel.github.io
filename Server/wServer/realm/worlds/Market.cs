namespace wServer.realm.worlds
{
    public class Market : World
    {
        public Market()
        {
            Id = FMARKET;
            Name = "Market";
            ClientWorldName = "Player Market";
            Background = 2;
            AllowTeleport = true;
            Difficulty = -1;
        }

        protected override void Init()
        {
            LoadMap("wServer.realm.worlds.maps.Market.jm", MapType.Json);
        }
    }
}