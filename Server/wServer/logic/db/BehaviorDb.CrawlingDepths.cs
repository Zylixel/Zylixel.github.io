using wServer.logic.behaviors;
using wServer.logic.loot;
using wServer.logic.transitions;

namespace wServer.logic
{
    partial class BehaviorDb
    {
        private _ CrawlingDepths = () => Behav()
                        .Init("Son of Arachna",
                new State(
                    new RealmPortalDrop(),
                    new State("Idle",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new PlayerWithinTransition(9, "MakeWeb")
                    ),
                    new State("MakeWeb",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new TossObject("Epic Arachna Web Spoke 1", 10, 0, 100000),
                        new TossObject("Epic Arachna Web Spoke 7", 6, 0, 100000),
                        new TossObject("Epic Arachna Web Spoke 2", 10, 60, 100000),
                        new TossObject("Epic Arachna Web Spoke 3", 10, 120, 100000),
                        new TossObject("Epic Arachna Web Spoke 8", 6, 120, 100000),
                        new TossObject("Epic Arachna Web Spoke 4", 10, 180, 100000),
                        new TossObject("Epic Arachna Web Spoke 5", 10, 240, 100000),
                        new TossObject("Epic Arachna Web Spoke 9", 6, 240, 100000),
                        new TossObject("Epic Arachna Web Spoke 6", 10, 300, 100000),
                        new TimedTransition(3500, "AttackFINE")
                        ),
                    new State("Attack",
                        //new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new EntitiesNotExistsTransition(999, "AttackFINE", "Silver Son of Arachna Giant Egg Sac", "Blue Son of Arachna Giant Egg Sac", /*"Red Son of Arachna Giant Egg Sac",*/ "Yellow Son of Arachna Giant Egg Sac"),
                        new Shoot(1, projectileIndex: 0, count: 8, coolDown: 2200, shootAngle: 45, fixedAngle: 0),
                        new Shoot(10, projectileIndex: 1, coolDown: 3000),
                        new Shoot(25, projectileIndex: 5, count: 7, coolDown: 3000, coolDownOffset: 14000),
                        new Shoot(25, projectileIndex: 3, count: 7, coolDown: 3000, coolDownOffset: 19000),
                        new Shoot(25, projectileIndex: 4, count: 7, coolDown: 3000, coolDownOffset: 24000),
                        new Shoot(25, projectileIndex: 2, count: 7, coolDown: 3000, coolDownOffset: 32000),
                        new State("Follow",
                            new Prioritize(
                                //new StayAbove(.2, 1),
                                new StayBack(.2, 6),
                                new Wander(.3)
                                ),
                            new TimedTransition(1000, "Return")
                                ),
                        new State("Return",
                            //new StayCloseToSpawn(.4, 1),
                            new ReturnToSpawn(true, 1),
                            new TimedTransition(8000, "Follow")
                            ),
                        new State("AttackFINE",
                            new Shoot(1, projectileIndex: 0, count: 8, coolDown: 2200, shootAngle: 45, fixedAngle: 0),
                            new Shoot(10, projectileIndex: 1, coolDown: 3000),
        #region //Check for the Eggs being alive and shoots if they are
                            new If(
                                           new EntityCountEqual("Yellow Son of Arachna Giant Egg Sac", 9999, 1),
                                           new Shoot(25, projectileIndex: 5, count: 7, coolDown: 3000, coolDownOffset: 4000)
                                          ),
                            new If(
                                           new EntityCountEqual("Red Son of Arachna Giant Egg Sac", 9999, 1),
                                           new Shoot(25, projectileIndex: 3, count: 7, coolDown: 4000, coolDownOffset: 4900)
                                          ),
                            new If(
                                           new EntityCountEqual("Blue Son of Arachna Giant Egg Sac", 9999, 1),
                                           new Shoot(25, projectileIndex: 4, count: 7, coolDown: 4000, coolDownOffset: 6000)
                                          ),
                            new If(
                                           new EntityCountEqual("Silver Son of Arachna Giant Egg Sac", 9999, 1),
                                           new Shoot(25, projectileIndex: 2, count: 7, coolDown: 3000, coolDownOffset: 6000)
                                          ),
        #endregion
                            new State("FollowFINE",
                                new Prioritize(
                                    new StayAbove(.6, 1),
                                    new StayBack(.6),
                                    new Wander(.7)
                                    ),
                                new TimedTransition(1000, "ReturnFINE")
                                    ),
                            new State("ReturnFINE",
                                //new StayCloseToSpawn(.4, 1),
                                new ReturnToSpawn(true),
                                new TimedTransition(3000, "FollowFINE")
                                )
                            )
                        )
                        ),
                        new Threshold(0.03,
                                  new TierLoot(10, ItemType.Weapon, 0.06),
                                  new TierLoot(11, ItemType.Weapon, 0.05),
                                  new TierLoot(12, ItemType.Weapon, 0.04),
                                  new TierLoot(5, ItemType.Ability, 0.06),
                                  new TierLoot(6, ItemType.Ability, 0.04),
                                  new TierLoot(11, ItemType.Armor, 0.06),
                                  new TierLoot(12, ItemType.Armor, 0.05),
                                  new TierLoot(13, ItemType.Armor, 0.04),
                                  new TierLoot(5, ItemType.Ring, 0.05),
                                  new ItemLoot("Potion of Mana", 1),
                                  new ItemLoot("Doku No Ken", 0.02)
                                  )
                        )
           .Init("Crawling Depths Egg Sac",
                new State(
                    new State("CheckOrDeath",
                    new PlayerWithinTransition(2, "Urclose"),
                    new TransformOnDeath("Crawling Spider Hatchling", 5, 7)
                    ),
                new State("Urclose",
                    new Spawn("Crawling Spider Hatchling", 6),
                    new Suicide()
            )))
         .Init("Crawling Spider Hatchling",
                new State(
                    new Prioritize(
                        new Wander(.4)
                    ),
                    new Shoot(7, 1, 0, coolDown: 650),
                    new Shoot(7, 1, 0, 1, predictive: 1, coolDown: 850)
                )
            )
                 .Init("Crawling Grey Spotted Spider",
                new State(
                    new Prioritize(
                        new Charge(2, 8, 1050),
                        new Wander(.4)
                    ),
                    new Shoot(10, 1, 0, coolDown: 500)
                ),
                new ItemLoot("Healing Ichor", 0.2),
                new ItemLoot("Magic Potion", 0.3)
            )
          .Init("Crawling Grey Spider",
                new State(
                    new Prioritize(
                        new Charge(2, 8, 1050),
                        new Wander(.4)
                    ),
                    new Shoot(9, 1, 0, coolDown: 850)
                ),
                new ItemLoot("Healing Ichor", 0.2),
                new ItemLoot("Magic Potion", 0.3)
            )
        .Init("Crawling Red Spotted Spider",
                new State(
                    new Prioritize(
                        new Wander(.4)
                    ),
                    new Shoot(8, 1, 0, coolDown: 750)
                ),
                new ItemLoot("Healing Ichor", 0.2),
                new ItemLoot("Magic Potion", 0.3)
            )
         .Init("Crawling Green Spider",
                new State(
                    new Prioritize(
                        new Follow(.6, 11, 1),
                        new Wander(.4)
                    ),
                    new Shoot(8, 3, 10, coolDown: 400)
                ),
                new ItemLoot("Healing Ichor", 0.2),
                new ItemLoot("Magic Potion", 0.3)
            )
         .Init("Yellow Son of Arachna Giant Egg Sac",
                new State(
                    new TransformOnDeath("Yellow Egg Summoner"),
                new State("Spawn",
                    new Spawn("Crawling Green Spider", 2),
                    new EntityNotExistsTransition("Crawling Green Spider", 20, "Spawn2")
                    ),
                new State("Spawn2",
                    new Spawn("Crawling Grey Spider", 2),
                    new EntityNotExistsTransition("Crawling Grey Spider", 20, "Spawn3")
                    ),
                new State("Spawn3",
                    new Spawn("Crawling Red Spotted Spider", 2),
                    new EntityNotExistsTransition("Crawling Red Spotted Spider", 20, "Spawn4")
                    ),
                 new State("Spawn4",
                    new Spawn("Crawling Spider Hatchling", 2),
                    new EntityNotExistsTransition("Crawling Spider Hatchling", 20, "Spawn5")
                     ),
                 new State("Spawn5",
                    new Spawn("Crawling Grey Spotted Spider", 2),
                    new EntityNotExistsTransition("Crawling Grey Spotted Spider", 20, "Spawn")
            )),
            new Threshold(0.15,
                new TierLoot(11, ItemType.Weapon, 0.1),
                new TierLoot(12, ItemType.Armor, 0.1),
                new ItemLoot("Doku No Ken", 0.015),
                new ItemLoot("Wine Cellar Incantation", 0.015)
                )
            )
         .Init("Blue Son of Arachna Giant Egg Sac",
                new State(
                    new State("DeathSpawn",
                    new TransformOnDeath("Crawling Spider Hatchling", 5, 7)

            )),
            new Threshold(0.15,
                new TierLoot(11, ItemType.Weapon, 0.1),
                new TierLoot(12, ItemType.Armor, 0.1),
                new ItemLoot("Doku No Ken", 0.015),
                new ItemLoot("Wine Cellar Incantation", 0.015)
                )
            )
         .Init("Red Son of Arachna Giant Egg Sac",
                new State(
                    new State("DeathSpawn",
                    new TransformOnDeath("Crawling Red Spotted Spider", 3, 3)

            )),
            new Threshold(0.15,
                new TierLoot(11, ItemType.Weapon, 0.1),
                new TierLoot(12, ItemType.Armor, 0.1),
                new ItemLoot("Doku No Ken", 0.015),
                new ItemLoot("Wine Cellar Incantation", 0.015)
                )
            )
         .Init("Silver Son of Arachna Giant Egg Sac",
                new State(
                    new State("DeathSpawn",
                    new TransformOnDeath("Crawling Grey Spider", 3, 3)

            )),
            new Threshold(0.15,
                new TierLoot(11, ItemType.Weapon, 0.1),
                new TierLoot(12, ItemType.Armor, 0.1),
                new ItemLoot("Doku No Ken", 0.015),
                new ItemLoot("Wine Cellar Incantation", 0.015)
                )
            )
         .Init("Silver Egg Summoner",
                new State(
                    new ConditionalEffect(ConditionEffectIndex.Invincible)
                    )
            )
         .Init("Yellow Egg Summoner",
                new State(
                    new ConditionalEffect(ConditionEffectIndex.Invincible)
                    )
            )
         .Init("Red Egg Summoner",
                new State(
                    new ConditionalEffect(ConditionEffectIndex.Invincible)
                    )
            )
         .Init("Blue Egg Summoner",
                new State(
                    new ConditionalEffect(ConditionEffectIndex.Invincible)
                    )
            )
         .Init("Epic Arachna Web Spoke 1",
                new State(
                    new EntityNotExistsTransition("Son of Arachna", 30, "die"),
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                new Shoot(200, 1, fixedAngle: 180, coolDown: 1500),
                new Shoot(200, 1, fixedAngle: 120, coolDown: 1500),
                new Shoot(200, 1, fixedAngle: 240, coolDown: 1500),
                    new State("die",
                        new Suicide()
                    )
                    )
            )
           .Init("Epic Arachna Web Spoke 2",
                new State(
                    new EntityNotExistsTransition("Son of Arachna", 30, "die"),
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                new Shoot(200, 1, fixedAngle: 240, coolDown: 1500),
                new Shoot(200, 1, fixedAngle: 180, coolDown: 1500),
                new Shoot(200, 1, fixedAngle: 300, coolDown: 1500),
                    new State("die",
                        new Suicide()
                    )
                    )
            )
           .Init("Epic Arachna Web Spoke 3",
                new State(
                    new EntityNotExistsTransition("Son of Arachna", 30, "die"),
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                new Shoot(200, 1, fixedAngle: 300, coolDown: 1500),
                new Shoot(200, 1, fixedAngle: 240, coolDown: 1500),
                new Shoot(200, 1, fixedAngle: 0, coolDown: 1500),
                    new State("die",
                        new Suicide()
                    )
                    )
            )
           .Init("Epic Arachna Web Spoke 4",
                new State(
                    new EntityNotExistsTransition("Son of Arachna", 30, "die"),
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                new Shoot(200, 1, fixedAngle: 0, coolDown: 1500),
                new Shoot(200, 1, fixedAngle: 60, coolDown: 1500),
                new Shoot(200, 1, fixedAngle: 300, coolDown: 1500),
                    new State("die",
                        new Suicide()
                    )
                    )
            )
           .Init("Epic Arachna Web Spoke 5",
                new State(
                    new EntityNotExistsTransition("Son of Arachna", 30, "die"),
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                new Shoot(200, 1, fixedAngle: 60, coolDown: 1500),
                new Shoot(200, 1, fixedAngle: 0, coolDown: 1500),
                new Shoot(200, 1, fixedAngle: 120, coolDown: 1500),
                    new State("die",
                        new Suicide()
                    )
     )
            )
           .Init("Epic Arachna Web Spoke 6",
                new State(
                    new EntityNotExistsTransition("Son of Arachna", 30, "die"),
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                new Shoot(200, 1, fixedAngle: 120, coolDown: 1500),
                new Shoot(200, 1, fixedAngle: 60, coolDown: 1500),
                new Shoot(200, 1, fixedAngle: 180, coolDown: 1500),
                    new State("die",
                        new Suicide()
                    )
                    )
            )
           .Init("Epic Arachna Web Spoke 7",
                new State(
                    new EntityNotExistsTransition("Son of Arachna", 30, "die"),
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                new Shoot(200, 1, fixedAngle: 180, coolDown: 1500),
                new Shoot(200, 1, fixedAngle: 120, coolDown: 1500),
                new Shoot(200, 1, fixedAngle: 240, coolDown: 1500),
                    new State("die",
                        new Suicide()
                    )
                    )
            )
           .Init("Epic Arachna Web Spoke 8",
                new State(
                    new EntityNotExistsTransition("Son of Arachna", 30, "die"),
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                new Shoot(200, 1, fixedAngle: 360, coolDown: 1500),
                new Shoot(200, 1, fixedAngle: 240, coolDown: 1500),
                new Shoot(200, 1, fixedAngle: 300, coolDown: 1500),
                    new State("die",
                        new Suicide()
                    )
                    )
            )
           .Init("Epic Arachna Web Spoke 9",
                new State(
                    new EntityNotExistsTransition("Son of Arachna", 30, "die"),
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                new Shoot(200, 1, fixedAngle: 0, coolDown: 1500),
                new Shoot(200, 1, fixedAngle: 60, coolDown: 1500),
                new Shoot(200, 1, fixedAngle: 120, coolDown: 1500),
                    new State("die",
                        new Suicide()
                    )
                    )
            );
    }
}

//Not Gonna Lie, Decided to take this from the LRv2 Source. I mean, why spend time on this when I can dedicate my resources elsewhere.