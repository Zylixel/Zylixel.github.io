using System;
using wServer.logic.behaviors;
using wServer.logic.loot;
using wServer.logic.transitions;

namespace wServer.logic
{
    partial class BehaviorDb
    {
        private _ GrandSphinx = () => Behav()
            .Init("Grand Sphinx",
                    new State(
                        new HpLessOrder(50, 0.15, "Horrid Reaper", "Go away"),
                        new DropPortalOnDeath("Tomb of the Ancients Portal", 35),
                        new Spawn("Horrid Reaper", 8, 1),
                        new State("BlindAttack",
                            new Wander(0.0005),
                            new StayCloseToSpawn(0.5, 8),
                            new Taunt("You hide like cowards... but you can't hide from this!"),
                            new State("1",
                                new Shoot(10, projectileIndex: 1, count: 10, shootAngle: 10, fixedAngle: 0),
                                new Shoot(10, projectileIndex: 1, count: 10, shootAngle: 10, fixedAngle: 180),
                                new TimedTransition(1500, "2")
                            ),
                            new State("2",
                                new Shoot(10, projectileIndex: 1, count: 10, shootAngle: 10, fixedAngle: 270),
                                new Shoot(10, projectileIndex: 1, count: 10, shootAngle: 10, fixedAngle: 90),
                                new TimedTransition(1500, "1")
                            ),
                            new TimedTransition(10000, "Ilde")
                        ),
                        new State("Ilde",
                            new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                            new Flash(0xff00ff00, 0.01, 50),
                            new TimedTransition(2000, "ArmorBreakAttack")
                        ),
                        new State("ArmorBreakAttack",
                            new Wander(0.0005),
                            new StayCloseToSpawn(0.5, 8),
                            new Shoot(0, projectileIndex: 2, count: 8, shootAngle: 5, coolDown: 300),
                            new TimedTransition(10000, "Ilde2")
                        ),
                        new State("Ilde2",
                            new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                            new Flash(0xff00ff00, 0.01, 50),
                            new TimedTransition(2000, "WeakenAttack")
                        ),
                        new State("WeakenAttack",
                            new Wander(0.0005),
                            new StayCloseToSpawn(0.5, 8),
                            new Shoot(10, projectileIndex: 0, count: 3, shootAngle: 120, coolDown: 900),
                            new Shoot(10, projectileIndex: 2, count: 3, shootAngle: 15, fixedAngle: 40, coolDown: 1600),
                            new Shoot(10, projectileIndex: 2, count: 3, shootAngle: 15, fixedAngle: 220, coolDown: 1600),
                            new Shoot(10, projectileIndex: 2, count: 3, shootAngle: 15, fixedAngle: 130, coolDown: 1600, coolDownOffset: 800),
                            new Shoot(10, projectileIndex: 2, count: 3, shootAngle: 15, fixedAngle: 310, coolDown: 1600, coolDownOffset: 800),
                            new TimedTransition(10000, "Ilde3")
                        ),
                        new State("Ilde3",
                            new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                            new Flash(0xff00ff00, 0.01, 50),
                            new TimedTransition(2000, "BlindAttack")
                        )
                    ),
                    new MostDamagers(3,
                        new ItemLoot("Potion of Vitality", 1),
                        new ItemLoot("Potion of Wisdom", 1)
                    ),
                    new Threshold(0.05,
                        new ItemLoot("Helm of the Juggernaut", 0.005)
                    ),
                    new Threshold(0.1,
                        new OnlyOne(
                            LootTemplates.DefaultEggLoot(EggRarity.Legendary)
                        )
                    )
                )
                .Init("Horrid Reaper",
                    new State(
                        new EntityNotExistsTransition("Grand Sphinx", 50, "Go away"),
                        new Shoot(25, shootAngle: 10 * (float)Math.PI / 180, count: 1, projectileIndex: 0, coolDown: 1000),
                        new State("Idle",
                            new StayCloseToSpawn(0.5, 15),
                            new Wander(0.5),
                            new ConditionalEffect(ConditionEffectIndex.Invulnerable, true)
                        ),
                        new State("Go away",
                            new TimedTransition(5000, "I AM OUT"),
                            new Taunt("OOaoaoAaAoaAAOOAoaaoooaa!!!")
                        ),
                        new State("I AM OUT",
                            new Suicide()
                        )
                    )
                );
    }
}
