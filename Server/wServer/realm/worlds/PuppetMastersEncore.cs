using wServer.networking;

namespace wServer.realm.worlds
{
    public class PuppetMastersEncore : World
    {
        public PuppetMastersEncore()
        {
            Name = "Puppet Master's Encore";
            ClientWorldName = "Puppet Master's Encore";
            Background = 0;
            AllowTeleport = false;
            Difficulty = 5;
        }

        protected override void Init()
        {
            LoadMap("wServer.realm.worlds.maps.encore.jm", MapType.Json);
        }

        public override World GetInstance(Client psr)
        {
            return Manager.AddWorld(new ManoroftheImmortals());
        }
    }
}
