using System;
using FirstLightMod.Modules;
using FirstLightMod.Survivors.Cavalier.Achievements;

namespace FirstLightMod.Survivors.Cavalier
{
    public static class CavalierTokens
    {
        public static void Init()
        {
            AddAdmiralTokens();

            ////uncomment this to spit out a language file with all the above tokens that people can translate
            ////make sure you set Language.usingLanguageFolder and printingEnabled to true
            //Language.PrintOutput("Henry.txt");
            ////refer to guide on how to build and distribute your mod with the proper folders
        }

        public static void AddAdmiralTokens()
        {
            string prefix = CavalierSurvivor.CAVALIER_PREFIX;

            string desc = "Cavalier is a skilled swordswoman who takes command of the battlefield, striking in close range and catching enemies off guard .<color=#CCD3E0>" + Environment.NewLine + Environment.NewLine
             + "< ! > Rapier is a good weapon that is able to get in between enemy armor, good for taking down heavier targets." + Environment.NewLine + Environment.NewLine
             + "< ! > Thrust can be used in any direction, helping engage and disengage from combat decisively." + Environment.NewLine + Environment.NewLine
             + "< ! > Flourish provides great mobility for traveling larger distances or repositioning in combat." + Environment.NewLine + Environment.NewLine
             + "< ! > Flare Gun can be used in tight spots, giving your team a boost in damage while lower enemy capabilities." + Environment.NewLine + Environment.NewLine;

            string outro = "..and so she left, rallying reinforcements for a return.";
            string outroFailure = "..and so she vanished, blade broken and all out of flares.";

            Language.Add(prefix + "NAME", "Cavalier");
            Language.Add(prefix + "DESCRIPTION", desc);
            Language.Add(prefix + "SUBTITLE", "First Lieutenant");
            Language.Add(prefix + "LORE", "sample lore");
            Language.Add(prefix + "OUTRO_FLAVOR", outro);
            Language.Add(prefix + "OUTRO_FAILURE", outroFailure);

            #region Skins
            Language.Add(prefix + "MASTERY_SKIN_NAME", "Fleet Master");
            Language.Add(prefix + "MERIDIAN_SKIN_NAME", "Broken");
            #endregion

            #region Passive
            Language.Add(prefix + "PASSIVE_NAME", "Focus Fire");
            Language.Add(prefix + "PASSIVE_DESCRIPTION", "Every third attack will make the enemy vulnerable. Enemies effected will have reduced <style=cShrine>Armor</style>."); //Could become unique to primary
            #endregion

            #region Primary
            Language.Add(prefix + "PRIMARY_SLASH_NAME", "Rapier");
            Language.Add(prefix + "PRIMARY_SLASH_DESCRIPTION", Tokens.agilePrefix + $"Swing forward for <style=cIsDamage>{100f * CavalierStaticValues.flambergeDamageCoefficient}% damage</style>.");
            #endregion

            #region Secondary
            Language.Add(prefix + "SECONDARY_GUN_NAME", "Thrust");
            Language.Add(prefix + "SECONDARY_GUN_DESCRIPTION", Tokens.agilePrefix + $"Thrust your blade forward, dealing damage to creatures along your path.");
            #endregion

            #region Utility
            Language.Add(prefix + "UTILITY_ROLL_NAME", "Flourish");
            Language.Add(prefix + "UTILITY_ROLL_DESCRIPTION", Tokens.agilePrefix + $"Quickly dash, gaining movement and attack speed for a short duration. Has 2 charges.");


            Language.Add(prefix + "UTILITY_PARRY_NAME", "Parry");
            Language.Add(prefix + "UTILITY_PARRY_DESCRIPTION", Tokens.agilePrefix + $"Block all incoming attacks for {CavalierStaticValues.parryDurationSeconds} seconds, before striking back, dealing bonus damage per attack blocked");
            #endregion

            #region Special
            Language.Add(prefix + "SPECIAL_BOMB_NAME", "Flare Gun");
            Language.Add(prefix + "SPECIAL_BOMB_DESCRIPTION", $"Fire a flare into the sky, putting enemies off guard and rallying teammates.");
            #endregion

            #region Achievements
            Language.Add(Tokens.GetAchievementNameToken(CavalierMasteryAchievement.identifier), "Cavalier: Mastery");
            Language.Add(Tokens.GetAchievementDescriptionToken(CavalierMasteryAchievement.identifier), "As Cavalier, beat the game or obliterate on Monsoon.");

            Language.Add(Tokens.GetAchievementNameToken(CavalierMeridianAchievement.identifier), "Cavalier: Cleared Prime Meridian");
            Language.Add(Tokens.GetAchievementDescriptionToken(CavalierMeridianAchievement.identifier), "As Cavalier, Complete the Event on Prime Meridian.");
            #endregion
        }
    }
}
