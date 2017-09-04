using Mono.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wServer.realm;
using wServer.realm.entities.player;

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
                else if (r == 2)
             {
                    return "White Demon";
                }
                else if (r == 3)
            {
                    return "ghost god";
                }
                else if (r == 4)
                {
                return "leviathan";
                }
            else if (r == 5)
            {
                return "ent god";
            }
            else if (r == 6)
            {
                return "native sprite god";
            }
            else if (r == 7)
            {
                return "slime god";
            }
            else
            {
                return "medusa";
            }
        }
    }
}
