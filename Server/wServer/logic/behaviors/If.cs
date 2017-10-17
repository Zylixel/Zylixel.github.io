using wServer.realm;

namespace wServer.logic.behaviors
{
    internal class If : CycleBehavior
    {
        //State storage: time

        private readonly ConditionalBehavior condition;
        private readonly Behavior[] behaviors;

        public If(ConditionalBehavior condition, params Behavior[] behaviors)
        {
            this.condition = condition;
            this.behaviors = behaviors;
        }

        protected override void TickCore(Entity host, RealmTime time, ref object state)
        {
            condition.Tick(host, time);
            if (condition.Result)
                foreach (var i in behaviors)
                    i.Tick(host, time);
        }
    }
}

//Not Gonna Lie, Decided to take this from the LRv2 Source. I mean, why spend time on this when I can dedicate my resources elsewhere.
//Devwarlt, If you want me to take this out, I will do it without complaints