using FirstLightMod.Survivors.Cavalier.Achievements;
using RoR2;
using UnityEngine;

namespace FirstLightMod.Survivors.Cavalier
{
    public static class CavalierUnlockables
    {
        public static UnlockableDef characterUnlockableDef = null;
        public static UnlockableDef masterySkinUnlockableDef = null;
        public static UnlockableDef meridianSkinUnlockableDef = null;

        public static void Init()
        {
            masterySkinUnlockableDef = Modules.Content.CreateAndAddUnlockbleDef(
                CavalierMasteryAchievement.unlockableIdentifier,
                Modules.Tokens.GetAchievementNameToken(CavalierMasteryAchievement.identifier),
                CavalierSurvivor.instance.assetBundle.LoadAsset<Sprite>("texMasteryAchievement"));

            meridianSkinUnlockableDef = Modules.Content.CreateAndAddUnlockbleDef(
                CavalierMeridianAchievement.unlockableIdentifier,
                Modules.Tokens.GetAchievementNameToken(CavalierMeridianAchievement.identifier),
                CavalierSurvivor.instance.assetBundle.LoadAsset<Sprite>("texMeridianAchievement"));
        }
    }
}
