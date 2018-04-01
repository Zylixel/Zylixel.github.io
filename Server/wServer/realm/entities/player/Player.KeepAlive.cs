#region

using System;
using wServer.networking.cliPackets;

#endregion

namespace wServer.realm.entities.player
{
    public partial class Player
    {
        private const int PingPeriod = 1000;

        private int _updateLastSeen;
        private int curClientTime;
        private int oldClientTime;
        private int first5;
        
        public void ClientTick(RealmTime time, MovePacket packet)
        {
            oldClientTime = curClientTime;
            curClientTime = packet.Time;
            if (first5 < 5) first5++;
        }

        internal void Pong(int time, PongPacket pkt)
        {
            try
            {
                _updateLastSeen++;

                if (_updateLastSeen >= 60)
                {
                    Manager.Database.DoActionAsync(db =>
                    {
                        db.UpdateLastSeen(Client.Account.AccountId, Client.Character.CharacterId, WorldInstance.Name);
                        _updateLastSeen = 0;
                    });
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}