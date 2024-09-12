using System;
using FirstLightMod.Modules;
using FirstLightMod.Survivors.Beekeeper;
using FirstLightMod.Survivors.Beekeeper.Achievements;
using R2API;

namespace FirstLightMod.Survivors.Beekeeper
{
    public static class BeekeeperTokens
    {
        public static void Init()
        {
            AddBeekeeperTokens();

            ////uncomment this to spit out a language file with all the above tokens that people can translate
            ////make sure you set Language.usingLanguageFolder and printingEnabled to true
            //Language.PrintOutput("Henry.txt");
            ////refer to guide on how to build and distribute your mod with the proper folders
        }

        public static void AddBeekeeperTokens()
        {
            string prefix = BeekeeperSurvivor.BEEKEEPER_PREFIX;

            string desc = "Beekeeper is experienced and a leader. Using their bees and hornets, they can focus fire or spread it out depending on the situation<color=#CCD3E0>" + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > Assault Rifle gives consistent damage and is accurate at most ranges." + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > Pheremone Jar allows Beekeeper to continue dealing damage while focusing on making a tactical retreat." + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > Natural Medicine can help in emergencies or to help allies." + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > Bees fire whenever Beekeeper fires, keeping them alive and healthy will enable constant suppressive fire." + Environment.NewLine + Environment.NewLine;

            string outro = "..and so he left, .";
            string outroFailure = "..and so he vanished, .";


            string lore = "";

            LanguageAPI.Add(prefix + "NAME", "Beekeeper");
            LanguageAPI.Add(prefix + "DESCRIPTION", desc);
            LanguageAPI.Add(prefix + "SUBTITLE", "The Humble Hive");
            LanguageAPI.Add(prefix + "LORE", lore);
            LanguageAPI.Add(prefix + "OUTRO_FLAVOR", outro);
            LanguageAPI.Add(prefix + "OUTRO_FAILURE", outroFailure);

            #region Skins
            LanguageAPI.Add(prefix + "DEFAULT_SKIN_NAME", "Default");
            Language.Add(prefix + "MASTERY_SKIN_NAME", "Alternate");
            #endregion

            #region Passive
            LanguageAPI.Add(prefix + "PASSIVE_NAME", "Honeycomb Defense");
            LanguageAPI.Add(prefix + "PASSIVE_DESCRIPTION", $"While <style=cShrine>Bees</style> and <style=cShrine>Hornets</style> do not have a target, they will fire alongside you.");
            #endregion

            #region Primary
            LanguageAPI.Add(prefix + "PRIMARY_RIFLE_NAME", "Assault Rifle");
            LanguageAPI.Add(prefix + "PRIMARY_RIFLE_DESCRIPTION", $"Fire a constient and accurate stream of bullets for <style=cIsDamage>{100f * BeekeeperStaticValues.assaultRifleDamageCoefficient}% damage</style> per shot.");
            #endregion

            #region Secondary
            LanguageAPI.Add(prefix + "SECONDARY_TARGET_JAR_NAME", "Pheromone Jar");
            LanguageAPI.Add(prefix + "SECONDARY_TARGET_JAR_DESCRIPTION", $"Throw a jar of attractive pheromones, that marks enemies to be targeted by <style=cShrine>Bees</style> and <style=cShrine>Hornets</style>.");
            #endregion

            #region Utility
            LanguageAPI.Add(prefix + "UTILITY_HEAL_NAME", "Natural Medicine");
            LanguageAPI.Add(prefix + "UTILITY_HEAL_DESCRIPTION", $"Take a dose of HON-Y, healing <style=cIsHealing>{BeekeeperStaticValues.honeyHealPercentage}%</style> <style=cIsHealth>max health</style>.");
            #endregion

            #region Special
            LanguageAPI.Add(prefix + "SPECIAL_BEE_NAME", "Bee");
            LanguageAPI.Add(prefix + "SPECIAL_BEE_DESCRIPTION", $"Summon up to 4 light drones that inherit half your items and initially deal <style=cIsDamage>damage</style>.");

            LanguageAPI.Add(prefix + "SPECIAL_HORNET_NAME", "Hornet");
            LanguageAPI.Add(prefix + "SPECIAL_HORNET_DESCRIPTION", $"Summon up to 2 heavy drones that inherit all your items and initially deal <style=cIsDamage>damage</style>.");
            #endregion

            #region Achievements
            Language.Add(Tokens.GetAchievementNameToken(BeekeeperMasteryAchievement.identifier), "Beekeeper: Mastery");
            Language.Add(Tokens.GetAchievementDescriptionToken(BeekeeperMasteryAchievement.identifier), "As the Beekeeper, beat the game or obliterate on Monsoon.");
            #endregion
        }
    }
}
