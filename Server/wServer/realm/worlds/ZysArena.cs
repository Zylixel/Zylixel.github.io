using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wServer.realm.worlds
{
    public class ZylixelsArena : World
    {
        public ZylixelsArena()
        {
            Name = "Zylixel's Arena";
            ClientWorldName = "Zylixel's Arena";
            Background = 0;
            Difficulty = 5;
            AllowTeleport = true;
        }

        protected override void Init()
        {
            LoadMap("wServer.realm.worlds.maps.ZysArena.jm", MapType.Json);
        }
    }
}