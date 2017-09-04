using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wServer.realm.worlds
{
    public class BilgewatersGrotto : World
    {
        public BilgewatersGrotto()
        {
            Name = "Bilgewater's Grotto";
            ClientWorldName = "Bilgewater's Grotto";
            Background = 0;
            Difficulty = 5;
            AllowTeleport = true;
        }

        public override bool NeedsPortalKey => true;

        protected override void Init()
        {
            LoadMap("wServer.realm.worlds.maps.bgrotto.jm", MapType.Json);
        }
    }
}