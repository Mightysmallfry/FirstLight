using FirstLightMod.Survivors.Cavalier.SkillStates;

namespace FirstLightMod.Survivors.Cavalier
{
    public static class CavalierStates
    {
        public static void Init()
        {
            Modules.Content.AddEntityState(typeof(SlashCombo));

            Modules.Content.AddEntityState(typeof(Roll));

            Modules.Content.AddEntityState(typeof(Rapier));
            Modules.Content.AddEntityState(typeof(Thrust));
            Modules.Content.AddEntityState(typeof(Flare));
        }
    }
}
