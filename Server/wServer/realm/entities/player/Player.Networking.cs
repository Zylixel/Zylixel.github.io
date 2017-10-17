#region

using System;
using System.Collections.Generic;
using System.Linq;
using wServer.networking;

#endregion

namespace wServer.realm.entities.player
{
    partial class Player
    {
        private bool _worldBroadcast = true;

        private readonly Queue<Tuple<Packet, Predicate<Player>>> _pendingPackets =
            new Queue<Tuple<Packet, Predicate<Player>>>();

        internal void Flush()
        {
            if (Owner != null)
            {
                foreach (var i in Owner.Players.Values)
                    foreach (var j in _pendingPackets.Where(j => j.Item2(i)))
                        i.Client.SendPacket(j.Item1);
            }
            _pendingPackets.Clear();
        }

        public void BroadcastSync(Packet packet) //sync at Move
        {
            BroadcastSync(packet, _ => true);
        }

        public void BroadcastSync(Packet packet, Predicate<Player> cond)
        {
            if(_worldBroadcast)
                Owner.BroadcastPacketSync(packet, cond);
            else
                _pendingPackets.Enqueue(Tuple.Create(packet, cond));
        }

        private void BroadcastSync(IEnumerable<Packet> packets)
        {
            foreach (var i in packets)
                BroadcastSync(i, _ => true);
        }

        private void BroadcastSync(IEnumerable<Packet> packets, Predicate<Player> cond)
        {
            foreach (var i in packets)
                BroadcastSync(i, cond);
        }
    }
}