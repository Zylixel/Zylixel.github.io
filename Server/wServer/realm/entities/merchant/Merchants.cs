#region

using System;
using System.Collections.Generic;
using System.Xml.Linq;
using log4net;
using wServer.networking.svrPackets;
using wServer.realm.entities.player;
using db;
using MySql.Data.MySqlClient;

#endregion

namespace wServer.realm.entities.merchant
{
    public class Merchants : SellableObject
    {
        private const int BUY_NO_FAME = 6;
        private const int MERCHANT_SIZE = 100;
        private static readonly ILog log = LogManager.GetLogger(typeof(Merchants));

        private readonly Dictionary<int, Tuple<int, CurrencyType>> prices = MerchantLists.prices;

        private bool closing;
        private bool newMerchant;
        private int tickcount;
        private int accID;
        int itemChange;

        public static Random Random { get; private set; }

        public Merchants(RealmManager manager, ushort objType, int itemChangeLocal, World owner = null)
            : base(manager, objType)
        {
            MType = -1;
            Size = MERCHANT_SIZE;
            if (owner != null)
                Owner = owner;

            itemChange = itemChangeLocal;

            if (Random == null) Random = new Random();
            if (AddedTypes == null) AddedTypes = new List<KeyValuePair<string, int>>();
            if (owner != null) ResolveMType();
        }

        private static List<KeyValuePair<string, int>> AddedTypes { get; set; }
        
        public int MType { get; set; }
        public int MRemaining { get; set; }
        public int MTime { get; set; }
        public int Discount { get; set; }
        public static int refreshMerchants { get; internal set; }

        protected override void ExportStats(IDictionary<StatsType, object> stats)
        {
            stats[StatsType.MerchantMerchandiseType] = MType;
            stats[StatsType.MerchantRemainingCount] = MRemaining;
            stats[StatsType.MerchantRemainingMinute] = newMerchant ? Int32.MaxValue : MTime;
            stats[StatsType.MerchantDiscount] = Discount;
            stats[StatsType.SellablePrice] = Price;
            stats[StatsType.SellableRankRequirement] = RankReq;
            stats[StatsType.SellablePriceCurrency] = Currency;

            base.ExportStats(stats);
        }

        public override void Init(World owner)
        {
            base.Init(owner);
            ResolveMType();
            UpdateCount++;
            if (MType == -1) Owner.LeaveWorld(this);
        }

