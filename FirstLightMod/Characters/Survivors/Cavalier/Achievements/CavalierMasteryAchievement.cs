using FirstLightMod.Modules.Achievements;
using RoR2;

namespace FirstLightMod.Survivors.Cavalier.Achievements
{
    //automatically creates language tokens "ACHIEVMENT_{identifier.ToUpper()}_NAME" and "ACHIEVMENT_{identifier.ToUpper()}_DESCRIPTION" 
    [RegisterAchievement(identifier, unlockableIdentifier, null, 10u, null)]
    public class CavalierMasteryAchievement : BaseMasteryAchievement
    {
        public const string identifier = CavalierSurvivor.CAVALIER_PREFIX + "masteryAchievement";
        public const string unlockableIdentifier = CavalierSurvivor.CAVALIER_PREFIX + "masteryUnlockable";

        public override string RequiredCharacterBody => CavalierSurvivor.instance.bodyName;

        //difficulty coeff 3 is monsoon. 3.5 is typhoon for grandmastery skins
        public override float RequiredDifficultyCoefficient => 3;
    }
}