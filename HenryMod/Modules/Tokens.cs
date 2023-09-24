using EntityStates.Vulture;
using R2API;
using System;

namespace FirstLightMod.Modules
{
    internal static class Tokens
    {
        internal static void AddTokens()
        {
            //Moved to Character.Initialize(), for both organization and ease of reading

            //    string prefix = FirstLightPlugin.DEVELOPER_PREFIX;
            //    string farmerPrefix = prefix + "_HENRY_BODY_";
            //    string beekeeperPrefix = prefix + "_HENRY_BODY_";
            //AddFarmer(farmerPrefix);
            //AddBeekeeper(beekeeperPrefix);



            #region Henry
            //string prefix = FirstLightPlugin.DEVELOPER_PREFIX + "_HENRY_BODY_";

            //string desc = "Henry is a skilled fighter who makes use of a wide arsenal of weaponry to take down his foes.<color=#CCD3E0>" + Environment.NewLine + Environment.NewLine;
            //desc = desc + "< ! > Sword is a good all-rounder while Boxing Gloves are better for laying a beatdown on more powerful foes." + Environment.NewLine + Environment.NewLine;
            //desc = desc + "< ! > Pistol is a powerful anti air, with its low cooldown and high damage." + Environment.NewLine + Environment.NewLine;
            //desc = desc + "< ! > Roll has a lingering armor buff that helps to use it aggressively." + Environment.NewLine + Environment.NewLine;
            //desc = desc + "< ! > Bomb can be used to wipe crowds with ease." + Environment.NewLine + Environment.NewLine;

            //string outro = "..and so he left, searching for a new identity.";
            //string outroFailure = "..and so he vanished, forever a blank slate.";

            //LanguageAPI.Add(prefix + "NAME", "Henry");
            //LanguageAPI.Add(prefix + "DESCRIPTION", desc);
            //LanguageAPI.Add(prefix + "SUBTITLE", "The Chosen One");
            //LanguageAPI.Add(prefix + "LORE", "sample lore");
            //LanguageAPI.Add(prefix + "OUTRO_FLAVOR", outro);
            //LanguageAPI.Add(prefix + "OUTRO_FAILURE", outroFailure);

            //#region Skins
            //LanguageAPI.Add(prefix + "DEFAULT_SKIN_NAME", "Default");
            //LanguageAPI.Add(prefix + "MASTERY_SKIN_NAME", "Alternate");
            //#endregion

            //#region Passive
            //LanguageAPI.Add(prefix + "PASSIVE_NAME", "Henry passive");
            //LanguageAPI.Add(prefix + "PASSIVE_DESCRIPTION", "Sample text.");
            //#endregion

            //#region Primary
            //LanguageAPI.Add(prefix + "PRIMARY_SLASH_NAME", "Sword");
            //LanguageAPI.Add(prefix + "PRIMARY_SLASH_DESCRIPTION", Helpers.agilePrefix + $"Swing forward for <style=cIsDamage>{100f * StaticValues.swordDamageCoefficient}% damage</style>.");
            //#endregion

            //#region Secondary
            //LanguageAPI.Add(prefix + "SECONDARY_GUN_NAME", "Handgun");
            //LanguageAPI.Add(prefix + "SECONDARY_GUN_DESCRIPTION", Helpers.agilePrefix + $"Fire a handgun for <style=cIsDamage>{100f * StaticValues.gunDamageCoefficient}% damage</style>.");
            //#endregion

            //#region Utility
            //LanguageAPI.Add(prefix + "UTILITY_ROLL_NAME", "Roll");
            //LanguageAPI.Add(prefix + "UTILITY_ROLL_DESCRIPTION", "Roll a short distance, gaining <style=cIsUtility>300 armor</style>. <style=cIsUtility>You cannot be hit during the roll.</style>");
            //#endregion

            //#region Special
            //LanguageAPI.Add(prefix + "SPECIAL_BOMB_NAME", "Bomb");
            //LanguageAPI.Add(prefix + "SPECIAL_BOMB_DESCRIPTION", $"Throw a bomb for <style=cIsDamage>{100f * StaticValues.bombDamageCoefficient}% damage</style>.");
            //#endregion

            //#region Achievements
            //LanguageAPI.Add(prefix + "MASTERYUNLOCKABLE_ACHIEVEMENT_NAME", "Henry: Mastery");
            //LanguageAPI.Add(prefix + "MASTERYUNLOCKABLE_ACHIEVEMENT_DESC", "As Henry, beat the game or obliterate on Monsoon.");
            //LanguageAPI.Add(prefix + "MASTERYUNLOCKABLE_UNLOCKABLE_NAME", "Henry: Mastery");
            //#endregion
            #endregion
        }

