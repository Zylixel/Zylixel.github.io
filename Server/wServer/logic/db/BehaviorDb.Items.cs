#region

using wServer.logic.loot;
using wServer.logic.transitions;
using wServer.logic.behaviors;

#endregion
// by Fade
namespace wServer.logic
{
    partial class BehaviorDb
    {
        private _ Items = () => Behav()
            .Init("Marble Pillar",
                new State(
                    new State("Idle",
                        new AoeEffect(3, 500, ConditionEffectIndex.Armored),
                        new AoeEffect(3, 500, ConditionEffectIndex.Damaging),
                        new TimedTransition(4500, "die")
                    ),
                    new State("die",
                        new Suicide()
                        )
                )
            );
    }
}