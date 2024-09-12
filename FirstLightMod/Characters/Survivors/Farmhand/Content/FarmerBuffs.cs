using RoR2;
using UnityEngine;

namespace FirstLightMod.Survivors.Farmer
{
    public static class FarmerBuffs
    {
        public static BuffDef farmerArborPassive;
        public static BuffDef farmerSpartanPassive;

        public static void Init(AssetBundle assetBundle)
        {
            farmerArborPassive = Modules.Content.CreateAndAddBuff("Arbor Warden",
                LegacyResourcesAPI.Load<BuffDef>("BuffDefs/HiddenInvincibility").iconSprite,
                Color.green,
                true,
                false);

            farmerSpartanPassive = Modules.Content.CreateAndAddBuff("SPRT-N Plating",
                LegacyResourcesAPI.Load<BuffDef>("BuffDefs/ArmorBoost").iconSprite,
                Color.grey,
                true,
                false);
        }
    }
}
