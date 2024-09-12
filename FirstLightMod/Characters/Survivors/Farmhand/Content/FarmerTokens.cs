using System;
using FirstLightMod.Modules;
using FirstLightMod.Survivors.Farmer;
using FirstLightMod.Survivors.Farmer.Achievements;
using R2API;

namespace FirstLightMod.Survivors.Farmer
{
    public static class FarmerTokens
    {
        public static void Init()
        {
            AddFarmerTokens();

            ////uncomment this to spit out a language file with all the above tokens that people can translate
            ////make sure you set Language.usingLanguageFolder and printingEnabled to true
            //Language.PrintOutput("Henry.txt");
            ////refer to guide on how to build and distribute your mod with the proper folders
        }

        public static void AddFarmerTokens()
        {
            string prefix = FarmerSurvivor.FARMER_PREFIX;

            string desc = "FARM-R is an old robot that has learned a few new tricks, it has learned which creatures turn into the best kind of fertilizer.<color=#CCD3E0>" + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > Seed Cannon fires in a horizontal spread. Be careful with fighting enemies in a line." + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > Shovel Toss pierces enemies, lining them up will help increase its overall damage." + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > Bungal Grove becomes stronger the more you level up and is an easy way to activate your passive." + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > Fertilizer improves your abilities, use it often to get the most out of it." + Environment.NewLine + Environment.NewLine;

            string outro = "..and so he left, searching for a new identity.";
            string outroFailure = "..and so he vanished, forever a dog of war.";


            string lore = "";

            LanguageAPI.Add(prefix + "NAME", "FARM-R");
            LanguageAPI.Add(prefix + "DESCRIPTION", desc);
            LanguageAPI.Add(prefix + "SUBTITLE", "The Iron Sapling");
            LanguageAPI.Add(prefix + "LORE", lore);
            LanguageAPI.Add(prefix + "OUTRO_FLAVOR", outro);
            LanguageAPI.Add(prefix + "OUTRO_FAILURE", outroFailure);

            #region Skins
            LanguageAPI.Add(prefix + "DEFAULT_SKIN_NAME", "Slumber Wood");
            LanguageAPI.Add(prefix + "MASTERY_SKIN_NAME", "SPRT-N");
            LanguageAPI.Add(prefix + "MERIDIAN_SKIN_NAME", "LASE-R");


            #endregion

            #region Passive
            LanguageAPI.Add(prefix + "PASSIVE_ARBOR_NAME", "Arbor Warden");
            LanguageAPI.Add(prefix + "PASSIVE_ARBOR_DESCRIPTION", $"Improve the effectiveness of healing by <style=cIsHealing>{FarmerStaticValues.farmerArborPassiveHealEffectivenessFraction * 100}%</style> per level");

            LanguageAPI.Add(prefix + "PASSIVE_SPARTAN_NAME", "SPRT-N Plating");
            LanguageAPI.Add(prefix + "PASSIVE_SPARTAN_DESCRIPTION", $"Upon leveling up gain an additional <style=cShrine>{FarmerStaticValues.farmerSpartanPassiveArmorPerLevel} base armor</style>.");
            #endregion

            #region Primary
            LanguageAPI.Add(prefix + "PRIMARY_SHOTGUN_NAME", "Seed Shotgun");
            LanguageAPI.Add(prefix + "PRIMARY_SHOTGUN_DESCRIPTION", $"Fire a horizontal line of seeds, dealing <style=cIsDamage>5*{FarmerStaticValues.shotgunDamageCoefficient * 100}% damage</style> with each volley.");

            LanguageAPI.Add(prefix + "PRIMARY_SUPER_SHOTGUN_NAME", "Vine Blaster");
            LanguageAPI.Add(prefix + "PRIMARY_SUPER_SHOTGUN_DESCRIPTION", $"");

            LanguageAPI.Add(prefix + "PRIMARY_CANNON_NAME", "Spud Launcher");
            LanguageAPI.Add(prefix + "PRIMARY_CANNON_DESCRIPTION", $"Fire a high speed potato at your target dealing <style=cIsDamage>{FarmerStaticValues.cannonDamageCoefficient * 100}% damage</style> with each spud hit.");

            LanguageAPI.Add(prefix + "PRIMARY_SUPER_CANNON_NAME", "Hot Spud Launcher");
            LanguageAPI.Add(prefix + "PRIMARY_SUPER_CANNON_DESCRIPTION", $"");

            LanguageAPI.Add(prefix + "PRIMARY_PULSE_RIFLE_NAME", "A27 Pulse Rifle");
            LanguageAPI.Add(prefix + "PRIMARY_PULSE_RIFLE_DESCRIPTION", $"Fire a burst of bullets, dealing <style=cIsDamage>3*{FarmerStaticValues.burstRifleDamageCoefficient * 100}% damage</style>.");

            LanguageAPI.Add(prefix + "PRIMARY_ASSAULT_RIFLE_NAME", "Assault Rifle");
            LanguageAPI.Add(prefix + "PRIMARY_ASSAULT_RIFLE_DESCRIPTION", $"Fire a flurry of bullets, dealing <style=cIsDamage>{FarmerStaticValues.assaultRifleDamageCoefficient * 100}% damage</style>.");

            #endregion

            #region Secondary
            LanguageAPI.Add(prefix + "SECONDARY_SHOVEL_NAME", "Shovel Toss");
            LanguageAPI.Add(prefix + "SECONDARY_SHOVEL_DESCRIPTION", $"");

            LanguageAPI.Add(prefix + "SECONDARY_FORK_NAME", "Pitchfork Hurl");
            LanguageAPI.Add(prefix + "SECONDARY_FORK_DESCRIPTION", $"");

            LanguageAPI.Add(prefix + "SECONDARY_REAP_NAME", "Harvesting Scythe");
            LanguageAPI.Add(prefix + "SECONDARY_REAP_DESCRIPTION", $"");

            LanguageAPI.Add(prefix + "SECONDARY_RAZOR_GRENADE_NAME", "Razor Wire Grenade");
            LanguageAPI.Add(prefix + "SECONDARY_RAZOR_GRENADE_DESCRIPTION", $"Aim and throw a grenade deals <style=cIsDamage>{FarmerStaticValues.grenadeRazorDamageCoefficient * 100}% damage</style> over the course of {FarmerStaticValues.grenadeRazorDuration} seconds");

            LanguageAPI.Add(prefix + "SECONDARY_THERMAL_GRENADE_NAME", "Geothermic Grenade");
            LanguageAPI.Add(prefix + "SECONDARY_THERMAL_GRENADE_DESCRIPTION", $"Aim and throw a grenade deals <style=cIsDamage>{FarmerStaticValues.GrenadeThermalDamageCoefficient * 100}% damage</style> over the course of {FarmerStaticValues.GrenadeThermalDuration} seconds");


            #endregion

            #region Utility
            LanguageAPI.Add(prefix + "UTILITY_BUNGAL_GROVE_NAME", "Bungal Grove");
            LanguageAPI.Add(prefix + "UTILITY_BUNGAL_GROVE_DESCRIPTION", $"");

            LanguageAPI.Add(prefix + "UTILITY_LIGHTNING_GROVE_NAME", "Static Grove");
            LanguageAPI.Add(prefix + "UTILITY_LIGHTNING_GROVE_DESCRIPTION", $"");

            LanguageAPI.Add(prefix + "UTILITY_MORTAR_NAME", "Mobile Mortar");
            LanguageAPI.Add(prefix + "UTILITY_MORTAR_DESCRIPTION", $"Fire a mortar strike dealing <style=cIsDamage>{FarmerStaticValues.mortarDamageCoefficient * 100}% damage</style> to hostiles within close proximity of target.");


            LanguageAPI.Add(prefix + "UTILITY_SUPER_MORTAR_NAME", "Doom Mortar");
            LanguageAPI.Add(prefix + "UTILITY_SUPER_MORTAR_DESCRIPTION", $"Fire a mortar strike dealing <style=cIsDamage>{FarmerStaticValues.mortarSuperDamageCoefficient * 100}% damage</style> to hostiles within close proximity of target.");

            #endregion

            #region Special
            LanguageAPI.Add(prefix + "SPECIAL_FERTILIZER_NAME", "Fertilizer");
            LanguageAPI.Add(prefix + "SPECIAL_FERTILIZER_DESCRIPTION", $"Enhances your other abilities.");


            LanguageAPI.Add(prefix + "SPECIAL_HELLFIRE_NAME", "Hellfire Rockets");
            LanguageAPI.Add(prefix + "SPECIAL_HELLFIRE_DESCRIPTION", $"Fire up to <style=cIsDamage>{FarmerStaticValues.hellfireBaseStock} incendiary rockets</style>. Each rocket dealing <style=cIsDamage>{FarmerStaticValues.hellfireDamageCoefficient * 100}% damage.</style>");


            #endregion

            #region Achievements
            Language.Add(Tokens.GetAchievementNameToken(FarmerMasteryAchievement.identifier), "Farmer: Mastery");
            Language.Add(Tokens.GetAchievementDescriptionToken(FarmerMasteryAchievement.identifier), "As FARM-R, beat the game or obliterate on Monsoon.");

            Language.Add(Tokens.GetAchievementNameToken(FarmerMeridianAchievement.identifier), "Farmer: Cleared Prime Meridian");
            Language.Add(Tokens.GetAchievementDescriptionToken(FarmerMeridianAchievement.identifier), "As FARM-R, complete the Event on Prime Meridian.");

            #endregion
        }
    }
}
