using RoR2;
using UnityEngine;

namespace FirstLightMod.Survivors.Cavalier
{
    public static class CavalierBuffs
    {
        //For allies
        public static BuffDef riposteBuff; //Increase attack Speed 
        public static BuffDef stylishBuff; //Increase movement Speed -> Flourish
        public static BuffDef rallyBuff; // -> Flare Gun

        //For enemies -> Debuffs
        public static BuffDef vulnerableBuff; //Reduce armor -> Passive Focus Fire
        public static BuffDef offGuardBuff; //Reduce movement and attack speed -> Flare Gun


        public static void Init(AssetBundle assetBundle)
        {

            riposteBuff = Modules.Content.CreateAndAddBuff("Riposte",
                LegacyResourcesAPI.Load<BuffDef>("BuffDefs/HiddenInvincibility").iconSprite,
                Color.red,
                true,
                false);

            stylishBuff = Modules.Content.CreateAndAddBuff("Riposte",
                LegacyResourcesAPI.Load<BuffDef>("BuffDefs/HiddenInvincibility").iconSprite,
                Color.yellow,
                true,
                false);

            rallyBuff = Modules.Content.CreateAndAddBuff("Rallied",
                LegacyResourcesAPI.Load<BuffDef>("BuffDefs/HiddenInvincibility").iconSprite,
                Color.white,
                true,
                false);

            vulnerableBuff = Modules.Content.CreateAndAddBuff("Vulnerable",
                LegacyResourcesAPI.Load<BuffDef>("BuffDefs/HiddenInvincibility").iconSprite,
                Color.cyan,
                true,
                true);


            offGuardBuff = Modules.Content.CreateAndAddBuff("Off Guard",
                LegacyResourcesAPI.Load<BuffDef>("BuffDefs/HiddenInvincibility").iconSprite,
                Color.magenta,
                false,
                true);

        }
    }

}
