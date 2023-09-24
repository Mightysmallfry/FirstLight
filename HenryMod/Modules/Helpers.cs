using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using RoR2;
using UnityEngine;
using FirstLightMod.Modules.Equipment;
using FirstLightMod.Modules.Equipment.EliteEquipment;
using FirstLightMod.Modules.Items;
using System;

namespace FirstLightMod.Modules
{
    /// <summary>
    /// A helper class for items, equipment and everything. This should be split up into more classes in the future for the sake of organization.
    /// </summary>
    public class Helpers
    {
        public static List<ItemBase> Items = new List<ItemBase>();
        public static List<EquipmentBase> Equipments = new List<EquipmentBase>();
        public static List<EliteEquipmentBase> EliteEquipments = new List<EliteEquipmentBase>();
        private BepInEx.Configuration.ConfigFile config;

        public Helpers(BepInEx.Configuration.ConfigFile config)
        {
            this.config = config;
        }

        internal const string agilePrefix = "<style=cIsUtility>Agile.</style> ";

        internal static string ScepterDescription(string desc)
        {
            return "\n<color=#d299ff>SCEPTER: " + desc + "</color>";
        }

        public static T[] Append<T>(ref T[] array, List<T> list)
        {
            var orig = array.Length;
            var added = list.Count;
            Array.Resize<T>(ref array, orig + added);
            list.CopyTo(array, orig);
            return array;
        }

        public static Func<T[], T[]> AppendDel<T>(List<T> list) => (r) => Append(ref r, list);

        public void LoadAllItems()
        {

            //This section automatically scans the project for all items
            var ItemTypes = Assembly.GetExecutingAssembly().GetTypes().Where(type => !type.IsAbstract && type.IsSubclassOf(typeof(ItemBase)));

            foreach (var itemType in ItemTypes)
            {
                ItemBase item = (ItemBase)System.Activator.CreateInstance(itemType);
                if (ValidateItem(item, Items))
                {
                    item.Init(config);
                }
            }

            //this section automatically scans the project for all equipment
            var EquipmentTypes = Assembly.GetExecutingAssembly().GetTypes().Where(type => !type.IsAbstract && type.IsSubclassOf(typeof(EquipmentBase)));

            foreach (var equipmentType in EquipmentTypes)
            {
                EquipmentBase equipment = (EquipmentBase)System.Activator.CreateInstance(equipmentType);
                if (ValidateEquipment(equipment, Equipments))
                {
                    equipment.Init(config);
                }
            }

            //this section automatically scans the project for all elite equipment
            var EliteEquipmentTypes = Assembly.GetExecutingAssembly().GetTypes().Where(type => !type.IsAbstract && type.IsSubclassOf(typeof(EliteEquipmentBase)));

            foreach (var eliteEquipmentType in EliteEquipmentTypes)
            {
                EliteEquipmentBase eliteEquipment = (EliteEquipmentBase)System.Activator.CreateInstance(eliteEquipmentType);
                if (ValidateEliteEquipment(eliteEquipment, EliteEquipments))
                {
                    eliteEquipment.Init(config);

                }
            }
        }



        /// <summary>
        /// A helper to easily set up and initialize an item from your item classes if the user has it enabled in their configuration files.
        /// </summary>
        /// <param name="item">A new instance of an ItemBase class."</param>
        /// <param name="itemList">The list you would like to add this to if it passes the config check.</param>
        public bool ValidateItem(ItemBase item, List<ItemBase> itemList)
        {
            var enabled = config.Bind("Item: " + item.ItemName, "Enable", true, "Should this item appear in runs?").Value;
            if (enabled)
            {
                itemList.Add(item);
            }
            return enabled;
        }

        /// <summary>
        /// A helper to easily set up and initialize an equipment from your equipment classes if the user has it enabled in their configuration files.
        /// </summary>
        /// <param name="equipment">A new instance of an EquipmentBase class."</param>
        /// <param name="equipmentList">The list you would like to add this to if it passes the config check.</param>
        public bool ValidateEquipment(EquipmentBase equipment, List<EquipmentBase> equipmentList)
        {
            if (config.Bind("Equipment: " + equipment.EquipmentName, "Enable Equipment?", true, "Should this equipment appear in runs?").Value)
            {
                equipmentList.Add(equipment);
                return true;
            }
            return false;
        }

        /// <summary>
        /// A helper to easily set up and initialize an elite equipment from your elite equipment classes if the user has it enabled in their configuration files.
        /// </summary>
        /// <param name="eliteEquipment">A new instance of an EliteEquipmentBase class.</param>
        /// <param name="eliteEquipmentList">The list you would like to add this to if it passes the config check.</param>
        /// <returns></returns>
        public bool ValidateEliteEquipment(EliteEquipmentBase eliteEquipment, List<EliteEquipmentBase> eliteEquipmentList)
        {
            var enabled = config.Bind("Equipment: " + eliteEquipment.EliteEquipmentName, "Enable Elite Equipment?", true, "Should this elite equipment appear in runs? If disabled, the associated elite will not appear in runs either.").Value;

            if (enabled)
            {
                eliteEquipmentList.Add(eliteEquipment);
                return true;
            }
            return false;
        }

        public static CharacterModel.RendererInfo[] ItemDisplaySetup(GameObject obj)
        {
            MeshRenderer[] meshes = obj.GetComponentsInChildren<MeshRenderer>();
            CharacterModel.RendererInfo[] renderInfos = new CharacterModel.RendererInfo[meshes.Length];

            for (int i = 0; i < meshes.Length; i++)
            {
                renderInfos[i] = new CharacterModel.RendererInfo
                {
                    defaultMaterial = meshes[i].material,
                    renderer = meshes[i],
                    defaultShadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On,
                    ignoreOverlays = false //We allow the mesh to be affected by overlays like OnFire or PredatoryInstinctsCritOverlay.
                };
            }

            return renderInfos;
        }
    }
}
