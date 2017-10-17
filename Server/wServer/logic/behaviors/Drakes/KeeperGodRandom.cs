using System;

namespace wServer.logic.behaviors.Drakes
{
    public class KeeperGodRandom
    {
        public static string generate(int seed)
        {
            Random rand = new Random(seed);
                double r  = rand.Next(1,9);
                if (r == 1)
                {
                    return "beholder";
                }
            if (r == 2)
            {
                return "White Demon";
            }
            if (r == 3)
            {
                return "ghost god";
            }
            if (r == 4)
            {
                return "leviathan";
            }
            if (r == 5)
            {
                return "ent god";
            }
            if (r == 6)
            {
                return "native sprite god";
            }
            if (r == 7)
            {
                return "slime god";
            }
            return "medusa";
        }
    }
}
