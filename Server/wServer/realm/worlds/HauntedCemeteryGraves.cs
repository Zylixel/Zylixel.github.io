using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wServer.realm.worlds
{
    public class HauntedCemeteryGraves : World
    {
        public HauntedCemeteryGraves()
        {
            Name = "Haunted Cemetery Graves";
            ClientWorldName = "Haunted Cemetery Graves";
            Background = 0;
            Difficulty = 3;
            AllowTeleport = true;
        }

        public override bool NeedsPortalKey => true;

        protected override void Init()
        {
            LoadMap("wServer.realm.worlds.maps.ceme3.jm", MapType.Json);
        }
    }
}
