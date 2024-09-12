using FirstLightMod.Survivors.Farmer.Achievements;
using RoR2;
using UnityEngine;

namespace FirstLightMod.Survivors.Farmer
{
    public static class FarmerUnlockables
    {
        public static UnlockableDef characterUnlockableDef = null;
        public static UnlockableDef masterySkinUnlockableDef = null;
        public static UnlockableDef meridianSkinUnlockableDef = null;

        public static void Init()
        {
            masterySkinUnlockableDef = Modules.Content.CreateAndAddUnlockbleDef(
                FarmerMasteryAchievement.unlockableIdentifier,
                Modules.Tokens.GetAchievementNameToken(FarmerMasteryAchievement.identifier),
                FarmerSurvivor.instance.assetBundle.LoadAsset<Sprite>("texMasteryAchievement"));


            meridianSkinUnlockableDef = Modules.Content.CreateAndAddUnlockbleDef(
                FarmerMeridianAchievement.unlockableIdentifier,
                Modules.Tokens.GetAchievementNameToken(FarmerMeridianAchievement.identifier),
                FarmerSurvivor.instance.assetBundle.LoadAsset<Sprite>("texMeridianAchievement"));
        }
    }
}
