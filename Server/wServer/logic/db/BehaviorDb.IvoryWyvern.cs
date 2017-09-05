#region

using wServer.logic.behaviors;
using wServer.logic.transitions;
using wServer.logic.loot;

#endregion

namespace wServer.logic
{
    partial class BehaviorDb
    {
        private _ TheIvoryWyvern = () => Behav()
            .Init("lod Ivory Wyvern",
            new State(
                new HpLessTransition(0.05, "death"),
                new State("Idle",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new PlayerWithinTransition(10, "talk")
                    ),
                new State("talk",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new Taunt("Thank you adventurer, you have freed the souls of the four dragons so that I may consume their powers."),
                    new TimedTransition(3000, "talk2")
                    ),
                new State("talk2",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new Taunt("I owe you much, but I cannot let you leave. These walls will make a fine graveyard for your bones."),
                    new TimedTransition(3000, "toss")
                    ),
                new State("toss",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new TossObject("lod Mirror Wyvern1", 4, 360, coolDown: 300000, randomToss: false),
                    new TossObject("lod Mirror Wyvern1", 8, 360, coolDown: 300000, randomToss: false),
                    new TossObject("lod Mirror Wyvern1", 4, 180, coolDown: 300000, randomToss: false),
                    new TossObject("lod Mirror Wyvern1", 8, 180, coolDown: 300000, randomToss: false),
                    new MoveTo(0, 1, 1, isMapPosition: false, once: true),
                    new TimedTransition(2000, "start")
                    ),
                new State("start",
                    new MoveTo(6, 0, 0.5, isMapPosition: false, once: true),
                    new Shoot(20, 1, 0, projectileIndex: 0, coolDown: 1600),
                    new Shoot(20, 1, 0, projectileIndex: 0, coolDown: 1600, coolDownOffset: 200),
                    new Shoot(20, 1, 0, projectileIndex: 0, coolDown: 1600, coolDownOffset: 400),
                    new Shoot(20, 1, 0, projectileIndex: 0, coolDown: 1600, coolDownOffset: 600),
                    new Shoot(20, 1, 0, projectileIndex: 0, coolDown: 1600, coolDownOffset: 800),
                    new Shoot(20, 1, 0, projectileIndex: 0, coolDown: 1600, coolDownOffset: 1000),
                    new Shoot(20, 1, 0, projectileIndex: 0, coolDown: 1600, coolDownOffset: 1200),
                    new Shoot(20, 1, 0, projectileIndex: 0, coolDown: 1600, coolDownOffset: 1400),
                    new Shoot(20, 1, 0, projectileIndex: 0, coolDown: 1600, coolDownOffset: 1600),
                    new Shoot(20, 1, 0, projectileIndex: 0, coolDown: 1600, coolDownOffset: 1800),
                    new Shoot(20, 1, 0, projectileIndex: 0, coolDown: 1600, coolDownOffset: 2000),
                    new Shoot(20, 1, 0, projectileIndex: 0, coolDown: 1600, coolDownOffset: 2200),
                    new Shoot(20, 1, 0, projectileIndex: 0, coolDown: 1600, coolDownOffset: 2400),
                    new Shoot(20, 1, 0, projectileIndex: 0, coolDown: 1600, coolDownOffset: 2600),
                    new Shoot(20, 1, 0, projectileIndex: 0, coolDown: 1600, coolDownOffset: 2800),
                    new Shoot(20, 1, 0, projectileIndex: 0, coolDown: 1600, coolDownOffset: 3000),
                    new Shoot(20, 1, 0, projectileIndex: 0, coolDown: 1600, coolDownOffset: 3200),
                    new Shoot(20, 1, 0, projectileIndex: 0, coolDown: 1600, coolDownOffset: 3400),
                    new Shoot(20, 1, 0, projectileIndex: 0, coolDown: 1600, coolDownOffset: 3600),
                    new Shoot(20, 1, 0, projectileIndex: 0, coolDown: 1600, coolDownOffset: 3800),
                    new Shoot(20, 1, 0, projectileIndex: 0, coolDown: 1600, coolDownOffset: 4000),
                    new Shoot(20, 1, 0, projectileIndex: 0, coolDown: 1600, coolDownOffset: 4200),
                    new Shoot(20, 1, 0, projectileIndex: 0, coolDown: 1600, coolDownOffset: 4400),
                    new Shoot(20, 1, 0, projectileIndex: 0, coolDown: 1600, coolDownOffset: 4600),
                    new Shoot(20, 1, 0, projectileIndex: 0, coolDown: 1600, coolDownOffset: 4800),
                    new Shoot(20, 1, 0, projectileIndex: 0, coolDown: 1600, coolDownOffset: 5000),
                    new TimedTransition(5000, "return")
                    ),
                new State("return",
                    new ReturnToSpawn(true, 0.9),
                    new TimedTransition(2000, "prestart2")
                    ),
                new State("prestart2",
                    new MoveTo(0, 1, 1, isMapPosition: false, once: true),
                    new TimedTransition(100, "start2")
                    ),
                new State("start2",
                    new MoveTo(-6, 0, 0.5, isMapPosition: false, once: true),
                    new Shoot(20, 1, 0, projectileIndex: 0, coolDown: 1600),
                    new Shoot(20, 1, 0, projectileIndex: 0, coolDown: 1600, coolDownOffset: 200),
                    new Shoot(20, 1, 0, projectileIndex: 0, coolDown: 1600, coolDownOffset: 400),
                    new Shoot(20, 1, 0, projectileIndex: 0, coolDown: 1600, coolDownOffset: 600),
                    new Shoot(20, 1, 0, projectileIndex: 0, coolDown: 1600, coolDownOffset: 800),
                    new Shoot(20, 1, 0, projectileIndex: 0, coolDown: 1600, coolDownOffset: 1000),
                    new Shoot(20, 1, 0, projectileIndex: 0, coolDown: 1600, coolDownOffset: 1200),
                    new Shoot(20, 1, 0, projectileIndex: 0, coolDown: 1600, coolDownOffset: 1400),
                    new Shoot(20, 1, 0, projectileIndex: 0, coolDown: 1600, coolDownOffset: 1600),
                    new Shoot(20, 1, 0, projectileIndex: 0, coolDown: 1600, coolDownOffset: 1800),
                    new Shoot(20, 1, 0, projectileIndex: 0, coolDown: 1600, coolDownOffset: 2000),
                    new Shoot(20, 1, 0, projectileIndex: 0, coolDown: 1600, coolDownOffset: 2200),
                    new Shoot(20, 1, 0, projectileIndex: 0, coolDown: 1600, coolDownOffset: 2400),
                    new Shoot(20, 1, 0, projectileIndex: 0, coolDown: 1600, coolDownOffset: 2600),
                    new Shoot(20, 1, 0, projectileIndex: 0, coolDown: 1600, coolDownOffset: 2800),
                    new Shoot(20, 1, 0, projectileIndex: 0, coolDown: 1600, coolDownOffset: 3000),
                    new Shoot(20, 1, 0, projectileIndex: 0, coolDown: 1600, coolDownOffset: 3200),
                    new Shoot(20, 1, 0, projectileIndex: 0, coolDown: 1600, coolDownOffset: 3400),
                    new Shoot(20, 1, 0, projectileIndex: 0, coolDown: 1600, coolDownOffset: 3600),
                    new Shoot(20, 1, 0, projectileIndex: 0, coolDown: 1600, coolDownOffset: 3800),
                    new Shoot(20, 1, 0, projectileIndex: 0, coolDown: 1600, coolDownOffset: 4000),
                    new Shoot(20, 1, 0, projectileIndex: 0, coolDown: 1600, coolDownOffset: 4200),
                    new Shoot(20, 1, 0, projectileIndex: 0, coolDown: 1600, coolDownOffset: 4400),
                    new Shoot(20, 1, 0, projectileIndex: 0, coolDown: 1600, coolDownOffset: 4600),
                    new Shoot(20, 1, 0, projectileIndex: 0, coolDown: 1600, coolDownOffset: 4800),
                    new Shoot(20, 1, 0, projectileIndex: 0, coolDown: 1600, coolDownOffset: 5000),
                    new TimedTransition(5000, "return2")
                    ),
                new State("return2",
                    new ReturnToSpawn(true, 0.9),
                    new TimedTransition(2000, "pre2ndphase")
                    ),
                new State("pre2ndphase",
                    new MoveTo(0, 1, 1, isMapPosition: false, once: true),
                    new TimedTransition(2000, "2ndphase")
                    ),
                new State("2ndphase",
                    new RemoveEntity(500, "lod Mirror Wyvern1"),
                    new TimedTransition(500, "pre2ndphasestart")
                    ),
                new State("pre2ndphasestart",
                    new MoveTo(6, 0, 0.5, isMapPosition: false, once: true),
                    new TimedTransition(1500, "2ndphasestart")
                    ),
                new State("2ndphasestart",
                        new Shoot(10, count: 8, fixedAngle: 90, shootAngle: 20, projectileIndex: 0, coolDown: 500),
                        new TimedTransition(3000, "prepre2ndphasestart2")
                    ),
                new State("prepre2ndphasestart2",
                    new ReturnToSpawn(true, 0.9),
                    new TimedTransition(2000, "preprepre2ndphasestart2")
                    ),
                new State("preprepre2ndphasestart2",
                    new MoveTo(0, 1, 1, isMapPosition: false, once: true),
                    new TimedTransition(500, "pre2ndphasestart2")
                    ),
                new State("pre2ndphasestart2",
                    new MoveTo(-6, 0, 0.5, isMapPosition: false, once: true),
                    new TimedTransition(1500, "2ndphasestart2")
                    ),
                new State("2ndphasestart2",
                        new Shoot(10, count: 8, fixedAngle: 90, shootAngle: 20, projectileIndex: 0, coolDown: 500),
                        new TimedTransition(5000, "preflames")
                    ),
                new State("preflames",
                    new ReturnToSpawn(true, 0.9),
                    new RemoveEntity(420, "lod Mirror Wyvern1"),
                    new TimedTransition(2000, "flames")
                    ),
                new State("flames",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new TossObject("lod Green Soul Flame", 4, 180, coolDown: 300000, randomToss: false),
                    new TossObject("lod Blue Soul Flame", 8, 180, coolDown: 300000, randomToss: false),
                    new TossObject("lod Red Soul Flame", 4, 360, coolDown: 300000, randomToss: false),
                    new TossObject("lod Black Soul Flame", 8, 360, coolDown: 300000, randomToss: false),
                    new TimedTransition(3000, "Wait")
                    ),
                new State("Wait",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new EntitiesNotExistsTransition(500, "preMiddle", "lod Green Soul Flame", "lod Red Soul Flame", "lod Blue Soul Flame", "lod Black Soul Flame")
                    ),
                new State("preMiddle",
                    new ReturnToSpawn(true, 0.9),
                    new TimedTransition(100, "Middle")
                    ),
                new State("Middle",
                    new MoveTo(0, 12, 1, isMapPosition: false, once: true),
                    new TimedTransition(2000, "TossShootShit")
                    ),
                new State("TossShootShit",
                    new TossObject("lod White Dragon Orb", 12, 45, coolDown: 300000, randomToss: false),
                    new TossObject("lod White Dragon Orb", 12, 135, coolDown: 300000, randomToss: false),
                    new TossObject("lod White Dragon Orb", 12, 225, coolDown: 300000, randomToss: false),
                    new TossObject("lod White Dragon Orb", 12, 315, coolDown: 300000, randomToss: false),
                    new TimedTransition(1000, "ShootShit")
                    ),
                new State("ShootShit",
                    new Shoot(10, 5, 20, projectileIndex: 1, fixedAngle: 0, coolDown: 700),
                    new Shoot(10, 5, 20, projectileIndex: 2, fixedAngle: 90, coolDown: 700),
                    new Shoot(10, 5, 20, projectileIndex: 3, fixedAngle: 180, coolDown: 700),
                    new Shoot(10, 5, 20, projectileIndex: 4, fixedAngle: 270, coolDown: 700),
                    new TimedTransition(700, "ShootShit2")
                    ),
                new State("ShootShit2",
                    new Shoot(10, 5, 20, projectileIndex: 1, fixedAngle: 90, coolDown: 700),
                    new Shoot(10, 5, 20, projectileIndex: 2, fixedAngle: 180, coolDown: 700),
                    new Shoot(10, 5, 20, projectileIndex: 3, fixedAngle: 270, coolDown: 700),
                    new Shoot(10, 5, 20, projectileIndex: 4, fixedAngle: 0, coolDown: 700),
                    new TimedTransition(700, "ShootShit3")
                    ),
                new State("ShootShit3",
                    new Shoot(10, 5, 20, projectileIndex: 1, fixedAngle: 180, coolDown: 700),
                    new Shoot(10, 5, 20, projectileIndex: 2, fixedAngle: 270, coolDown: 700),
                    new Shoot(10, 5, 20, projectileIndex: 3, fixedAngle: 0, coolDown: 700),
                    new Shoot(10, 5, 20, projectileIndex: 4, fixedAngle: 90, coolDown: 700),
                    new TimedTransition(700, "ShootShit4")
                    ),
                new State("ShootShit4",
                    new Shoot(10, 5, 20, projectileIndex: 1, fixedAngle: 270, coolDown: 700),
                    new Shoot(10, 5, 20, projectileIndex: 2, fixedAngle: 0, coolDown: 700),
                    new Shoot(10, 5, 20, projectileIndex: 3, fixedAngle: 90, coolDown: 700),
                    new Shoot(10, 5, 20, projectileIndex: 4, fixedAngle: 180, coolDown: 700),
                    new TimedTransition(700, "ShootShit")
                    ),
                new State("death",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new Taunt("You have defeated me, hero!"),
                    new TimedTransition(2000, "Suicide")
                    ),
                new State("Suicide",
                    new Suicide()
                    )
                )
            )
        .Init("lod Mirror Wyvern1",
                new State(
                    new State("Idle",
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                    new Shoot(20, 1, 0, projectileIndex: 0, coolDown: 200)
                        )
                    )
            )
        .Init("lod Green Soul Flame",
            new State(
                new Shoot(10, count: 8, fixedAngle: 90, shootAngle: 20, projectileIndex: 0, coolDown: 500)
                )
            )
        .Init("lod Red Soul Flame",
            new State(
                new Shoot(10, count: 8, fixedAngle: 90, shootAngle: 20, projectileIndex: 0, coolDown: 500)
                )
            )
        .Init("lod Black Soul Flame",
            new State(
                new Shoot(10, count: 8, fixedAngle: 90, shootAngle: 20, projectileIndex: 0, coolDown: 500)
                )
            )
        .Init("lod Blue Soul Flame",
            new State(
                new Shoot(10, count: 8, fixedAngle: 90, shootAngle: 20, projectileIndex: 0, coolDown: 500)
                )
            )
        .Init("lod White Dragon Orb",
            new State(
                new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                new State("Idle",
                    new Shoot(25, 20, projectileIndex: 0, shootAngle: 18, fixedAngle: 18, coolDown: 300)
                    )
                )
            )

        ;
    }
}