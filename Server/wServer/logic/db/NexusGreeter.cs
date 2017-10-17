#region

using wServer.logic.behaviors;
using wServer.logic.transitions;

#endregion

namespace wServer.logic
{
    partial class BehaviorDb
    {
        private _ NexusGreeter = () => Behav()
                .Init("Nexus Summoner",
                    new State(
                            new State("init",
                                new ApplySetpiece("NexusDark"),
                                new TimedTransition(300000, "Reload1")
                                ),
                            new State("Reload1",
                                new ApplySetpiece("NexusLight"),
                                new TimedTransition(300000, "init")
                                )
                        )
            )
        .Init("NiceZylixel2",
                    new State(
                            new State("init",
                                new Taunt("Did you know that you can access the marketplace by typing /market into the chat?"),
                                new TimedTransition(120000, "message1")
                                ),
                            new State("message1",
                                new Taunt("Thanks to Nozen for creating this nexus!"),
                                new TimedTransition(120000, "init")
                                )
                        )
            )
              .Init("NiceZylixel",
                  new State(
                          new Spawn("NiceZylixel2", maxChildren: 1),
                          new State("Idle",
                               new ChatTransition("hi", "Hi"),
                               new ChatTransition("hi", "Hi!"),
                               new ChatTransition("hi", "hi"),
                               new ChatTransition("hi", "hi!"),
                               new ChatTransition("hi", "Hello"),
                               new ChatTransition("hi", "Hello!"),
                               new ChatTransition("hi", "hello"),
                               new ChatTransition("hi", "Hello!"),
                               new ChatTransition("hi", "Hey"),
                               new ChatTransition("hi", "Hey!"),
                               new ChatTransition("hi", "hey"),
                               new ChatTransition("hi", "hey!"),
                               //Oryx
                               new ChatTransition("oryx", "Oryx"),
                               new ChatTransition("oryx", "oryx"),
                               //Pets
                               new ChatTransition("pet", "Pets"),
                               new ChatTransition("pet", "pets"),
                               new ChatTransition("pet", "Pet"),
                               new ChatTransition("pet", "pet"),
                               new ChatTransition("pet", "Pets?"),
                               new ChatTransition("pet", "pets?"),
                               new ChatTransition("pet", "Pet?"),
                               new ChatTransition("pet", "pet?"),
                               //Fame
                               new ChatTransition("fame", "fame"),
                               new ChatTransition("fame", "Fame"),
                               new ChatTransition("fame", "fame?"),
                               new ChatTransition("fame", "Fame?"),
                               //Gold
                               new ChatTransition("gold", "gold"),
                               new ChatTransition("gold", "Gold"),
                               new ChatTransition("gold", "gold?"),
                               new ChatTransition("gold", "Gold?"),
                               //Die
                               new ChatTransition("Die", "Die"),
                               new ChatTransition("Die", "Die!"),
                               new ChatTransition("Die", "die"),
                               new ChatTransition("Die", "die!"),
                               //Tier
                               new ChatTransition("Tier", "Tiers"),
                               new ChatTransition("Tier", "tiers"),
                               new ChatTransition("Tier", "Tier"),
                               new ChatTransition("Tier", "tier"),
                               new ChatTransition("Tier", "Tiers?"),
                               new ChatTransition("Tier", "tiers?"),
                               new ChatTransition("Tier", "Tier?"),
                               new ChatTransition("Tier", "tier?")
                              ),
                    new State("hi",
                    new Taunt("Hello {PLAYER}!"),
                    new TimedTransition(1500, "Idle")
                        ),
                    new State("oryx",
                    new Taunt("NO, THIS CANNOT BE!!!!"),
                    new TimedTransition(1500, "Idle")
                        ),
                    new State("pet",
                    new Taunt("Pets in Zy's Realm are much like the ones in prod, but they are slightly nerfed and have a higher drop rate!"),
                    new TimedTransition(1500, "Idle")
                        ),
                    new State("fame",
                    new Taunt("Fame in Zy's Realm is the same as it is in prod, but you can use fame to buy cool items in the market!"),
                    new TimedTransition(1500, "Idle")
                        ),
                    new State("gold",
                    new Taunt("Gold? No such thing in Zy's Realm!"),
                    new TimedTransition(1500, "Idle")
                        ),
                    new State("Die",
                    new Taunt("No! Don't... kill... me!"),
                    new ConditionalEffect(ConditionEffectIndex.Invisible),
                    new TimedTransition(3500, "Die2")
                        ),
                    new State("Die2",
                    new Taunt("Ha! I can't die!"),
                    new TimedTransition(1500, "Idle")
                        ),
                    new State("Tier",
                    new Taunt("Tiers in Zy's Realm are all the same, but there are many new ones that you may enjoy!"),
                    new TimedTransition(1500, "Idle")
                        )
                  )
              );
    }
}