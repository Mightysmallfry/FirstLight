using RoR2;
using R2API;
using FirstLightMod;
using FirstLightMod.Content.Achievements;

namespace FirstLightMod.Modules
{
    public static class FLUnlockables
    {

        public static UnlockableDef farmerUnlockableDef;
        public static UnlockableDef farmerMasteryUnlockableDef;


        public static void RegisterUnlockables()
        {
            farmerUnlockableDef = Config.forceFarmerUnlock.Value ? null : UnlockableAPI.AddUnlockable<FarmerUnlockAchievement>(typeof(FarmerUnlockAchievement.FarmerUnlockAchievementServer));

            // Why no worky game!? T-T
            //farmerMasteryUnlockableDef = Config.forceFarmerMasteryUnlock.Value ? null : UnlockableAPI.AddUnlockable<FarmerMasteryAchievement>();


        }
    }
}
