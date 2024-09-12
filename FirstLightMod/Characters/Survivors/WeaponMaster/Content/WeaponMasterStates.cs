using FirstLightMod.Survivors.WeaponMaster.SkillStates;

namespace FirstLightMod.Survivors.WeaponMaster
{
    public static class WeaponMasterStates
    {
        public static void Init()
        {
            Modules.Content.AddEntityState(typeof(SlashCombo));

            Modules.Content.AddEntityState(typeof(Shoot));

            Modules.Content.AddEntityState(typeof(Roll));

            Modules.Content.AddEntityState(typeof(ThrowBomb));

            Modules.Content.AddEntityState(typeof(HandCrossbowStart));
            Modules.Content.AddEntityState(typeof(HandCrossbowPaint));

        }
    }
}
