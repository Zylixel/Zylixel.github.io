#region

using wServer.realm;

#endregion

namespace wServer.logic.transitions
{
    public class EntityNotExistsTransition : Transition
    {
        //State storage: none

        private readonly double _dist;
        private readonly ushort _target;

        public EntityNotExistsTransition(string target, double dist, string targetState)
            : base(targetState)
        {
            _dist = dist;
            _target = BehaviorDb.InitGameData.IdToObjectType[target];
        }

        protected override bool TickCore(Entity host, RealmTime time, ref object state)
        {
            return host.GetNearestEntity(_dist, _target) == null;
        }
    }
}