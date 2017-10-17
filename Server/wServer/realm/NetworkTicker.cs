using System;
using System.Collections.Concurrent;
using System.Threading;
using log4net;
using wServer.networking;

namespace wServer.realm
{
    using Work = Tuple<Client, PacketID, byte[]>;
    public class NetworkTicker
    {
        ILog log = LogManager.GetLogger(typeof(NetworkTicker));

        public RealmManager Manager { get; private set; }
        public NetworkTicker(RealmManager manager)
        {
            Manager = manager;
        }

        public void AddPendingPacket(Client client, PacketID id, byte[] packet)
        {
            pendings.Enqueue(new Work(client, id, packet));
        }
        static ConcurrentQueue<Work> pendings = new ConcurrentQueue<Work>();
        static SpinWait loopLock = new SpinWait();


        public void TickLoop()
        {
            log.Info("Procces: Starting network loop.");
            Work work;
            while (true)
            {
                if (Manager.Terminating) break;
                loopLock.Reset();
                while (pendings.TryDequeue(out work))
                {
                    if (Manager.Terminating) return;
                    if (work.Item1.Stage == ProtocalStage.Disconnected)
                    {
                        Client client;
                        Manager.Clients.TryRemove(work.Item1.Account.AccountId, out client);
                        continue;
                    }
                    try
                    {
                        Packet packet = Packet.Packets[work.Item2].CreateInstance();
                        packet.Read(work.Item1, work.Item3, 0, work.Item3.Length);
                        work.Item1.ProcessPacket(packet);
                    }
                    catch { }
                }
                while (pendings.Count == 0 && !Manager.Terminating)
                    loopLock.SpinOnce();
            }
            log.Info("Procces: Stopping network loop.");
        }
    }
}