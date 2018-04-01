#region

using wServer.logic.transitions;
using wServer.logic.behaviors;
using wServer.logic.loot;

#endregion

namespace wServer.logic
{
    partial class BehaviorDb
    {
        private _ Items = () => Behav()
            .Init("Dire Blast",
                new State(
                    new State("blastoff",
                        new Shoot(100, 12),
                        new TimedTransition(50, "die")
                    ),
                    new State("die",
                        new Suicide()
                        )
                )
            )
            .Init("Quest Chest",
                new State(
                    new State("Idle",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new Flash(0xFFFFF, 1, 5),
                        new TimedTransition(5000, "UnsetEffect")
                    ),
                    new State("UnsetEffect")
                ),
                new Threshold(0.01,
                    new TierLoot(2, ItemType.Potion, 1),
                    new TierLoot(2, ItemType.Potion, 0.5),
                    new TierLoot(2, ItemType.Potion, 0.5),
                    new TierLoot(10, ItemType.Weapon, 0.25),
                    new TierLoot(11, ItemType.Armor, 0.25),
                    new TierLoot(4, ItemType.Ring, 0.25),
                    new TierLoot(4, ItemType.Ability, 0.25),
                    new ItemLoot("Experimental Ring", 0.03),
                    new ItemLoot("Amulet of Resurrection", 0.03),
                    new ItemLoot("Wine Cellar Incantation", 0.03),
                    new ItemLoot("Etherite Dagger", 0.01),
                    new ItemLoot("Wand of the Bulwark", 0.01),
                    new ItemLoot("Staff of Extreme Prejudice", 0.03),
                    new ItemLoot("Cloak of the Planewalker", 0.03),
                    new ItemLoot("Demon Blade", 0.02),
                    new ItemLoot("Doom Bow", 0.02),
                    new ItemLoot("Void Blade", 0.01),
                    new ItemLoot("Conducting Wand", 0.03),
                    new ItemLoot("Harlequin Armor", 0.01),
                    new ItemLoot("Prism of Dancing Swords", 0.02),
                    new ItemLoot("Plague Poison", 0.005),
                    new ItemLoot("Resurrected Warrior's Armor", 0.005),
                    new ItemLoot("Mystery Pet Stone", 0.01)
                )
            )
        .Init("Epic Quest Chest",
                new State(
                    new State("Idle",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                        new Flash(0xFFFFF, 1, 5),
                        new TimedTransition(5000, "UnsetEffect")
                    ),
                    new State("UnsetEffect")
                ),
                new Threshold(0.01,
                    new TierLoot(2, ItemType.Potion, 1),
                    new TierLoot(2, ItemType.Potion, 0.5),
                    new TierLoot(11, ItemType.Weapon, 0.4),
                    new TierLoot(12, ItemType.Armor, 0.4),
                    new TierLoot(5, ItemType.Ring, 0.4),
                    new TierLoot(5, ItemType.Ability, 0.4),
                    new ItemLoot("Potion of Life", 0.5),
                    new ItemLoot("Potion of Mana", 0.5),
                    new ItemLoot("Wine Cellar Incantation", 0.05),
                    new ItemLoot("Coral Silk Armor", 0.03),
                    new ItemLoot("Spectral Cloth Armor", 0.03),
                    new ItemLoot("Spirit Dagger", 0.03),
                    new ItemLoot("Ghostly Prism", 0.03),
                    new ItemLoot("Doku No Ken", 0.03),
                    new ItemLoot("Staff of Esben", 0.03),
                    new ItemLoot("Mystery Pet Stone", 0.03),
                    new ItemLoot("Ancient Spell: Pierce", 0.02),
                    new ItemLoot("Coral Bow", 0.02),
                    new ItemLoot("Ring of the Pyramid", 0.02),
                    new ItemLoot("Ring of the Sphinx", 0.02),
                    new ItemLoot("Ring of the Nile", 0.02),
                    new ItemLoot("Prism of Dire Instability", 0.02),
                    new ItemLoot("Tome of Holy Protection", 0.02),
                    new ItemLoot("Fire Dragon Battle Armor", 0.02),
                    new ItemLoot("Water Dragon Silk Robe", 0.02),
                    new ItemLoot("Leaf Dragon Hide Armor", 0.02),
                    new ItemLoot("EH Hivemaster helm", 0.01),
                    new ItemLoot("Marble Seal", 0.01),
                    new ItemLoot("Thousand Shot", 0.01),
                    new ItemLoot("Bracer of the Guardian", 0.005),
                    new ItemLoot("The Twilight Gemstone", 0.005),
                    new ItemLoot("The Forgotten Crown", 0.005)
                )
            )
            .Init("Marble Pillar",
                new State(
                    new State("Idle",
                        new AoeEffect(3, 1, ConditionEffectIndex.Armored),
                        new AoeEffect(3, 1, ConditionEffectIndex.Damaging),
                        new TimedTransition(4500, "die")
                    ),
                    new State("die",
                        new Suicide()
                        )
                )
            )
        .Init("EH Ability Bee 1",
                new State(
                    new State("start",
                         new BeesAttack(),
                         new PlayerOrbit(0.9, 1.2),
                         new TimedTransition(6000, "die")
                    ),
                    new State("die",
                        new Suicide()
                    )
                )
            )
        .Init("EH Ability Bee 2",
                new State(
                    new State("start",
                         new BeesRandom(),
                         new TimedTransition(1, "init")
                    ),
                    new State("init",
                        new PlayerOrbit(0.9, 1.2),
                        new BeesAttackCurse(),
                        new TimedTransition(5700, "die")
                    ),
                    new State("die",
                        new Suicide()
                    )
                )
            )
            .Init("EH Ability Bee 3",
                new State(
                    new State("start",
                         new PlayerOrbit(0.9, 1.2),
                         new BeesAttack(),
                         new TimedTransition(6000, "die")
                    ),
                    new State("die",
                        new Suicide()
                    )
                )
            );
    }
}