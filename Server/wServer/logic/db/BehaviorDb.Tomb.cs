#region

using wServer.logic.behaviors;
using wServer.logic.loot;
using wServer.logic.transitions;
#endregion

namespace wServer.logic
{
    partial class BehaviorDb
    {
        private _ Tomb = () => Behav()
            .Init("Tomb Defender",
                new State(
                    new State("idle",
                        new Taunt("THIS WILL NOW BE YOUR TOMB!"),
                        new ConditionalEffect(ConditionEffectIndex.Armored),
                        new Orbit(.6, 5, target: "Tomb Boss Anchor", radiusVariance: 0.5),
                        new HpLessTransition(.99, "weakning")
                        ),
                    new State("weakning",
                        new Orbit(.6, 5, target: "Tomb Boss Anchor", radiusVariance: 0.5),
                        new Taunt("Impudence! I am an immortal, I needn't take you seriously."),
                        new Shoot(50, 24, projectileIndex: 3, coolDown: 6000, coolDownOffset: 2000),
                        new HpLessTransition(.97, "active")
                        ),
                    new State("active",
                        new Orbit(.7, 5, target: "Tomb Boss Anchor", radiusVariance: 0.5),
                        new Shoot(50, 8, projectileIndex: 2, coolDown: 1000, coolDownOffset: 500),
                        new Shoot(50, 4, projectileIndex: 1, coolDown: 3000, coolDownOffset: 500),
                        new Shoot(50, 6, projectileIndex: 0, coolDown: 3100, coolDownOffset: 500),
                        new HpLessTransition(.9, "boomerang")
                        ),
                    new State("boomerang",
                         new Taunt(0.75, "The others use tricks, but I shall stun you with my brute strength!"),
                        new ConditionalEffect(ConditionEffectIndex.Armored),
                        new Orbit(.6, 5, target: "Tomb Boss Anchor", radiusVariance: 0.5),
                        new Shoot(50, 8, projectileIndex: 2, coolDown: 1000, coolDownOffset: 500),
                        new Shoot(50, 8, 10, 1, coolDown: 4750, coolDownOffset: 500),
                        new Shoot(50, 1, 10, 0, coolDown: 4750, coolDownOffset: 500),
                        new HpLessTransition(.66, "double shot")
                        ),
                    new State("double shot",
                        new ConditionalEffect(ConditionEffectIndex.Armored),
                        new Orbit(.7, 5, target: "Tomb Boss Anchor", radiusVariance: 0.5),
                        new Shoot(50, 8, projectileIndex: 2, coolDown: 1000, coolDownOffset: 500),
                        new Shoot(50, 8, 10, 1, coolDown: 4750, coolDownOffset: 500),
                        new Shoot(50, 2, 10, 0, coolDown: 4750, coolDownOffset: 500),
                        new HpLessTransition(.5, "artifacts")
                        ),
                    new State("triple shot",
                        new ConditionalEffect(ConditionEffectIndex.Armored),
                        new Orbit(.6, 5, target: "Tomb Boss Anchor", radiusVariance: 0.5),
                        new Shoot(50, 8, projectileIndex: 2, coolDown: 1000, coolDownOffset: 500),
                        new Shoot(50, 8, 10, 1, coolDown: 4750, coolDownOffset: 500),
                        new Shoot(50, 3, 10, 0, coolDown: 4750, coolDownOffset: 500),
                        new HpLessTransition(.1, "rage")
                        ),
                    new State("artifacts",
                        new Taunt(0.65, "Awe at my wondrous defense"),
                        new ConditionalEffect(ConditionEffectIndex.Armored),
                        new Taunt("My artifacts shall prove my wall of defense is impenetrable!"),
                        new Orbit(.6, 5, target: "Tomb Boss Anchor", radiusVariance: 0.5),
                        new Shoot(50, 8, projectileIndex: 2, coolDown: 1000, coolDownOffset: 500),
                        new Shoot(50, 8, 10, 1, coolDown: 4750, coolDownOffset: 500),
                        new Shoot(50, 2, 10, 0, coolDown: 4750, coolDownOffset: 500),
                        new Spawn("Pyramid Artifact 1", 3, 3, 2000),
                        new Reproduce("Pyramid Artifact 1", 10, 3, 1500),
                        new Spawn("Pyramid Artifact 2", 3, 0, 3500000),
                        new Reproduce("Pyramid Artifact 2", 10, 3, 1500),
                        new Spawn("Pyramid Artifact 3", 3, 0, 3500000),
                        new Reproduce("Pyramid Artifact 3", 10, 3, 1500),
                        new HpLessTransition(.33, "triple shot")
                        ),
                    new State("rage",
                        new Taunt("The end of your path is here!"),
                        new Follow(0.6, range: 1, duration: 5000, coolDown: 0),
                        new Flash(0xfFF0000, 1, 9000001),
                        new Shoot(50, 8, 10, 1, coolDown: 4750, coolDownOffset: 500),
                        new Shoot(50, 4, 10, 4, coolDown: 300),
                        new Shoot(50, 3, 10, 0, coolDown: 4750, coolDownOffset: 500)
                        )
                    ),
                    new Threshold(0.32,
                        new ItemLoot("Potion of Life", 2)
                    ),
                    new Threshold(0.1,
                        new ItemLoot("Ring of the Pyramid", 0.05),
                        new ItemLoot("Tome of Holy Protection", 0.025),
                        new ItemLoot("Wine Cellar Incantation", 0.045)
                    ),
                    new Threshold(0.2,
                        new EggLoot(EggRarity.Common, 0.1),
                        new EggLoot(EggRarity.Uncommon, 0.05),
                        new EggLoot(EggRarity.Rare, 0.01),
                        new EggLoot(EggRarity.Legendary, 0.002)
                    )
            )
              .Init("Pyramid Artifact 1",
              new State(
                 new Prioritize(
                    new Orbit(1, 3, 30, "Tomb Defender")
                 ),
                    new Shoot(4, count: 1, shootAngle: 12, coolDown: 1000)
                 )
                 )
                        .Init("Pyramid Artifact 2",
                                new State(
                                        new State("Normal",
                                                new Prioritize(
                                                        new Orbit(1, 3, 30, "Tomb Support")
                                                ),
                                                new Shoot(4, count: 1, shootAngle: 12, coolDown: 1000),
                                                new EntityNotExistsTransition("Tomb Support", 50, "Attacker Exist?")
                                        ),
                                        new State("Attacker Exist?",
                                                new Prioritize(
                                                        new Orbit(1, 3, 30, "Tomb Attacker")
                                                ),
                                                new Shoot(4, count: 1, shootAngle: 12, coolDown: 1000),
                                                new EntityNotExistsTransition("Tomb Attacker", 50, "Suicide")
                                        ),
                                        new State("Suicide",
                                                new Suicide()
                                        )
                                )
                        )
           .Init("Pyramid Artifact 3",
              new State(
                                        new State("Normal",
                                new Prioritize(
                             new Orbit(1, 3, 30, "Tomb Attacker")
                            ),
                                 new Shoot(4, count: 1, shootAngle: 12, coolDown: 1000),
                                                new EntityNotExistsTransition("Tomb Attacker", 50, "Support Exist?")
                                        ),
                                        new State("Support Exist?",
                                                new Prioritize(
                                                        new Orbit(1, 3, 30, "Tomb Support")
                                                ),
                                                new Shoot(4, count: 1, shootAngle: 12, coolDown: 1000),
                                                new EntityNotExistsTransition("Tomb Support", 50, "Suicide")
                                        ),
                                                new State("Suicide",
                                                new Suicide()
                 )
                                )
                        )
           .Init("Tomb Defender Statue",
              new State(
                 new State("Active",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new EntityNotExistsTransition("Active Sarcophagus", 50000, "Start")
                    ),
                 new State("Start",
                    new Spawn("Tomb Defender", 1),
                    new Suicide()
                            )
                    )
                    )
                        .Init("Tomb Support Statue",
                                new State(
                                        new State("Active",
                                                new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                                                new EntityNotExistsTransition("Active Sarcophagus", 50000, "Start")
                                        ),
                                        new State("Start",
                                                new Spawn("Tomb Support", 1),
                                                new Suicide()
                                        )
                                        )
                                        )
                        .Init("Tomb Attacker Statue",
                                new State(
                                        new State("Active",
                                                new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                                                new EntityNotExistsTransition("Active Sarcophagus", 50000, "Start")
                                        ),
                                        new State("Start",
                                                new Spawn("Tomb Attacker", 1),
                                                new Suicide()
                                        )
                                        )
                                        )
           .Init("Active Sarcophagus",
              new State(
                 new State("Idle",
                    new HpLessTransition(0.9, "Shoot")
                 ),
                 new State("Shoot",
                   new Shoot(24, count: 12, shootAngle: 360 / 12, coolDown: 20000)
                 )
                 ),
                new Threshold(0.01,
                 new ItemLoot("Tincture of Life", 0.2),
                 new ItemLoot("Tincture of Mana", 0.2),
                 new ItemLoot("Tincture of Defense", 0.2),
                 new ItemLoot("Tincture of Dexterity", 0.2)
                 )
                        )
           .Init("Beam Priestess",
                                new State(
                                        new State("State1",
                                                new Prioritize(
                                                        new Orbit(0.6, 5, 10, "Active Sarcophagus")
                                                ),
                                                new Shoot(12, count: 1, shootAngle: 12, coolDown: 1100, projectileIndex: 0),
                                                new Shoot(14, count: 1, shootAngle: 12, coolDown: 2000, projectileIndex: 1),
                                                new Heal(10, "Support Beam", coolDown: 1000),
                                                new HpLessTransition(0.3, "State2"),
                                                new EntityNotExistsTransition("Active Sarcophagus", 10, "Free")
                                        ),
                                        new State("State2",
                                                new ConditionalEffect(ConditionEffectIndex.Armored),
                                                new Prioritize(
                                                        new Protect(0.6, "Active Sarcophagus", protectionRange: 0, reprotectRange: 0)
                                                ),
                                                new EntityExistsTransition("Active Sarcophagus", 1, "Heal")
                                        ),
                                        new State("Heal",
                                                new Flash(0xFF0000, 1, 100),
                                                new Heal(10, "Self", coolDown: 1000),
                                                new TimedTransition(5000, "State3")
                                        ),
                                        new State("State3",
                                                new ChangeSize(10, 120),
                                                new Prioritize(
                                                        new Orbit(1, 5, 10, "Active Sarcophagus")
                                                ),
                                                new Shoot(12, count: 1, shootAngle: 12, coolDown: 700, projectileIndex: 0),
                                                new Shoot(14, count: 1, shootAngle: 12, coolDown: 1400, projectileIndex: 1),
                                                new Heal(10, "Support Beam", coolDown: 1000),
                                                new HpLessTransition(0.1, "Explode")
                                        ),
                                        new State("Free",
                                                new Prioritize(
                                                        new Wander(0.6),
                                                        new StayCloseToSpawn(1.6, 10)
                                                ),
                                                new Shoot(12, count: 1, shootAngle: 12, coolDown: 1100, projectileIndex: 0),
                                                new Shoot(14, count: 1, shootAngle: 12, coolDown: 2000, projectileIndex: 1),
                                                new HpLessTransition(0.1, "Explode")
                                        ),
                                        new State("Explode",
                                                new Flash(0xFF0000, 1, 1000),
                                                new Shoot(12, count: 9, shootAngle: 360 / 9, coolDown: 1100, projectileIndex: 0),
                                                new Suicide()
                                        )
                                         ),
                new Threshold(0.01,
                new ItemLoot("Magic Potion", 0.2),
                new ItemLoot("Health Potion", 0.2),
                new ItemLoot("Geb's Ring of Wisdom", 0.02)
                           )
                           )
                        .Init("Beam Priest",
                                new State(
                                        new State("State1",
                                        new Prioritize(
                                                new Orbit(0.6, 3, 10, "Active Sarcophagus")
                                                ),
                                        new Shoot(12, count: 1, shootAngle: 12, coolDown: 1100, projectileIndex: 0),
                                        new Shoot(14, count: 1, shootAngle: 12, coolDown: 2000, projectileIndex: 1),
                                         new Heal(10, "Support Beam", coolDown: 1000),
                                        new HpLessTransition(0.3, "State2"),
                                        new EntityNotExistsTransition("Active Sarcophagus", 10, "Free")
                                                ),
                                                new State("State2",
                                                new ConditionalEffect(ConditionEffectIndex.Armored),
                                                new Prioritize(
                                                        new Protect(0.6, "Active Sarcophagus", protectionRange: 0, reprotectRange: 0)
                                                ),
                                                new EntityExistsTransition("Active Sarcophagus", 1, "Heal")
                                        ),
                                        new State("Heal",
                                                new Flash(0xFF0000, 1, 100),
                                                new Heal(10, "Self", coolDown: 1000),
                                                new TimedTransition(5000, "State3")
                                        ),
                                        new State("State3",
                                                new ChangeSize(10, 120),
                                                new Prioritize(
                                                        new Orbit(1, 3, 10, "Active Sarcophagus")
                                                ),
                                                new Shoot(12, count: 1, shootAngle: 12, coolDown: 700, projectileIndex: 0),
                                                new Shoot(14, count: 1, shootAngle: 12, coolDown: 1400, projectileIndex: 1),
                                                new Heal(10, "Support Beam", coolDown: 1000),
                                                new HpLessTransition(0.1, "Explode")
                                        ),
                                        new State("Free",
                                                new Prioritize(
                                                        new Wander(0.6),
                                                        new StayCloseToSpawn(1.6, 10)
                                                ),
                                                new Shoot(12, count: 1, shootAngle: 12, coolDown: 1100, projectileIndex: 0),
                                                new Shoot(14, count: 1, shootAngle: 12, coolDown: 2000, projectileIndex: 1),
                                                new HpLessTransition(0.1, "Explode")
                                        ),
                                        new State("Explode",
                                                new Flash(0xFF0000, 1, 1000),
                                                new Shoot(12, count: 9, shootAngle: 360 / 9, coolDown: 1100, projectileIndex: 0),
                                                new Suicide()
                                )
                                ),
                new Threshold(0.01,
                new ItemLoot("Magic Potion", 0.2),
                new ItemLoot("Health Potion", 0.2),
                new ItemLoot("Geb's Ring of Wisdom", 0.02)
                           )
                           )
                        .Init("Bloated Mummy",
                                new State(
                                        new State("Normal",
                                        new Prioritize(
                                                new Follow(2, 7, 4)
                                        ),

                                        new Spawn("Scarab", 5, 1, coolDown: 5000),
                                        new Shoot(14, count: 3, shootAngle: 12, projectileIndex: 0, coolDown: 1300),
                                        new PlayerWithinTransition(2, "Explode")
                                        ),
                                        new State("Explode",
                                            new ChangeSize(35, 150),
                                                new Flash(0xFF0000, .1, 10),
                                                new Shoot(14, count: 24, shootAngle: 360 / 36, projectileIndex: 1, coolDown: 4000),
                                                new TransformOnDeath("Scarab", 5, 7, 1),
                                                new Suicide()
                                        )
                                )
                        )
                        .Init("Scarab",
                                new State(
                                        new Prioritize(
                                                new Follow(2, 7, 1)
                                        ),
                                        new Shoot(1.4, count: 1, projectileIndex: 0, coolDown: 1950),
                                        new Shoot(1.4, count: 5, projectileIndex: 1, coolDown: 1525)
                                        )
                                        )
                        .Init("Canopic Jar 1",
                                new State(
                                                new TransformOnDeath("Scarab", 1, 1, 1)
                                                ),
                                        new ItemLoot("Magic Potion", 0.3),
                                        new ItemLoot("Health Potion", 0.3)
                                )
                        .Init("Canopic Jar 2",
                                new State(
                                        new TransformOnDeath("Scarab", 1, 1, 1)
                                ),
                                new ItemLoot("Magic Potion", 0.3),
                                new ItemLoot("Health Potion", 0.3)
                        )
                        .Init("Canopic Jar 3",
                                new State(
                                        new TransformOnDeath("Scarab", 1, 1, 1)
                                ),
                                new ItemLoot("Magic Potion", 0.3),
                                new ItemLoot("Health Potion", 0.3)
                        )
                        .Init("Canopic Jar 4",
                                new State(
                                        new TransformOnDeath("Scarab", 1, 1, 1)
                                ),
                                new ItemLoot("Magic Potion", 0.3),
                                new ItemLoot("Health Potion", 0.3)
                        )
                        .Init("Inactive Sarcophagus",
                                new State(
                                        new State("Waiting",
                                                new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                                                new PlayerWithinTransition(10, "Idle")
                                        ),
                                        new State("Idle",
                                                new Taunt(0.99, "Those who damage me may find great treasure,{PLAYER}, but the gods will not forget this insult!"),
                                                new HpLessTransition(0.99, "Spawning1")
                                        ),
                                        new State("Spawning1",
                                                new TossObject("Canopic Jar 1", 5, angle: 45, coolDown: 5000000),
                                                new TimedTransition(1000, "Spawning2")
                                        ),
                                        new State("Spawning2",
                                                new TossObject("Canopic Jar 2", 5, angle: 90, coolDown: 5000000),
                                                new TimedTransition(1000, "Spawning3")
                                        ),
                                        new State("Spawning3",
                                                new TossObject("Canopic Jar 3", 5, angle: 135, coolDown: 5000000),
                                                new TimedTransition(1000, "Spawning4")
                                        ),
                                        new State("Spawning4",
                                                new TossObject("Canopic Jar 4", 5, angle: 180, coolDown: 5000000),
                                                new TimedTransition(1000, "Spawning5")
                                        ),
                                        new State("Spawning5",
                                                new TossObject("Canopic Jar 1", 5, angle: 225, coolDown: 5000000),
                                                new TimedTransition(1000, "Spawning6")
                                        ),
                                        new State("Spawning6",
                                                new TossObject("Canopic Jar 2", 5, angle: 270, coolDown: 5000000),
                                                new TimedTransition(1000, "Spawning7")
                                        ),
                                        new State("Spawning7",
                                                new TossObject("Canopic Jar 3", 5, angle: 315, coolDown: 5000000),
                                                new TimedTransition(1000, "Spawning8")
                                        ),
                                        new State("Spawning8",
                                                new TossObject("Canopic Jar 4", 5, angle: 360, coolDown: 5000000)
                                        )
                                        ),
                                new Threshold(0.01,
                                new ItemLoot("Potion of Attack", 0.3),
                                new ItemLoot("Potion of Speed", 0.3),
                                new ItemLoot("Potion of Defense", 0.3),
                                new TierLoot(9, ItemType.Armor, .08),
                                new TierLoot(9, ItemType.Weapon, .08),
                                new TierLoot(10, ItemType.Armor, .06),
                                new TierLoot(10, ItemType.Weapon, .06),
                                new TierLoot(4, ItemType.Ring, .08)
                                                )
                        )
                        .Init("Center Tomb Frost Turret",
                                new State(
                                        new Shoot(7.2, count: 5, shootAngle: 360 / 5, coolDown: 300)
                                                )
                                                )
                        .Init("Tomb Frost Turret",
                                new State(
                                        new Shoot(11.2, count: 1, coolDown: 300)
                                                )
                                                )
                        .Init("Center Tomb Fire Turret",
                                new State(
                                        new Shoot(6.4, count: 3, shootAngle: 360 / 3, coolDown: 300)
                                                )
                                                )
                        .Init("Tomb Fire Turret",
                                new State(
                                        new Shoot(5, count: 1, coolDown: 300)
                                                )
                                                )
                        .Init("Center Tomb Thunder Turret",
                                new State(
                                        new Shoot(20, count: 4, shootAngle: 360 / 4, coolDown: 300)
                                                )
                                                )
                        .Init("Tomb Thunder Turret",
                                new State(
                                        new Shoot(12, count: 2, shootAngle: 360 / 2, coolDown: 300)
                                                )
                                                )
                        .Init("Eagle Sentry",
                                new State(
                                        new Prioritize(
                                                new Follow(0.8, 10, 1),
                                                new Wander(0.7)
                                                ),
                                        new Shoot(6.3, count: 1, projectileIndex: 0, coolDown: 1960),
                                        new Shoot(6.3, count: 1, projectileIndex: 1, coolDown: 1760),
                                         new Shoot(6.3, count: 6, shootAngle: 60, projectileIndex: 2, coolDown: 1950),
                                        new Shoot(6.3, count: 1, projectileIndex: 2, coolDown: 2360)
                                                )
                                                )
                        .Init("Lion Archer",
                                new State(
                                        new Prioritize(
                                                new Follow(.8, 14, 8.4),
                                                new Wander(0.4)
                                        ),
                                        new Shoot(8.4, count: 3, shootAngle: 12, projectileIndex: 0, coolDown: 3000),
                                        new Shoot(8.4, count: 3, shootAngle: 12, projectileIndex: 1, coolDown: 3750),
                                        new Shoot(12, count: 1, projectileIndex: 2, coolDown: 1700),
                                        new Shoot(15, count: 5, fixedAngle: 360 / 5, projectileIndex: 3, coolDown: 3500)
                                                )
                                                )
                        .Init("Jackal Priest",
                                new State(
                                        new Prioritize(
                                                new Follow(.8, 14, 10),
                                                new Wander(0.4)
                                                ),
                                        new Shoot(18, count: 1, projectileIndex: 0, coolDown: 1000),
                                        new Shoot(18, count: 1, projectileIndex: 1, coolDown: 1200)
                                                )
                                                )
                        .Init("Tomb Bomb Trap",
                                new State(
                                        new State("Normal",
                                                //                                              Help with Shoot ??
                                                new EntityNotExistsTransition("Tomb Bomb Trap Switch", 10, "Death")
                                                ),
                                        new State("Death",
                                                new TransformOnDeath("Deactivated Tomb Bomb Trap", 1, 1, 1),
                                                new Suicide()
                                                )
                                                )
                                                )
                        .Init("Tomb Bomb Trap Switch",
                                new State(
                                        new State("Normal",
                                                new PlayerWithinTransition(1, "5")
                                                ),
                                        new State("5",
                                                new Taunt(0.99, "5"),
                                                new TimedTransition(1000, "4")
                                        ),
                                        new State("4",
                                                new Taunt(0.99, "4"),
                                                new TimedTransition(1000, "3")
                                        ),
                                        new State("3",
                                                new Taunt(0.99, "3"),
                                                new TimedTransition(1000, "2")
                                        ),
                                        new State("2",
                                                new Taunt(0.99, "2"),
                                                new TimedTransition(1000, "1")
                                        ),
                                        new State("1",
                                                new Taunt(0.99, "1"),
                                                new TimedTransition(1000, "Suicide")
                                                ),
                                        new State("Suicide",
                                                new Suicide()
                                                )
                                                )
                                                )
                        .Init("Deactivated Tomb Bomb Trap",
                                new State(
                                        )
                                        )
                        .Init("Blue Swarm Masters",
                                new State(
                                        new State("Normal",
                                                new Prioritize(
                                                        new Follow(0.3, 5, 3),
                                                        new Wander(0.3)
                                                ),
                                                new Spawn("Blue Swarm Minions", 8, 1),
                                                new Shoot(12, count: 1, projectileIndex: 0, coolDown: 1400),
                                                new Shoot(12, count: 1, projectileIndex: 1, coolDown: 1700),
                                                new Shoot(18, count: 1, projectileIndex: 2, coolDown: 2000),
                                                new Shoot(18, count: 1, projectileIndex: 3, coolDown: 2100),
                                                new EntityNotExistsTransition("Blue Swarm Minions", 10, "Respawn")
                                        ),
                                        new State("Respawn",
                                                new Spawn("Blue Swarm Minions", 1, coolDown: 4000),
                                                new TimedTransition(100, "Normal")
                                        )
                                )
                        )
                        .Init("Blue Swarm Minions",
                                new State(
                                        new Prioritize(
                                                new Protect(0.6, "Blue Swarm Masters"),
                                                new Wander(0.3)
                                        ),
                                        new Shoot(2.8, count: 1, projectileIndex: 0, coolDown: 1400)
                                )
                        )
                        .Init("Yellow Swarm Masters",
                                new State(
                                        new State("Normal",
                                        new Prioritize(
                                                new Follow(0.3, 5, 3),
                                                new Wander(0.3)
                                        ),
                                        new Spawn("Yellow Swarm Minions", 8, 1),
                                        new Shoot(8, count: 1, projectileIndex: 0, coolDown: 1400),
                                        new Shoot(8, count: 1, projectileIndex: 1, coolDown: 1700),
                                        new Shoot(12, count: 1, projectileIndex: 2, coolDown: 2000),
                                        new Shoot(12, count: 1, projectileIndex: 3, coolDown: 2100),
                                        new EntityNotExistsTransition("Yellow Swarm Minions", 10, "Respawn")
                                ),
                                new State("Respawn",
                                        new Spawn("Yellow Swarm Minions", 1, coolDown: 4000),
                                        new TimedTransition(100, "Normal")
                                )
                        )
                        )
                        .Init("Yellow Swarm Minions",
                                new State(
                                        new Prioritize(
                                                new Protect(0.6, "Yellow Swarm Masters"),
                                                new Wander(0.3)
                                        ),
                                        new Shoot(2.5, count: 1, projectileIndex: 0, coolDown: 1400)
                                )
                        )
                        .Init("Red Swarm Masters",
                                new State(
                                        new State("Normal",
                                        new Prioritize(
                                                new Follow(0.3, 5, 3),
                                                new Wander(0.3)
                                        ),
                                        new Spawn("Red Swarm Minions", 8, 1),
                                        new Shoot(10, count: 1, projectileIndex: 0, coolDown: 1400),
                                        new Shoot(14, count: 1, projectileIndex: 1, coolDown: 1700),
                                        new Shoot(10, count: 1, projectileIndex: 2, coolDown: 2000),
                                        new Shoot(14, count: 1, projectileIndex: 3, coolDown: 2100),
                                        new EntityNotExistsTransition("Red Swarm Minions", 10, "Respawn")
                                ),
                                new State("Respawn",
                                        new Spawn("Red Swarm Minions", 1, coolDown: 4000),
                                        new TimedTransition(100, "Normal")
                                )
                        )
                        )
                        .Init("Red Swarm Minions",
                                new State(
                                        new Prioritize(
                                                new Protect(0.6, "Red Swarm Masters"),
                                                new Wander(0.3)
                                        ),
                                        new Shoot(2.4, count: 1, projectileIndex: 0, coolDown: 1400)
                                )
                        )
                        .Init("Jackal Lord",
                                new State(
                                        new State("Normal",
                                        new Prioritize(
                                                new Follow(0.7, 7, 1),
                                                new Wander(0.3)
                                        ),
                                        new Spawn("Jackal Assassin", 1, 1),
                                        new Spawn("Jackal Veteran", 1, 1),
                                        new Spawn("Jackal Warrior", 1, 1),
                                        new Shoot(6.3, count: 6, shootAngle: 60, projectileIndex: 1, coolDown: 4950),
                                        new Shoot(8, count: 3, coolDown: 1400),
                                        new Shoot(8, count: 2, coolDown: 6500),
                                        new EntityNotExistsTransition("Jackal Assassin", 10, "Respawn Ass"),
                                        new EntityNotExistsTransition("Jackal Veteran", 10, "Respawn Vet"),
                                        new EntityNotExistsTransition("Jackal Warrior", 10, "Respawn Warr")
                                ),
                                new State("Respawn Ass",
                                        new Spawn("Jackal Assassin", 1, coolDown: 4000),
                                        new TimedTransition(100, "Normal")
                                ),
                                new State("Respawn Vet",
                                        new Spawn("Jackal Veteran", 1, coolDown: 4000),
                                        new TimedTransition(100, "Normal")
                                ),
                                new State("Respawn Warr",
                                        new Spawn("Jackal Warrior", 1, coolDown: 4000),
                                        new TimedTransition(100, "Normal")
                                )
                        )
                        )
                        .Init("Jackal Assassin",
                                new State(
                                        new Prioritize(
                                                new Follow(0.7, 7, 1),
                                                new Wander(0.3)
                                        ),
                                        new Shoot(8, count: 1, coolDown: 1400)
                                )
                        )
                        .Init("Jackal Veteran",
                                new State(
                                        new Prioritize(
                                                new Follow(0.7, 7, 1),
                                                new Wander(0.3)
                                        ),
                                        new Shoot(6.4, count: 1, coolDown: 1400)
                                )
                        )
                        .Init("Jackal Warrior",
                                new State(
                                        new Prioritize(
                                                new Follow(0.7, 7, 1),
                                                new Wander(0.3)
                                        ),
                                        new Shoot(4.8, count: 1, coolDown: 1400)
                                )
                        )
                        .Init("Tomb Support",
                                new State(
                                        new Taunt(0.99, "ENOUGHT OF YOUR VANDALISM!"),
                                        new State("Idle",
                                                new ConditionalEffect(ConditionEffectIndex.Armored),
                                                new EntityNotExistsTransition("Tomb Boss Anchor", 50, "IdlePhase1"),
                                                new EntityExistsTransition("Tomb Boss Anchor", 50, "IdlePhase2")
                                        ),
                                        new State("IdlePhase1",
                                                new ConditionalEffect(ConditionEffectIndex.Armored),
                                                new HpLessTransition(0.99, "Phase1")
                                        ),
                                        new State("IdlePhase2",
                                                new ConditionalEffect(ConditionEffectIndex.Armored),
                                                new Prioritize(
                                                        new Orbit(0.4, 3, 20, "Tomb Boss Anchor")
                                                ),
                                                new HpLessTransition(0.98, "2Phase1")
                                        ),
                                        //                                      If Anchor Don't Exist  
                                        new State("Phase1",
                                                new ConditionalEffect(ConditionEffectIndex.Armored),
                                                new Prioritize(
                                                        new Wander(0.3)
                                                ),
                                                new Heal(20, "Tomb Bosses", coolDown: 1500),
                                                new HpLessTransition(0.97, "Phase2")
                                        ),
                                        new State("Phase2",
                                                new ConditionalEffect(ConditionEffectIndex.Armored),
                                                new Prioritize(
                                                        new Wander(0.3)
                                                ),
                                                new SpecificHeal(1, 1500, "Tomb Bosses", 1500),
                                                new Shoot(60, count: 15, fixedAngle: 360 / 15, projectileIndex: 7, coolDown: 10000),
                                                new HpLessTransition(0.94, "Phase3")
                                        ),

                                        new State("Phase3",
                                                new Prioritize(
                                                        new Wander(0.3)
                                                ),
                                                new SpecificHeal(1, 1500, "Tomb Bosses", 1500),
                                                new Shoot(16, count: 1, projectileIndex: 6, coolDown: 3000),
                                                new Shoot(32, count: 1, projectileIndex: 5, coolDown: 1000),
                                                new HpLessTransition(0.92, "Phase4")
                                        ),
                                        new State("Phase4",
                                                new Prioritize(
                                                        new Wander(0.3)
                                                ),
                                                new SpecificHeal(1, 1000, "Tomb Bosses", 2500),
                                                new Shoot(16, count: 1, projectileIndex: 6, coolDown: 3000),
                                                new Shoot(32, count: 1, projectileIndex: 5, coolDown: 1000),
                                                new Shoot(16, count: 3, shootAngle: 360 / 3, projectileIndex: 1, coolDown: 2000),
                                                new Shoot(16, count: 4, shootAngle: 360 / 4, projectileIndex: 2, coolDown: 3000),
                                                new HpLessTransition(0.80, "Phase5")
                                        ),
                                        new State("Phase5",
                                                new Prioritize(
                                                        new Wander(0.4)
                                                ),
                                                new Shoot(16, count: 1, projectileIndex: 6, coolDown: 3000),
                                                new Shoot(32, count: 1, projectileIndex: 5, coolDown: 1000),
                                                new Shoot(16, count: 3, shootAngle: 360 / 3, projectileIndex: 1, coolDown: 2700),
                                                new Shoot(16, count: 4, shootAngle: 360 / 4, projectileIndex: 2, coolDown: 3000),
                                                new Shoot(16, count: 6, shootAngle: 360 / 6, projectileIndex: 4, coolDown: 2000),
                                                new Shoot(16, count: 5, shootAngle: 360 / 5, projectileIndex: 3, coolDown: 2500),
                                                new HpLessTransition(0.60, "Phase6")
                                        ),
                                        new State("Phase6",
                                                new Prioritize(
                                                        new Wander(0.4)
                                                ),
                                                new Shoot(16, count: 1, projectileIndex: 6, coolDown: 2600),
                                                new Shoot(32, count: 1, projectileIndex: 5, coolDown: 800),
                                                new Shoot(16, count: 3, shootAngle: 360 / 3, projectileIndex: 1, coolDown: 2000),
                                                new Shoot(16, count: 4, shootAngle: 360 / 4, projectileIndex: 2, coolDown: 2400),
                                                new Shoot(16, count: 6, shootAngle: 360 / 6, projectileIndex: 4, coolDown: 1500),
                                                new Shoot(16, count: 5, shootAngle: 360 / 5, projectileIndex: 3, coolDown: 1800),
                                                new Shoot(40, count: 2, fixedAngle: 140, shootAngle: 9, projectileIndex: 8, coolDown: 1000),
                                                new HpLessTransition(0.50, "Phase7")
                                        ),
                                        new State("Phase7",
                                                new Taunt(0.99, "My artifacts shall make your lethargic lives end much more swiftly!"),
                                                new Prioritize(
                                                        new Wander(0.4)
                                                ),
                                                new Spawn("Sphinx Artifact 1", 3, initialSpawn: 1),
                                                new Spawn("Sphinx Artifact 2", 3, initialSpawn: 1),
                                                new Spawn("Sphinx Artifact 3", 3, initialSpawn: 1),
                                                new Shoot(16, count: 1, projectileIndex: 6, coolDown: 2600),
                                                new Shoot(32, count: 1, projectileIndex: 5, coolDown: 800),
                                                new Shoot(16, count: 3, shootAngle: 360 / 3, projectileIndex: 1, coolDown: 2000),
                                                new Shoot(16, count: 4, shootAngle: 360 / 4, projectileIndex: 2, coolDown: 2400),
                                                new Shoot(16, count: 6, shootAngle: 360 / 6, projectileIndex: 4, coolDown: 1500),
                                                new Shoot(16, count: 5, shootAngle: 360 / 5, projectileIndex: 3, coolDown: 1800),
                                                new HpLessTransition(0.30, "Phase8")
                                        ),
                                        new State("Phase8",
                                                new Prioritize(
                                                        new Wander(0.6)
                                                ),
                                                new Spawn("Sphinx Artifact 1", 3, coolDown: 20000),
                                                new Spawn("Sphinx Artifact 2", 3, coolDown: 20000),
                                                new Spawn("Sphinx Artifact 3", 3, coolDown: 20000),
                                                new Shoot(16, count: 1, projectileIndex: 6, coolDown: 2300),
                                                new Shoot(32, count: 1, projectileIndex: 5, coolDown: 800),
                                                new Shoot(16, count: 3, shootAngle: 360 / 3, projectileIndex: 1, coolDown: 1700),
                                                new Shoot(16, count: 4, shootAngle: 360 / 4, projectileIndex: 2, coolDown: 2000),
                                                new Shoot(16, count: 6, shootAngle: 360 / 6, projectileIndex: 4, coolDown: 1300),
                                                new Shoot(16, count: 5, shootAngle: 360 / 5, projectileIndex: 3, coolDown: 1500),
                                                new HpLessTransition(0.10, "Rage")
                                        ),
                                        new State("Rage",
                                                new Taunt(0.99, "This cannot be! You shall not succeed!"),
                                                new Flash(0xFF0000, .1, 1000),
                                                new Prioritize(
                                                        new Follow(1, 10, 3)
                                                ),
                                                new Shoot(16, count: 1, projectileIndex: 6, coolDown: 2600),
                                                new Shoot(32, count: 1, projectileIndex: 5, coolDown: 800),
                                                new Shoot(16, count: 3, shootAngle: 360 / 3, projectileIndex: 1, coolDown: 2000),
                                                new Shoot(16, count: 4, shootAngle: 360 / 4, projectileIndex: 2, coolDown: 2400),
                                                new Shoot(16, count: 6, shootAngle: 360 / 6, projectileIndex: 4, coolDown: 1500),
                                                new Shoot(16, count: 5, shootAngle: 360 / 5, projectileIndex: 3, coolDown: 1800),
                                                new Shoot(40, count: 7, fixedAngle: 160, shootAngle: 12, projectileIndex: 8, coolDown: 1500)
                                        ),
                                        new State("2Phase1",
                                                new ConditionalEffect(ConditionEffectIndex.Armored),
                                                new Prioritize(
                                                        new Orbit(0.4, 3, 20, "Tomb Boss Anchor")
                                                ),
                                                new SpecificHeal(1, 1500, "Tomb Bosses", 1500),
                                                new HpLessTransition(0.97, "2Phase2")
                                        ),
                                        new State("2Phase2",
                                                new ConditionalEffect(ConditionEffectIndex.Armored),
                                                new Prioritize(
                                                        new Orbit(0.4, 3, 20, "Tomb Boss Anchor")
                                                ),
                                                new SpecificHeal(1, 1500, "Tomb Bosses", 1500),
                                                new Shoot(60, count: 15, fixedAngle: 360 / 15, projectileIndex: 7, coolDown: 10000),
                                                new HpLessTransition(0.94, "2Phase3")
                                        ),

                                        new State("2Phase3",
                                                new Prioritize(
                                                        new Orbit(0.4, 3, 20, "Tomb Boss Anchor")
                                                ),
                                                new SpecificHeal(1, 2000, "Tomb Bosses", 1500),
                                                new Shoot(16, count: 1, projectileIndex: 6, coolDown: 3000),
                                                new Shoot(32, count: 1, projectileIndex: 5, coolDown: 1000),
                                                new HpLessTransition(0.92, "2Phase4")
                                        ),
                                        new State("2Phase4",
                                                new Prioritize(
                                                        new Orbit(0.4, 3, 20, "Tomb Boss Anchor")
                                                ),
                                                new SpecificHeal(1, 1500, "Tomb Bosses", 1500),
                                                new Shoot(16, count: 1, projectileIndex: 6, coolDown: 3000),
                                                new Shoot(32, count: 1, projectileIndex: 5, coolDown: 1000),
                                                new Shoot(16, count: 3, shootAngle: 360 / 3, projectileIndex: 1, coolDown: 2000),
                                                new Shoot(16, count: 4, shootAngle: 360 / 4, projectileIndex: 2, coolDown: 3000),
                                                new HpLessTransition(0.80, "2Phase5")
                                        ),
                                        new State("2Phase5",
                                                new Prioritize(
                                                        new Orbit(0.4, 3, 20, "Tomb Boss Anchor")
                                                ),
                                                new Shoot(16, count: 1, projectileIndex: 6, coolDown: 3000),
                                                new Shoot(32, count: 1, projectileIndex: 5, coolDown: 1000),
                                                new Shoot(16, count: 3, shootAngle: 360 / 3, projectileIndex: 1, coolDown: 2700),
                                                new Shoot(16, count: 4, shootAngle: 360 / 4, projectileIndex: 2, coolDown: 3000),
                                                new Shoot(16, count: 6, shootAngle: 360 / 6, projectileIndex: 4, coolDown: 2000),
                                                new Shoot(16, count: 5, shootAngle: 360 / 5, projectileIndex: 3, coolDown: 2500),
                                                new HpLessTransition(0.60, "2Phase6")
                                        ),
                                        new State("2Phase6",
                                                new Prioritize(
                                                        new Orbit(0.4, 5, 20, "Tomb Boss Anchor")
                                                ),
                                                new Shoot(16, count: 1, projectileIndex: 6, coolDown: 2600),
                                                new Shoot(32, count: 1, projectileIndex: 5, coolDown: 800),
                                                new Shoot(16, count: 3, shootAngle: 360 / 3, projectileIndex: 1, coolDown: 2700),
                                                new Shoot(16, count: 4, shootAngle: 360 / 4, projectileIndex: 2, coolDown: 3000),
                                                new Shoot(16, count: 6, shootAngle: 360 / 6, projectileIndex: 4, coolDown: 2000),
                                                new Shoot(16, count: 5, shootAngle: 360 / 5, projectileIndex: 3, coolDown: 2500),
                                                new Shoot(40, count: 4, fixedAngle: 140, shootAngle: 9, projectileIndex: 8, coolDown: 1000),
                                                new HpLessTransition(0.50, "2Phase7")
                                        ),
                                        new State("2Phase7",
                                                new Taunt(0.99, "My artifacts shall make your lethargic lives end much more swiftly!"),
                                                new Prioritize(
                                                        new Orbit(0.4, 3, 20, "Tomb Boss Anchor")
                                                ),
                                                new Spawn("Sphinx Artifact 1", 3, initialSpawn: 1),
                                                new Spawn("Sphinx Artifact 2", 3, initialSpawn: 1),
                                                new Spawn("Sphinx Artifact 3", 3, initialSpawn: 1),
                                                new Shoot(16, count: 1, projectileIndex: 6, coolDown: 2600),
                                                new Shoot(32, count: 1, projectileIndex: 5, coolDown: 800),
                                                new Shoot(16, count: 3, shootAngle: 360 / 3, projectileIndex: 1, coolDown: 2700),
                                                new Shoot(16, count: 4, shootAngle: 360 / 4, projectileIndex: 2, coolDown: 3000),
                                                new Shoot(16, count: 6, shootAngle: 360 / 6, projectileIndex: 4, coolDown: 2000),
                                                new Shoot(16, count: 5, shootAngle: 360 / 5, projectileIndex: 3, coolDown: 2500),
                                                new HpLessTransition(0.30, "2Phase8")
                                        ),
                                        new State("2Phase8",
                                                new Prioritize(
                                                        new Orbit(0.4, 3, 20, "Tomb Boss Anchor")
                                                ),
                                                new Spawn("Sphinx Artifact 1", 3, coolDown: 20000),
                                                new Spawn("Sphinx Artifact 2", 3, coolDown: 20000),
                                                new Spawn("Sphinx Artifact 3", 3, coolDown: 20000),
                                                new Shoot(16, count: 1, projectileIndex: 6, coolDown: 2300),
                                                new Shoot(32, count: 1, projectileIndex: 5, coolDown: 800),
                                                new Shoot(16, count: 3, shootAngle: 360 / 3, projectileIndex: 1, coolDown: 2700),
                                                new Shoot(16, count: 4, shootAngle: 360 / 4, projectileIndex: 2, coolDown: 3000),
                                                new Shoot(16, count: 6, shootAngle: 360 / 6, projectileIndex: 4, coolDown: 2000),
                                                new Shoot(16, count: 5, shootAngle: 360 / 5, projectileIndex: 3, coolDown: 2500),
                                                new HpLessTransition(0.10, "Rage")
                                        )
                                ),
                                new Threshold(0.01,
                                        new ItemLoot("Potion of Life", 1),
                                        new ItemLoot("Ring of the Sphinx", 0.05),
                                        new ItemLoot("Bow of the Nile", 0.01),
                                        new ItemLoot("Quiver of the Nile", 0.01),
                                        new ItemLoot("Osiris Armor", 0.01),
                                        new ItemLoot("Eye of Osiris", 0.01)
                                )
                        )
                        .Init("Sphinx Artifact 1",
                                new State(
                                        new Prioritize(
                                                new Orbit(1, 3, 30, "Tomb Support")
                                        ),
                                        new Shoot(16, count: 1, shootAngle: 12, coolDown: 1500)
                                )
                        )
                        .Init("Sphinx Artifact 2",
                                new State(
                                        new State("Normal",
                                                new Prioritize(
                                                        new Orbit(1, 3, 30, "Tomb Defender")
                                                ),
                                                new Shoot(16, count: 1, shootAngle: 12, coolDown: 1500),
                                                new EntityNotExistsTransition("Tomb Defender", 50, "Attacker Exist?")
                                        ),
                                        new State("Attacker Exist?",
                                                new Prioritize(
                                                        new Orbit(1, 3, 30, "Tomb Attacker")
                                                ),
                                                new Shoot(4, count: 1, shootAngle: 12, coolDown: 1000),
                                                new EntityNotExistsTransition("Tomb Attacker", 50, "Suicide")
                                        ),
                                        new State("Suicide",
                                                new Suicide()
                                        )
                                )
                        )
                        .Init("Sphinx Artifact 3",
                                new State(
                                        new State("Normal",
                                                new Prioritize(
                                                        new Orbit(1, 3, 30, "Tomb Attacker")
                                                ),
                                                new Shoot(16, count: 1, shootAngle: 12, coolDown: 1500),
                                                new EntityNotExistsTransition("Tomb Attacker", 50, "Defender Exist?")
                                        ),
                                        new State("Defender Exist?",
                                                new Prioritize(
                                                        new Orbit(1, 3, 30, "Tomb Defender")
                                                ),
                                                new Shoot(4, count: 1, shootAngle: 12, coolDown: 1000),
                                                new EntityNotExistsTransition("Tomb Defender", 50, "Suicide")
                                        ),
                                        new State("Suicide",
                                                new Suicide()
                                        )
                                )
                        )
                        .Init("Tomb Attacker",
                                new State(
                                        new Taunt(0.99, "YOU HAVE AWAKENED US!"),
                                        new State("Idle",
                                                new ConditionalEffect(ConditionEffectIndex.Armored),
                                                new EntityNotExistsTransition("Tomb Boss Anchor", 50, "IdlePhase1"),
                                                new EntityExistsTransition("Tomb Boss Anchor", 50, "IdlePhase2")
                                        ),
                                        new State("IdlePhase1",
                                                new ConditionalEffect(ConditionEffectIndex.Armored),
                                                new HpLessTransition(0.99, "1Phase")
                                        ),
                                        new State("IdlePhase2",
                                                new ConditionalEffect(ConditionEffectIndex.Armored),
                                                new Prioritize(
                                                        new Orbit(0.4, 3, 20, "Tomb Boss Anchor")
                                                ),
                                                new HpLessTransition(0.98, "1Phase2")
                                        ),
                                        //                                      If Anchor Don't Exist  
                                        new State("1Phase",
                                                new ConditionalEffect(ConditionEffectIndex.Armored),
                                                new Taunt(0.99, "Nut, protect me at once!"),
                                                new Prioritize(
                                                        new Wander(0.3)
                                                ),
                                                new Shoot(60, count: 15, fixedAngle: 360 / 15, projectileIndex: 3, coolDown: 10000),
                                                new HpLessTransition(0.97, "2Phase")
                                        ),
                                        new State("2Phase",
                                                new Prioritize(
                                                        new Wander(0.4)
                                                ),
                                                new Shoot(20, count: 2, shootAngle: 12, projectileIndex: 2, coolDown: 500, predictive: 0.5),
                                                new Grenade(3, 50, 7, coolDown: 3000),
                                                new HpLessTransition(.9, "3Phase")
                                        ),
                                        new State("3Phase",
                                                new Prioritize(
                                                        new Wander(0.4)
                                                ),
                                                new Spawn("Scarab", 3),
                                                new Shoot(14.4, count: 8, fixedAngle: 360 / 8, projectileIndex: 1, coolDown: 3500),
                                                new Shoot(20, count: 4, shootAngle: 360 / 4, projectileIndex: 2, coolDown: 1500),
                                                new Shoot(20, count: 2, shootAngle: 12, projectileIndex: 5, coolDown: 1500),
                                                new Shoot(16, count: 1, projectileIndex: 0, coolDown: 2300),
                                                new Shoot(20, count: 1, projectileIndex: 2, coolDown: 500, predictive: 0.5),
                                                new Grenade(3, 50, 7, coolDown: 2500),
                                                new HpLessTransition(.8, "4Phase")
                                                ),
                                        new State("4Phase",
                                                new Prioritize(
                                                        new Wander(0.4)
                                                ),
                                                new Spawn("Scarab", 3),
                                                new Shoot(14.4, count: 8, fixedAngle: 360 / 8, projectileIndex: 1, coolDown: 3500),
                                                new Shoot(20, count: 4, shootAngle: 360 / 4, projectileIndex: 2, coolDown: 1500),
                                                new Shoot(20, count: 2, shootAngle: 12, projectileIndex: 5, coolDown: 1500),
                                                new Shoot(16, count: 1, projectileIndex: 0, coolDown: 2300),
                                                new Shoot(20, count: 1, projectileIndex: 2, coolDown: 500, predictive: 0.5),
                                                new Grenade(3, 50, 7, coolDown: 2500),
                                                new HpLessTransition(.7, "5Phase")
                                        ),
                                        new State("5Phase",
                                                new Prioritize(
                                                        new Wander(0.4)
                                                ),
                                                new Spawn("Scarab", 3),
                                                new Shoot(14.4, count: 8, fixedAngle: 360 / 8, projectileIndex: 1, coolDown: 3500),
                                                new Shoot(20, count: 4, shootAngle: 360 / 4, projectileIndex: 2, coolDown: 1500),
                                                new Shoot(20, count: 2, shootAngle: 12, projectileIndex: 5, coolDown: 1500),
                                                new Shoot(16, count: 1, projectileIndex: 0, coolDown: 2300),
                                                new Shoot(20, count: 1, projectileIndex: 2, coolDown: 500, predictive: 0.5),
                                                new Grenade(3, 50, 7, coolDown: 2500),
                                                new HpLessTransition(.5, "6Phase")
                                        ),
                                        new State("6Phase",
                                                new Taunt(0.99, "My artifacts shall destroy from your soul to your flesh"),
                                                new Prioritize(
                                                        new Wander(0.4)
                                                ),
                                                new Spawn("Nile Artifact 1", 3, initialSpawn: 1),
                                                new Spawn("Nile Artifact 2", 3, initialSpawn: 1),
                                                new Spawn("Nile Artifact 3", 3, initialSpawn: 1),
                                                new Shoot(20, count: 5, shootAngle: 360 / 5, projectileIndex: 2, coolDown: 2000),
                                                new Shoot(20, count: 4, shootAngle: 40, projectileIndex: 5, coolDown: 1500),
                                                new Shoot(20, count: 3, shootAngle: 10, projectileIndex: 2, coolDown: 500, predictive: 0.5),
                                                new Shoot(14.4, count: 8, fixedAngle: 360 / 8, projectileIndex: 1, coolDown: 5000),
                                                new Shoot(16, count: 1, projectileIndex: 0, coolDown: 1500, predictive: 0.5),
                                                new Grenade(3, 50, 7, coolDown: 2000),
                                                new Reproduce("Scarab", 3, 3, coolDown: 10000),
                                                new HpLessTransition(0.30, "7Phase")
                                        ),
                                        new State("7Phase",
                                                new Prioritize(
                                                        new Wander(0.6)
                                                ),
                                                new Spawn("Nile Artifact 1", 3, coolDown: 20000),
                                                new Spawn("Nile Artifact 2", 3, coolDown: 20000),
                                                new Spawn("Nile Artifact 3", 3, coolDown: 20000),
                                                new Shoot(20, count: 5, shootAngle: 360 / 5, projectileIndex: 2, coolDown: 2000),
                                                new Shoot(20, count: 4, shootAngle: 40, projectileIndex: 5, coolDown: 1500),
                                                new Shoot(20, count: 3, shootAngle: 10, projectileIndex: 2, coolDown: 500, predictive: 0.5),
                                                new Shoot(14.4, count: 8, fixedAngle: 360 / 8, projectileIndex: 1, coolDown: 5000),
                                                new Shoot(16, count: 1, projectileIndex: 0, coolDown: 1500, predictive: 0.5),
                                                new Grenade(3, 50, 10, coolDown: 2000),
                                                new Reproduce("Scarab", 3, 3, coolDown: 10000),
                                                new HpLessTransition(0.10, "Rage")
                                        ),
                                        new State("Rage",
                                                new Taunt(0.99, "Argh! You shall pay for your crimes!"),
                                                new Flash(0xFF0000, 1, 1000),
                                                new Prioritize(
                                                        new StayBack(1, 7)
                                                ),
                                                new Spawn("Nile Artifact 1", 3, coolDown: 20000),
                                                new Spawn("Nile Artifact 2", 3, coolDown: 20000),
                                                new Spawn("Nile Artifact 3", 3, coolDown: 20000),
                                                new Reproduce("Scarab", 3, 3, coolDown: 5000),
                                                new Grenade(4, 50, 7, coolDown: 2000),
                                                new Shoot(14.4, count: 8, fixedAngle: 360 / 8, projectileIndex: 1, coolDown: 3000),
                                                new Shoot(20, count: 1, fixedAngle: 10, projectileIndex: 5),
                                                new Shoot(20, count: 1, fixedAngle: 350, projectileIndex: 5),
                                                new Shoot(20, count: 1, projectileIndex: 2, coolDown: 500, predictive: 0.5)
                                        ),
                                        //                                                                                              IfAnchorExist  
                                        new State("1Phase2",
                                                new ConditionalEffect(ConditionEffectIndex.Armored),
                                                new Taunt(0.99, "Nut, protect me at once!"),
                                                new Prioritize(
                                                        new Orbit(0.4, 3, 20, "Tomb Boss Anchor")
                                                ),
                                                new Shoot(60, count: 15, fixedAngle: 360 / 15, projectileIndex: 3, coolDown: 10000),
                                                new HpLessTransition(0.97, "2Phase2")
                                        ),
                                        new State("2Phase2",
                                                new Prioritize(
                                                        new Orbit(0.4, 3, 20, "Tomb Boss Anchor")
                                                ),
                                                new Shoot(20, count: 2, shootAngle: 12, projectileIndex: 2, coolDown: 500, predictive: 0.5),
                                                new Grenade(3, 50, 7, coolDown: 3000),
                                                new HpLessTransition(.9, "3Phase2")
                                        ),
                                        new State("3Phase2",
                                                new Prioritize(
                                                        new Orbit(0.4, 3, 20, "Tomb Boss Anchor")
                                                ),
                                                new Spawn("Scarab", 3),
                                                new Shoot(14.4, count: 8, fixedAngle: 360 / 8, projectileIndex: 1, coolDown: 3500),
                                                new Shoot(20, count: 4, shootAngle: 360 / 4, projectileIndex: 2, coolDown: 1500),
                                                new Shoot(20, count: 2, shootAngle: 12, projectileIndex: 5, coolDown: 1500),
                                                new Shoot(16, count: 1, projectileIndex: 0, coolDown: 2300),
                                                new Shoot(20, count: 1, projectileIndex: 2, coolDown: 500, predictive: 0.5),
                                                new Grenade(3, 50, 7, coolDown: 2500),
                                                new HpLessTransition(.8, "4Phase2")
                                        ),
                                        new State("4Phase2",
                                                new Prioritize(
                                                        new Orbit(0.4, 3, 20, "Tomb Boss Anchor")
                                                ),
                                                new Spawn("Scarab", 3),
                                                new Shoot(14.4, count: 8, fixedAngle: 360 / 8, projectileIndex: 1, coolDown: 3500),
                                                new Shoot(20, count: 4, shootAngle: 360 / 4, projectileIndex: 2, coolDown: 1500),
                                                new Shoot(20, count: 2, shootAngle: 12, projectileIndex: 5, coolDown: 1500),
                                                new Shoot(16, count: 1, projectileIndex: 0, coolDown: 2300),
                                                new Shoot(20, count: 1, projectileIndex: 2, coolDown: 500, predictive: 0.5),
                                                new Grenade(3, 50, 7, coolDown: 2500),
                                                new HpLessTransition(.7, "5Phase2")
                                        ),
                                        new State("5Phase2",
                                                new Prioritize(
                                                        new Orbit(0.6, 3, 20, "Tomb Boss Anchor")
                                                ),
                                                new Spawn("Scarab", 3),
                                                new Shoot(14.4, count: 8, fixedAngle: 360 / 8, projectileIndex: 1, coolDown: 3500),
                                                new Shoot(20, count: 4, shootAngle: 360 / 4, projectileIndex: 2, coolDown: 1500),
                                                new Shoot(20, count: 2, shootAngle: 12, projectileIndex: 5, coolDown: 1500),
                                                new Shoot(16, count: 1, projectileIndex: 0, coolDown: 2300),
                                                new Shoot(20, count: 1, projectileIndex: 2, coolDown: 500, predictive: 0.5),
                                                new Grenade(3, 50, 7, coolDown: 2500),
                                                new HpLessTransition(.5, "6Phase2")
                                        ),
                                        new State("6Phase2",
                                                new Taunt(0.99, "My artifacts shall destroy from your soul to your flesh"),
                                                new Prioritize(
                                                        new Orbit(0.6, 3, 20, "Tomb Boss Anchor")
                                                ),
                                                new Spawn("Nile Artifact 1", 3, initialSpawn: 1),
                                                new Spawn("Nile Artifact 2", 3, initialSpawn: 1),
                                                new Spawn("Nile Artifact 3", 3, initialSpawn: 1),
                                                new Shoot(20, count: 5, shootAngle: 360 / 5, projectileIndex: 2, coolDown: 2000),
                                                new Shoot(20, count: 4, shootAngle: 40, projectileIndex: 5, coolDown: 1500),
                                                new Shoot(20, count: 3, shootAngle: 10, projectileIndex: 2, coolDown: 500, predictive: 0.5),
                                                new Shoot(14.4, count: 8, fixedAngle: 360 / 8, projectileIndex: 1, coolDown: 5000),
                                                new Shoot(16, count: 1, projectileIndex: 0, coolDown: 1500, predictive: 0.5),
                                                new Grenade(3, 50, 7, coolDown: 2000),
                                                new Reproduce("Scarab", 3, 3, coolDown: 10000),
                                                new HpLessTransition(0.30, "7Phase2")
                                        ),
                                        new State("7Phase2",
                                                new Prioritize(
                                                        new Orbit(0.6, 3, 20, "Tomb Boss Anchor")
                                                ),
                                                new Spawn("Nile Artifact 1", 3, coolDown: 20000),
                                                new Spawn("Nile Artifact 2", 3, coolDown: 20000),
                                                new Spawn("Nile Artifact 3", 3, coolDown: 20000),
                                                new Shoot(20, count: 5, shootAngle: 360 / 5, projectileIndex: 2, coolDown: 2000),
                                                new Shoot(20, count: 4, shootAngle: 40, projectileIndex: 5, coolDown: 1500),
                                                new Shoot(20, count: 3, shootAngle: 10, projectileIndex: 2, coolDown: 500, predictive: 0.5),
                                                new Shoot(14.4, count: 8, fixedAngle: 360 / 8, projectileIndex: 1, coolDown: 5000),
                                                new Shoot(16, count: 1, projectileIndex: 0, coolDown: 1500, predictive: 0.5),
                                                new Grenade(3, 50, 10, coolDown: 2000),
                                                new Reproduce("Scarab", 3, 3, coolDown: 10000),
                                                new HpLessTransition(0.10, "Rage")
                                        )
                                ),
                                new Threshold(0.01,
                                        new ItemLoot("Potion of Life", 1),
                                        new ItemLoot("Ring of the Nile", 0.05),
                                        new ItemLoot("Book of Geb", 0.032),
                                        new ItemLoot("Scepter of Geb", 0.032),
                                        new ItemLoot("Shendyt of Geb", 0.032)
                        )
                        )
                                .Init("Nile Artifact 1",
                                        new State(
                                                new Prioritize(
                                                        new Orbit(1, 3, 30, "Tomb Attacker")
                                                ),
                                                new Shoot(12, count: 1, shootAngle: 12, coolDown: 1500)
                                        )
                                )
                                .Init("Nile Artifact 2",
                                        new State(
                                                new State("Normal",
                                                        new Prioritize(
                                                                new Orbit(1, 3, 30, "Tomb Defender")
                                                        ),
                                                        new Shoot(12, count: 1, shootAngle: 12, coolDown: 1500),
                                                        new EntityNotExistsTransition("Tomb Defender", 50, "Support Exist?")
                                                ),
                                                new State("Support Exist?",
                                                        new Prioritize(
                                                                new Orbit(1, 3, 30, "Tomb Support")
                                                        ),
                                                        new Shoot(12, count: 1, shootAngle: 12, coolDown: 1000),
                                                        new EntityNotExistsTransition("Tomb Support", 50, "Suicide")
                                                ),
                                                new State("Suicide",
                                                        new Suicide()
                                                )
                                        )
                                )
                                .Init("Nile Artifact 3",
                                        new State(
                                                new State("Normal",
                                                        new Prioritize(
                                                                new Orbit(1, 3, 30, "Tomb Support")
                                                        ),
                                                        new Shoot(12, count: 1, shootAngle: 12, coolDown: 1500),
                                                        new EntityNotExistsTransition("Tomb Support", 50, "Defender Exist?")
                                                ),
                                                new State("Defender Exist?",
                                                        new Prioritize(
                                                                new Orbit(1, 3, 30, "Tomb Defender")
                                                        ),
                                                        new Shoot(12, count: 1, shootAngle: 12, coolDown: 1000),
                                                        new EntityNotExistsTransition("Tomb Defender", 50, "Suicide")
                                                ),
                                                new State("Suicide",
                                                        new Suicide()
                                                )
                                        )
                                )
                        ;
    }
}