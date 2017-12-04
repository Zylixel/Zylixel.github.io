#region

using System.Collections.Generic;
using wServer.realm.entities.player;

#endregion

namespace wServer.realm.worlds
{
    public class Nexus : World
    {
        public const string Resource = "wServer.realm.worlds.maps.GodsNexus.jm";

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

            CheckDupers();
            UpdatePortals();
        }

        private void CheckDupers()
        {
            foreach (KeyValuePair<int, World> w in Manager.Worlds)
            {
                foreach (KeyValuePair<int, World> x in Manager.Worlds)
                {
                    foreach (KeyValuePair<int, Player> y in w.Value.Players)
                    {
                        foreach (KeyValuePair<int, Player> z in x.Value.Players)
                        {
                            if (y.Value.AccountId == z.Value.AccountId && y.Value != z.Value)
                            {
                                y.Value.Client.Disconnect();
                                z.Value.Client.Disconnect();
                            }
                        }
                    }
                }
            }
        }

        private void UpdatePortals()
        {
            foreach (var i in Manager.Monitor.portals)
            {
                foreach (var j in RealmManager.CurrentRealmNames)
                {
                    if (i.Value.Name.StartsWith(j))
                    {
                        if (i.Value.Name == j) i.Value.PortalName = i.Value.Name;
                        i.Value.Name = j + " (" + i.Key.Players.Count + "/" + RealmManager.MAX_REALM_PLAYERS + ")";
                        i.Value.UpdateCount++;
                        break;
                    }
                }
            }
        }
    }
}