#region

using wServer.networking;

#endregion

namespace wServer.realm.worlds
{
    public class LairofDraconis : World
    {
        public LairofDraconis()
        {
            Name = "Lair of Draconis";
            ClientWorldName = "dungeons.Lair_of_Draconis";
            Background = 0;
            Difficulty = 5;
            AllowTeleport = true;
        }

        protected override void Init()
        {
            LoadMap("wServer.realm.worlds.maps.LoD.jm", MapType.Json);
        }

        public override World GetInstance(Client psr)
        {
            return Manager.AddWorld(new LairofDraconis());
        }
    }
}