using System;

namespace wServer.logic.behaviors.Drakes
{
    public class KeeperGodRandom
    {
        public static string generate(int seed)
        {
            switch (new Random(seed).Next(1, 7)) {
                case 1: return "beholder";
                case 2: return "White Demon";
                case 3: return "ghost god";
                case 4: return "leviathan";
                case 5: return "ent god";
                case 6: return "native sprite god";
                case 7: return "slime god";
                default: return "White Demon";
            }
        }
    }
}
