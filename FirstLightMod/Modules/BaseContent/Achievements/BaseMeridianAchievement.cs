using RoR2;
using RoR2.Achievements;

namespace FirstLightMod.Modules.Achievements
{
    public abstract class BaseMeridianAchievement : BaseAchievement
    {
        public abstract string RequiredCharacterBody { get; }

        public override BodyIndex LookUpRequiredBodyIndex()
        {
            return BodyCatalog.FindBodyIndex(RequiredCharacterBody);
        }

        public override void OnBodyRequirementMet()
        {
            base.OnBodyRequirementMet();
            base.SetServerTracked(true);
        }

        public override void OnBodyRequirementBroken()
        {
            base.SetServerTracked(false);
            base.OnBodyRequirementBroken();
        }

        
    }
}