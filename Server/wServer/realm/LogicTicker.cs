#region

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using wServer.logic;
using wServer.realm.entities.player;

#endregion

namespace wServer.realm
{
    public class LogicTicker
    {
        public static RealmTime CurrentTime;
        private readonly ConcurrentQueue<Action<RealmTime>>[] pendings;

        public int MsPT;
        public int TPS;

        public LogicTicker(RealmManager manager)
        {
            Manager = manager;
            pendings = new ConcurrentQueue<Action<RealmTime>>[5];
            for (int i = 0; i < 5; i++)
                pendings[i] = new ConcurrentQueue<Action<RealmTime>>();

            TPS = manager.TPS;
            MsPT = 1000/TPS;
        }

        public RealmManager Manager { get; private set; }

        public void AddPendingAction(Action<RealmTime> callback)
        {
            AddPendingAction(callback, PendingPriority.Normal);
        }

        public void AddPendingAction(Action<RealmTime> callback, PendingPriority priority)
        {
            pendings[(int) priority].Enqueue(callback);
        }

        public void TickLoop()
        {
            if (CheckConfig.IsDebugOn())
                Console.WriteLine("Logic loop started.");
            Stopwatch watch = new Stopwatch();
            long dt = 0;
            long count = 0;

            watch.Start();
            RealmTime t = new RealmTime();
            do
            {
                if (Manager.Terminating) break;

                long times = dt/MsPT;
                dt -= times*MsPT;
                times++;

                long b = watch.ElapsedMilliseconds;

                count += times;
                if (times > 3)
                    Console.WriteLine("LAGGED!| time:" + times + " dt:" + dt + " count:" + count + " time:" + b + " tps:" +
                             count/(b/1000.0));

                t.tickTimes = b;
                t.tickCount = count;
                t.thisTickCounts = (int) times;
                t.thisTickTimes = (int) (times*MsPT);

                foreach (ConcurrentQueue<Action<RealmTime>> i in pendings)
                {
                    Action<RealmTime> callback;
                    while (i.TryDequeue(out callback))
                    {
                        try
                        {
                            callback(t);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex);
                        }
                    }
                }
                TickWorlds1(t);

                Player[] tradingPlayers = TradeManager.TradingPlayers.Where(_ => _.Owner == null).ToArray();
                foreach (var player in tradingPlayers)
                    TradeManager.TradingPlayers.Remove(player);

                KeyValuePair<Player, Player>[] requestPlayers = TradeManager.CurrentRequests.Where(_ => _.Key.Owner == null || _.Value.Owner == null).ToArray();
                foreach (var players in requestPlayers)
                    TradeManager.CurrentRequests.Remove(players);

                try
                {
                    GuildManager.Tick(CurrentTime);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }

                Thread.Sleep(MsPT);
                dt += Math.Max(0, watch.ElapsedMilliseconds - b - MsPT);
            } while (true);
            Console.WriteLine("Logic loop stopped.");
        }

        private void TickWorlds1(RealmTime t) //Continous simulation
        {
            CurrentTime = t;
            foreach (World i in Manager.Worlds.Values.Distinct())
                i.Tick(t);
        }
    }
}