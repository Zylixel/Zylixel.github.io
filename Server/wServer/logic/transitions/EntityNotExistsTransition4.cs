#region

using wServer.realm;

#endregion

namespace wServer.logic.transitions
{
    public class EntityNotExistsTransition4 : Transition
    {
        //State storage: none

        private readonly double dist;
        private readonly ushort target;
        private readonly ushort target2;
        private readonly ushort target3;
        private readonly ushort target4;


        public EntityNotExistsTransition4(string target, string target2, string target3, string target4, double dist, string targetState)
            : base(targetState)
        {
            this.dist = dist;
            this.target = BehaviorDb.InitGameData.IdToObjectType[target];
            this.target2 = BehaviorDb.InitGameData.IdToObjectType[target2];
            this.target3 = BehaviorDb.InitGameData.IdToObjectType[target3];
            this.target4 = BehaviorDb.InitGameData.IdToObjectType[target4];

        }

        protected override bool TickCore(Entity host, RealmTime time, ref object state)
        {
            if (host.GetNearestEntity(dist, target) == null && host.GetNearestEntity(dist, target2) == null)
            {
                return true;
            }
            return false;
        }
    }
}