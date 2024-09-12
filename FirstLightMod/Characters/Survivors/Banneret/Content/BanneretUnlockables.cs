using FirstLightMod.Survivors.Banneret.Achievements;
using RoR2;
using UnityEngine;

namespace FirstLightMod.Survivors.Banneret
{
    public static class BanneretUnlockables
    {
        public static UnlockableDef characterUnlockableDef = null;
        public static UnlockableDef masterySkinUnlockableDef = null;

        public static void Init()
        {
            masterySkinUnlockableDef = Modules.Content.CreateAndAddUnlockbleDef(
                BanneretMasteryAchievement.unlockableIdentifier,
                Modules.Tokens.GetAchievementNameToken(BanneretMasteryAchievement.identifier),
                BanneretSurvivor.instance.assetBundle.LoadAsset<Sprite>("texMasteryAchievement"));
        }
    }
}
