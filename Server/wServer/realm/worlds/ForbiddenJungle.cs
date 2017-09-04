using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wServer.realm.worlds
{
    public class ForbiddenJungle : World
    {
        public ForbiddenJungle()
        {
            Name = "Forbidden Jungle";
            ClientWorldName = "Forbidden Jungle";
            Background = 0;
            Difficulty = 2;
            AllowTeleport = true;
        }

        protected override void Init()
        {
            LoadMap("wServer.realm.worlds.maps.jungles.jm", MapType.Json);
        }
    }
}