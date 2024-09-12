using FirstLightMod.Survivors.Beekeeper.Achievements;
using RoR2;
using UnityEngine;

namespace FirstLightMod.Survivors.Beekeeper
{
    public static class BeekeeperUnlockables
    {
        public static UnlockableDef characterUnlockableDef = null;
        public static UnlockableDef masterySkinUnlockableDef = null;

        public static void Init()
        {
            masterySkinUnlockableDef = Modules.Content.CreateAndAddUnlockbleDef(
                BeekeeperMasteryAchievement.unlockableIdentifier,
                Modules.Tokens.GetAchievementNameToken(BeekeeperMasteryAchievement.identifier),
                BeekeeperSurvivor.instance.assetBundle.LoadAsset<Sprite>("texMasteryAchievement"));
        }
    }
}
