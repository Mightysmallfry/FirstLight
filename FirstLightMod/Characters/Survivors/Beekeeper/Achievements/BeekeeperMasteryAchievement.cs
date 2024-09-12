using FirstLightMod.Modules.Achievements;
using RoR2;

namespace FirstLightMod.Survivors.Beekeeper.Achievements
{
    //automatically creates language tokens "ACHIEVMENT_{identifier.ToUpper()}_NAME" and "ACHIEVMENT_{identifier.ToUpper()}_DESCRIPTION" 
    [RegisterAchievement(identifier, unlockableIdentifier, null, null)]
    public class BeekeeperMasteryAchievement : BaseMasteryAchievement
    {
        public const string identifier = BeekeeperSurvivor.BEEKEEPER_PREFIX + "masteryAchievement";
        public const string unlockableIdentifier = BeekeeperSurvivor.BEEKEEPER_PREFIX + "masteryUnlockable";

        public override string RequiredCharacterBody => BeekeeperSurvivor.instance.bodyName;

        //difficulty coeff 3 is monsoon. 3.5 is typhoon for grandmastery skins
        public override float RequiredDifficultyCoefficient => 3;
    }
}