using wServer.networking;

namespace wServer.realm.worlds
{
    public class ManoroftheImmortals : World
    {
        public ManoroftheImmortals()
        {
            Name = "Manor of the Immortals";
            ClientWorldName = "Manor of the Immortals";
            Background = 0;
            AllowTeleport = true;
        }

        protected override void Init()
        {
            LoadMap("wServer.realm.worlds.maps.manor.wmap", MapType.Wmap);
        }

        public override World GetInstance(Client psr)
        {
            return Manager.AddWorld(new ManoroftheImmortals());
        }
    }
}
