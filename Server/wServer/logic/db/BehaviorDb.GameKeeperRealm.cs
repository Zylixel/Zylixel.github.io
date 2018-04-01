using wServer.logic.behaviors;
using wServer.logic.loot;
using wServer.logic.transitions;

namespace wServer.logic
{
    partial class BehaviorDb
    {
        private _ GameKeeperRealm = () => Behav()
        .Init("Keeper Minion Rise",
            new State(
                new State("0",
                    new SetAltTexture(0),
                    new EntityNotExistsTransition("Keeper Minion Activator", 50000, "1")
                    ),
                new State("1",
                    new SetAltTexture(1),
                    new TimedTransition(200, "2")
                    ),
                new State("2",
                    new SetAltTexture(2),
                    new TimedTransition(200, "3")
                    ),
                new State("3",
                    new SetAltTexture(3),
                    new TimedTransition(200, "4")
                    ),
                new State("4",
                    new SetAltTexture(4),
                    new TimedTransition(200, "5")
                    ),
                new State("5",
                    new SetAltTexture(5),
                    new TimedTransition(200, "6")
                    ),
                new State("6",
                    new SetAltTexture(6),
                    new TimedTransition(200, "7")
                    ),
                new State("7",
                    new SetAltTexture(7),
                    new TimedTransition(200, "8")
                    ),
                new State("8",
                    new SetAltTexture(8),
                    new TimedTransition(200, "9")
                    ),
                new State("9",
                    new SetAltTexture(9),
                    new TimedTransition(200, "10")
                    ),
                new State("10",
                    new SetAltTexture(10),
                    new TimedTransition(200, "11")
                    ),
                new State("11",
                    new SetAltTexture(11),
                    new TimedTransition(200, "12")
                    ),
                new State("12",
                    new SetAltTexture(12),
                    new TimedTransition(200, "13")
                    ),
                new State("13",
                    new SetAltTexture(13),
                    new TimedTransition(10, "spawn")
                    ),
                new State("spawn",
                    new Spawn("Keeper Minion", 1),
                    new Suicide()
                    )
                )
            )
            .Init("Keeper Minion",
            new State(
                new State("begin",
                     new Follow(1.5, range: 1, acquireRange: 99),
                     new Shoot(100, projectileIndex: 0, count: 1, coolDown: 75),
                     new HpLessTransition(0.2, "dosomethingcool")
                    ),
                new State("dosomethingcool",
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new Flash(0xffffff, 0.5, 6),
                    new SpecificHeal(1, 50, "Self", 500),
                    new TimedTransition(3000, "gettemAgain")
                    ),
                new State("gettemAgain",
                    new Follow(1.5, range: 1, acquireRange: 99),
                    new Shoot(100, projectileIndex: 1, count: 1, coolDown: 75)
                    )
                )
            )
            .Init("Keeper Minion Activator",
            new State(
                new State("begin",
                     new TimedTransition(5000, "talk")
                    ),
                new State("talk",
                    new Taunt(true, "Bored without anything to ruin? I’ll give you some excitement."),
                    new TimedTransition(1000, "die")
                    ),
                new State("die",
                    new Suicide()
                    )
                )
            )
            .Init("Keeper Defender",
                new State(
                    new State("begin"
                        ),
                    new State("die",
                        new Decay()
                    )
                    )
            )
        .Init("Keeper Attacker",
                new State(
                    new State("begin"
                        ),
                    new State("die",
                        new Decay()
                    )
                    )
            )
        .Init("Keeper Support",
                new State(
                    new State("begin"
                        ),
                    new State("die",
                        new Decay()
                    )
                    )
            )
        .Init("Keeper Boss Anchor",
                new State(
                    new State("begin",
                        new Spawn("Keeper Gilgor Boss", 1)
                        )
                    )
            )
            .Init("Keeper Gods Activator",
            new State(
                new State("begin",
                    new EntitiesNotExistsTransition(999, "talk", "Keeper Minion Activator", "Keeper Minion", "Keeper Minion Rise")
                    ),
                new State("talk",
                    new Taunt(true, "You survived my minions? No problem, they were the weakest of the bunch!"),
                    new TimedTransition(2000, "talk1")
                    ),
                new State("talk1",
                    new Taunt(true, "I AM GOD! LET THERE BE LIGHT!"),
                    new ApplySetpiece("KeeperGodland"),
                    new TimedTransition(2000, "talk2")
                    ),
                new State("talk2",
                    new Taunt(true, "Let’s see what you can do against the gods you have killed!"),
                    new EntitiesNotExistsTransition(999, "godsDead", "beholder", "ent god", "leviathan", "White Demon", "Slime God", "native sprite god", "medusa", "ghost god")
                    ),
                new State("godsDead",
                    new Taunt(true, "Fine, you may have killed my gods, but you are forgetting one thing..."),
                    new TimedTransition(2000, "godsDead2")
                    ),
                new State("godsDead2",
                    new Taunt(true, "THIS REALM IS MINE"),
                    new TimedTransition(4000, "KeeperClose")
                    ),
                new State("KeeperClose",
                    new ApplySetpiece("KeeperClose"),
                    new TimedTransition(3700, "KeeperClose1")
                    ),
                new State("KeeperClose1",
                    new ApplySetpiece("KeeperClose1"),
                    new TimedTransition(3000, "KeeperClose2")
                    ),
                new State("KeeperClose2",
                    new ApplySetpiece("KeeperClose2"),
                    new TimedTransition(3000, "KeeperClose3")
                    ),
                new State("KeeperClose3",
                    new ApplySetpiece("KeeperClose3"),
                    new TimedTransition(3000, "Tomb")
                    ),
                new State("Tomb",
                    new ApplySetpiece("KeeperTomb")
                    ),
                new State("Shatters",
                    new ApplySetpiece("KeeperShatters")
                    ),
                new State("Main",
                    new ApplySetpiece("KeeperMain")
                    ),
                new State("Oryx",
                    new ApplySetpiece("KeeperOryx")
                    )
                )
            )
        .Init("Keeper Gilgor Boss Appear",
                new State(
                    new State("init",
                        new TimedTransition(700, "die")
                        ),
                    new State("die",
                        new Suicide()
                        )
                    )
            )
        .Init("Keeper Gilgor Boss Disappear",
                new State(
                    new State("init",
                        new TimedTransition(700, "die")
                        ),
                    new State("die",
                        new Suicide()
                        )
                    )
            )