        protected override bool TryDeduct(Player player)
        {
            if (player.Client.Account.Stats.Fame < Price) return false;
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
                                // Exploit fix - No more mnovas as weapons!
                                {
                                    player.Inventory[i] = Manager.GameData.Items[(ushort)MType];
                                    
                                    player.CurrentFame =
                                        player.Client.Account.Stats.Fame =
                                            db.UpdateFame(player.Client.Account, -Price);
                                    using (Database db2 = new Database())
                                    {
                                        log.Error("Attemping to delete item from database: " + MType + " | " + Price);
                                        MySqlCommand cmd = db2.CreateQuery();
                                        cmd.CommandText = "DELETE FROM market WHERE itemid=@itemid AND fame=@fame";
                                        cmd.Parameters.AddWithValue("@itemID", MType);
                                        cmd.Parameters.AddWithValue("@fame", Price);
                                        cmd.ExecuteNonQuery();
                                    }
                                    {
                                        log.Error("Attemping to find player to give fame to: " + MType + " | " + Price);
                                        MySqlCommand cmd = db.CreateQuery();
                                        cmd.CommandText = "SELECT * FROM market WHERE itemid='@itemID' AND fame='@fame' LIMIT 1";
                                        cmd.Parameters.AddWithValue("@itemID", MType);
                                        cmd.Parameters.AddWithValue("@fame", Price);
                                        using (MySqlDataReader rdr = cmd.ExecuteReader())
                                        {
                                            if (!rdr.HasRows)
                                                accID = 0;
                                            rdr.Read();
                                            accID = rdr.GetInt32("id");
                                        }
                                    }
                                    {
                                        log.Error("Updating Player Info...");
                                        MySqlCommand cmd1 = db.CreateQuery();
                                        cmd1.CommandText = "UPDATE stats SET fame = fame + @Price WHERE accId=@accId";
                                        cmd1.Parameters.AddWithValue("@accId", accID);
                                        cmd1.Parameters.AddWithValue("@Price", Price);
                                        log.Error("Attempted to give Player " + accID + ", " + Price + " fame");
                                        cmd1.ExecuteNonQuery();
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
                        switch (Currency)
                        {
                            case CurrencyType.Fame:
                                player.Client.SendPacket(new BuyResultPacket
                                {
                                    Result = BUY_NO_FAME,
                                    Message = "{\"key\":\"server.not_enough_fame\"}"
                                });
                                break;
                        }
                    }
                }
            });
        }

        public override void Tick(RealmTime time)
        {
            try
            {
                if (Size == 0 && MType != -1)
                {
                    Size = MERCHANT_SIZE;
                    UpdateCount++;
                }
                if (refreshMerchants > 0)
                {
                    foreach (var t1 in MerchantLists.ZyList)
                    {
                        log.Info("Looking for updates on item | " + t1);
                        if (refreshMerchants == t1)
                        {
                            log.Info("Found Update on Item | " + t1);
                            itemChange = t1;
                            Refresh(this, t1);
                            UpdateCount++;
                        }
                    }
                    refreshMerchants = 0;
                }
                

                if (!closing)
                {
                    tickcount++;
                    if (tickcount % (Manager?.TPS * 60) == 0) //once per minute after spawning
                    {
                        MTime--;
                        UpdateCount++;
                    }
                }

                if (MRemaining == 0 && MType != -1)
                {
                    if (AddedTypes.Contains(new KeyValuePair<string, int>(Owner.Name, MType)))
                        AddedTypes.Remove(new KeyValuePair<string, int>(Owner.Name, MType));
                    Recreate(this);
                    UpdateCount++;
                }

                if (MTime == -1 && Owner != null)
                {
                    if (AddedTypes.Contains(new KeyValuePair<string, int>(Owner.Name, MType)))
                        AddedTypes.Remove(new KeyValuePair<string, int>(Owner.Name, MType));
                    Recreate(this);
                    UpdateCount++;
                }

                if (MTime == 1 && !closing)
                {
                    closing = true;
                    Owner?.Timers.Add(new WorldTimer(30 * 1000, (w1, t1) =>
                    {
                        MTime--;
                        UpdateCount++;
                        w1.Timers.Add(new WorldTimer(30 * 1000, (w2, t2) =>
                        {
                            MTime--;
                            UpdateCount++;
                        }));
                    }));
                }

                if (MType == -1) Owner?.LeaveWorld(this);

                base.Tick(time);
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }

        public void Recreate(Merchants x)
        {
            try
            {
                var mrc = new Merchants(Manager, x.ObjectType, 0, x.Owner);
                mrc.Move(x.X, x.Y);
                var w = Owner;
                Owner.LeaveWorld(this);
                w.Timers.Add(new WorldTimer(Random.Next(100, 500), (world, time) => w.EnterWorld(mrc)));
            }
            catch (Exception e)
            {
                log.Error(e);
            }
        }

        public void Refresh(Merchants x, int item)
        {
            try
            {
                Tuple<int, CurrencyType> price;
                if (prices.TryGetValue(MType, out price))
                {
                    var mrc = new Merchants(Manager, x.ObjectType, item, x.Owner);
                    log.Info("Attempting to refresh Merchant | " + item);
                    mrc.Move(x.X, x.Y);
                    var w = Owner;
                    Owner.LeaveWorld(this);
                    w.Timers.Add(new WorldTimer(Random.Next(1000, 2000), (world, time) => w.EnterWorld(mrc)));
                }
            }
            catch (Exception e)
            {
                log.Error(e);
            }
        }

        public void ResolveMType()
        {
            MType = -1;
            var list = MerchantLists.ZyList;

            if (AddedTypes == null) AddedTypes = new List<KeyValuePair<string, int>>();
            list.Shuffle();
            foreach (var t1 in list)
            {
                AddedTypes.Add(new KeyValuePair<string, int>(Owner.Name, t1));
                if (itemChange > 0)
                {
                    MType = itemChange;
                    log.Info("Refreshing Merchant | " + MType);
                }
                if (itemChange <= 0)
                {
                    MType = t1;
                    log.Info("Randomizing Merchant to be item | " + MType);
                }

                MTime = Random.Next(2, 5);
                MRemaining = 1;
                newMerchant = false;

                Discount = 0;

                Tuple<int, CurrencyType> price;
                if (prices.TryGetValue(MType, out price))
                {
                    using (Database db = new Database())
                        Price = db.GetMarketInfo(price.Item1, 1);
                    Currency = price.Item2;
                }

                break;
            }
            UpdateCount++;
        }
    }
}