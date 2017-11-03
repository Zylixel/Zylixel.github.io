#region

using System;
using System.Collections.Generic;
using System.Xml.Linq;
using db;
using log4net;
using MySql.Data.MySqlClient;
using wServer.networking.svrPackets;
using wServer.realm.entities.player;

#endregion

namespace wServer.realm.entities.merchant
{
    public class Merchants : SellableObject
    {
        private const int BuyNoFame = 6;
        private const int MerchantSize = 100;
        private static readonly ILog LogIt = LogManager.GetLogger(typeof(Merchants));

        private readonly Dictionary<int, Tuple<int, CurrencyType>> _prices = MerchantLists.Prices;

        private bool _closing;
        private bool _newMerchant;
        private int _tickcount;
        private int _accId;
        private int _itemChange;

        public static Random Random { get; private set; }
          
        public Merchants(RealmManager manager, ushort objType, int itemChangeLocal, World owner = null)
            : base(manager, objType)
        {
            MType = -1;
            Size = MerchantSize;
            if (owner != null)
                Owner = owner;

            _itemChange = itemChangeLocal;

            if (Random == null) Random = new Random();
            if (AddedTypes == null) AddedTypes = new List<KeyValuePair<string, int>>();
            if (owner != null) ResolveMType();
        }

        private static List<KeyValuePair<string, int>> AddedTypes { get; set; }
        
        public int MType { get; set; }
        public int MRemaining { get; set; }
        public int MTime { get; set; }
        public int Discount { get; set; }
        public static int RefreshMerchants { get; internal set; }
        public static int RefreshMerchantsCooldown { get; internal set; }

        protected override void ExportStats(IDictionary<StatsType, object> stats)
        {
            stats[StatsType.MerchantMerchandiseType] = MType;
            stats[StatsType.MerchantRemainingCount] = MRemaining;
            stats[StatsType.MerchantRemainingMinute] = _newMerchant ? int.MaxValue : MTime;
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
            return player.Client.Account.Stats.Fame >= Price;
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
                                        _accId = db.GetMarketCharId(MType, Price);
                                    }
                                    {
                                        if (logic.CheckConfig.IsDebugOn())
                                            LogIt.Error("Attemping to delete item from database: " + MType + " | " + Price);
                                        MySqlCommand cmd = db.CreateQuery();
                                        cmd.CommandText = "DELETE FROM market WHERE itemid=@itemid AND fame=@fame AND id=@id";
                                        cmd.Parameters.AddWithValue("@itemid", MType);
                                        cmd.Parameters.AddWithValue("@fame", Price);
                                        cmd.Parameters.AddWithValue("@id", _accId);
                                        cmd.ExecuteNonQuery();
                                    }
                                    {
                                        if (logic.CheckConfig.IsDebugOn())
                                            LogIt.Error("Attempted to give Player " + _accId + ", " + Price + " fame");
                                        MySqlCommand cmd = db.CreateQuery();
                                        cmd.CommandText = "UPDATE stats SET fame = fame + @Price WHERE accId=@accId";
                                        cmd.Parameters.AddWithValue("@accId", _accId);
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
                                LogIt.Error(e);
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
                                    Result = BuyNoFame,
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
                    Size = MerchantSize;
                    UpdateCount++;
                }

                if (RefreshMerchantsCooldown <= 0)
                    RefreshMerchants = 0;

                if (RefreshMerchants > 0)
                {
                    MerchantLists.RefreshMerchatLists();
                    if (logic.CheckConfig.IsDebugOn())
                        LogIt.Info("Looking for updates on item | " + MType);
                    if (RefreshMerchants == MType)
                    {
                        if (logic.CheckConfig.IsDebugOn())
                            LogIt.Info("Found Update on Item | " + RefreshMerchants);
                        _itemChange = RefreshMerchants;
                        Refresh(this, RefreshMerchants);
                        RefreshMerchants = 0;
                        UpdateCount++;
                    }
                    else
                        RefreshMerchantsCooldown--;
                }

                if (!_closing)
                {
                    _tickcount++;
                    if (_tickcount % (Manager?.TPS * 60) == 0) //once per minute after spawning
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

                if (MTime == 1 && !_closing)
                {
                    _closing = true;
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
                LogIt.Error(ex);
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
                w.Timers.Add(new WorldTimer(Random.Next(1, 2), (world, time) => w.EnterWorld(mrc)));
            }
            catch (Exception e)
            {
                LogIt.Error(e);
            }
        }

        public void Refresh(Merchants x, int item)
        {
            try
            {
                Tuple<int, CurrencyType> price;
                if (_prices.TryGetValue(MType, out price))
                {
                    var mrc = new Merchants(Manager, x.ObjectType, item, x.Owner);
                    if (logic.CheckConfig.IsDebugOn())
                        LogIt.Info("Attempting to refresh Merchant | " + item);
                    mrc.Move(x.X, x.Y);
                    var w = Owner;
                    w.LeaveWorld(this);
                    w.Timers.Add(new WorldTimer(Random.Next(1, 2), (world, time) => w.EnterWorld(mrc)));
                }
            }
            catch (Exception e)
            {
                LogIt.Error(e);
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
                if (_itemChange > 0)
                {
                    MType = _itemChange;
                    if (logic.CheckConfig.IsDebugOn())
                        LogIt.Info("Refreshing Merchant | " + MType);
                }
                else
                {
                    if (!AddedTypes.Contains(new KeyValuePair<string, int>(Owner.Name, t1)))
                    {
                        AddedTypes.Add(new KeyValuePair<string, int>(Owner.Name, t1));
                        MType = t1;
                        if (logic.CheckConfig.IsDebugOn())
                            LogIt.Info("Randomizing Merchant to be item | " + MType);
                    }
                }
                

                MTime = Random.Next(2, 5);
                    MRemaining = 1;
                    _newMerchant = false;

                    Discount = 0;

                    Tuple<int, CurrencyType> price;
                    if (_prices.TryGetValue(MType, out price))
                    {
                        using (Database db = new Database())
                            Price = db.GetMarketInfo(price.Item1, 1);
                        Currency = price.Item2;
                    }

                UpdateCount++;
                    break;
            }
        }
    }
}