        .Init("Pillar of Gilgor",
                new State(
                    new EntityNotExistsTransition("Keeper Gilgor Boss", 999, "die"),
                    new State("begin",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable, true),
                        new Flash(0xffffff, 0.5, 6)
                        ),
                    new State("Shoot",
                        new Shoot(99, 1, coolDown: 3000, coolDownOffset: 1000)
                    ),
                    new State("blowthemup",
                        new TossObject("shtrs Firebomb", 1, randomToss: true, coolDown: 5000),
                        new TossObject("shtrs Firebomb", 2, randomToss: true, coolDown: 5000),
                        new TossObject("shtrs Firebomb", 3, randomToss: true, coolDown: 5000),
                        new TossObject("shtrs Firebomb", 4, randomToss: true, coolDown: 5000),
                        new TossObject("shtrs Firebomb", 5, randomToss: true, coolDown: 5000),
                        new TossObject("shtrs Firebomb", 6, randomToss: true, coolDown: 5000),
                        new TossObject("shtrs Firebomb", 6, randomToss: true, coolDown: 5000),
                        new TossObject("shtrs Firebomb", 5, randomToss: true, coolDown: 5000),
                        new TossObject("shtrs Firebomb", 4, randomToss: true, coolDown: 5000),
                        new TossObject("shtrs Firebomb", 3, randomToss: true, coolDown: 5000),
                        new TossObject("shtrs Firebomb", 2, randomToss: true, coolDown: 5000)
                    ),
                    new State("die",
                        new Suicide()
                    )
                    )
            )

