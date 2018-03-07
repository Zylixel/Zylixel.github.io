#region

using System.Collections.Generic;
using wServer.networking.svrPackets;
using wServer.realm.entities.player;
using wServer.realm.worlds;

#endregion

namespace wServer.realm.entities
{
    public class SellableObject : StaticObject
    {

        public SellableObject(RealmManager manager, ushort objType)
            : base(manager, objType, null, true, false, false)
        {
            if (objType == 0x0505) //Vault chest
            {
                Price = 50;
                Currency = CurrencyType.Fame;
                RankReq = 0;
            }
            else if (objType == 0x0736)
            {
                Currency = CurrencyType.GuildFame;
                Price = 10000;
                RankReq = 0;
            }
        }

        public int Price { get; set; }
        public CurrencyType Currency { get; set; }
        public int RankReq { get; set; }

        protected override void ExportStats(IDictionary<StatsType, object> stats)
        {
            stats[StatsType.SellablePrice] = Price;
            stats[StatsType.SellablePriceCurrency] = (int)Currency;
            stats[StatsType.SellableRankRequirement] = RankReq;
            base.ExportStats(stats);
        }

        protected virtual bool TryDeduct(Player player)
        {
            Account acc = player.Client.Account;
            if (!player.NameChosen) return false;
            if (player.Stars < RankReq) return false;

            if (Currency == CurrencyType.Fame)
                if (acc.Stats.Fame < Price) return false;

            if (Currency == CurrencyType.Gold)
                if (acc.Credits < Price) return false;

            return true;
        }

        public virtual void Buy(Player player)
        {
            Manager.Database.DoActionAsync(db =>
            {
                if (ObjectType == 0x0505) //Vault chest
                {
                    if (player._buyCooldown > 0)
                    {
                        player.SendDialogError("You are buying vaults too fast, please slow down and re-enter the world if needed");
                        return;
                    }
                    if (TryDeduct(player))
                    {
                        player._buyCooldown = 15;
                        VaultChest chest = db.CreateChest(player.Client.Account);
                        db.UpdateFame(player.Client.Account, -Price);
                        (Owner as Vault).AddChest(chest, this);
                        player.Client.SendPacket(new BuyResultPacket
                        {
                            Result = 0,
                            Message = "{\"key\":\"server.buy_success\"}"
                        });
                    }
                    else
                    {
                        player.Client.SendPacket(new BuyResultPacket
                        {
                            Result = 6,
                            Message = "{\"key\":\"server.not_enough_fame\"}"
                        });
                    }
                }
                if (ObjectType == 0x0736)
                {
                    player.Client.SendPacket(new BuyResultPacket
                    {
                        Result = 9,
                        Message = "{\"key\":\"server.not_enough_fame\"}"
                    });
                }
            });
        }
    }
}