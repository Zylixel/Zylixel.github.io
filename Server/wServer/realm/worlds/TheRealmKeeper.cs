namespace wServer.realm.worlds
{
    public class TheOtherSide : World
    {
        public TheOtherSide()
        {
            Name = "The Other Side";
            ClientWorldName = "The Other Side";
            Background = 0;
            Difficulty = 5;
            AllowTeleport = false;
        }

        protected override void Init()
        {
            LoadMap("wServer.realm.worlds.maps.TheRealmKeeper.jm", MapType.Json);
        }
    }
}
