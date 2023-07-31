using R2API;
using RoR2;
using System.Collections.Generic;
using UnityEngine;

namespace FirstLightMod.Modules
{
    public static class Buffs
    {
        // armor buff gained during roll
        internal static BuffDef armorBuff;

        // Farmer's Passive
        public static BuffDef farmerPassive;

        static string farmerPrefix = FirstLightPlugin.DEVELOPER_PREFIX + "_HENRY_BODY_";

        internal static void RegisterBuffs()
        {
            armorBuff = AddNewBuff("HenryArmorBuff",
                LegacyResourcesAPI.Load<BuffDef>("BuffDefs/HiddenInvincibility").iconSprite, 
                Color.white, 
                false, 
                false);

            farmerPassive = AddNewBuff(farmerPrefix + "PASSIVE_NAME",
                Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("texPrimaryIcon"),
                new Color(255f / 255f, 0f / 255f, 84f / 255f),
                false,
                false);

        }

        // simple helper method
        internal static BuffDef AddNewBuff(string buffName, Sprite buffIcon, Color buffColor, bool canStack, bool isDebuff)
        {
            BuffDef buffDef = ScriptableObject.CreateInstance<BuffDef>();
            buffDef.name = buffName;
            buffDef.buffColor = buffColor;
            buffDef.canStack = canStack;
            buffDef.isDebuff = isDebuff;
            buffDef.eliteDef = null;
            buffDef.iconSprite = buffIcon;

            Modules.Content.AddBuffDef(buffDef);

            return buffDef;
        }



    }
}