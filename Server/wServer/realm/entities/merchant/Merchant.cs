using db;

namespace wServer.realm.entities.merchant
{
    class Merchant
    {
        private static readonly string[] NoShopContains =
        {
            "Egg", "Skin", "Cloth", "Dye", "Tincture", "Effusion", "Elixer", "Tarot", "Gumball", "Pail"
        };

        public static bool checkItem(OldItem item)
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

        public static void updatePrice(int ItemType, RealmManager Manager)
        {
            MerchantLists.price.Remove(ItemType);
            using (var db = new Database())
            {
                var _price = db.GetMarketPrice(Manager.GameData.Items[(ushort) ItemType]);
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
