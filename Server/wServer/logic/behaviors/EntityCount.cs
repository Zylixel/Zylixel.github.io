using System.Linq;
using wServer.realm;

namespace wServer.logic.behaviors
{
    internal class EntityCountLessThan : ConditionalBehavior
    {
        //State storage: timer

        private readonly ushort target;
        private readonly int amount;
        private readonly double dist;

        public EntityCountLessThan(string target, double dist, int amount)
        {
            this.target = BehaviorDb.InitGameData.IdToObjectType[target];
            this.dist = dist;
            this.amount = amount;
        }

        protected override void TickCore(Entity host, RealmTime time, ref object state)
        {
            Result = host.GetNearestEntities(dist, target).Count() < amount;
        }
    }

    internal class EntityCountGreaterThan : ConditionalBehavior
    {
        //State storage: timer

        private readonly ushort target;
        private readonly int amount;
        private readonly double dist;

        public EntityCountGreaterThan(string target, double dist, int amount)
        {
            this.target = BehaviorDb.InitGameData.IdToObjectType[target];
            this.dist = dist;
            this.amount = amount;
        }

        protected override void TickCore(Entity host, RealmTime time, ref object state)
        {
            Result = host.GetNearestEntities(dist, target).Count() > amount;
        }

    }
    internal class EntityCountEqual : ConditionalBehavior
    {
        //State storage: timer

        private readonly ushort target;
        private readonly int amount;
        private readonly double dist;

        public EntityCountEqual(string target, double dist, int amount)
        {
            this.target = BehaviorDb.InitGameData.IdToObjectType[target];
            this.dist = dist;
            this.amount = amount;
        }

        protected override void TickCore(Entity host, RealmTime time, ref object state)
        {
            Result = host.GetNearestEntities(dist, target).Count() == amount;
        }
    }
}

//Not Gonna Lie, Decided to take this from the LRv2 Source. I mean, why spend time on this when I can dedicate my resources elsewhere.
//Devwarlt, If you want me to take this out, I will do it without complaints