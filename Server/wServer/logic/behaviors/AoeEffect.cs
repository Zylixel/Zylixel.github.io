using System.Linq;
using wServer.realm;
using wServer.realm.entities.player;

namespace wServer.logic.behaviors
{
    public class AoeEffect : Behavior
    {
        //State storage: nothing

        private readonly float radius;
        private readonly int duration;
        private readonly ConditionEffectIndex effect;

        public AoeEffect(double radius, int duration, ConditionEffectIndex effect)
        {
            this.radius = (float)radius;
            this.effect = effect;
            this.duration = duration;
        }

        protected override void TickCore(Entity host, RealmTime time, ref object state)
        {
            foreach (Player i in host.Owner.PlayersCollision.HitTest(host.X, host.Y, radius).OfType<Player>())
            {
                i.ApplyConditionEffect(new ConditionEffect
                {
                    Effect = effect,
                    DurationMS = duration
                });
            }
        }
    }
}
