#region

using wServer.networking;

#endregion

namespace wServer.realm.worlds
{
    public class EpicUndeadLair : World
    {
        public EpicUndeadLair()
        {
            Name = "Epic Undead Lair";
            ClientWorldName = "Epic Undead Lair";
            Dungeon = true;
            Background = 0;
            AllowTeleport = false;
        }

        protected override void Init()
        {
            LoadMap("wServer.realm.worlds.maps.EpicUDL.jm", MapType.Json);
        }

        public override World GetInstance(Client psr)
        {
            return Manager.AddWorld(new EpicUndeadLair());
        }
    }
}