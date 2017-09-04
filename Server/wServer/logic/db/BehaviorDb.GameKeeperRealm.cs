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
                    new State("begin",
                        new TimedTransition(5000, "die")
                        ),
                    new State("die",
                        new Suicide()
                    )
                    )
            )
        .Init("Keeper Attacker",
                new State(
                    new State("begin",
                        new TimedTransition(5000, "die")
                        ),
                    new State("die",
                        new Suicide()
                    )
                    )
            )
        .Init("Keeper Support",
                new State(
                    new State("begin",
                        new TimedTransition(5000, "die")
                        ),
                    new State("die",
                        new Suicide()
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
                    new TimedTransition(2000, "KeeperClose")
                    ),
                new State("KeeperClose",
                    new ApplySetpiece("KeeperClose"),
                    new TimedTransition(1000, "KeeperClose1")
                    ),
                new State("KeeperClose1",
                    new ApplySetpiece("KeeperClose1"),
                    new TimedTransition(1000, "KeeperClose2")
                    ),
                new State("KeeperClose2",
                    new ApplySetpiece("KeeperClose2"),
                    new TimedTransition(1000, "KeeperClose3")
                    ),
                new State("KeeperClose3",
                    new ApplySetpiece("KeeperClose3"),
                    new TimedTransition(1000, "Tomb")
                    ),
                new State("Tomb",
                    new ApplySetpiece("KeeperTomb"),
                    new TimedTransition(5000, "Shatters")
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
            );
    }
}
