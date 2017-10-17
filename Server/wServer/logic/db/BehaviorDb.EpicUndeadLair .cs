using wServer.logic.behaviors;
using wServer.logic.loot;
using wServer.logic.transitions;

namespace wServer.logic
{
    partial class BehaviorDb
    {
        private _ EpicUndeadLair = () => Behav()
            .Init("Zepts the Ghost King",
                new State(
                    new DropPortalOnDeath("Realm Portal", 100),
                    new ConditionalEffect(ConditionEffectIndex.Invulnerable, true),
                    new State("Chillin",
                    new PlayerWithinTransition(3, "Hello!")
                    ),
                    new State("Hello!",
                    new Taunt("So, you want to challenge me huh?"),
                    new TimedTransition(3000, "Cycle")
                    ),
                    new State("Cycle",
                        new ConditionalEffect(ConditionEffectIndex.Invulnerable, false),
                        new Shoot(15, projectileIndex: 3, coolDown: 1000),
                        new State("Cycle Begin",
                            new State("Cycle 1",
                                new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 3, coolDown: 17900),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 4, coolDown: 17900),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 6, coolDown: 17900, coolDownOffset: 200),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 8, coolDown: 17900, coolDownOffset: 200),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 10, coolDown: 17900, coolDownOffset: 400),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 12, coolDown: 17900, coolDownOffset: 400),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 14, coolDown: 17900, coolDownOffset: 600),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 16, coolDown: 17900, coolDownOffset: 600),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 18, coolDown: 17900, coolDownOffset: 800),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 20, coolDown: 17900, coolDownOffset: 800),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 22, coolDown: 17900, coolDownOffset: 1000),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 24, coolDown: 17900, coolDownOffset: 1000),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 26, coolDown: 17900, coolDownOffset: 1200),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 28, coolDown: 17900, coolDownOffset: 1200),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 30, coolDown: 17900, coolDownOffset: 1400),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 32, coolDown: 17900, coolDownOffset: 1400),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 34, coolDown: 17900, coolDownOffset: 1600),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 36, coolDown: 17900, coolDownOffset: 1600),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 38, coolDown: 17900, coolDownOffset: 1800),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 40, coolDown: 17900, coolDownOffset: 1800),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 42, coolDown: 17900, coolDownOffset: 2000),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 44, coolDown: 17900, coolDownOffset: 2000),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 46, coolDown: 17900, coolDownOffset: 2200),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 48, coolDown: 17900, coolDownOffset: 2200),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 50, coolDown: 17900, coolDownOffset: 2400),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 52, coolDown: 17900, coolDownOffset: 2400),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 54, coolDown: 17900, coolDownOffset: 2600),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 56, coolDown: 17900, coolDownOffset: 2600),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 58, coolDown: 17900, coolDownOffset: 2800),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 60, coolDown: 17900, coolDownOffset: 2800),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 62, coolDown: 17900, coolDownOffset: 3000),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 64, coolDown: 17900, coolDownOffset: 3000),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 66, coolDown: 17900, coolDownOffset: 3200),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 68, coolDown: 17900, coolDownOffset: 3200),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 70, coolDown: 17900, coolDownOffset: 3400),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 72, coolDown: 17900, coolDownOffset: 3400),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 74, coolDown: 17900, coolDownOffset: 3600),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 76, coolDown: 17900, coolDownOffset: 3600),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 78, coolDown: 17900, coolDownOffset: 3800),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 80, coolDown: 17900, coolDownOffset: 3800),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 82, coolDown: 17900, coolDownOffset: 4000),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 84, coolDown: 17900, coolDownOffset: 4000),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 86, coolDown: 17900, coolDownOffset: 4200),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 88, coolDown: 17900, coolDownOffset: 4200),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 90, coolDown: 17900, coolDownOffset: 4400),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 92, coolDown: 17900, coolDownOffset: 4400),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 94, coolDown: 17900, coolDownOffset: 4600),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 96, coolDown: 17900, coolDownOffset: 4600),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 98, coolDown: 17900, coolDownOffset: 4800),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 100, coolDown: 17900, coolDownOffset: 4800),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 102, coolDown: 17900, coolDownOffset: 5000),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 104, coolDown: 17900, coolDownOffset: 5000),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 106, coolDown: 17900, coolDownOffset: 5200),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 108, coolDown: 17900, coolDownOffset: 5200),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 110, coolDown: 17900, coolDownOffset: 5400),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 112, coolDown: 17900, coolDownOffset: 5400),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 114, coolDown: 17900, coolDownOffset: 5600),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 116, coolDown: 17900, coolDownOffset: 5600),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 118, coolDown: 17900, coolDownOffset: 5800),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 120, coolDown: 17900, coolDownOffset: 5800),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 122, coolDown: 17900, coolDownOffset: 6000),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 124, coolDown: 17900, coolDownOffset: 6000),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 126, coolDown: 17900, coolDownOffset: 6200),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 128, coolDown: 17900, coolDownOffset: 6200),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 130, coolDown: 17900, coolDownOffset: 6400),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 132, coolDown: 17900, coolDownOffset: 6400),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 134, coolDown: 17900, coolDownOffset: 6600),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 136, coolDown: 17900, coolDownOffset: 6600),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 138, coolDown: 17900, coolDownOffset: 6800),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 140, coolDown: 17900, coolDownOffset: 6800),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 142, coolDown: 17900, coolDownOffset: 7000),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 144, coolDown: 17900, coolDownOffset: 7000),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 146, coolDown: 17900, coolDownOffset: 7200),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 148, coolDown: 17900, coolDownOffset: 7200),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 150, coolDown: 17900, coolDownOffset: 7400),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 152, coolDown: 17900, coolDownOffset: 7400),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 154, coolDown: 17900, coolDownOffset: 7600),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 156, coolDown: 17900, coolDownOffset: 7600),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 158, coolDown: 17900, coolDownOffset: 7800),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 160, coolDown: 17900, coolDownOffset: 7800),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 162, coolDown: 17900, coolDownOffset: 8000),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 164, coolDown: 17900, coolDownOffset: 8000),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 166, coolDown: 17900, coolDownOffset: 8200),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 168, coolDown: 17900, coolDownOffset: 8200),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 170, coolDown: 17900, coolDownOffset: 8400),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 172, coolDown: 17900, coolDownOffset: 8400),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 174, coolDown: 17900, coolDownOffset: 8600),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 176, coolDown: 17900, coolDownOffset: 8600),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 178, coolDown: 17900, coolDownOffset: 8800),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 180, coolDown: 17900, coolDownOffset: 8800),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 182, coolDown: 17900, coolDownOffset: 9000),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 184, coolDown: 17900, coolDownOffset: 9000),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 186, coolDown: 17900, coolDownOffset: 9200),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 188, coolDown: 17900, coolDownOffset: 9200),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 190, coolDown: 17900, coolDownOffset: 9400),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 192, coolDown: 17900, coolDownOffset: 9400),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 194, coolDown: 17900, coolDownOffset: 9600),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 196, coolDown: 17900, coolDownOffset: 9600),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 198, coolDown: 17900, coolDownOffset: 9800),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 200, coolDown: 17900, coolDownOffset: 9800),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 202, coolDown: 17900, coolDownOffset: 10000),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 204, coolDown: 17900, coolDownOffset: 10000),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 206, coolDown: 17900, coolDownOffset: 10200),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 208, coolDown: 17900, coolDownOffset: 10200),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 210, coolDown: 17900, coolDownOffset: 10400),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 212, coolDown: 17900, coolDownOffset: 10400),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 214, coolDown: 17900, coolDownOffset: 10600),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 216, coolDown: 17900, coolDownOffset: 10600),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 218, coolDown: 17900, coolDownOffset: 10800),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 220, coolDown: 17900, coolDownOffset: 10800),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 222, coolDown: 17900, coolDownOffset: 11000), //
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 224, coolDown: 17900, coolDownOffset: 11000),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 226, coolDown: 17900, coolDownOffset: 11200),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 228, coolDown: 17900, coolDownOffset: 11400),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 230, coolDown: 17900, coolDownOffset: 11400),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 232, coolDown: 17900, coolDownOffset: 11600),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 234, coolDown: 17900, coolDownOffset: 11600), //
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 236, coolDown: 17900, coolDownOffset: 11800),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 238, coolDown: 17900, coolDownOffset: 11800),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 240, coolDown: 17900, coolDownOffset: 12000),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 242, coolDown: 17900, coolDownOffset: 12000),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 244, coolDown: 17900, coolDownOffset: 12200),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 246, coolDown: 17900, coolDownOffset: 12200),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 248, coolDown: 17900, coolDownOffset: 12400),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 250, coolDown: 17900, coolDownOffset: 12400),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 252, coolDown: 17900, coolDownOffset: 12600),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 254, coolDown: 17900, coolDownOffset: 12600),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 256, coolDown: 17900, coolDownOffset: 12800),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 258, coolDown: 17900, coolDownOffset: 12800),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 260, coolDown: 17900, coolDownOffset: 13000),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 262, coolDown: 17900, coolDownOffset: 13000),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 264, coolDown: 17900, coolDownOffset: 13200),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 268, coolDown: 17900, coolDownOffset: 13200),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 270, coolDown: 17900, coolDownOffset: 13400),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 272, coolDown: 17900, coolDownOffset: 13400),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 274, coolDown: 17900, coolDownOffset: 13600),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 276, coolDown: 17900, coolDownOffset: 13600),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 278, coolDown: 17900, coolDownOffset: 13800),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 280, coolDown: 17900, coolDownOffset: 13800),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 282, coolDown: 17900, coolDownOffset: 14000),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 284, coolDown: 17900, coolDownOffset: 14000),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 286, coolDown: 17900, coolDownOffset: 14200),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 288, coolDown: 17900, coolDownOffset: 14200),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 290, coolDown: 17900, coolDownOffset: 14400),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 292, coolDown: 17900, coolDownOffset: 14400),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 294, coolDown: 17900, coolDownOffset: 14600),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 296, coolDown: 17900, coolDownOffset: 14700),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 298, coolDown: 17900, coolDownOffset: 14800),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 300, coolDown: 17900, coolDownOffset: 14900),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 302, coolDown: 17900, coolDownOffset: 15000),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 304, coolDown: 17900, coolDownOffset: 15100),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 306, coolDown: 17900, coolDownOffset: 15200),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 308, coolDown: 17900, coolDownOffset: 15300),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 310, coolDown: 17900, coolDownOffset: 15400),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 312, coolDown: 17900, coolDownOffset: 15500),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 314, coolDown: 17900, coolDownOffset: 15600),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 316, coolDown: 17900, coolDownOffset: 15700),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 318, coolDown: 17900, coolDownOffset: 15800),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 320, coolDown: 17900, coolDownOffset: 15900),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 322, coolDown: 17900, coolDownOffset: 16000),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 324, coolDown: 17900, coolDownOffset: 16100),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 326, coolDown: 17900, coolDownOffset: 16200),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 328, coolDown: 17900, coolDownOffset: 16300),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 330, coolDown: 17900, coolDownOffset: 16400),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 332, coolDown: 17900, coolDownOffset: 16500),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 334, coolDown: 17900, coolDownOffset: 16600),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 336, coolDown: 17900, coolDownOffset: 16700),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 338, coolDown: 17900, coolDownOffset: 16800),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 340, coolDown: 17900, coolDownOffset: 16900),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 342, coolDown: 17900, coolDownOffset: 17000),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 344, coolDown: 17900, coolDownOffset: 17100),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 346, coolDown: 17900, coolDownOffset: 17200),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 348, coolDown: 17900, coolDownOffset: 17300),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 350, coolDown: 17900, coolDownOffset: 17400),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 352, coolDown: 17900, coolDownOffset: 17500),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 354, coolDown: 17900, coolDownOffset: 17600),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 356, coolDown: 17900, coolDownOffset: 17700),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 358, coolDown: 17900, coolDownOffset: 17800),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 360, coolDown: 17900, coolDownOffset: 17900),
                                new TimedTransition(15000, "Ring Attack + Flashing")
                                )
                        )
                    ),
                    new State("Ring Attack + Flashing",
                        new HpLessTransition(0.1, "Spawn Minions"),
                        new State("Flash 1",
                            new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                            new Flash(0x0000FF0C, 0.5, 4),
                            new TimedTransition(2000, "Ring Attack")
                        ),
                        new State("Ring Attack",
                            new Shoot(12, count: 30, fixedAngle: 4, projectileIndex: 3, coolDown: 2500),
                            new State("Ring Attack Idle",
                                new TimedTransition(2500, "SetEffect")
                            ),
                            new State("SetEffect",
                                new ConditionalEffect(ConditionEffectIndex.Invulnerable, true),
                                new TimedTransition(7500, "Flash 2")
                            )
                        ),
                        new State("Flash 2",
                            new Flash(0x0000FF0C, 0.2, 8),
                            new TimedTransition(1600, "Confuse + Quiet")
                        )
                    ),
                    new State("Confuse + Quiet",
                        new Follow(0.6, range: 1, acquireRange: 9),
                        new HpLessTransition(0.1, "Spawn Minions"),
                        new State("Shoot",
                            new Shoot(15, count: 5, shootAngle: 7.5, projectileIndex: 2, coolDown: 300),
                            new Shoot(25, count: 48, fixedAngle: 0, projectileIndex: 1, coolDown: 500),
                            new State("Confuse + Quiet Shoot Idle",
                                new TimedTransition(5000, "Start UnsetConditionEffect")
                            ),
                            new State("Start UnsetConditionEffect",
                                new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                                new TimedTransition(0, "Unset")
                            ),
                            new State("Unset",
                                new TimedTransition(5000, "Stop Shooting")
                            )
                        ),
                        new State("Stop Shooting",
                            new ConditionalEffect(ConditionEffectIndex.Invulnerable, true),
                            new Flash(0x0000FF0C, 0.5, 4),
                            new TimedTransition(3000, "Spawn Minions"))
                    ),
                    new State("Spawn Minions",
                        new State("Spawn the Fegits",
                            new Taunt("Boo!"),
                            new ConditionalEffect(ConditionEffectIndex.Invulnerable),
                            new Spawn("Ghost Warrior of Zepts", 10, 1.1),
                            new Spawn("Ghost Mage of Zepts", 10, 1.1),
                            new Spawn("Ghost Rogue of Zepts", 10, 1.1),
                            new TimedTransition(0, "Lets Shoot Fegits")
                        ),
                        new State("Lets Shoot Fegits",
                            new Shoot(15, count: 6, shootAngle: 7.5, projectileIndex: 4, coolDown: new Cooldown(750, 250)),
                            new Spawn("Ghost Warrior of Zepts", 10, 1.1),
                            new Spawn("Ghost Mage of Zepts", 10, 1.1),
                            new Spawn("Ghost Rogue of Zepts", 10, 1.1),
                            new State("Cycle_Spawn",
                                new State("Cycle_Spawn_2 Begin",
                                    new State("Cycle_Spawn 1",
                                                                        new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 3, coolDown: 17900),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 4, coolDown: 17900),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 6, coolDown: 17900, coolDownOffset: 200),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 8, coolDown: 17900, coolDownOffset: 200),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 10, coolDown: 17900, coolDownOffset: 400),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 12, coolDown: 17900, coolDownOffset: 400),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 14, coolDown: 17900, coolDownOffset: 600),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 16, coolDown: 17900, coolDownOffset: 600),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 18, coolDown: 17900, coolDownOffset: 800),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 20, coolDown: 17900, coolDownOffset: 800),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 22, coolDown: 17900, coolDownOffset: 1000),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 24, coolDown: 17900, coolDownOffset: 1000),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 26, coolDown: 17900, coolDownOffset: 1200),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 28, coolDown: 17900, coolDownOffset: 1200),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 30, coolDown: 17900, coolDownOffset: 1400),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 32, coolDown: 17900, coolDownOffset: 1400),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 34, coolDown: 17900, coolDownOffset: 1600),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 36, coolDown: 17900, coolDownOffset: 1600),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 38, coolDown: 17900, coolDownOffset: 1800),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 40, coolDown: 17900, coolDownOffset: 1800),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 42, coolDown: 17900, coolDownOffset: 2000),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 44, coolDown: 17900, coolDownOffset: 2000),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 46, coolDown: 17900, coolDownOffset: 2200),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 48, coolDown: 17900, coolDownOffset: 2200),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 50, coolDown: 17900, coolDownOffset: 2400),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 52, coolDown: 17900, coolDownOffset: 2400),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 54, coolDown: 17900, coolDownOffset: 2600),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 56, coolDown: 17900, coolDownOffset: 2600),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 58, coolDown: 17900, coolDownOffset: 2800),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 60, coolDown: 17900, coolDownOffset: 2800),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 62, coolDown: 17900, coolDownOffset: 3000),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 64, coolDown: 17900, coolDownOffset: 3000),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 66, coolDown: 17900, coolDownOffset: 3200),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 68, coolDown: 17900, coolDownOffset: 3200),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 70, coolDown: 17900, coolDownOffset: 3400),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 72, coolDown: 17900, coolDownOffset: 3400),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 74, coolDown: 17900, coolDownOffset: 3600),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 76, coolDown: 17900, coolDownOffset: 3600),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 78, coolDown: 17900, coolDownOffset: 3800),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 80, coolDown: 17900, coolDownOffset: 3800),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 82, coolDown: 17900, coolDownOffset: 4000),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 84, coolDown: 17900, coolDownOffset: 4000),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 86, coolDown: 17900, coolDownOffset: 4200),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 88, coolDown: 17900, coolDownOffset: 4200),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 90, coolDown: 17900, coolDownOffset: 4400),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 92, coolDown: 17900, coolDownOffset: 4400),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 94, coolDown: 17900, coolDownOffset: 4600),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 96, coolDown: 17900, coolDownOffset: 4600),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 98, coolDown: 17900, coolDownOffset: 4800),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 100, coolDown: 17900, coolDownOffset: 4800),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 102, coolDown: 17900, coolDownOffset: 5000),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 104, coolDown: 17900, coolDownOffset: 5000),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 106, coolDown: 17900, coolDownOffset: 5200),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 108, coolDown: 17900, coolDownOffset: 5200),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 110, coolDown: 17900, coolDownOffset: 5400),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 112, coolDown: 17900, coolDownOffset: 5400),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 114, coolDown: 17900, coolDownOffset: 5600),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 116, coolDown: 17900, coolDownOffset: 5600),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 118, coolDown: 17900, coolDownOffset: 5800),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 120, coolDown: 17900, coolDownOffset: 5800),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 122, coolDown: 17900, coolDownOffset: 6000),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 124, coolDown: 17900, coolDownOffset: 6000),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 126, coolDown: 17900, coolDownOffset: 6200),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 128, coolDown: 17900, coolDownOffset: 6200),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 130, coolDown: 17900, coolDownOffset: 6400),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 132, coolDown: 17900, coolDownOffset: 6400),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 134, coolDown: 17900, coolDownOffset: 6600),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 136, coolDown: 17900, coolDownOffset: 6600),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 138, coolDown: 17900, coolDownOffset: 6800),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 140, coolDown: 17900, coolDownOffset: 6800),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 142, coolDown: 17900, coolDownOffset: 7000),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 144, coolDown: 17900, coolDownOffset: 7000),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 146, coolDown: 17900, coolDownOffset: 7200),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 148, coolDown: 17900, coolDownOffset: 7200),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 150, coolDown: 17900, coolDownOffset: 7400),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 152, coolDown: 17900, coolDownOffset: 7400),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 154, coolDown: 17900, coolDownOffset: 7600),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 156, coolDown: 17900, coolDownOffset: 7600),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 158, coolDown: 17900, coolDownOffset: 7800),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 160, coolDown: 17900, coolDownOffset: 7800),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 162, coolDown: 17900, coolDownOffset: 8000),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 164, coolDown: 17900, coolDownOffset: 8000),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 166, coolDown: 17900, coolDownOffset: 8200),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 168, coolDown: 17900, coolDownOffset: 8200),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 170, coolDown: 17900, coolDownOffset: 8400),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 172, coolDown: 17900, coolDownOffset: 8400),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 174, coolDown: 17900, coolDownOffset: 8600),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 176, coolDown: 17900, coolDownOffset: 8600),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 178, coolDown: 17900, coolDownOffset: 8800),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 180, coolDown: 17900, coolDownOffset: 8800),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 182, coolDown: 17900, coolDownOffset: 9000),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 184, coolDown: 17900, coolDownOffset: 9000),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 186, coolDown: 17900, coolDownOffset: 9200),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 188, coolDown: 17900, coolDownOffset: 9200),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 190, coolDown: 17900, coolDownOffset: 9400),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 192, coolDown: 17900, coolDownOffset: 9400),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 194, coolDown: 17900, coolDownOffset: 9600),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 196, coolDown: 17900, coolDownOffset: 9600),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 198, coolDown: 17900, coolDownOffset: 9800),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 200, coolDown: 17900, coolDownOffset: 9800),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 202, coolDown: 17900, coolDownOffset: 10000),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 204, coolDown: 17900, coolDownOffset: 10000),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 206, coolDown: 17900, coolDownOffset: 10200),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 208, coolDown: 17900, coolDownOffset: 10200),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 210, coolDown: 17900, coolDownOffset: 10400),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 212, coolDown: 17900, coolDownOffset: 10400),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 214, coolDown: 17900, coolDownOffset: 10600),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 216, coolDown: 17900, coolDownOffset: 10600),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 218, coolDown: 17900, coolDownOffset: 10800),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 220, coolDown: 17900, coolDownOffset: 10800),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 222, coolDown: 17900, coolDownOffset: 11000), //
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 224, coolDown: 17900, coolDownOffset: 11000),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 226, coolDown: 17900, coolDownOffset: 11200),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 228, coolDown: 17900, coolDownOffset: 11400),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 230, coolDown: 17900, coolDownOffset: 11400),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 232, coolDown: 17900, coolDownOffset: 11600),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 234, coolDown: 17900, coolDownOffset: 11600), //
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 236, coolDown: 17900, coolDownOffset: 11800),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 238, coolDown: 17900, coolDownOffset: 11800),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 240, coolDown: 17900, coolDownOffset: 12000),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 242, coolDown: 17900, coolDownOffset: 12000),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 244, coolDown: 17900, coolDownOffset: 12200),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 246, coolDown: 17900, coolDownOffset: 12200),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 248, coolDown: 17900, coolDownOffset: 12400),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 250, coolDown: 17900, coolDownOffset: 12400),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 252, coolDown: 17900, coolDownOffset: 12600),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 254, coolDown: 17900, coolDownOffset: 12600),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 256, coolDown: 17900, coolDownOffset: 12800),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 258, coolDown: 17900, coolDownOffset: 12800),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 260, coolDown: 17900, coolDownOffset: 13000),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 262, coolDown: 17900, coolDownOffset: 13000),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 264, coolDown: 17900, coolDownOffset: 13200),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 268, coolDown: 17900, coolDownOffset: 13200),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 270, coolDown: 17900, coolDownOffset: 13400),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 272, coolDown: 17900, coolDownOffset: 13400),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 274, coolDown: 17900, coolDownOffset: 13600),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 276, coolDown: 17900, coolDownOffset: 13600),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 278, coolDown: 17900, coolDownOffset: 13800),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 280, coolDown: 17900, coolDownOffset: 13800),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 282, coolDown: 17900, coolDownOffset: 14000),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 284, coolDown: 17900, coolDownOffset: 14000),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 286, coolDown: 17900, coolDownOffset: 14200),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 288, coolDown: 17900, coolDownOffset: 14200),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 290, coolDown: 17900, coolDownOffset: 14400),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 292, coolDown: 17900, coolDownOffset: 14400),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 294, coolDown: 17900, coolDownOffset: 14600),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 296, coolDown: 17900, coolDownOffset: 14700),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 298, coolDown: 17900, coolDownOffset: 14800),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 300, coolDown: 17900, coolDownOffset: 14900),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 302, coolDown: 17900, coolDownOffset: 15000),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 304, coolDown: 17900, coolDownOffset: 15100),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 306, coolDown: 17900, coolDownOffset: 15200),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 308, coolDown: 17900, coolDownOffset: 15300),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 310, coolDown: 17900, coolDownOffset: 15400),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 312, coolDown: 17900, coolDownOffset: 15500),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 314, coolDown: 17900, coolDownOffset: 15600),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 316, coolDown: 17900, coolDownOffset: 15700),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 318, coolDown: 17900, coolDownOffset: 15800),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 320, coolDown: 17900, coolDownOffset: 15900),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 322, coolDown: 17900, coolDownOffset: 16000),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 324, coolDown: 17900, coolDownOffset: 16100),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 326, coolDown: 17900, coolDownOffset: 16200),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 328, coolDown: 17900, coolDownOffset: 16300),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 330, coolDown: 17900, coolDownOffset: 16400),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 332, coolDown: 17900, coolDownOffset: 16500),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 334, coolDown: 17900, coolDownOffset: 16600),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 336, coolDown: 17900, coolDownOffset: 16700),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 338, coolDown: 17900, coolDownOffset: 16800),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 340, coolDown: 17900, coolDownOffset: 16900),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 342, coolDown: 17900, coolDownOffset: 17000),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 344, coolDown: 17900, coolDownOffset: 17100),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 346, coolDown: 17900, coolDownOffset: 17200),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 348, coolDown: 17900, coolDownOffset: 17300),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 350, coolDown: 17900, coolDownOffset: 17400),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 352, coolDown: 17900, coolDownOffset: 17500),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 354, coolDown: 17900, coolDownOffset: 17600),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 356, coolDown: 17900, coolDownOffset: 17700),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 358, coolDown: 17900, coolDownOffset: 17800),
                            new Shoot(15, count: 3, shootAngle: 120, fixedAngle: 360, coolDown: 17900, coolDownOffset: 17900),
                                new TimedTransition(15000, "Ring Attack + Flashing")
                                    )
                                )
                            )
                        )
                    )
                ),
                new Threshold(0.01,
                    new ItemLoot("Greater Potion of Wisdom", 1)
                ),
                new Threshold(0.01,
                    new ItemLoot("Bow of the Morning Star", 0.025),
                    new ItemLoot("Doom Bow", 0.05),
                    new ItemLoot("Wine Cellar Incantation", 0.025),
                    new TierLoot(4, ItemType.Ring, 0.2),
                    new TierLoot(5, ItemType.Ring, 0.1),
                    new TierLoot(9, ItemType.Weapon, 0.2),
                    new TierLoot(10, ItemType.Weapon, 0.1),
                    new TierLoot(4, ItemType.Ability, 0.3),
                    new TierLoot(5, ItemType.Ability, 0.2)
                ),
                new Threshold(0.2,
                    new EggLoot(EggRarity.Common, 0.1),
                    new EggLoot(EggRarity.Uncommon, 0.05),
                    new EggLoot(EggRarity.Rare, 0.01),
                    new EggLoot(EggRarity.Legendary, 0.001)
                )
            )
        .Init("Ghost Warrior of Zepts",
                new State(
                    new Shoot(10, coolDown: new Cooldown(2000, 1000)),
                    new State("Follow",
                        new Prioritize(
                            new Follow(.4, 7, 1),
                            new Protect(1, "Zepts the ghost king", protectionRange: 1, reprotectRange: 2)
                        )
                    ),
                    new State("Wander",
                        new Wander(0.4)
                    )
                ),
                new ItemLoot("Health Potion", 0.05),
                new ItemLoot("Magic Potion", 0.05)
            )
            .Init("Ghost Mage of Zepts",
                new State(
                    new Shoot(10, coolDown: new Cooldown(2000, 1000)),
                    new State("Follow",
                        new Prioritize(
                            new Follow(.4, 7, 1),
                            new Protect(1, "Zepts the ghost king", protectionRange: 1, reprotectRange: 2)
                        )
                    ),
                    new State("Wander",
                        new Wander(0.4)
                    )
                ),
                new ItemLoot("Health Potion", 0.05),
                new ItemLoot("Magic Potion", 0.05)
            )
            .Init("Ghost Rogue of Zepts",
                new State(
                    new Shoot(10, coolDown: new Cooldown(2000, 1000)),
                    new State("Follow",
                        new Prioritize(
                            new Follow(.4, 7, 1),
                            new Protect(1, "Zepts the ghost king", protectionRange: 1, reprotectRange: 2)
                        )
                    ),
                    new State("Wander",
                        new Wander(0.4)
                    )
                ),
                new ItemLoot("Health Potion", 0.05),
                new ItemLoot("Magic Potion", 0.05)
            )
                .Init("Epic Lair Ghost Bat",
            new State(
              new State("Wander",
                    new Wander(0.4),
                    new PlayerWithinTransition(10, "Follow")
                  ),
              new State("Follow",
                new NoPlayerWithinTransition(11, "Wander"),
                new Follow(5.0, 10, coolDown: 0),
                  new Shoot(10, projectileIndex: 0, coolDown: 3000)
                  )))
                .Init("Epic Lair Grey Spectre",
                   new State(
                   new Wander(0.4),
                    new Grenade(4, 300, 8, coolDown: 1000),
                    new Shoot(10, projectileIndex: 0, coolDown: 1000)
                       ))
                .Init("Epic Lair Blue Spectre",
                    new State(
                        new Wander(0.4),
                    new Grenade(4, 300, 8, coolDown: 1000),
                    new Shoot(10, projectileIndex: 0, coolDown: 1000)
                        ))
                   .Init("Epic Lair White Spectre",
                     new State(
                    new Wander(0.4),
                    new Grenade(4, 300, 8, coolDown: 1000),
                    new Shoot(10, projectileIndex: 0, coolDown: 1000)
                         ))
                .Init("Epic Lair Skeleton",
                       new State(
                           new State("Wander",
                           new Wander(0.4),
                           new PlayerWithinTransition(5, "Follow")
                           ),
                       new State("Follow",
                          new Shoot(10, projectileIndex: 0, coolDown: 1000),
                           new NoPlayerWithinTransition(11, "Wander"),
                             new Follow(1.0, 10, coolDown: 0)
                           )))
        .Init("Epic Lair Skeleton King",
            new State(
                new State("Wander",
                new PlayerWithinTransition(5, "Follow"),
                new Wander(0.4)
                    ),
                new State("Follow",
                    new NoPlayerWithinTransition(11, "Wander"),
                    new Shoot(10, count: 3, projectileIndex: 0, coolDown: 1000),
                    new Follow(1.0, 10, coolDown: 0)
                    )))
        .Init("Epic Lair Skeleton Mage",
            new State(
                new State("Wander",
                    new PlayerWithinTransition(5, "Follow"),
                    new Wander(0.4)
                    ),
                new State("Follow",
                    new NoPlayerWithinTransition(11, "Wander"),
                    new Follow(1.0, 10, coolDown: 0),
                    new Shoot(10, count: 1, projectileIndex: 0, coolDown: 1000)
                    )))
        .Init("Epic Lair Skeleton Swordsman",
            new State(
                new State("Wander",
                    new Wander(0.4),
                    new PlayerWithinTransition(5, "Follow")
                    ),
                new State("Follow",
                    new Follow(1.0, 10, coolDown: 0),
                    new Shoot(10, projectileIndex: 0, coolDown: 1),
                    new NoPlayerWithinTransition(11, "Wander")
                    )))
        .Init("Epic Lair Skeleton Veteran",
            new State(
                new State("Wander",
                    new Wander(0.4),
                    new PlayerWithinTransition(5, "Follow")
                    ),
                new State("Follow",
                    new NoPlayerWithinTransition(11, "Wander"),
                    new Shoot(10, projectileIndex: 0, coolDown: 1000),
                    new Follow(1.0, 10, coolDown: 0)
                    )))
        .Init("Epic Lair Mummy",
            new State(
                new State("Wander",
                    new Wander(0.4),
                    new PlayerWithinTransition(5, "Follow")
                    ),
                new State("Follow",
                    new NoPlayerWithinTransition(11, "Wander"),
                    new Shoot(10, projectileIndex: 0, coolDown: 1000),
                    new Follow(1.0, 10, coolDown: 0)
                      )))
        .Init("Epic Lair Mummy King",
            new State(
                new State("Wander",
                    new Wander(0.4),
                    new PlayerWithinTransition(5, "Follow")
                    ),
                new State("Follow",
                    new NoPlayerWithinTransition(11, "Wander"),
                    new Shoot(10, projectileIndex: 0, coolDown: 1000),
                    new Follow(1.0, 10, coolDown: 0)
                      )))
        .Init("Epic Lair Mummy Pharaoh",
            new State(
                new State("Wander",
                    new Wander(0.4),
                    new PlayerWithinTransition(5, "Follow")
                    ),
                new State("Follow",
                    new NoPlayerWithinTransition(11, "Wander"),
                    new Shoot(10, projectileIndex: 0, coolDown: 1000),
                    new Follow(1.0, 10, coolDown: 0)
                    )))
        .Init("Epic Lair Big Brown Slime",
            new State(
                new Wander(0.4),
                new Shoot(10, count: 3, coolDown: 500)
               // new TransformOnDeath("Epic Lair Little Brown Slime", min: 5, max: 6)
           ))
        .Init("Epic Lair Little Brown Slime",
            new State(
                new Wander(0.4),
                new Shoot(10, count: 3, coolDown: 500)
                 ))
        .Init("Epic Lair Big Black Slime",
            new State(
                new Wander(0.4),
                new Shoot(10, count: 1, coolDown: 500)
             //   new TransformOnDeath("Epic Lair Medium Black Slime", min: 3, max: 4)
           ))
        .Init("Epic Lair Medium Black Slime",
            new State(
                new Wander(0.4),
               // new TransformOnDeath("Epic Lair Small Black Slime", min: 5, max: 6),
                new Shoot(10, count: 1, coolDown: 500)
                ))
        .Init("Epic Lair Little Black Slime",
            new State(
                new Wander(0.4),
                new Shoot(10, count: 3, coolDown: 500)
                ))
         .Init("Epic Lair Construct Giant",
            new State(
                new State("Wander",
                    new Wander(0.4),
                    new PlayerWithinTransition(5, "Follow")
                    ),
                new State("Follow",
                    new NoPlayerWithinTransition(11, "Wander"),
                    new Shoot(10, count: 3, projectileIndex: 0, coolDown: 1000),
                    new Shoot(10, count: 1, projectileIndex: 1, coolDown: 1000),
                    new Follow(1.0, 10, coolDown: 0)
                     )))
         .Init("Epic Lair Construct Titan",
            new State(
                new State("Wander",
                    new Wander(0.4),
                    new PlayerWithinTransition(5, "Follow")
                    ),
                new State("Follow",
                    new NoPlayerWithinTransition(11, "Wander"),
                    new Shoot(10, count: 3, projectileIndex: 0, coolDown: 1000),
                    new Shoot(10, count: 3, projectileIndex: 1, coolDown: 1000),
                    new Follow(1.0, 10, coolDown: 0)
                    )))
          .Init("Epic Lair Brown Bat",
            new State(
              new State("Wander",
                    new Wander(0.4),
                    new PlayerWithinTransition(10, "Follow")
                  ),
              new State("Follow",
                new NoPlayerWithinTransition(11, "Wander"),
                new Follow(5.0, 10, coolDown: 0),
                  new Shoot(10, projectileIndex: 0, coolDown: 3000)
                  )))
        .Init("Epic Lair Reaper",
            new State(
                 new State("Wander",
                    new Wander(0.4),
                    new PlayerWithinTransition(10, "Follow")
                  ),
              new State("Follow",
                new NoPlayerWithinTransition(11, "Wander"),
                new Follow(1.5, 10, coolDown: 0),
                  new Shoot(10, projectileIndex: 0, coolDown: 200)
                  )))
        .Init("Epic Lair Vampire",
            new State(
                new State("Wander",
                    new Wander(0.4),
                    new PlayerWithinTransition(10, "Follow")
                  ),
              new State("Follow",
                new NoPlayerWithinTransition(11, "Wander"),
                new Follow(1.5, 10, coolDown: 0),
                  new Shoot(10, projectileIndex: 0, coolDown: 500),
                  new Shoot(10, projectileIndex: 1, coolDown: 1000)
                    )))
        .Init("Epic Lair Vampire King",
            new State(
                new State("Wander",
                    new Wander(0.4),
                    new PlayerWithinTransition(10, "Follow")
                  ),
              new State("Follow",
                new NoPlayerWithinTransition(11, "Wander"),
                new Follow(1.5, 10, coolDown: 0),
                  new Shoot(10, projectileIndex: 0, coolDown: 500),
                  new Shoot(10, projectileIndex: 1, coolDown: 1000)
                 )));
    }
}