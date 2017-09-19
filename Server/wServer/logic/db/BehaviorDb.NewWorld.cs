using wServer.logic.behaviors;
using wServer.logic.transitions;
using wServer.logic.loot;

namespace wServer.logic
{
    partial class BehaviorDb
    {
        private _ NewWorld = () => Behav()
        .Init("NewWorld Water Summoner",
            new State(
                new State("DoTheThing",
                    new ApplySetpiece("NewWorldWaterFill"),
                    new Suicide()
                    )
                )
            )
            


        ;
    }
}
