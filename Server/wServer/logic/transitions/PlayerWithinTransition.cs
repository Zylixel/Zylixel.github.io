#region

using wServer.realm;

#endregion

namespace wServer.logic.transitions
{
    public class PlayerWithinTransition : Transition
    {
        //State storage: none

        private readonly double _dist;

        public PlayerWithinTransition(double dist, string targetState)
            : base(targetState)
        {
            _dist = dist;
        }

        protected override bool TickCore(Entity host, RealmTime time, ref object state)
        {
            return host.GetNearestEntity(_dist, null) != null;
        }
    }
}