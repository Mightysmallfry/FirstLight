using FirstLightMod.Survivors.Banneret.SkillStates;

namespace FirstLightMod.Survivors.Banneret
{
    public static class BanneretStates
    {
        public static void Init()
        {
            Modules.Content.AddEntityState(typeof(SlashCombo));

            Modules.Content.AddEntityState(typeof(Shoot));

            Modules.Content.AddEntityState(typeof(Roll));

            Modules.Content.AddEntityState(typeof(ThrowBomb));
        }
    }
}
