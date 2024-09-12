using EntityStates.FalseSonBoss;
using FirstLightMod.Modules.Achievements;
using FirstLightMod.Survivors.Cavalier;
using RoR2;
using RoR2.Achievements;

namespace FirstLightMod.Survivors.Cavalier.Achievements
{

    [RegisterAchievement(identifier, unlockableIdentifier, null, 5u, typeof(CavalierMeridianAchievement.CavalierMeridianServerAchievement))]
    public class CavalierMeridianAchievement : BaseMeridianAchievement
    {

        public const string identifier = CavalierSurvivor.CAVALIER_PREFIX + "meridianAchievement";
        public const string unlockableIdentifier = CavalierSurvivor.CAVALIER_PREFIX + "meridianUnlockable";
        public override string RequiredCharacterBody => CavalierSurvivor.instance.bodyName;


        private class CavalierMeridianServerAchievement : BaseServerAchievement
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
