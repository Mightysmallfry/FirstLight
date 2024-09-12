using FirstLightMod.Survivors.WeaponMaster.Achievements;
using RoR2;
using UnityEngine;

namespace FirstLightMod.Survivors.WeaponMaster
{
    public static class WeaponMasterUnlockables
    {
        public static UnlockableDef characterUnlockableDef = null;
        public static UnlockableDef masterySkinUnlockableDef = null;

        public static void Init()
        {
            masterySkinUnlockableDef = Modules.Content.CreateAndAddUnlockbleDef(
                WeaponMasterMasteryAchievement.unlockableIdentifier,
                Modules.Tokens.GetAchievementNameToken(WeaponMasterMasteryAchievement.identifier),
                WeaponMasterSurvivor.instance.assetBundle.LoadAsset<Sprite>("texMasteryAchievement"));
        }
    }
}
