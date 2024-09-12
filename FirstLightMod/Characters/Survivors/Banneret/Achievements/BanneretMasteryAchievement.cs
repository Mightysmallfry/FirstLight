using FirstLightMod.Modules.Achievements;
using RoR2;

namespace FirstLightMod.Survivors.Banneret.Achievements
{
    //automatically creates language tokens "ACHIEVMENT_{identifier.ToUpper()}_NAME" and "ACHIEVMENT_{identifier.ToUpper()}_DESCRIPTION" 
    [RegisterAchievement(identifier, unlockableIdentifier, null, 10)]
    public class BanneretMasteryAchievement : BaseMasteryAchievement
    {
        public const string identifier = BanneretSurvivor.BANNERET_PREFIX + "masteryAchievement";
        public const string unlockableIdentifier = BanneretSurvivor.BANNERET_PREFIX + "masteryUnlockable";

        public override string RequiredCharacterBody => BanneretSurvivor.instance.bodyName;

        //difficulty coeff 3 is monsoon. 3.5 is typhoon for grandmastery skins
        public override float RequiredDifficultyCoefficient => 3;
    }
}