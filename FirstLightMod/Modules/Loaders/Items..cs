using FirstLightMod.Modules.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using BepInEx.Configuration;


namespace FirstLightMod.Modules.Loaders
{
    internal static class Items
    {
        public static int numberOfLoadedItems = 0;
        public static List<ItemBase> itemBases = new List<ItemBase>();
        public static ConfigFile config = Config.MyConfig;

        //public static List<EquipmentBase> equipmentBases = new List<EquipmentBase>();
        //public static List<EliteEquipmentBase> eliteEquipmentBases = new List<EliteEquipmentBase>();
        //public static List<ArtifactBase> artifactBases = new List<ArtifactBase>();


        public static void Init()
        {
            Log.Info("-------------------Items----------------------");

            //This section automatically scans the project for all items
            var ItemTypes = Assembly.GetExecutingAssembly().GetTypes().Where(type => !type.IsAbstract && type.IsSubclassOf(typeof(ItemBase)));

            
            foreach (var itemType in ItemTypes)
            {
                ItemBase item = (ItemBase)System.Activator.CreateInstance(itemType);
                if (ValidateItem(item, itemBases))
                {
                    Log.Info("Started Loading Item : " + item.ItemNameToken);
                    item.Init(config);
                    numberOfLoadedItems++;
                    Log.Info("Finished Item : " + item.ItemNameToken);
                }
            }
        }

        /// <summary>
        /// A helper to easily set up and initialize an item from your item classes if the user has it enabled in their configuration files.
        /// </summary>
        /// <param name="item">A new instance of an ItemBase class."</param>
        /// <param name="itemList">The list you would like to add this to if it passes the config check.</param>
        public static bool ValidateItem(ItemBase item, List<ItemBase> itemList)
        {
            var enabled = config.Bind("Item: " + item.ItemName, "Enable", true, "Should this item appear in runs?").Value;
            if (enabled)
            {
                itemList.Add(item);
            }
            return enabled;
        }

    }
}
