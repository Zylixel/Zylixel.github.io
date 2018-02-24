#region

using System;
using System.Collections.Generic;
using System.Linq;
using db;
using db.data;
using CheckConfig = wServer.logic.CheckConfig;

#endregion

namespace wServer.realm.entities.merchant
{
    internal class MerchantLists
    {
        public static int[] AccessoryClothList;
        public static int[] AccessoryDyeList;
        public static int[] ClothingClothList;
        public static int[] ClothingDyeList;
        public static int[] ZyList;
        public static List<int> accessoryDyeList = new List<int>();
        public static List<int> clothingDyeList = new List<int>();
        public static List<int> accessoryClothList = new List<int>();
        public static List<int> clothingClothList = new List<int>();
        public static List<int> zyList = new List<int>();

        public static Dictionary<int, int> price = new Dictionary<int, int>();

        public static XmlData PublicData;

        private static readonly string[] NoShopItems =
        {
            "Crown", "Muscat", "Cabernet", "Vial of Pure Darkness", "Omnipotence Ring", "Draconis Potion",
            "Sauvignon Blanc", "Snake Oil", "Pollen Powder",
            "XP Booster Test", "Gravel"
        };
        private static readonly string[] noShopCloths =
        {
            "Large Ivory Dragon Scale Cloth", "Small Ivory Dragon Scale Cloth",
            "Large Green Dragon Scale Cloth", "Small Green Dragon Scale Cloth",
            "Large Midnight Dragon Scale Cloth", "Small Midnight Dragon Scale Cloth",
            "Large Blue Dragon Scale Cloth", "Small Blue Dragon Scale Cloth",
            "Large Red Dragon Scale Cloth", "Small Red Dragon Scale Cloth",
            "Large Jester Argyle Cloth", "Small Jester Argyle Cloth",
            "Large Alchemist Cloth", "Small Alchemist Cloth",
            "Large Mosaic Cloth", "Small Mosaic Cloth",
            "Large Spooky Cloth", "Small Spooky Cloth",
            "Large Flame Cloth", "Small Flame Cloth",
            "Large Heavy Chainmail Cloth", "Small Heavy Chainmail Cloth",
        };

        public static void InitMerchatLists(XmlData data)
        {
            if (CheckConfig.IsDebugOn())
                Console.WriteLine("Loading merchant lists...");
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
                                Console.WriteLine("Loading " + item.Value.ObjectId);
                            zyList.Add(item.Value.ObjectType);
                            price.Add(item.Value.ObjectType, _price);
                        }
                    }
                }
            }
            foreach (var item in data.Items)
            {
                if (item.Value.Texture1 != 0 && item.Value.ObjectId.Contains("Clothing") && item.Value.Class == "Dye")
                {
                    clothingDyeList.Add(item.Value.ObjectType);
                }
                if (item.Value.Texture2 != 0 && item.Value.ObjectId.Contains("Accessory") && item.Value.Class == "Dye")
                {
                    accessoryDyeList.Add(item.Value.ObjectType);
                }
                if (item.Value.Texture1 != 0 && item.Value.ObjectId.Contains("Cloth") &&
                    item.Value.ObjectId.Contains("Large"))
                {
                    clothingClothList.Add(item.Value.ObjectType);
                }
                if (item.Value.Texture2 != 0 && item.Value.ObjectId.Contains("Cloth") &&
                    item.Value.ObjectId.Contains("Small"))
                {
                    accessoryClothList.Add(item.Value.ObjectType);
                }
            }

            ClothingDyeList = clothingDyeList.ToArray();
            ClothingClothList = clothingClothList.ToArray();
            AccessoryClothList = accessoryClothList.ToArray();
            AccessoryDyeList = accessoryDyeList.ToArray();
            ZyList = zyList.ToArray();
            if (CheckConfig.IsDebugOn())
                Console.WriteLine("Merchat lists added.");
           
        }

        public static void RemoveItem(int Item)
        {
            if (CheckConfig.IsDebugOn())
                Console.WriteLine("Removing " + Item + " from List");

            zyList.Remove(Item);
            ZyList = zyList.ToArray();
        }

        public static void AddItem(Item Item, int Price)
        {
            var _oldPrice = 0;
            if (CheckConfig.IsDebugOn())
                Console.WriteLine("Adding Item " + Item.ObjectId + " to List");

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