        internal static void AddFarmer(string prefix)
        {
            #region FARM-R Character

            string desc = "FARM-R is an old robot that has learned a few new tricks, it has learned which creatures turn into the best kind of fertilizer.<color=#CCD3E0>" + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > Seed Cannon fires in a horizontal spread. Be careful with fighting enemies in a line." + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > Shovel Toss pierces enemies, lining them up will help increase its overall damage." + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > Bungal Grove becomes stronger the more you level up and is an easy way to activate your passive." + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > Fertilizer improves your abilities, use it often to get the most out of it." + Environment.NewLine + Environment.NewLine;

            string outro = "..and so he left, searching for a new identity.";
            string outroFailure = "..and so he vanished, forever a dog of war.";
            //string outroFailure = "..and so he vanished, forever a blank slate.";


            string lore =
                "The mossy and rusted storage bin fizzled as the welding torch cut through it. The smell of ozone permeated the surrounding area. Inside was an old MUL-T model. Intact and offline.\n\n" +

                "\"You know, they never made them like they used to. These old MUL-T models were just built different.\" The old engineer motioning for his drones.\n\n" +

                "\"Yeah well, they're not any harder to reprogram. Plus, it looks like this thing is dead already. I'm not too sure it'll boot up.\"\n\n" +

                "\"The power supply from one of my bees should be enough. One of their purposes is to be sacrificial anyway.\" The old engineer removed the core of one of his drones with a descending hum of its last moments.\n\n" +

                "The old MUL-T whirred to life, servos slowly taking action once more.\n\n" +

                "\"Good, seems like he's back to life, lets start with the reprogramming.\"\n\n" +

                "\"We may have made a mistake, this isn't a normal MUL-T... It's previous designation is not MUL-T but SPRT-N? This thing was militech!?\"\n\n" +

                "\"Explains a lot as to why we couldn't go into certain cargo hulls. Even better though, he'll be able to fight like a dog of war when needed.\"\n\n" +

                "\"Sure but, you're asking me to fit farming protocols onto experimental militech. Do you know how much trouble we'll be in when we're found.\"\n\n" +

                "\"Do you know how much trouble we'll be in if he can't farm and fight? He'll turn the creatures on this world into fertilizer and we get something to live off of for the next who knows how long.\"\n\n" +

                "\"You got it boss, I just have to make sure it doesn't turn us into fertilizer. Easy right?\"\n\n" +

                "\"I understand your concern son, and with a name such as SPRT-N the others might get spooked. Why don't we rename him, something more acurrate.\"\n\n" +

                "\"FARM-R, let's call him what he is. An old farmer with a few tricks up his sleeve.\"\n\n";

            LanguageAPI.Add(prefix + "NAME", "FARM-R");
            LanguageAPI.Add(prefix + "DESCRIPTION", desc);
            LanguageAPI.Add(prefix + "SUBTITLE", "The Iron Sapling");
            LanguageAPI.Add(prefix + "LORE", lore);
            LanguageAPI.Add(prefix + "OUTRO_FLAVOR", outro);
            LanguageAPI.Add(prefix + "OUTRO_FAILURE", outroFailure);

            #region Skins
            LanguageAPI.Add(prefix + "DEFAULT_SKIN_NAME", "Default");
            LanguageAPI.Add(prefix + "MASTERY_SKIN_NAME", "MOSS-E");
            #endregion

            #region Passive
            LanguageAPI.Add(prefix + "PASSIVE_NAME", "Arbor Warden");
            LanguageAPI.Add(prefix + "PASSIVE_DESCRIPTION", $"While <style=cIsHealing>healing</style> gain <style=cIsUtility>{100f * Modules.Config.farmerPassiveAttackSpeedCoefficient.Value}%</style> total additional <style=cIsUtility>attack speed</style>.");

            LanguageAPI.Add(prefix + "ALT_PASSIVE_NAME", "SPRT-N Plating");
            LanguageAPI.Add(prefix + "ALT_PASSIVE_DESCRIPTION", $"Gain an addition <style=cShrine>{Modules.Config.farmerAltPassiveArmorPerLevel.Value} armor</style> upon leveling up");

            #endregion

            #region Primary
            //LanguageAPI.Add(prefix + "PRIMARY_SLASH_NAME", "Sword");
            //LanguageAPI.Add(prefix + "PRIMARY_SLASH_DESCRIPTION", Helpers.agilePrefix + $"Swing forward for <style=cIsDamage>{100f * StaticValues.swordDamageCoefficient}% damage</style>.");


            LanguageAPI.Add(prefix + "PRIMARY_SHOTGUN_NAME", "Seed Shotgun");
            LanguageAPI.Add(prefix + "PRIMARY_SHOTGUN_DESCRIPTION", Helpers.agilePrefix + $"Fire a horizontal line of seeds for <style=cIsDamage>{100f * Modules.Config.shotgunDamageCoefficient.Value}% damage</style>.");

            LanguageAPI.Add(prefix + "PRIMARY_SUPER_SHOTGUN_NAME", "Vine Blaster");
            LanguageAPI.Add(prefix + "PRIMARY_SUPER_SHOTGUN_DESCRIPTION", Helpers.agilePrefix + $"Fire a horizontal line of vines for <style=cIsDamage>{100f * Modules.Config.shotgunDamageCoefficient.Value}% damage that root targets hit.</style>.");


            LanguageAPI.Add(prefix + "PRIMARY_CANNON_NAME", "Spud Launcher");
            LanguageAPI.Add(prefix + "PRIMARY_CANNON_DESCRIPTION", Helpers.agilePrefix + $"Fire a hot spud <style=cIsDamage>{100f * Modules.Config.cannonDamageCoefficient.Value}% damage</style> that explodes on contact.");

            LanguageAPI.Add(prefix + "PRIMARY_SUPER_CANNON_NAME", "Hot Spud Launcher");
            LanguageAPI.Add(prefix + "PRIMARY_SUPER_CANNON_DESCRIPTION", Helpers.agilePrefix + $"Fire a hot spud <style=cIsDamage>{100f * Modules.Config.cannonDamageCoefficient.Value}% damage</style> that explodes on contact.");

            LanguageAPI.Add(prefix + "PRIMARY_PULSE_RIFLE_NAME", "A27 Pulse Rifle");
            LanguageAPI.Add(prefix + "PRIMARY_PULSE_RIFLE_DESCRIPTION", $"Fire bursts of 3 bullets dealing <style=cIsDamage>{100f * Modules.Config.pulseRifleDamageCoefficient.Value}% damage</style> each");

            #endregion

            #region Secondary
            //LanguageAPI.Add(prefix + "SECONDARY_GUN_NAME", "Handgun");
            //LanguageAPI.Add(prefix + "SECONDARY_GUN_DESCRIPTION", Helpers.agilePrefix + $"Fire a handgun for <style=cIsDamage>{100f * StaticValues.gunDamageCoefficient}% damage</style>.");


            LanguageAPI.Add(prefix + "SECONDARY_SHOVEL_NAME", "Shovel Toss");
            LanguageAPI.Add(prefix + "SECONDARY_SHOVEL_DESCRIPTION", $"Throw a mighty shovel, that pierces enemies for <style=cIsDamage>{100f * Modules.Config.shovelDamageCoefficient.Value}% damage</style>.");

            LanguageAPI.Add(prefix + "SECONDARY_FORK_NAME", "Pitchfork Hurl");
            LanguageAPI.Add(prefix + "SECONDARY_FORK_DESCRIPTION", $"Throw a sharp pitchfork, that pierces enemies for <style=cIsDamage>{100f * Modules.Config.forkDamageCoefficient.Value}% damage</style> and causes enemies hit to bleed.");

            LanguageAPI.Add(prefix + "SECONDARY_RAZOR_GRENADE_NAME", "Razor Wire Grenade");
            LanguageAPI.Add(prefix + "SECONDARY_RAZOR_GRENADE_DESCRIPTION", $"Throw a grenade that creates a lingering field of Razor Wire, causing creatures within the area to bleed");


            #endregion

            #region Utility
            //LanguageAPI.Add(prefix + "UTILITY_ROLL_NAME", "Roll");
            //LanguageAPI.Add(prefix + "UTILITY_ROLL_DESCRIPTION", "Roll a short distance, gaining <style=cIsUtility>300 armor</style>. <style=cIsUtility>You cannot be hit during the roll.</style>");

            LanguageAPI.Add(prefix + "UTILITY_BUNGAL_GROVE_NAME", "Bungal Grove");
            LanguageAPI.Add(prefix + "UTILITY_BUNGAL_GROVE_DESCRIPTION", $"Deploy a sprinker, creating a healing field that heals <style=cIsHealing>{Modules.Config.bungalHealingCoefficient.Value}%</style> <style=cIsHealth>max health</style> every second, increasing based off of your level.");

            LanguageAPI.Add(prefix + "UTILITY_LIGHTNING_GROVE_NAME", "Static Grove");
            LanguageAPI.Add(prefix + "UTILITY_LIGHTNING_GROVE_DESCRIPTION", $"Deploy a sprinker, that heals you and deals damage.");

            LanguageAPI.Add(prefix + "UTILITY_MORTAR_NAME", "Mobile Mortar");
            LanguageAPI.Add(prefix + "UTILITY_MORTAR_DESCRIPTION", $"Fire a mortar that comes crashing down dealing, <style=cIsDamage>{100 * Modules.Config.mortarDamageCoefficient.Value}% damage</style>.");

            #endregion

            #region Special
            //LanguageAPI.Add(prefix + "SPECIAL_BOMB_NAME", "Bomb");
            //LanguageAPI.Add(prefix + "SPECIAL_BOMB_DESCRIPTION", $"Throw a bomb for <style=cIsDamage>{100f * StaticValues.bombDamageCoefficient}% damage</style>.");

            LanguageAPI.Add(prefix + "SPECIAL_FERTILIZER_NAME", "Fertilizer");
            LanguageAPI.Add(prefix + "SPECIAL_FERTILIZER_DESCRIPTION", $"Enhances your other abilities.");

            #endregion

            #region Achievements
            LanguageAPI.Add(prefix + "MASTERYUNLOCKABLE_UNLOCKABLE_NAME", "FARM-R: Mastery");
            LanguageAPI.Add(prefix + "MASTERYUNLOCKABLE_ACHIEVEMENT_NAME", "FARM-R: Mastery");
            LanguageAPI.Add(prefix + "MASTERYUNLOCKABLE_ACHIEVEMENT_DESC", "As FARM-R, beat the game or obliterate on Monsoon.");
            #endregion

            #endregion
        }

