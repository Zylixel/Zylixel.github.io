using wServer.logic.behaviors;
using wServer.logic.transitions;
using wServer.logic.loot;

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
                    new Spawn("Keeper Minion", maxChildren: 1, initialSpawn: 0.5),
                    new Suicide()
                    )
                )
            )
            .Init("Keeper Minion",
            new State(
                new State("begin",
                     new Follow(1.5, range: 0.2),
                     new Shoot(100, projectileIndex: 0, count: 1, coolDown: 75)
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
                        new Suicide()
                    )
                    )
            )
        .Init("Keeper Attacker",
                new State(
                    new State("begin"
                        ),
                    new State("die",
                        new Suicide()
                    )
                    )
            )
        .Init("Keeper Support",
                new State(
                    new State("begin"
                        ),
                    new State("die",
                        new Suicide()
                    )
                    )
            )
        .Init("Keeper Boss Anchor",
                new State(
                    new State("begin",
                        new Spawn("Keeper Gilgor Boss", maxChildren: 1)
                        )
                    )
            )
        .Init("Keeper Crown",
                new State(
                    new State("begin",
                        new TimedTransition(5000, "die")
                        ),
                    new State("die",
                        new Suicide()
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
                    new ApplySetpiece("KeeperTomb"),
                    new TimedTransition(500000, "Shatters")
                    ),
                new State("Shatters",
                    new ApplySetpiece("KeeperShatters"),
                    new TimedTransition(5000, "Ocean")
                    ),
                new State("Ocean",
                    new ApplySetpiece("KeeperOcean"),
                    new TimedTransition(5000, "die")
                    ),
                new State("die",
                    new Suicide()
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
        .Init("Keeper Gilgor Boss",
                new State(
                    new State("init",
                        new ConditionalEffect(ConditionEffectIndex.Invincible, perm: true),
                        new Spawn("Keeper Gilgor Boss Appear", maxChildren: 1),
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
                        new Spawn("Keeper Gilgor Boss Disappear", maxChildren: 1),
                        new TimedTransition(800, "moveToSpawn1")
                        ),
                    new State("moveToSpawn1",
                        new ReturnToSpawn(true, 0.9),
                        new TimedTransition(4000, "moveToTomb")
                        ),
                    new State("moveToTomb",
                        new MoveTo(8, 8, speed: 1, isMapPosition: false, once: true, instant: false),
                        new TimedTransition(50, "fadeIn1")
                        ),
                    new State("fadeIn1",
                        new Spawn("Keeper Gilgor Boss Appear", maxChildren: 1),
                        new TimedTransition(700, "fadeIn2")
                        ),
                    new State("fadeIn2",
                        new SetAltTexture(1),
                        new TimedTransition(3000, "shootTomb1")
                        ),
                    new State("shootTomb1",
                        new Taunt(true, "Say goodbye to your precious Gods!"),
                        new Shoot(100, count: 1, projectileIndex: 0, shootAngle: 200, fixedAngle: 200, coolDown: 10000000),
                        new TimedTransition(500, "kill1")
                        ),
                    new State("kill1",
                        new Order(999, "Keeper Support", "die"),
                        new SetAltTexture(2),
                        new Spawn("Keeper Gilgor Boss Disappear", maxChildren: 1),
                        new TimedTransition(100, "moveToTomb2")
                        ),
                    new State("moveToTomb2",
                        new MoveTo(-2, -5),
                        new TimedTransition(500, "fadeIn3")
                        ),
                    new State("fadeIn3",
                        new Spawn("Keeper Gilgor Boss Appear", maxChildren: 1),
                        new TimedTransition(700, "fadeIn4")
                        ),
                    new State("fadeIn4",
                        new SetAltTexture(1),
                        new TimedTransition(200, "shootTomb2")
                        ),
                    new State("shootTomb2",
                        new Shoot(100, count: 1, projectileIndex: 0, shootAngle: 200, fixedAngle: 200, coolDown: 10000000)
                        )
                    )
            )


        ;
    }
}
