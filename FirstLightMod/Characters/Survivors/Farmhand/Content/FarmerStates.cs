using FirstLightMod.Survivors.Farmer.SkillStates;

namespace FirstLightMod.Survivors.Farmer
{
    public static class FarmerStates
    {
        public static void Init()
        {
            //FARM-R Kit

            Modules.Content.AddEntityState(typeof(Cannon));
            Modules.Content.AddEntityState(typeof(SuperCannon));
            Modules.Content.AddEntityState(typeof(Shotgun));
            Modules.Content.AddEntityState(typeof(SuperShotgun));

            Modules.Content.AddEntityState(typeof(BungalGrove));
            Modules.Content.AddEntityState(typeof(LightningGrove));

            Modules.Content.AddEntityState(typeof(Shovel));
            Modules.Content.AddEntityState(typeof(SuperShovel));
            Modules.Content.AddEntityState(typeof(Reap));

            Modules.Content.AddEntityState(typeof(Fertilizer));

            //SPRT-N Kit

            Modules.Content.AddEntityState(typeof(PulseRifle));
            Modules.Content.AddEntityState(typeof(AssaultRifle));

            Modules.Content.AddEntityState(typeof(AimRazorGrenade));
            Modules.Content.AddEntityState(typeof(ThrowRazorGrenade));

            Modules.Content.AddEntityState(typeof(AimThermalGrenade));
            Modules.Content.AddEntityState(typeof(ThrowThermalGrenade));

            Modules.Content.AddEntityState(typeof(AimMortarShell));
            Modules.Content.AddEntityState(typeof(FireMortarShell));

            Modules.Content.AddEntityState(typeof(AimSuperMortarShell));
            Modules.Content.AddEntityState(typeof(FireSuperMortarShell));

            Modules.Content.AddEntityState(typeof(HellFireRockets));

        }
    }
}
