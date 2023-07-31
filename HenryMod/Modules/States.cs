using FirstLightMod.SkillStates;
using FirstLightMod.SkillStates.BaseStates;
using System.Collections.Generic;
using System;

namespace FirstLightMod.Modules
{
    public static class States
    {
        internal static void RegisterStates()
        {
            //By using these functions, we are adding these states to the content pack that holds all the data for this mod
            //State definitions are likely located with the characters
            Modules.Content.AddEntityState(typeof(BaseMeleeAttack));
            Modules.Content.AddEntityState(typeof(SlashCombo));

            Modules.Content.AddEntityState(typeof(Shoot));

            Modules.Content.AddEntityState(typeof(Roll));

            Modules.Content.AddEntityState(typeof(ThrowBomb));

            Modules.Content.AddEntityState(typeof(Shotgun));
            Modules.Content.AddEntityState(typeof(SuperShotgun));

            Modules.Content.AddEntityState(typeof(Cannon));
            Modules.Content.AddEntityState(typeof(SuperCannon));

            Modules.Content.AddEntityState(typeof(Shovel));
            Modules.Content.AddEntityState(typeof(Pitchfork));

            Modules.Content.AddEntityState(typeof(BungalGrove));
            Modules.Content.AddEntityState(typeof(LightningGrove));

            Modules.Content.AddEntityState(typeof(Fertilizer));

        }
    }
}