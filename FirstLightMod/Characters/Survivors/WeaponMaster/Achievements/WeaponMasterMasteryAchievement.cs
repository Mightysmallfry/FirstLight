using FirstLightMod.Modules.Achievements;
using RoR2;

namespace FirstLightMod.Survivors.WeaponMaster.Achievements
{
    //automatically creates language tokens "ACHIEVMENT_{identifier.ToUpper()}_NAME" and "ACHIEVMENT_{identifier.ToUpper()}_DESCRIPTION" 
    [RegisterAchievement(identifier, unlockableIdentifier, null, null)]
    public class WeaponMasterMasteryAchievement : BaseMasteryAchievement
    {
        public const string identifier = WeaponMasterSurvivor.WEAPON_MASTER_PREFIX + "masteryAchievement";
        public const string unlockableIdentifier = WeaponMasterSurvivor.WEAPON_MASTER_PREFIX + "masteryUnlockable";

        public override string RequiredCharacterBody => WeaponMasterSurvivor.instance.bodyName;

        //difficulty coeff 3 is monsoon. 3.5 is typhoon for grandmastery skins
        public override float RequiredDifficultyCoefficient => 3;
    }
}