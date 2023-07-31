using RoR2;
using R2API;
using BepInEx.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace FirstLightMod.Modules.Items
{
    public abstract class EquipmentBase
    {
        string prefix = FirstLightPlugin.DEVELOPER_PREFIX + "_EQUIPMENT_";
        public abstract string EquipmentName { get; }
        public abstract string EquipmentNameToken { get; }
        public abstract string EquipmentPickupDesc { get; }
        public abstract string EquipmentFullDescription { get; }
        public abstract string EquipmentLore { get; }
        public abstract GameObject EquipmentModel { get; }
        public abstract Sprite EquipmentIcon { get; }

        public EquipmentDef EquipmentDef;

        public virtual bool AppearsInSinglePlayer { get; } = true;
        public virtual bool AppearsInMultiPlayer { get; } = true;
        public virtual bool CanDrop { get; } = true;
        public virtual float Cooldown { get; } = 60f;
        public virtual bool EnigmaCompatible { get; } = true;
        public virtual bool IsBoss { get; } = false;
        public virtual bool IsLunar { get; } = false;

        public abstract void Init(ConfigFile config);

        protected void CreateLang() 
        {

            LanguageAPI.Add(prefix + EquipmentNameToken + "_NAME", EquipmentName);
            LanguageAPI.Add(prefix + EquipmentNameToken + "_PICKUP", EquipmentPickupDesc);
            LanguageAPI.Add(prefix + EquipmentNameToken + "_DESCRIPTION", EquipmentFullDescription);
            LanguageAPI.Add(prefix + EquipmentNameToken + "_LORE", EquipmentLore);
        }

        public abstract ItemDisplayRuleDict CreateItemDisplayRules();

        protected void CreateEquipment()
        {
            EquipmentDef = ScriptableObject.CreateInstance<EquipmentDef>();
            EquipmentDef.name = prefix + EquipmentNameToken;
            EquipmentDef.nameToken = prefix + EquipmentNameToken + "_NAME";
            EquipmentDef.pickupToken = prefix + EquipmentNameToken + "_PICKUP";
            EquipmentDef.descriptionToken = prefix + EquipmentNameToken + "_DESCRIPTION";
            EquipmentDef.loreToken = prefix + EquipmentNameToken + "_LORE";
            EquipmentDef.pickupModelPrefab = EquipmentModel;
            EquipmentDef.pickupIconSprite = EquipmentIcon;
            EquipmentDef.appearsInSinglePlayer = AppearsInSinglePlayer;
            EquipmentDef.appearsInMultiPlayer = AppearsInMultiPlayer;
            EquipmentDef.canDrop = CanDrop;
            EquipmentDef.cooldown = Cooldown;
            EquipmentDef.enigmaCompatible = EnigmaCompatible;
            EquipmentDef.isBoss = IsBoss;
            EquipmentDef.isLunar = IsLunar;

            ItemAPI.Add(new CustomEquipment(EquipmentDef, CreateItemDisplayRules()));
            On.RoR2.EquipmentSlot.PerformEquipmentAction += EquipmentSlot_PerformEquipmentAction;
        }

        private bool EquipmentSlot_PerformEquipmentAction(On.RoR2.EquipmentSlot.orig_PerformEquipmentAction orig, EquipmentSlot self, EquipmentDef equipmentDef)
        {
            if (equipmentDef == EquipmentDef)
            {
                return ActivateEquipment(self);
            }
            else
            {
               return orig(self, equipmentDef);
            }
        }

        protected abstract bool ActivateEquipment(EquipmentSlot equipmentSlot);


        public abstract void Hooks();


    }
}
