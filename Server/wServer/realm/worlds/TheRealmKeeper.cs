using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wServer.realm.worlds
{
    public class TheRealmKeeper : World
    {
        public TheRealmKeeper()
        {
            Name = "The Other Side";
            ClientWorldName = "The Other Side ";
            Background = 0;
            Difficulty = 4;
            AllowTeleport = false;
        }

        protected override void Init()
        {
            LoadMap("wServer.realm.worlds.maps.TheRealmKeeper.jm", MapType.Json);
        }
    }
}