        .Init("Pillar of Gilgor2",
                new State(
                    new EntityNotExistsTransition("Keeper Gilgor Boss", 999, "die"),
                    new State("begin",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable, true),
                        new Flash(0xffffff, 0.5, 6),
                        new TimedTransition(3000, "Shoot")
                        ),
                    new State("Shoot",
                        new Shoot(99, 1, coolDown: 3000, coolDownOffset: 1000)
                    ),
                    new State("blowthemup",
                        new TossObject("shtrs Firebomb", 1, randomToss: true, coolDown: 5000),
                        new TossObject("shtrs Firebomb", 2, randomToss: true, coolDown: 5000),
                        new TossObject("shtrs Firebomb", 3, randomToss: true, coolDown: 5000),
                        new TossObject("shtrs Firebomb", 4, randomToss: true, coolDown: 5000),
                        new TossObject("shtrs Firebomb", 5, randomToss: true, coolDown: 5000),
                        new TossObject("shtrs Firebomb", 6, randomToss: true, coolDown: 5000),
                        new TossObject("shtrs Firebomb", 6, randomToss: true, coolDown: 5000),
                        new TossObject("shtrs Firebomb", 5, randomToss: true, coolDown: 5000),
                        new TossObject("shtrs Firebomb", 4, randomToss: true, coolDown: 5000),
                        new TossObject("shtrs Firebomb", 3, randomToss: true, coolDown: 5000),
                        new TossObject("shtrs Firebomb", 2, randomToss: true, coolDown: 5000)
                    ),
                    new State("die",
                        new Suicide()
                    )
                    )
            )

        .Init("Minion of Gilgor",
                new State(
                    new State("init"),
                    new State("begin",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new Flash(0xffffff, 0.5, 6),
                        new TimedTransition(3000, "gettem")
                        ),
                    new State("gettem",
                        new Follow(1, 99, 1),
                        new Shoot(99, 1, coolDown: 500)
                    )
                    )
            )

        .Init("Minion of Gilgor2",
                new State(
                    new EntityNotExistsTransition("Keeper Gilgor Boss", 999, "die"),
                    new State("begin",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new Flash(0xffffff, 0.5, 6),
                        new TimedTransition(3000, "gettem")
                        ),
                    new State("gettem",
                        new Follow(1, 99, 1),
                        new Shoot(99, 1, coolDown: 500)
                    ),
                    new State("die",
                        new Suicide()
                        )
                    )
            )

        .Init("Keeper Craig",
                new State(
                    new RealmPortalDrop(),
                    new State("begin",
                        new ConditionalEffect(ConditionEffectIndex.Invincible, true),
                        new TimedTransition(10000, "ohhi")
                        ),
                    new State("ohhi",
                        new SetAltTexture(1),
                        new Taunt(true, "Oh, ummm this is awkward..."),
                        new TimedTransition(3000, "explain")
                    ),
                    new State("explain",
                        new Taunt(true, "Apparently this guy was some sort of phony, Oryx said that Gilgor was just one of the Realm Keeper's pawns..."),
                        new TimedTransition(3000, "explain2")
                    ),
                    new State("explain2",
                        new Taunt(true, "Anyway, I found some treasure, take it!"),
                        new TimedTransition(3000, "explain3")
                    ),
                    new State("explain3",
                        new TossObject("The Tainted Marked Spot", 1, randomToss: true, coolDown: 500000),
                        new TossObject("The Tainted Marked Spot", 2, randomToss: true, coolDown: 500000),
                        new TossObject("The Tainted Marked Spot", 3, randomToss: true, coolDown: 500000),
                        new TossObject("The Tainted Marked Spot", 4, randomToss: true, coolDown: 500000),
                        new TossObject("The Tainted Marked Spot", 5, randomToss: true, coolDown: 500000),
                        new TossObject("The Tainted Marked Spot", 6, randomToss: true, coolDown: 500000),
                        new TimedTransition(3000, "die")
                    ),
                    new State("die",
                        new Suicide()
                    )
                    )
            )

        .Init("Gilgor Artifact",
              new State(
                 new Prioritize(
                    new Orbit(1, 3, 30, "Keeper Gilgor Boss")
                 ),
                    new Shoot(4, 1, coolDown: 500, predictive: 1)
                 )
                 )

        .Init("Keeper Gilgor Boss",
                new State(
                    new TransformOnDeath("Keeper Craig"),
                    new State("init",
                        new ConditionalEffect(ConditionEffectIndex.Invincible, true),
                        new Spawn("Keeper Gilgor Boss Appear", 1),
                        new TimedTransition(800, "sneakIn0")
                        ),
                    new State("sneakIn0",
                        new SetAltTexture(1),
                        new Orbit(2, 19, target: "Keeper Boss Anchor", acquireRange: 30),
                        new TimedTransition(3000, "sneakIn1")
                        ),
                    new State("sneakIn1",
                        new Orbit(2, 18, target: "Keeper Boss Anchor", acquireRange: 30),
                        new TimedTransition(3000, "sneakIn2")
                        ),
                    new State("sneakIn2",
                        new Orbit(2, 17, target: "Keeper Boss Anchor", acquireRange: 30),
                        new TimedTransition(3000, "sneakIn3")
                        ),
                    new State("sneakIn3",
                        new Orbit(2, 16, target: "Keeper Boss Anchor", acquireRange: 30),
                        new TimedTransition(3000, "fadeOut1")
                        ),
                    new State("fadeOut1",
                        new SetAltTexture(2),
                        new Spawn("Keeper Gilgor Boss Disappear", 1),
                        new TimedTransition(800, "moveToSpawn1")
                        ),
                    new State("moveToSpawn1",
                        new ReturnToSpawn(true, 0.9),
                        new TimedTransition(4000, "moveToTomb")
                        ),
                    new State("moveToTomb",
                        new MoveTo(8, 9, 1, isMapPosition: false, once: true),
                        new TimedTransition(50, "fadeIn1")
                        ),
                    new State("fadeIn1",
                        new Spawn("Keeper Gilgor Boss Appear", 1),
                        new TimedTransition(700, "fadeIn2")
                        ),
                    new State("fadeIn2",
                        new SetAltTexture(1),
                        new TimedTransition(3000, "shootTomb1")
                        ),
                    new State("shootTomb1",
                        new Taunt(true, "Say goodbye to your precious Gods!"),
                        new Shoot(100, 1, projectileIndex: 0, shootAngle: 200, fixedAngle: 200, coolDown: 10000000),
                        new TimedTransition(500, "kill1")
                        ),
                    new State("kill1",
                        new Order(999, "Keeper Attacker", "die"),
                        new SetAltTexture(2),
                        new Spawn("Keeper Gilgor Boss Disappear", 1),
                        new TimedTransition(100, "moveToTomb2")
                        ),
                    new State("moveToTomb2",
                        new MoveTo(-2, -5),
                        new TimedTransition(500, "fadeIn3")
                        ),
                    new State("fadeIn3",
                        new Spawn("Keeper Gilgor Boss Appear", 1),
                        new TimedTransition(700, "fadeIn4")
                        ),
                    new State("fadeIn4",
                        new SetAltTexture(1),
                        new TimedTransition(200, "shootTomb2")
                        ),
                    new State("shootTomb2",
                        new Shoot(100, 1, projectileIndex: 0, shootAngle: 200, fixedAngle: 200, coolDown: 10000000),
                        new TimedTransition(200, "kill2")
                        ),
                    new State("kill2",
                        new Order(999, "Keeper Defender", "die"),
                        new SetAltTexture(2),
                        new Spawn("Keeper Gilgor Boss Disappear", 1),
                        new TimedTransition(100, "moveToTomb3")
                        ),
                    new State("moveToTomb3",
                        new MoveTo(5, 3),
                        new TimedTransition(500, "fadeIn5")
                        ),
                    new State("fadeIn5",
                        new Spawn("Keeper Gilgor Boss Appear", 1),
                        new TimedTransition(700, "fadeIn6")
                        ),
                    new State("fadeIn6",
                        new SetAltTexture(1),
                        new TimedTransition(200, "shootTomb3")
                        ),
                    new State("shootTomb3",
                        new Shoot(100, 1, projectileIndex: 0, shootAngle: 200, fixedAngle: 200, coolDown: 10000000),
                        new TimedTransition(200, "kill3")
                        ),
                    new State("kill3",
                        new Order(999, "Keeper Support", "die"),
                        new Taunt(true, "By killing the gracious ones who provide life, your offspring will be weakened!"),
                        new TimedTransition(2500, "disappear")
                        ),
                    new State("disappear",
                        new SetAltTexture(2),
                        new Spawn("Keeper Gilgor Boss Disappear", 1),
                        new TimedTransition(500, "initShatts")
                        ),
                     new State("initShatts",
                        new Order(999, "Keeper Gods Activator", "Main"),
                        new ReturnToSpawn(true, 1),
                        new TimedTransition(1000, "fadeIn7")
                        ),
                     new State("fadeIn7",
                        new Spawn("Keeper Gilgor Boss Appear", 1),
                        new TimedTransition(700, "fadeIn8")
                        ),
                    new State("fadeIn8",
                        new SetAltTexture(1),
                        new TimedTransition(200, "LetsDoThis2")
                        ),
                    new State("LetsDoThis2",
                        new Taunt(true, "I summon my pillars from hell to take you to their creator"),
                        new TimedTransition(2000, "ItsHappening")
                        ),
                    new State("ItsHappening",
                        new TossObject("Pillar of Gilgor", 8, 0, 1000000),
                        new TossObject("Pillar of Gilgor", 8, 180, 1000000),
                        new ConditionalEffect(ConditionEffectIndex.Invincible),
                        new TimedTransition(3000, "ShootThePillars1")
                            ),
                    new State("ShootThePillars1",
                        new Taunt(true, "FIRE!"),
                        new Order(99, "Pillar Of Gilgor", "Shoot"),
                        new HpLessTransition(0.9, "getAngery")
                        ),
                    new State("getAngery",
                        new SetAltTexture(3),
                        new ConditionalEffect(ConditionEffectIndex.Invincible, true),
                        new Taunt(true, "MINIONS, STOP THEM!"),
                        new TossObject("Minion of Gilgor", 3, 0, 10000),
                        new TossObject("Minion of Gilgor", 3, 45, 10000, 200),
                        new TossObject("Minion of Gilgor", 3, 90, 10000, 400),
                        new TossObject("Minion of Gilgor", 3, 135, 10000, 600),
                        new TossObject("Minion of Gilgor", 3, 180, 10000, 800),
                        new TossObject("Minion of Gilgor", 3, 225, 10000, 1000),
                        new TossObject("Minion of Gilgor", 3, 270, 10000, 1200),
                        new TossObject("Minion of Gilgor", 3, 315, 10000, 1400),
                        new TimedTransition(2800, "startMinions")
                        ),
                    new State("startMinions",
                        new Order(999, "Minion of Gilgor", "begin"),
                        new TimedTransition(1, "getouttathere")
                        ),
                    new State("getouttathere",
                        new ConditionalEffect(ConditionEffectIndex.Invincible),
                        new EntitiesNotExistsTransition(999, "getAngery2", "Minion of Gilgor")
                        ),
                    new State("getAngery2",
                        new Taunt(true, "You. Will. Stop."),
                        new TossObject("Pillar of Gilgor2", 8, 90, 1000000),
                        new TossObject("Pillar of Gilgor2", 8, 270, 1000000),
                        new HpLessTransition(0.7, "oryxfake")
                        ),
                    new State("oryxfake",
                        new Order(999, "Keeper Gods Activator", "Oryx"),
                        new Taunt(true, "Puny mortals! My {HP} HP will annihilate you!"),
                        new Shoot(25, 30, projectileIndex: 7, coolDown: 90000001, coolDownOffset: 8000),
                        new Shoot(25, 30, projectileIndex: 8, coolDown: 90000001, coolDownOffset: 8500),
                        new Shoot(25, projectileIndex: 7, count: 8, shootAngle: 45, coolDown: 1500, coolDownOffset: 1500),
                        new Shoot(25, projectileIndex: 8, count: 3, shootAngle: 10, coolDown: 1000, coolDownOffset: 1000),
                        new Shoot(25, projectileIndex: 9, count: 3, shootAngle: 10, predictive: 0.2, coolDown: 1000,
                            coolDownOffset: 1000),
                        new Shoot(25, projectileIndex: 3, count: 2, shootAngle: 10, predictive: 0.4, coolDown: 1000,
                            coolDownOffset: 1000),
                        new Shoot(25, projectileIndex: 4, count: 3, shootAngle: 10, predictive: 0.6, coolDown: 1000,
                            coolDownOffset: 1000),
                        new Shoot(25, projectileIndex: 5, count: 2, shootAngle: 10, predictive: 0.8, coolDown: 1000,
                            coolDownOffset: 1000),
                        new Shoot(25, projectileIndex: 6, count: 3, shootAngle: 10, predictive: 1, coolDown: 1000,
                            coolDownOffset: 1000),
                        new SetAltTexture(6),
                        new Spawn("Henchman of Oryx", 4, coolDown: 5000),
                        new HpLessTransition(0.6, "blowthemtoshreds")
                        ),
                    new State("blowthemtoshreds",
                        new Order(999, "Keeper Gods Activator", "Shatters"),
                        new SetAltTexture(4),
                        new Taunt(true, "YOU TEST THE PATIENCE OF A GOD"),
                        new Order(999, "Pillar of Gilgor", "blowthemup"),
                        new Order(999, "Pillar of Gilgor2", "blowthemup"),
                        new HpLessTransition(0.5, "bes")
                        ),
                    new State("bes",
                        new Order(999, "Keeper Gods Activator", "Tomb"),
                        new Order(999, "Keeper Support", "die"),
                        new Order(999, "Keeper Attacker", "die"),
                        new Order(999, "Keeper Defender", "die"),
                        new Shoot(50, 8, 10, 7, coolDown: 4750, coolDownOffset: 500),
                        new Shoot(50, 4, 10, 9, coolDown: 300),
                        new Shoot(50, 3, 10, 8, coolDown: 4750, coolDownOffset: 500),
                        new Taunt(true, "My artifacts shall prove my wall of defense is impenetrable!"),
                        new SetAltTexture(9),
                        new Spawn("Gilgor Artifact", 3, 3, 2000),
                        new Reproduce("Gilgor Artifact", 10, 3, 1500),
                        new Order(999, "Pillar of Gilgor", "die"),
                        new Order(999, "Pillar of Gilgor2", "die"),
                        new HpLessTransition(0.4, "rage")
                        ),
                    new State("rage",
                        new SetAltTexture(7),
                        new Order(999, "Keeper Gods Activator", "Main"),
                        new Taunt(true, "ENOUGH"),
                        new StayCloseToSpawn(0.75, 3),
                        new Shoot(99, 1, projectileIndex: 1, shootAngle: 0, fixedAngle: 0, coolDown: 1800),
                        new Shoot(99, 1, projectileIndex: 1, shootAngle: 45, fixedAngle: 45, coolDown: 1800, coolDownOffset: 200),
                        new Shoot(99, 1, projectileIndex: 1, shootAngle: 90, fixedAngle: 90, coolDown: 1800, coolDownOffset: 400),
                        new Shoot(99, 1, projectileIndex: 1, shootAngle: 135, fixedAngle: 135, coolDown: 1800, coolDownOffset: 600),
                        new Shoot(99, 1, projectileIndex: 1, shootAngle: 180, fixedAngle: 180, coolDown: 1800, coolDownOffset: 800),
                        new Shoot(99, 1, projectileIndex: 1, shootAngle: 225, fixedAngle: 225, coolDown: 1800, coolDownOffset: 1000),
                        new Shoot(99, 1, projectileIndex: 1, shootAngle: 270, fixedAngle: 270, coolDown: 1800, coolDownOffset: 1200),
                        new Shoot(99, 1, projectileIndex: 1, shootAngle: 315, fixedAngle: 315, coolDown: 1800, coolDownOffset: 1400),
                        new TossObject("Minion of Gilgor2", 3, 0, 7000),
                        new TossObject("Minion of Gilgor2", 3, 45, 7000, 200),
                        new TossObject("Minion of Gilgor2", 3, 90, 7000, 400),
                        new TossObject("Minion of Gilgor2", 3, 135, 7000, 600),
                        new TossObject("Minion of Gilgor2", 3, 180, 7000, 800),
                        new TossObject("Minion of Gilgor2", 3, 225, 7000, 1000),
                        new TossObject("Minion of Gilgor2", 3, 270, 7000, 1200),
                        new TossObject("Minion of Gilgor2", 3, 315, 7000, 1400),
                        new HpLessTransition(0.2, "fakeit")
                        ),
                    new State("fakeit",
                        new SetAltTexture(8),
                        new ConditionalEffect(ConditionEffectIndex.Invincible),
                        new ReturnToSpawn(true, 0.5),
                        new Flash(0xff0000, 0.5, 8),
                        new Taunt("Hero, You have done the impossible, and you shall be rewarded with..."),
                        new TimedTransition(4000, "prankit")
                    ),
                    new State("prankit",
                        new ConditionalEffect(ConditionEffectIndex.Invincible),
                        new SetAltTexture(5),
                        new Taunt(true, "A SWIFT DEATH!!!"),
                        new TimedTransition(2000, "ragerage")
                        ),
                    new State("ragerage",
                        new SetAltTexture(8),
                        new ConditionalEffect(ConditionEffectIndex.Invincible),
                        new TossObject("Minion of Gilgor2", 3, 0, 1000000),
                        new TossObject("Minion of Gilgor2", 3, 45, 1000000, 200),
                        new TossObject("Minion of Gilgor2", 3, 90, 1000000, 400),
                        new TossObject("Minion of Gilgor2", 3, 135, 1000000, 600),
                        new TossObject("Minion of Gilgor2", 3, 180, 1000000, 800),
                        new TossObject("Minion of Gilgor2", 3, 225, 1000000, 1000),
                        new TossObject("Minion of Gilgor2", 3, 270, 1000000, 1200),
                        new TossObject("Minion of Gilgor2", 3, 315, 1000000, 1400),
                        new TossObject("Pillar of Gilgor2", 8, 0, 1000000),
                        new TossObject("Pillar of Gilgor2", 8, 180, 1000000),
                        new TossObject("Pillar of Gilgor2", 8, 90, 1000000),
                        new TossObject("Pillar of Gilgor2", 8, 270, 1000000),
                        new TimedTransition(3000, "BegintheChaos")
                        ),
                    new State("BegintheChaos",
                        new Order(999, "Pillar of Gilgor", "blowthemup"),
                        new Order(999, "Pillar of Gilgor2", "blowthemup"),
                        new TimedTransition(1, "KeepBegin")
                        ),
                    new State("KeepBegin",
                        new TossObject("Minion of Gilgor2", 3, 0, 7000),
                        new TossObject("Minion of Gilgor2", 3, 45, 7000, 200),
                        new TossObject("Minion of Gilgor2", 3, 90, 7000, 400),
                        new TossObject("Minion of Gilgor2", 3, 135, 7000, 600),
                        new TossObject("Minion of Gilgor2", 3, 180, 7000, 800),
                        new TossObject("Minion of Gilgor2", 3, 225, 7000, 1000),
                        new TossObject("Minion of Gilgor2", 3, 270, 7000, 1200),
                        new TossObject("Minion of Gilgor2", 3, 315, 7000, 1400),
                        new Shoot(99, 1, projectileIndex: 2, shootAngle: 0, fixedAngle: 0, coolDown: 1500),
                        new Shoot(99, 1, projectileIndex: 2, shootAngle: 22.5, fixedAngle: 22.5, coolDown: 1500),
                        new Shoot(99, 1, projectileIndex: 2, shootAngle: 45, fixedAngle: 45, coolDown: 1500, coolDownOffset: 100),
                        new Shoot(99, 1, projectileIndex: 2, shootAngle: 67.5, fixedAngle: 67.5, coolDown: 1500, coolDownOffset: 200),
                        new Shoot(99, 1, projectileIndex: 2, shootAngle: 90, fixedAngle: 90, coolDown: 1500, coolDownOffset: 300),
                        new Shoot(99, 1, projectileIndex: 2, shootAngle: 112.5, fixedAngle: 112.5, coolDown: 1500, coolDownOffset: 400),
                        new Shoot(99, 1, projectileIndex: 2, shootAngle: 135, fixedAngle: 135, coolDown: 1500, coolDownOffset: 500),
                        new Shoot(99, 1, projectileIndex: 2, shootAngle: 157.5, fixedAngle: 157.5, coolDown: 1500, coolDownOffset: 600),
                        new Shoot(99, 1, projectileIndex: 2, shootAngle: 180, fixedAngle: 180, coolDown: 1500, coolDownOffset: 700),
                        new Shoot(99, 1, projectileIndex: 2, shootAngle: 202.5, fixedAngle: 202.5, coolDown: 1500, coolDownOffset: 800),
                        new Shoot(99, 1, projectileIndex: 2, shootAngle: 225, fixedAngle: 225, coolDown: 1500, coolDownOffset: 900),
                        new Shoot(99, 1, projectileIndex: 2, shootAngle: 247.5, fixedAngle: 247.5, coolDown: 1500, coolDownOffset: 1000),
                        new Shoot(99, 1, projectileIndex: 2, shootAngle: 270, fixedAngle: 270, coolDown: 1500, coolDownOffset: 1100),
                        new Shoot(99, 1, projectileIndex: 2, shootAngle: 292.5, fixedAngle: 292.5, coolDown: 1500, coolDownOffset: 1200),
                        new Shoot(99, 1, projectileIndex: 2, shootAngle: 315, fixedAngle: 315, coolDown: 1500, coolDownOffset: 1300),
                        new Shoot(99, 1, projectileIndex: 2, shootAngle: 337.5, fixedAngle: 337.5, coolDown: 1500, coolDownOffset: 1400),
                        new HpLessTransition(0.02, "cya")
                        ),
                    new State("cya",
                        new Taunt(true, "This shouldn't be possible..."),
                        new Flash(0xff0000, 1, 10),
                        new ConditionalEffect(ConditionEffectIndex.Invincible, true),
                        new TimedTransition(5000, "die")
                        ),
                    new State("die",
                        new Suicide()
                        )
            ),
            new Threshold(0.025,
                    new TierLoot(12, ItemType.Weapon, 0.4),
                    new TierLoot(13, ItemType.Weapon, 0.3),
                    new TierLoot(6, ItemType.Ability, 0.4),
                    new TierLoot(7, ItemType.Ability, 0.2),
                    new TierLoot(13, ItemType.Armor, 0.4),
                    new TierLoot(14, ItemType.Armor, 0.3),
                    new TierLoot(6, ItemType.Ring, 0.4),
                    new TierLoot(7, ItemType.Ring, 0.3),
                    //new ItemLoot("Tainted Treasure Shovel", 0.3),
                    new ItemLoot("Potion of Life", 1),
                    new ItemLoot("Potion of Mana", 1),
                    new ItemLoot("Sword of the Realm", 0.01),
                    new ItemLoot("Helm of Unadulterated Evil", 0.01),
                    new ItemLoot("Armor Of Gilgor", 0.01),
                    new ItemLoot("Amulet of Gods", 0.01)
                )
            )
        ;
    }
}
