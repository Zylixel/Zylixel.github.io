#region

using System;
using wServer.networking.svrPackets;
using wServer.realm;
using wServer.realm.entities;

#endregion

namespace wServer.logic.behaviors
{
    public class KeeperAppear : CycleBehavior
    {
        protected Cooldown coolDown;
        private int Stage;


        public KeeperAppear(Cooldown coolDown = new Cooldown())
        {
            this.coolDown = coolDown.Normalize();
            Stage = 9;
        }

        protected override void OnStateEntry(Entity host, RealmTime time, ref object state)
        {
            state = 0;
        }

        protected override void TickCore(Entity host, RealmTime time, ref object state)
        {
            if (Stage == 0) return;
            if (state == null) return;
            int cool = (int)state;
            Status = CycleStatus.NotStarted;

            if (cool <= 0)
            {
                {
                    (host as Enemy).AltTextureIndex = Stage;
                    host.UpdateCount++;
                    Stage = Stage - 1;
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