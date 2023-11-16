using R2API;
using RoR2;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace FirstLightMod.Modules
{
    public static class Buffs
    {

        //----------------------------------------------- PASSIVES
        public static BuffDef farmerPassive;

        //----------------------------------------------- BUFFS
        public static BuffDef MicrobotMatrixBuff;

        // armor buff gained during roll
        internal static BuffDef armorBuff;
        //----------------------------------------------- DEBUFFS
        public static BuffDef WendigoVestigeDebuff;


        //----------------------------------------------- CHARACTER PREFIXES
        static string farmerPrefix = FirstLightPlugin.DEVELOPER_PREFIX + "_HENRY_BODY_";
        static string beekeeperPrefix = FirstLightPlugin.DEVELOPER_PREFIX + "_HENRY_BODY_";

        internal static void RegisterBuffs()
        {
            //Naming Convention, characterPassive, ItemBuff, EquipmentBuff

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

            MicrobotMatrixBuff = AddNewBuff(FirstLightPlugin.DEVELOPER_PREFIX + "_MicrobotMatrixBuff",
                LegacyResourcesAPI.Load<BuffDef>("BuffDefs/HiddenInvincibility").iconSprite,
                Color.red,
                true,
                false);

            WendigoVestigeDebuff = AddNewBuff(FirstLightPlugin.DEVELOPER_PREFIX + "_WendigoVestigeDebuff",
                Addressables.LoadAssetAsync<Sprite>("RoR2/Base/Common/MiscIcons/texWIPIcon.png").WaitForCompletion(),
                Color.gray,
                false,
                true);


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