using db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wServer.realm.entities.player;

namespace wServer.realm.entities.merchant
{
    class Merchant
    {
        private static readonly string[] NoShopContains =
        {
            "Egg", "Skin", "Cloth", "Dye", "Tincture", "Effusion", "Elixer", "Tarot", "Gumball", "Pail"
        };

        public static bool checkItem(Item item)
        {
            for (var i = 0; i < NoShopContains.Length; i++)
            {
                if (item.ObjectId.Contains(NoShopContains[i]))
                {
                    return false;
                }
                
            }
            if ((item.Tier == -1 || item.Tier >= 4) && item.Treasure == false) {
                return true;
            }
            return false;
        }

        public static void updatePrice(int ItemType)
        {
            MerchantLists.price.Remove(ItemType);
            using (var db = new Database())
            {
                var _price = db.GetMarketPrice(ItemType);
                if (_price == 0) {
                    MerchantLists.RemoveItem(ItemType);
                }
                MerchantLists.price.Add(ItemType, _price);
            }
        }
        public static void updatePrice(int ItemType, int Price)
        {
            MerchantLists.price.Remove(ItemType);
            MerchantLists.price.Add(ItemType, Price);
        }
    }
}
