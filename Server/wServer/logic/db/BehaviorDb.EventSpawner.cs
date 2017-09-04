#region

using wServer.logic.behaviors;
using wServer.logic.loot;
using wServer.logic.transitions;

#endregion

namespace wServer.logic
{
    partial class BehaviorDb
    {
        private _ Jeebs = () => Behav()
              .Init("Zylixel",
                  new State(
                      new State("Chillin",
                        new PlayerWithinTransition(5, "PlayerChoice")
                        ),
                          new State("PlayerChoice",
                          new Taunt("Hey, Who said you could fight me? I'm the owner of the server for Pete's sake!"),

                        new TimedTransition(5000, "PlayerChoice2")
                              ),
                          new State("PlayerChoice2",
                          new Taunt("Here, I'll let you fight one of these guys, just tell me who! 'CUBE' 'SKULL' 'SPHINX' 'CRYSTAL' 'ORYX' 'Lord of the lost Lands'"),

                        new TimedTransition(25, "NOOOOOOWWWW")

                      ),
                          new State("NOOOOOOWWWW",
                               new ChatTransition("Cube God Prep", "cube"),
                        new ChatTransition("Skull Shrine Prep", "skull"),
                        new ChatTransition("Mysterious Crystal Prep", "crystal"),
                        new ChatTransition("Grand Sphinx Prep", "sphinx"),
                               new ChatTransition("Cube God Prep", "CUBE"),
                        new ChatTransition("Skull Shrine Prep", "SKULL"),
                        new ChatTransition("Mysterious Crystal Prep", "CRYSTAL"),
                        new ChatTransition("LOTL Prep", "lotl"),
                        new ChatTransition("LOTL Prep", "LOTL"),
                        new ChatTransition("LOTL Prep", "lord of the lost lands"),
                        new ChatTransition("LOTL Prep", "Lord of the Lost Lands"),
                        new ChatTransition("ORYX Prep", "ORYX"),
                        new ChatTransition("ORYX Prep", "Oryx"),
                        new ChatTransition("ORYX Prep", "oryx"),
                        new ChatTransition("Grand Sphinx Prep", "SPHINX")
                              ),
                          new State("Cube God Prep",
                              new Taunt("Prepare for Death in 5 seconds!"),
                              new TimedTransition(0, "Cube God")
                              ),
                    new State("Cube God",
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new Flash(0xfFF0000, 0.5, 666),
                    new Spawn("Cube God", 1, coolDown: 5000),

                        new TimedTransition(6000, "Cube God Check")
                        ),
                    new State("Cube God Check",
                        new EntityNotExistsTransition("Cube God", 10000, "suicide")
                        ),
                    new State("LOTL Prep",
                              new Taunt("Prepare for Death in 5 seconds!"),
                              new TimedTransition(0, "LOTL")
                              ),
                    new State("LOTL",
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new Flash(0xfFF0000, 0.5, 666),
                    new Spawn("Lord of the lost lands", 1, coolDown: 5000),

                        new TimedTransition(6000, "LOTL Check")
                        ),
                    new State("LOTL Check",
                        new EntityNotExistsTransition("Lord of the lost lands", 10000, "suicide")
                        ),
                          new State("Skull Shrine Prep",
                              new Taunt("Prepare for Death in 5 seconds!"),
                              new TimedTransition(0, "Skull Shrine")
                              ),

                    new State("ORYX Prep",
                              new Taunt("Prepare for Death in 5 seconds!"),
                              new TimedTransition(0, "ORYX")
                              ),
                    new State("ORYX",
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new Flash(0xfFF0000, 0.5, 666),
                    new Spawn("Oryx the mad god 3", 1, coolDown: 5000),

                        new TimedTransition(6000, "ORYX Check")
                        ),

                    new State("ORYX Check",
                        new EntityNotExistsTransition("Oryx the mad god 3", 10000, "suicide")
                        ),
                          new State("Skull Shrine Prep",
                              new Taunt("Prepare for Death in 5 seconds!"),
                              new TimedTransition(0, "Skull Shrine")
                              ),
                    new State("Skull Shrine",
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new Flash(0xfFF0000, 0.5, 666),
                    new Spawn("Skull Shrine", 1, coolDown: 5000),

                        new TimedTransition(6000, "Skull Shrine Check")
                        ),
                      new State("Skull Shrine Check",
                        new EntityNotExistsTransition("Skull Shrine", 10000, "suicide")
                         ),
                          new State("Mysterious Crystal Prep",
                              new Taunt("Prepare for Death in 5 seconds!"),
                              new TimedTransition(0, "Crystal")
                              ),
                    new State("Crystal",
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new Flash(0xfFF0000, 0.5, 666),
                    new Spawn("Mysterious Crystal", 1, coolDown: 5000),

                        new TimedTransition(6000, "Crystal Check")
                        ),
                      new State("Crystal Check",
                        new EntityNotExistsTransition("Mysterious Crystal", 12000, "Prisoner Check")
                        ),
                      new State("Prisoner Check",
                        new EntityNotExistsTransition("Crystal Prisoner", 12000, "suicide")
                         ),
                          new State("Grand Sphinx Prep",
                              new Taunt("Prepare for Death in 5 seconds!"),
                              new TimedTransition(0, "Grand Sphinx")
                              ),
                      new State("Grand Sphinx",
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                    new Flash(0xfFF0000, 0.5, 666),
                    new Spawn("Grand Sphinx", 1, coolDown: 5000),

                    new TimedTransition(6000, "Grand Sphinx Check")
                        ),
                      new State("Grand Sphinx Check",
                        new EntityNotExistsTransition("Grand Sphinx", 100, "suicide")
                            ),

                        new State("suicide",
                            new TimedTransition(4000, "Talk1")
                                 ),
                        new State("Talk1",
                            new Taunt("Well, I didn't think you would get this far..."),
                            new TimedTransition(3000, "Talk2")
                                 ),
                        new State("Talk2",
                            new Taunt("Oh, you still want to fight me?"),
                            new TimedTransition(3000, "Talk3")
                                 ),
                        new State("Talk3",
                            new Taunt("That's just asking for death, so I'll spare your life... for now"),
                            new TimedTransition(3000, "real suicide")
                                 ),
                        new State("real suicide",
                        new ConditionalEffect(ConditionEffectIndex.Invisible),
                        new DropPortalOnDeath("Realm Portal", 100),
                        new Suicide()
                            )

                        )


            );
    }
}