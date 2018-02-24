using wServer.realm;

namespace wServer.logic.behaviors
{
    internal class BeesRandom : CycleBehavior
    {
        protected override void TickCore(Entity host, RealmTime time, ref object state)
        {
            if (MathsUtils.GenerateProb(50))
            {
                host.Owner.LeaveWorld(host);
            }
        }
    }
}