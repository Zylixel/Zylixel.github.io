#region

using wServer.logic.behaviors;
using wServer.logic.loot;
using wServer.logic.transitions;

#endregion

namespace wServer.logic
{
    partial class BehaviorDb
    {
        private _ Missing = () => Behav()
            .Init("Abomination of Oryx",
                new State(
                    new Follow(0.7, range: 0.7),
                    new State("Shoot",
                    new Shoot(1, 2, 2, projectileIndex: 0, coolDown: 1200),
                    new Shoot(2, 4, 4, projectileIndex: 1, coolDown: 1200),
                    new Shoot(3, 6, 6, projectileIndex: 2, coolDown: 1200),
                    new Shoot(4, 7, 7, projectileIndex: 3, coolDown: 1200),
                    new Shoot(5, 8, 8, projectileIndex: 4, coolDown: 1200),
                    new TimedTransition(100, "Pause")
                    ),
                new State("Pause",
                    new TimedTransition(300, "Shoot")
                    )
                    )
            )
        .Init("Monstrosity of Oryx",
            new State(
                new Wander(0.3),
                new TossObject("Monstrosity Scarab", coolDown: 4000, randomToss: true)
                )
            )
        //        .Init("Bile of Oryx",
        //            new State(
        //                new Wander(0.2),
        //                new Spawn("Purple Goo", coolDown: 300, maxChildren: 7) //BUGGED
        //                )
        //            )
        .Init("Vintner of Oryx",
            new State(
                new State("Shoot",
                new Wander(0.2),
                new Shoot(8, 1, projectileIndex: 0, coolDown: 60),
                new Shoot(8, 1, projectileIndex: 0, coolDown: 60, coolDownOffset: 40),
                new Shoot(8, 1, projectileIndex: 0, coolDown: 60, coolDownOffset: 60),
                new TimedTransition(60, "Pause")
                ),
            new State("Pause",
                new TimedTransition(500, "Shoot")
                ))
            )
        .Init("Aberrant of Oryx",
            new State(
                new Wander(0.1),
                new TossObject("Aberrant Blaster", coolDown: 5000, randomToss: true)
                )
            )
        .Init("Aberrant Blaster",
            new State(
                new ConditionalEffect(ConditionEffectIndex.Invincible, true),
                new State("Shoot",
                    new Shoot(5, 9, 3, projectileIndex: 0, coolDown: 5000),
                    new TimedTransition(1000, "Suicide")
                    ),
                new State("Suicide",
                    new Suicide()
                    )
                )
            )
        .Init("Assassin of Oryx",
            new State(
                new Follow(0.5, range: 2),
                new Shoot(3, 4, projectileIndex: 1, coolDown: 600),
                new Shoot(5, 3, 5, projectileIndex: 0, coolDown: 200)
                )
            )
                ;
    }
}