#region

using wServer.realm;

#endregion

namespace wServer.logic.transitions
{
    public class NoPlayerWithinTransition : Transition
    {
        //State storage: none

        private readonly double _dist;

        public NoPlayerWithinTransition(double dist, string targetState)
            : base(targetState)
        {
            _dist = dist;
        }

        protected override bool TickCore(Entity host, RealmTime time, ref object state)
        {
            return host.GetNearestEntity(_dist, null) == null;
        }
    }
}