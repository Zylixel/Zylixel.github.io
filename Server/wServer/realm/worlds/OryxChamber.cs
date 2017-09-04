#region

using wServer.networking;

#endregion

namespace wServer.realm.worlds
{
    public class OryxChamber : World
    {
        public OryxChamber()
        {
            Name = "Oryx's Chamber";
            ClientWorldName = "Oryx's Chamber";
            Background = 4;
            AllowTeleport = false;
        }

        public override bool NeedsPortalKey => true;

        protected override void Init()
        {
            LoadMap("wServer.realm.worlds.maps.oryxchamber.jm", MapType.Json);
        }

        public override World GetInstance(Client psr)
        {
            return Manager.AddWorld(new OryxChamber());
        }
    }
}