#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using log4net;
using wServer.networking.svrPackets;
using wServer.realm.entities.player;
using db;
using MySql.Data.MySqlClient;

#endregion

namespace wServer.realm.entities.merchant
{
    public class MerchantsTest : SellableObject
    {
        public static bool newMerchant;
        private int tickcount;
        private static List<KeyValuePair<string, int>> AddedTypes { get; set; }

        private static readonly ILog log = LogManager.GetLogger(typeof(MerchantsTest));
        public static Random Random { get; private set; }

        public MerchantsTest(RealmManager manager, ushort objType, World owner = null)
            : base(manager, objType)
        {
            Size = 100;
            if (owner != null)
                Owner = owner;

            if (owner != null) ResolveMType();
        }

        public int MType { get; set; }
        public int MRemaining { get; set; }
        public int MTime { get; set; }

        protected override void ExportStats(IDictionary<StatsType, object> stats)
        {
            stats[StatsType.MerchantRemainingCount] = MRemaining;
            stats[StatsType.MerchantRemainingMinute] = newMerchant ? Int32.MaxValue : MTime;
            stats[StatsType.SellablePrice] = Price;
            stats[StatsType.SellablePriceCurrency] = Currency;

            base.ExportStats(stats);
        }

        public override void Init(World owner)
        {
            base.Init(owner);
            ResolveMType();
            UpdateCount++;
        }

        protected override bool TryDeduct(Player player)
        {
            var acc = player.Client.Account;
            if (acc.Stats.Fame < Price) return false;
            return true;
        }

        public override void Buy(Player player)
        {
            Manager.Database.DoActionAsync(db =>
            {
                if (ObjectType == 0x01ca) //Merchant
                {
                    if (TryDeduct(player))
                    {
                        for (var i = 0; i < player.Inventory.Length; i++)
                        {
                            try
                            {
                                XElement ist;
                                Manager.GameData.ObjectTypeToElement.TryGetValue((ushort)MType, out ist);
                                if (player.Inventory[i] == null &&
                                    (player.SlotTypes[i] == 10 ||
                                     player.SlotTypes[i] == Convert.ToInt16(ist.Element("SlotType").Value)))
                                {
                                    player.Inventory[i] = Manager.GameData.Items[(ushort)MType];

                                    player.CurrentFame =
                                        player.Client.Account.Stats.Fame =
                                            db.UpdateFame(player.Client.Account, -Price);
                                    {
                                        log.Error("Attemping to delete item from database: " + MType + " | " + Price);
                                        MySqlCommand cmd = db.CreateQuery();
                                        cmd.CommandText = "DELETE FROM market WHERE itemid=@itemid AND fame=@fame";
                                        cmd.Parameters.AddWithValue("@itemID", MType);
                                        cmd.Parameters.AddWithValue("@fame", Price);
                                        cmd.ExecuteNonQuery();
                                    }
                                    {
                                        int accID = 0;
                                        accID = db.GetMarketCharID(MType, Price);
                                        MySqlCommand cmd = db.CreateQuery();
                                        cmd.CommandText = "UPDATE stats SET fame = fame + @Price WHERE accId=@accId";
                                        cmd.Parameters.AddWithValue("@accId", accID);
                                        cmd.Parameters.AddWithValue("@Price", Price);
                                        cmd.ExecuteNonQuery();
                                    }

                                    player.Client.SendPacket(new BuyResultPacket
                                    {
                                        Result = 0,
                                        Message = "{\"key\":\"server.buy_success\"}"
                                    });
                                    MRemaining--;
                                    player.UpdateCount++;
                                    player.SaveToCharacter();
                                    UpdateCount++;
                                    return;
                                }
                            }
                            catch (Exception e)
                            {
                                log.Error(e);
                            }
                        }
                        player.Client.SendPacket(new BuyResultPacket
                        {
                            Result = 0,
                            Message = "{\"key\":\"server.inventory_full\"}"
                        });
                    }
                    else
                    {
                    }
                    switch (Currency)
                    {
                        case CurrencyType.Fame:
                            player.Client.SendPacket(new BuyResultPacket
                            {
                                Result = 6, //ID for noFame
                                Message = "{\"key\":\"server.not_enough_fame\"}"
                            });
                            break;
                    }
                }
            });
        }

        public override void Tick(RealmTime time)
        {
            try
            {
                {
                    tickcount++;
                    if (tickcount % (Manager?.TPS * 1) == 0) //once per minute after spawning
                    {
                        MTime--;
                        UpdateCount++;
                    }
                }

                if (MRemaining == 0 && MType != -1)
                {
                    Recreate(this);
                    UpdateCount++;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }

        public void Recreate(MerchantsTest x)
        {
            try
            {
                var mrc = new MerchantsTest(Manager, x.ObjectType, x.Owner);
                mrc.Move(x.X, x.Y);
                var w = Owner;
                Owner.LeaveWorld(this);
                w.Timers.Add(new WorldTimer(45, (world, time) => w.EnterWorld(mrc)));
            }
            catch (Exception e)
            {
                log.Error(e);
            }
        }

        private readonly Dictionary<int, Tuple<int, CurrencyType>> prices = MerchantLists.prices2;

        public void ResolveMType()
        {
            var list = new int[0];
                list = MerchantLists.ZyList;
            

            AddedTypes = new List<KeyValuePair<string, int>>();
            list.Shuffle();
            foreach (var t1 in list)
            {
                AddedTypes.Add(new KeyValuePair<string, int>(Owner.Name, t1));
                MType = t1;
                MTime = Random.Next(60, 90);
                MRemaining = Random.Next(1, 1);
                Owner.Timers.Add(new WorldTimer(30000, (w, t) =>
                {
                    newMerchant = false;
                    UpdateCount++;
                }));

                Tuple<int, CurrencyType> price;
                if (prices.TryGetValue(MType, out price))
                {
                    using (Database db = new Database())
                    {
                        Price = 1;
                    }
                    Currency = price.Item2;
                }

                break;
            }
            UpdateCount++;
        }
    }
}