#region

using System;
using System.Collections.Generic;
using System.Linq;
using db;
using db.data;
using log4net;
using CheckConfig = wServer.logic.CheckConfig;

#endregion

namespace wServer.realm.entities.merchant
{
    internal class MerchantLists
    {
        public static int[] ZyList;
        public static List<int> zyList = new List<int>();

        public static Dictionary<int, int> price = new Dictionary<int, int>();

        public static XmlData PublicData;

        public static int[] Store10List = {0xb41, 0xbab, 0xbad, 0xbac};
        public static int[] Store11List = {0xb41, 0xbab, 0xbad, 0xbac};
        public static int[] Store12List = {0xb41, 0xbab, 0xbad, 0xbac};
        public static int[] Store13List = {0xb41, 0xbab, 0xbad, 0xbac};
        public static int[] Store14List = {0xb41, 0xbab, 0xbad, 0xbac};
        public static int[] Store15List = {0xb41, 0xbab, 0xbad, 0xbac};
        public static int[] Store16List = {0xb41, 0xbab, 0xbad, 0xbac};
        public static int[] Store17List = {0xb41, 0xbab, 0xbad, 0xbac};
        public static int[] Store18List = {0xb41, 0xbab, 0xbad, 0xbac};
        public static int[] Store19List = {0xb41, 0xbab, 0xbad, 0xbac};

        public static int[] Store1List =
        {
            0xcdd, 0xcda, 0xccf, 0xcce, 0xc2f, 0xc2e, 0xc23, 0xc19, 0xc11, 0x71f, 0x710,
            0x70b, 0x70a, 0x705, 0x701, 0x2290, 0xcd4, 0x6021, 0xccd, 0x2294
        };

        public static int[] Store20List = {0xb41, 0xbab, 0xbad, 0xbac};

        //keys need to add etcetc
        public static int[] Store2List =
        {
            0xcbf, 0xcbe, 0xcbb, 0xcba, 0xcb7, 0xcb6, 0xcb2, 0xcb3, 0xcae, 0xcaf, 0xcab,
            0xcaa, 0xca7, 0xca6, 0xca3, 0xca2, 0xc9f, 0xc9e, 0xc9b, 0xc9a, 0xc97, 0xc96, 0xc93, 0xc92, 0xc8f, 0xc8e,
            0xc8b, 0xc8a, 0xc87, 0xc86
        };

        //pet eggs
        public static int[] Store3List = {0xccc, 0xccb, 0xcca, 0xcc9, 0xcc8, 0xcc7, 0xcc6, 0xcc5, 0xcc4};

        //pet food
        public static int[] Store4List =
        {
            0xb25, 0xa5b, 0xb22, 0xa0c, 0xb24, 0xa30, 0xb26, 0xa55, 0xb27, 0xae1, 0xb28,
            0xa65, 0xb29, 0xa6b, 0xb2a, 0xaa8, 0xb2b, 0xaaf, 0xb2c, 0xab6, 0xb2d, 0xa46, 0xb23, 0xb20, 0xb33, 0xb32,
            0xc59, 0xc58
        };

        //abilities
        public static int[] Store5List =
        {
            0xb05, 0xa96, 0xa95, 0xa94, 0xafc, 0xa93, 0xa92, 0xa91, 0xaf9,
            0xa90, 0xa8f, 0xa8e
        };

        //armors
        public static int[] Store6List =
        {
            0xaf6, 0xa87, 0xa86, 0xa85, 0xb02, 0xa8d, 0xa8c, 0xa8b, 0xb08,
            0xaa2, 0xaa1, 0xaa0
        };

        //Wands&staves&bows
        public static int[] Store7List =
        {
            0xb0b, 0xa47, 0xa84, 0xa83, 0xaff, 0xa8a, 0xa89, 0xa88, 0xc50,
            0xc4f, 0xc4e, 0xc4d, 0x611c, 0x6123, 0x6129
        };

        //Swords&daggers&samurai
        public static int[] Store8List =
        {
            0xabf, 0xac0, 0xac1, 0xac2, 0xac3, 0xac4, 0xac5, 0xac6, 0xac7, 0xac8, 0xac9,
            0xaca, 0xacb, 0xacc, 0xacd, 0xace
        };

        // rings
        public static int[] Store9List = {0xb41, 0xbab, 0xbad, 0xbac};

        private static readonly ILog Log = LogManager.GetLogger(typeof(MerchantLists));

        private static readonly string[] NoShopItems =
        {
            "Crown", "Muscat", "Cabernet", "Vial of Pure Darkness", "Omnipotence Ring", "Draconis Potion",
            "Sauvignon Blanc", "Snake Oil", "Pollen Powder",
            "XP Booster Test", "Gravel"
        };

        public static void InitMerchatLists(XmlData data)
        {
            if (CheckConfig.IsDebugOn())
                Log.Info("Loading merchant lists...");
            zyList = new List<int>();
            PublicData = data;
            foreach (var item in data.Items.Where(_ => NoShopItems.All(i => i != _.Value.ObjectId)))
            {
                if (Merchant.checkItem(item.Value))
                {
                    using (var db = new Database())
                    {
                        var _price = db.GetMarketPrice(item.Value);
                        if (_price > 0)
                        {
                            if (CheckConfig.IsDebugOn())
                                Log.Info("Loading " + item.Value.ObjectId);
                            zyList.Add(item.Value.ObjectType);
                            price.Add(item.Value.ObjectType, _price);
                        }
                    }
                }
            }
            ZyList = zyList.ToArray();
            if (CheckConfig.IsDebugOn())
                Log.Info("Merchat lists added.");
           
        }

        public static void RemoveItem(int Item)
        {
            if (CheckConfig.IsDebugOn())
                Log.Info("Removing " + Item + " from List");

            zyList.Remove(Item);
            ZyList = zyList.ToArray();
        }

        public static void AddItem(Item Item, int Price)
        {
            var _oldPrice = 0;
            if (CheckConfig.IsDebugOn())
                Log.Info("Adding Item " + Item.ObjectId + " to List");

            if (price.ContainsKey(Item.ObjectType))
            {
                _oldPrice = price[Item.ObjectType];
            }

            var _newPrice = Price;
            
            if (_oldPrice > _newPrice || _oldPrice == 0) {
                Merchant.updatePrice(Item.ObjectType, _newPrice);
            }

            if (!zyList.Contains(Item.ObjectType))
            {
                zyList.Add(Item.ObjectType);
                ZyList = zyList.ToArray();
            }
        }
    }
}