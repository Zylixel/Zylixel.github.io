#region

using wServer.logic.transitions;
using wServer.logic.behaviors;

#endregion

namespace wServer.logic
{
    partial class BehaviorDb
    {
        private _ Items = () => Behav()
            .Init("Marble Pillar",
                new State(
                    new State("Idle",
                        new AoeEffect(3, 1, ConditionEffectIndex.Armored),
                        new AoeEffect(3, 1, ConditionEffectIndex.Damaging),
                        new TimedTransition(4500, "die")
                    ),
                    new State("die",
                        new Suicide()
                        )
                )
            )
        .Init("EH Ability Bee 1",
                new State(
                    new State("start",
                         new BeesAttack(),
                         new PlayerOrbit(0.9, 1.2),
                         new TimedTransition(6000, "die")
                    ),
                    new State("die",
                        new Suicide()
                    )
                )
            )
        .Init("EH Ability Bee 2",
                new State(
                    new State("start",
                         new BeesRandom(),
                         new TimedTransition(1, "init")
                    ),
                    new State("init",
                        new PlayerOrbit(0.9, 1.2),
                        new BeesAttackCurse(),
                        new TimedTransition(5700, "die")
                    ),
                    new State("die",
                        new Suicide()
                    )
                )
            )
            .Init("EH Ability Bee 3",
                new State(
                    new State("start",
                         new PlayerOrbit(0.9, 1.2),
                         new BeesAttack(),
                         new TimedTransition(6000, "die")
                    ),
                    new State("die",
                        new Suicide()
                    )
                )
            );
    }
}