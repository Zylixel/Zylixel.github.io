#region

using System;
using wServer.networking.svrPackets;
using wServer.realm;
using wServer.realm.entities;

#endregion

namespace wServer.logic.behaviors
{
    public class KeeperDisappear : CycleBehavior
    {
        protected Cooldown coolDown;
        private readonly string Command;
        private int Stage;


        public KeeperDisappear(string Command, Cooldown coolDown = new Cooldown())
        {
            this.coolDown = coolDown.Normalize();
            this.Command = Command;
        }

        protected override void OnStateEntry(Entity host, RealmTime time, ref object state)
        {
            state = 0;
            if (Command == "Disappear") Stage = 1;
            if (Command == "Appear") Stage = 10;
        }

        protected override void TickCore(Entity host, RealmTime time, ref object state)
        {
            if (Command == "Disppear") if (Stage == 10) return;
            if (Command == "Appear") if (Stage == 0) return;
            if (state == null) return;
            int cool = (int)state;
            Status = CycleStatus.NotStarted;

            if (cool <= 0)
            {
                if ((host as Enemy).AltTextureIndex != Stage)
                {
                    (host as Enemy).AltTextureIndex = Stage;
                    host.UpdateCount++;
                    if (Command == "Appear") Stage = Stage - 1;
                    if (Command == "Disappear") Stage = Stage + 1;
                }
                cool = coolDown.Next(Random);
                Status = CycleStatus.Completed;
            }
            else
            {
                cool -= time.thisTickTimes;
                Status = CycleStatus.InProgress;
            }

            state = cool;
        }
    }
}