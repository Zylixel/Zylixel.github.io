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
            .Init("Tomb Defender",                                         //Tomb Defender == Bes; His rage phase is perfectly working, needs no adjustments, but
                                                                           //Feel free to change drops and anything you want if you'd like, just be careful so that
                                                                           //The behaviorDB.cs doesn't fuck up and not work anymore, happened to me a lot, had to recompile
                                                                           //This about 5 times prior to posting, so you guys didn't yell at me for it not working. -KO
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
                        new Taunt("The others use tricks, but I shall stun you with my brute strength!"),
                        new Shoot(50, 8, projectileIndex: 2, coolDown: 1000, coolDownOffset: 500),
                        new Shoot(50, 4, projectileIndex: 1, coolDown: 3000, coolDownOffset: 500),
                        new Shoot(50, 6, projectileIndex: 0, coolDown: 3100, coolDownOffset: 500),
                        new HpLessTransition(.9, "boomerang")
                        ),
                    new State("boomerang",
                        new ConditionalEffect(ConditionEffectIndex.Armored),
                        new Orbit(.6, 5, target: "Tomb Boss Anchor", radiusVariance: 0.5),
                        new Taunt("Nut, disable our foes!"),
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
                        new Taunt("Awe at my wondrous defense!"),
                        new Shoot(50, 8, projectileIndex: 2, coolDown: 1000, coolDownOffset: 500),
                        new Shoot(50, 8, 10, 1, coolDown: 4750, coolDownOffset: 500),
                        new Shoot(50, 3, 10, 0, coolDown: 4750, coolDownOffset: 500),
                        new HpLessTransition(.2, "rage")
                        ),
                    new State("artifacts",
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
                        new Follow(1.3, range: 1, duration: 5000, coolDown: 0),
                        new Flash(0xfFF0000, 1, 9000001),
                        new Shoot(50, 8, 10, 1, coolDown: 4750, coolDownOffset: 500),
                        new Shoot(50, 4, 10, 4, coolDown: 300),
                        new Shoot(50, 3, 10, 0, coolDown: 4750, coolDownOffset: 500)
                        )
                    ),
                    new Threshold(0.32,
                        new ItemLoot("Potion of Life", 1)
                    ),
                    new Threshold(0.1,
                        new ItemLoot("Ring of the Pyramid", 0.2),
                        new ItemLoot("Tome of Holy Protection", 0.1),
                        new ItemLoot("Wine Cellar Incantation", 0.6)
                    ),
                    new Threshold(0.2
                    )
            )
            .Init("Tomb Support",                                               //tomb support == Nut; I added my own rage phase, because Fabiano's didn't have
                                                                                //One set-up properly, she just raged at 20% and did nothing from 100-21 lol
                                                                                //Feel free to make your own idiosyncratic phases, I left a example above the rage code -KO
                new State(
                    new State("idle",
                        new Taunt("ENOUGH OF YOUR VANDALISM!"),
                        new ConditionalEffect(ConditionEffectIndex.Armored),
                        new Orbit(.6, 5, target: "Tomb Boss Anchor", radiusVariance: 0.5),
                        new HpLessTransition(.99, "weakning")
                        ),
                    new State("weakning",
                        new Orbit(.6, 5, target: "Tomb Boss Anchor", radiusVariance: 0.5),
                        new Taunt("Geb, eradicate these cretins from our tomb!"),
                        new Shoot(50, 24, projectileIndex: 3, coolDown: 6000, coolDownOffset: 2000),
                        new HpLessTransition(.97, "active")
                        ),
                    new State("active",
                        new Orbit(.7, 5, target: "Tomb Boss Anchor", radiusVariance: 0.5),
                        new Taunt("Bes, become my wall of defense!"),
                        new Shoot(50, 1, projectileIndex: 6, coolDown: 8000, shootAngle: 10, coolDownOffset: 500),
                        new Shoot(50, 1, projectileIndex: 5, coolDown: 5000, coolDownOffset: 3000),
                        new Shoot(50, 1, projectileIndex: 5, coolDown: 6000, coolDownOffset: 4000),
                        new HpLessTransition(.9, "boomerang")
                        ),
                    new State("boomerang",
                        new ConditionalEffect(ConditionEffectIndex.Armored),
                        new Taunt("My attacks shall make your lethargic lives end much more switfly!"),
                        new Orbit(.6, 5, target: "Tomb Boss Anchor", radiusVariance: 0.5),
                        new Shoot(50, 4, projectileIndex: 2, coolDown: 3000, coolDownOffset: 1000),
                        new Shoot(50, 5, projectileIndex: 3, coolDown: 4000, coolDownOffset: 3000),
                        new Shoot(50, 5, projectileIndex: 4, coolDown: 5000, coolDownOffset: 5000),
                        new Shoot(50, 6, projectileIndex: 3, coolDown: 4000, coolDownOffset: 3000),


                        //template for adding new shots into here, if it's the last line remember not to put a coma after the )
                        //new shoot( ,  , projectileIndex: , coolDown: (time in MS[1000=1sec]), coolDownOffset: )
                        new HpLessTransition(.2, "rage")
                        //example: new Shoot(50, 4, protjectileIndex 2, coolDown 1250, coolDownOffset: 1000),
                        ),
                    new State("rage",
                        new Taunt("This cannot be! You shall not succeed!"),

                        new Follow(1.3, range: 1, duration: 5000, coolDown: 0),
                        new Flash(0xfFF0000, 1, 9000001),
                        new Shoot(50, 3, projectileIndex: 2, coolDown: 3000, coolDownOffset: 1000),
                        new Shoot(50, 6, projectileIndex: 3, coolDown: 4000, coolDownOffset: 3000),
                        new Shoot(50, 3, projectileIndex: 2, coolDown: 3000, coolDownOffset: 1000),
                        new InvisiToss("Scarab", 1, 0, coolDown: 1500),
                        new Shoot(50, 8, 10, 1, coolDown: 4750, coolDownOffset: 500),
                        new Shoot(50, 4, 10, 4, coolDown: 300),
                        new Shoot(50, 3, 10, 0, coolDown: 4750, coolDownOffset: 500),
                        new Shoot(50, 6, projectileIndex: 4, coolDown: 4000, coolDownOffset: 2500),
                        new Shoot(50, 4, projectileIndex: 4, coolDown: 5000, coolDownOffset: 5000) //,
                        )
                    ),
                    new Threshold(0.32,
                        new ItemLoot("Potion of Life", 1)
                    ),
                    new Threshold(0.1,
                        new ItemLoot("Ring of the Sphinx", 0.2),
                        new ItemLoot("Wine Cellar Incantation", 0.6)
                    //new ItemLoot("Legendary Food", 0.09),
                    ),
                    new Threshold(0.2
                    )
            )

            .Init("Tomb Attacker",                                    //Tomb Attacker == Geb; I added my own rage phase, again, because Fabiano's didn't have anything from
                                                                      //Below 60% hp threshhold, it just shot weaks from 100->60% and then stood still from 60% till death
                                                                      //Feel free to edit it, yet again, I want no credit for your work, make it how you like it. -KO
                new State(
                    new DropPortalOnDeath("Glowing Realm Portal", 100, PortalDespawnTimeSec: 360),
                    new State("idle",
                        new Taunt("YOU HAVE AWAKENED US"),
                        new ConditionalEffect(ConditionEffectIndex.Armored),
                        new Orbit(1, 5, target: "Tomb Boss Anchor", radiusVariance: 0.5),
                        new HpLessTransition(.99, "weakning")
                    ),
                    new State("weakning",
                        new Orbit(.6, 5, target: "Tomb Boss Anchor", radiusVariance: 0.5),
                        new Taunt("I shall destroy you from your soul to your flesh!"),
                        new Shoot(50, 24, projectileIndex: 3, coolDown: 6000, coolDownOffset: 2000),
                        new Shoot(50, 2, projectileIndex: 2, coolDown: 1900, coolDownOffset: 2000),
                        new Shoot(50, 12, projectileIndex: 2, coolDown: 2500, coolDownOffset: 2000),
                    new HpLessTransition(.2, "rage")
                    ),

                    new State("rage",
                        new Taunt("Argh! You shall pay for your crimes!"),

                        new Follow(1.3, range: 1, duration: 5000, coolDown: 0),
                        new Flash(0xfFF0000, 1, 9000001),
                        new Shoot(50, 24, projectileIndex: 3, coolDown: 6000, coolDownOffset: 2500),
                        new Shoot(50, 8, 10, 1, coolDown: 4750, coolDownOffset: 500),
                        new Shoot(50, 4, 10, 4, coolDown: 300),
                        new Shoot(50, 3, 10, 0, coolDown: 4750, coolDownOffset: 500),
                        new Shoot(50, 6, projectileIndex: 2, coolDown: 3100, coolDownOffset: 2000),

                        new Shoot(50, 3, projectileIndex: 1, coolDown: 1500, coolDownOffset: 2000)
                    )
                ),
                new Threshold(0.32,
                        new ItemLoot("Potion of Life", 1)
                ),
                new Threshold(0.1,
                    new ItemLoot("Ring of the Nile", 0.2),
                    new ItemLoot("Wine Cellar Incantation", 0.6)
                //new ItemLoot("Legendary Food", 0.09),
                ),
                new Threshold(0.2
                )
            )

            //Minions
            .Init("Pyramid Artifact 1",
                new State(
                    new Prioritize(
                        new Orbit(1, 2, target: "Tomb Defender", radiusVariance: 0.5),
                        new Follow(0.85, range: 1, duration: 5000, coolDown: 0)
                        ),
                    new Shoot(3, coolDown: 2500)
                    ))

            .Init("Pyramid Artifact 2",
                new State(
                    new Prioritize(
                        new Orbit(1, 2, target: "Tomb Attacker", radiusVariance: 0.5),
                        new Follow(0.85, range: 1, duration: 5000, coolDown: 0)
                        ),
                    new Shoot(3, coolDown: 2500)
                    ))

            .Init("Pyramid Artifact 3",
                new State(
                    new Prioritize(
                        new Orbit(1, 2, target: "Tomb Support", radiusVariance: 0.5),
                        new Follow(0.85, range: 1, duration: 5000, coolDown: 0)
                        ),
                    new Shoot(3, coolDown: 2500)
                    ))

            .Init("Tomb Defender Statue",
                new State(
                    new State(
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new EntityNotExistsTransition("Inactive Sarcophagus", 1000, "checkActive"),
                        new EntityNotExistsTransition("Active Sarcophagus", 1000, "checkInactive")
                        ),
                    new State("checkActive",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new EntityNotExistsTransition("Active Sarcophagus", 1000, "ItsGoTime")
                        ),
                    new State("checkInactive",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new EntityNotExistsTransition("Inactive Sarcophagus", 1000, "ItsGoTime")
                        ),
                    new State("ItsGoTime",
                        new Taunt("THIS WILL NOW BE YOUR TOMB!"),
                        new Transform("Tomb Defender")
                        )
                    ))

            .Init("Tomb Support Statue",
                new State(
                    new State(
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new EntityNotExistsTransition("Inactive Sarcophagus", 1000, "checkActive"),
                        new EntityNotExistsTransition("Active Sarcophagus", 1000, "checkInactive")
                        ),
                    new State("checkActive",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new EntityNotExistsTransition("Active Sarcophagus", 1000, "ItsGoTime")
                        ),
                    new State("checkInactive",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new EntityNotExistsTransition("Inactive Sarcophagus", 1000, "ItsGoTime")
                        ),
                    new State("ItsGoTime",
                        new Taunt("ENOUGH OF YOUR VANDALISM!"),
                        new Transform("Tomb Support")
                        )
                    ))

            .Init("Tomb Attacker Statue",
                new State(
                    new State(
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new EntityNotExistsTransition("Inactive Sarcophagus", 1000, "checkActive"),
                        new EntityNotExistsTransition("Active Sarcophagus", 1000, "checkInactive")
                        ),
                    new State("checkActive",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new EntityNotExistsTransition("Active Sarcophagus", 1000, "ItsGoTime")
                        ),
                    new State("checkInactive",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new EntityNotExistsTransition("Inactive Sarcophagus", 1000, "ItsGoTime")
                        ),
                    new State("ItsGoTime",
                        new Taunt("YOU HAVE AWAKENED US"),
                        new Transform("Tomb Attacker")
                        )
                    ))

            .Init("Inactive Sarcophagus",
                new State(
                    new State(
                        new EntityNotExistsTransition("Beam Priestess", 14, "checkPriest"),
                        new EntityNotExistsTransition("Beam Priest", 1000, "checkPriestess")
                        ),
                    new State("checkPriest",
                        new EntityNotExistsTransition("Beam Priest", 1000, "activate")
                        ),
                    new State("checkPriestess",
                        new EntityNotExistsTransition("Beam Priestess", 1000, "activate")
                        ),
                    new State("activate",
                        new Transform("Active Sarcophagus")
                        )
                    ))

            .Init("Scarab",
                new State(
                    new NoPlayerWithinTransition(7, "Idle"),
                    new PlayerWithinTransition(7, "Chase"),
                    new State("Idle",
                        new Wander(.1)
                    ),
                    new State("Chase",
                        new Follow(1.5, 7, 0),
                        new Shoot(3, projectileIndex: 1, coolDown: 500)
                    )
                )
                )

         .Init("Eagle Sentry",
                new State(
                    new NoPlayerWithinTransition(12, "Idle"),
                    new PlayerWithinTransition(12, "Chase"),
                    new State("Idle",
                        new Wander(.03)
                    ),
                    new State("Chase",
                        new Follow(.7, 7, 0),
                        new Shoot(25, 12, projectileIndex: 1, coolDown: 3000)
                    )
                )
                )

           .Init("Bloated Mummy",
                new State(
                    new NoPlayerWithinTransition(12, "Idle"),
                    new PlayerWithinTransition(12, "Chase"),
                    new State("Idle",
                        new Wander(.03)
                    ),
                    new State("Chase",
                        new Follow(.6, 7, 0),
                        new Reproduce("Scarab", 10, 3, 3000),
                        new Shoot(25, 22, projectileIndex: 0, coolDown: 2250)
                    )
                )
                )

             .Init("Lion Archer",
                new State(
                    new NoPlayerWithinTransition(12, "Idle"),
                    new PlayerWithinTransition(12, "Chase"),
                    new State("Idle",
                        new Wander(.03)
                    ),
                    new State("Chase",
                        new Follow(.4, 7, 0),
                        new Shoot(25, 3, projectileIndex: 1, coolDown: 1250),
                        new Shoot(25, 1, projectileIndex: 3, fixedAngle: 0, coolDown: 6000),
                        new Shoot(25, 1, projectileIndex: 3, fixedAngle: 90, coolDown: 6000),
                        new Shoot(25, 1, projectileIndex: 3, fixedAngle: 180, coolDown: 6000),
                        new Shoot(25, 1, projectileIndex: 3, fixedAngle: 270, coolDown: 6000)
                    )
                )
                )

        .Init("Jackal Warrior",
                new State(
                    new NoPlayerWithinTransition(12, "Idle"),
                    new PlayerWithinTransition(12, "Chase"),
                    new State("Idle",
                        new Wander(.03)
                    ),
                    new State("Chase",
                        new Follow(.9, 7, 0),

                        new Shoot(25, 1, shootAngle: 25, projectileIndex: 0, coolDown: 1250)
                    )
                )
                )

        .Init("Jackal Assassin",
                new State(
                    new NoPlayerWithinTransition(12, "Idle"),
                    new PlayerWithinTransition(12, "Chase"),
                    new State("Idle",
                        new Wander(.03)
                    ),
                    new State("Chase",
                        new Follow(.9, 7, 0),

                        new Shoot(25, 1, shootAngle: 25, projectileIndex: 0, coolDown: 1250)
                    )
                )
              )

            .Init("Jackal Veteran",
                new State(
                    new NoPlayerWithinTransition(12, "Idle"),
                    new PlayerWithinTransition(12, "Chase"),
                    new State("Idle",
                        new Wander(.03)
                    ),
                    new State("Chase",
                        new Follow(.9, 7, 0),

                        new Shoot(25, 1, shootAngle: 25, projectileIndex: 0, coolDown: 1250)
                    )
                )
             )

             .Init("Jackal Lord",
                new State(
                    new NoPlayerWithinTransition(12, "Idle"),
                    new PlayerWithinTransition(12, "Chase"),
                    new State("Idle",
                        new Wander(.03)
                    ),
                    new State("Chase",
                        new Follow(.9, 7, 0),

                        new InvisiToss("Jackal Warrior", 1, 180, coolDown: 6000),
                        new InvisiToss("Jackal Veteran", 1, 90, coolDown: 6000),
                        new InvisiToss("Jackal Assassin", 1, 0, coolDown: 600),
                        new Shoot(25, 4, shootAngle: 25, projectileIndex: 0, coolDown: 1250)
                    )
                )
              )

            .Init("Active Sarcophagus",
                new State(
                    new State(
                        new HpLessTransition(95, "stun")
                        ),
                    new State("stun",
                        new Shoot(50, 8, 10, 0, coolDown: 1250, coolDownOffset: 500),
                        new Shoot(50, 8, 10, 0, coolDown: 1500, coolDownOffset: 1000),
                        new Shoot(50, 8, 10, 0, coolDown: 1750, coolDownOffset: 1500),
                        new TimedTransition(1500, "idle")
                        ),
                    new State("idle",
                        new ChangeSize(100, 100)
                        )
                    ),

                    new Threshold(0.32,
                        new ItemLoot("Tincture of Mana", 0.15),
                        new ItemLoot("Tincture of Dexterity", 0.15),
                        new ItemLoot("Tincture of Life", 0.15)
                    ),
                    new Threshold(0.2
                    )
            )

                    .Init("Beam Priest",
                new State(
                    new State("weakning",
                        new Orbit(.4, 6, target: "Active Sarcophagus", radiusVariance: 0.5),
                        new Shoot(50, 3, projectileIndex: 1, coolDown: 3500),
                        new Shoot(50, 6, projectileIndex: 0, coolDown: 7210)


                        )
                    )
            )
              .Init("Tomb Thunder Turret",
                new State(
                    new State("Idle",
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                        new TimedTransition(2500, "Spin")
                    ),
                    new State("Spin",
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                        new TimedTransition(2000, "Pause"),
                        new State("Quadforce1",
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                            new Shoot(0, projectileIndex: 0, count: 5, shootAngle: 60, fixedAngle: 0, coolDown: 300),
                            new TimedTransition(150, "Quadforce2")
                        ),
                        new State("Quadforce2",
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                            new Shoot(0, projectileIndex: 0, count: 5, shootAngle: 60, fixedAngle: 15, coolDown: 300),
                            new TimedTransition(150, "Quadforce3")
                        ),
                        new State("Quadforce3",
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                            new Shoot(0, projectileIndex: 0, count: 5, shootAngle: 60, fixedAngle: 30, coolDown: 300),
                            new TimedTransition(150, "Quadforce4")
                        ),
                        new State("Quadforce4",
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                            new Shoot(0, projectileIndex: 0, count: 5, shootAngle: 60, fixedAngle: 45, coolDown: 300),
                            new TimedTransition(150, "Quadforce5")
                        ),
                        new State("Quadforce5",
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                            new Shoot(0, projectileIndex: 0, count: 5, shootAngle: 60, fixedAngle: 60, coolDown: 300),
                            new TimedTransition(150, "Quadforce6")
                        ),
                        new State("Quadforce6",
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                            new Shoot(0, projectileIndex: 0, count: 5, shootAngle: 60, fixedAngle: 75, coolDown: 300),
                            new TimedTransition(150, "Quadforce7")
                        ),
                        new State("Quadforce7",
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                            new Shoot(0, projectileIndex: 0, count: 5, shootAngle: 60, fixedAngle: 90, coolDown: 300),
                            new TimedTransition(150, "Quadforce8")
                        ),
                        new State("Quadforce8",
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                            new Shoot(0, projectileIndex: 0, count: 5, shootAngle: 60, fixedAngle: 105, coolDown: 300),
                            new TimedTransition(150, "Quadforce1")
                        )
                    ),
                    new State("Pause",
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                       new TimedTransition(5000, "Spin")
                    )
                )
            )

              .Init("Tomb Fire Turret",
                new State(
                    new State("Idle",
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                        new TimedTransition(2500, "Spin")
                    ),
                    new State("Spin",
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                        new TimedTransition(2000, "Pause"),
                        new State("Quadforce1",
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                            new Shoot(0, projectileIndex: 0, count: 5, shootAngle: 60, fixedAngle: 0, coolDown: 300),
                            new TimedTransition(150, "Quadforce2")
                        ),
                        new State("Quadforce2",
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                            new Shoot(0, projectileIndex: 0, count: 5, shootAngle: 60, fixedAngle: 15, coolDown: 300),
                            new TimedTransition(150, "Quadforce3")
                        ),
                        new State("Quadforce3",
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                            new Shoot(0, projectileIndex: 0, count: 5, shootAngle: 60, fixedAngle: 30, coolDown: 300),
                            new TimedTransition(150, "Quadforce4")
                        ),
                        new State("Quadforce4",
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                            new Shoot(0, projectileIndex: 0, count: 5, shootAngle: 60, fixedAngle: 45, coolDown: 300),
                            new TimedTransition(150, "Quadforce5")
                        ),
                        new State("Quadforce5",
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                            new Shoot(0, projectileIndex: 0, count: 5, shootAngle: 60, fixedAngle: 60, coolDown: 300),
                            new TimedTransition(150, "Quadforce6")
                        ),
                        new State("Quadforce6",
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                            new Shoot(0, projectileIndex: 0, count: 5, shootAngle: 60, fixedAngle: 75, coolDown: 300),
                            new TimedTransition(150, "Quadforce7")
                        ),
                        new State("Quadforce7",
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                            new Shoot(0, projectileIndex: 0, count: 5, shootAngle: 60, fixedAngle: 90, coolDown: 300),
                            new TimedTransition(150, "Quadforce8")
                        ),
                        new State("Quadforce8",
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                            new Shoot(0, projectileIndex: 0, count: 5, shootAngle: 60, fixedAngle: 105, coolDown: 300),
                            new TimedTransition(150, "Quadforce1")
                        )
                    ),
                    new State("Pause",
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                       new TimedTransition(5000, "Spin")
                    )
                )
            )

        .Init("Tomb Frost Turret",
                new State(
                    new State("Idle",
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                        new TimedTransition(2500, "Spin")
                    ),
                    new State("Spin",
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                        new TimedTransition(2000, "Pause"),
                        new State("Quadforce1",
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                            new Shoot(0, projectileIndex: 0, count: 5, shootAngle: 60, fixedAngle: 0, coolDown: 300),
                            new TimedTransition(150, "Quadforce2")
                        ),
                        new State("Quadforce2",
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                            new Shoot(0, projectileIndex: 0, count: 5, shootAngle: 60, fixedAngle: 15, coolDown: 300),
                            new TimedTransition(150, "Quadforce3")
                        ),
                        new State("Quadforce3",
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                            new Shoot(0, projectileIndex: 0, count: 5, shootAngle: 60, fixedAngle: 30, coolDown: 300),
                            new TimedTransition(150, "Quadforce4")
                        ),
                        new State("Quadforce4",
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                            new Shoot(0, projectileIndex: 0, count: 5, shootAngle: 60, fixedAngle: 45, coolDown: 300),
                            new TimedTransition(150, "Quadforce5")
                        ),
                        new State("Quadforce5",
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                            new Shoot(0, projectileIndex: 0, count: 5, shootAngle: 60, fixedAngle: 60, coolDown: 300),
                            new TimedTransition(150, "Quadforce6")
                        ),
                        new State("Quadforce6",
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                            new Shoot(0, projectileIndex: 0, count: 5, shootAngle: 60, fixedAngle: 75, coolDown: 300),
                            new TimedTransition(150, "Quadforce7")
                        ),
                        new State("Quadforce7",
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                            new Shoot(0, projectileIndex: 0, count: 5, shootAngle: 60, fixedAngle: 90, coolDown: 300),
                            new TimedTransition(150, "Quadforce8")
                        ),
                        new State("Quadforce8",
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                            new Shoot(0, projectileIndex: 0, count: 5, shootAngle: 60, fixedAngle: 105, coolDown: 300),
                            new TimedTransition(150, "Quadforce1")
                        )
                    ),
                    new State("Pause",
                    new ConditionalEffect(ConditionEffectIndex.Invincible),
                       new TimedTransition(5000, "Spin")
                    )
                )
            )

        .Init("Beam Priestess",
                new State(
                    new State("weakning",
                        new Orbit(.6, 9, target: "Active Sarcophagus", radiusVariance: 0.5),
                        new Shoot(50, 6, projectileIndex: 1, coolDown: 3500),
                        new Shoot(50, 2, projectileIndex: 0, coolDown: 7210)


                        )
                    )
            )

            .Init("Tomb Boss Anchor",
                new State(
                    new ConditionalEffect(ConditionEffectIndex.Invincible, true),
                    new RealmPortalDrop(),
                    new State("Idle",
                        new EntitiesNotExistsTransition(300, "Death", "Tomb Support", "Tomb Attacker", "Tomb Defender",
                            "Active Sarcophagus", "Tomb Defender Statue", "Tomb Support Statue", "Tomb Attacker Statue")
                    ),
                    new State("Death",
                        new RemoveEntity(10, "Tomb Portal of Cowardice"),
                        new Suicide()
                    )
                )

            );
    }
} //Credits to user: reveng3d on MPGH