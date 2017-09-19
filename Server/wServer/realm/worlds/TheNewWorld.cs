using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wServer.realm.worlds
{
    public class TheNewWorld : World
    {
        public TheNewWorld()
        {
            Name = "The New World";
            ClientWorldName = "The New World";
            Background = 0;
            Difficulty = 4;
            AllowTeleport = true;
        }

        protected override void Init()
        {
            LoadMap("wServer.realm.worlds.maps.TheNewWorld.jm", MapType.Json);
        }
    }
}
