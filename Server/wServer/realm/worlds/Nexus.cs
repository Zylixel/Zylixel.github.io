#region

using System.Collections.Generic;
using wServer.realm.entities.player;

#endregion

namespace wServer.realm.worlds
{
    public class Nexus : World
    {
        public const string Resource = "wServer.realm.worlds.maps.GodsNexusNoMerchant.jm";

        public Nexus()
        {
            Id = NEXUS_ID;
            Name = "Nexus";
            ClientWorldName = "server.nexus";
            Background = 2;
            AllowTeleport = true;
            Difficulty = -1;
        }

        protected override void Init()
        {
            LoadMap(Resource, MapType.Json);
        }

        public override void Tick(RealmTime time)
        {
            base.Tick(time); //normal world tick
            UpdatePortals();
        }

        private void UpdatePortals()
        {
            foreach (var i in Manager.Monitor.portals)
            {
                foreach (var j in RealmManager.CurrentPortalNames)
                {
                    if (i.Value.Name.StartsWith(j))
                    {
                        if (i.Value.Name == j) i.Value.PortalName = i.Value.Name;
                        i.Value.Name = j + " (" + i.Key.Players.Count + ")";
                        i.Value.UpdateCount++;
                        break;
                    }
                }
            }
        }
    }
}