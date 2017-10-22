#region

using System;
using wServer.realm;

#endregion

namespace wServer.logic.behaviors
{
    public class RemoveEffect : Behavior
    {
        private readonly ConditionEffectIndex effect;

        public RemoveEffect(ConditionEffectIndex effect)
        {
            this.effect = effect;
        }

        protected override void OnStateEntry(Entity host, RealmTime time, ref object state) => host.ApplyConditionEffect(new ConditionEffect
        {
            Effect = effect,
            DurationMS = 0
        });


        protected override void OnStateExit(Entity host, RealmTime time, ref object state)
        {
        }

        protected override void TickCore(Entity host, RealmTime time, ref object state)
        {
        }
    }
}