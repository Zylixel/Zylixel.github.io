using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wServer.realm.worlds
{
    public class IceChamber : World
    {
        public IceChamber()
        {
            Name = "Ice Chamber";
            ClientWorldName = "Ice Chamber";
            Background = 0;
            Difficulty = 5;
            AllowTeleport = false;
        }

        public override bool NeedsPortalKey => false;

        protected override void Init()
        {
            LoadMap("wServer.realm.worlds.maps.icechamber.jm", MapType.Json);
        }
    }
}