        internal static void AddBeekeeper(string prefix)
        {
            #region Beekeeper Character

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
            LanguageAPI.Add(prefix + "MASTERY_SKIN_NAME", "Alternate");
            #endregion


            #region Passive
            LanguageAPI.Add(prefix + "PASSIVE_NAME", "Honeycomb Defense");
            LanguageAPI.Add(prefix + "PASSIVE_DESCRIPTION", $"While <style=cShrine>Bees</style> and <style=cShrine>Hornets</style> do not have a target, they will fire alongside you.");
            #endregion


            #region Primary
            LanguageAPI.Add(prefix + "PRIMARY_RIFLE_NAME", "Assault Rifle");
            LanguageAPI.Add(prefix + "PRIMARY_RIFLE_DESCRIPTION", $"Fire a constient and accurate stream of bullets for <style=cIsDamage>{100f * Modules.Config.assaultRifleDamageCoefficient.Value}% damage</style> per shot.");
            #endregion


            #region Secondary
            LanguageAPI.Add(prefix + "SECONDARY_TARGET_JAR_NAME", "Pheromone Jar");
            LanguageAPI.Add(prefix + "SECONDARY_TARGET_JAR_DESCRIPTION", $"Throw a jar of attractive pheromones, that markes enemies to be targeted by <style=cShrine>Bees</style> and <style=cShrine>Hornets</style>.");
            #endregion


            #region Utility
            LanguageAPI.Add(prefix + "UTILITY_HEAL_NAME", "Natural Medicine");
            LanguageAPI.Add(prefix + "UTILITY_HEAL_DESCRIPTION", $"Take a dose of HON-Y, healing <style=cIsHealing>{Modules.Config.honeyHealPercentage.Value}%</style> <style=cIsHealth>max health</style>.");
            #endregion

            #region Special
            LanguageAPI.Add(prefix + "SPECIAL_BEE_NAME", "Bee");
            LanguageAPI.Add(prefix + "SPECIAL_BEE_DESCRIPTION", $"Summon up to 4 light drones that inherit half your items and initially deal <style=cIsDamage>damage</style>.");

            LanguageAPI.Add(prefix + "SPECIAL_HORNET_NAME", "Hornet");
            LanguageAPI.Add(prefix + "SPECIAL_HORNET_DESCRIPTION", $"Summon up to 2 heavy drones that inherit all your items and initially deal <style=cIsDamage>damage</style>.");
            #endregion


            #region Achievements
            LanguageAPI.Add(prefix + "MASTERYUNLOCKABLE_UNLOCKABLE_NAME", "FARM-R: Mastery");
            LanguageAPI.Add(prefix + "MASTERYUNLOCKABLE_ACHIEVEMENT_NAME", "FARM-R: Mastery");
            LanguageAPI.Add(prefix + "MASTERYUNLOCKABLE_ACHIEVEMENT_DESC", "As FARM-R, beat the game or obliterate on Monsoon.");
            #endregion



            #endregion
        }
    }

}