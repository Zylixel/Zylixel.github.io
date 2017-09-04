using Mono.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wServer.realm;
using wServer.realm.entities.player;

namespace wServer.logic.behaviors.Drakes
{
    internal class BeesRandom : CycleBehavior
    {
        protected override void TickCore(Entity host, RealmTime time, ref object state)
        {
            Player player = host.GetPlayerOwner();
            if (MathsUtils.GenerateProb(50))
            {
                host.Owner.LeaveWorld(host);
            }
            return;
        }
    }
}