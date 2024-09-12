using FirstLightMod.Survivors.Banneret.SkillStates;

namespace FirstLightMod.Survivors.Beekeeper
{
    public static class BeekeeperStates
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
