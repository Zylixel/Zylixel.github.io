using wServer.realm;

namespace wServer.logic.transitions
{
    public class EntitiesNotExistsTransition : Transition
    {
        private readonly double _dist;
        private readonly string[] _childrens;

        public EntitiesNotExistsTransition(double dist, string targetState, params string[] childrens)
            : base(targetState)
        {
            _dist = dist;
            _childrens = childrens;
        }

        protected override bool TickCore(Entity host, RealmTime time, ref object state)
        {
            foreach (string children in _childrens)
                if (host.GetNearestEntity(_dist, host.Manager.GameData.IdToObjectType[children]) != null) return false;
            return true;
        }
    }
}
