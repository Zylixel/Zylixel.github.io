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
        }
    }
}