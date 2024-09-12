using EntityStates.FalseSonBoss;
using FirstLightMod.Modules.Achievements;
using RoR2;
using RoR2.Achievements;

namespace FirstLightMod.Survivors.Farmer.Achievements
{
    //automatically creates language tokens "ACHIEVMENT_{identifier.ToUpper()}_NAME" and "ACHIEVMENT_{identifier.ToUpper()}_DESCRIPTION" 
    [RegisterAchievement(identifier, unlockableIdentifier, null, 5u, typeof(FarmerMeridianAchievement.FarmerMeridianServerAchievement))]
    public class FarmerMeridianAchievement : BaseMeridianAchievement
    {
        public const string identifier = FarmerSurvivor.FARMER_PREFIX + "meridianAchievement";
        public const string unlockableIdentifier = FarmerSurvivor.FARMER_PREFIX + "meridianUnlockable";

        public override string RequiredCharacterBody => FarmerSurvivor.instance.bodyName;



        private class FarmerMeridianServerAchievement : BaseServerAchievement
        {
            public override void OnInstall()
            {
                base.OnInstall();
                SkyJumpDeathState.falseSonDeathEvent += this.OnMeridianEventTriggerActivated;
            }

            public override void OnUninstall()
            {
                base.OnUninstall();
                SkyJumpDeathState.falseSonDeathEvent -= this.OnMeridianEventTriggerActivated;
            }

            private void OnMeridianEventTriggerActivated()
            {
                base.Grant();
            }
        }

    }
}