using BepInEx.Configuration;
using HarmonyLib;
using R2API.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace FirstLightMod.Modules
{
    public static class Config
    {


        private static ConfigEntry<string> modVersion;

        private static string versionSuffix = " - " + FirstLightPlugin.MODVERSION;
        public static ConfigEntry<float> sortPosition;


        private static string characterUnlocks = "Unlocks";
        public static ConfigEntry<bool> forceFarmerUnlock;
        //public static ConfigEntry<bool> forceFarmerMasteryUnlock;


        private static string passiveSection;
        private static string primarySection;
        private static string secondarySection;
        private static string utilitySection;
        private static string specialSection;

        //----------------------------------------------- FARM-R
        private static string farmerPrefix = "FARM-R - ";
        #region Farmer

        #region Passive
        public static ConfigEntry<float> farmerPassiveAttackSpeedCoefficient;

        public static ConfigEntry<float> farmerAltPassiveArmorPerLevel;

        #endregion

        #region Primary
        public static ConfigEntry<float> shotgunDamageCoefficient;
        public static ConfigEntry<float> superShotgunDamageCoefficient;

        public static ConfigEntry<float> cannonDamageCoefficient;
        public static ConfigEntry<float> cannonExplosionRadius;
        public static ConfigEntry<float> superCannonDamageCoefficient;

        public static ConfigEntry<float> pulseRifleDamageCoefficient;
        //public static ConfigEntry<float> pulseRifleInterval;
        public static ConfigEntry<int> pulseRifleBulletCount;

        #endregion

        #region Secondary 

        public static ConfigEntry<float> shovelDamageCoefficient;
        public static ConfigEntry<float> forkDamageCoefficient;

        #endregion

        #region Utility

        public static ConfigEntry<float> bungalHealingCoefficient;
        public static ConfigEntry<float> bungalAdditionalHealingCoefficient;

        public static ConfigEntry<float> mortarDamageCoefficient;

        #endregion

        #region Special

        #endregion

        #endregion



        //----------------------------------------------- Beekeeper
        private static string keeperPrefix = "Beekeeper - ";



        #region Beekeeper
        public static ConfigEntry<float> assaultRifleDamageCoefficient;
        public static ConfigEntry<float> jarRadius;

        public static ConfigEntry<float> honeyHealPercentage;
        public static ConfigEntry<float> beeDamageCoefficient;
        public static ConfigEntry<float> hornetDamageCoefficient;



        #endregion



        public static void ReadConfig(FirstLightPlugin plugin)
        {

            plugin.Config.Clear();

            modVersion = plugin.Config.Bind<string>("General", "Mod Version", FirstLightPlugin.MODVERSION, "Current version; don't touch this or it will reset your config");

            sortPosition = plugin.Config.Bind<float>("General", "Lobby Sort Position", 10f, "Sort position of FARM-R in the character select lobby");

            if (modVersion.Value != modVersion.DefaultValue.ToString())
            {
                Log.Warning("FirstLight - version mismatch detected, clearing config");
                ((Dictionary<ConfigDefinition, string>)AccessTools.PropertyGetter(typeof(ConfigFile), "OrphanedEntries").Invoke(plugin.Config, null)).Clear();
                modVersion.Value = modVersion.DefaultValue.ToString();
            }

            FarmerConfig(plugin);
            BeekeeperConfig(plugin);

            #region Character Unlocks

            //Remember to change these to false
            forceFarmerUnlock = plugin.Config.Bind<bool>(characterUnlocks, "FARM-R Forced Unlock", true, "Makes FARM-R unlocked by default");
            //forceFarmerMasteryUnlock = plugin.Config.Bind<bool>(characterUnlocks, "FARM-R Forced Mastery Unlock", true, "Makes the FARM-R Logbook and Monsoon skin unlocked by default");


            #endregion



        }

        internal static void ReconfigureSections(string prefix)
        {
            passiveSection = prefix + "Passive";
            primarySection = prefix + "Primary";
            secondarySection = prefix + "Secondary";
            utilitySection = prefix + "Utility";
            specialSection = prefix + "Special";
        }

        internal static void FarmerConfig(FirstLightPlugin plugin)
        {
            ReconfigureSections(farmerPrefix);

            #region FARM-R

            #region Passive

            farmerPassiveAttackSpeedCoefficient = plugin.Config.Bind<float>(passiveSection, "Arbor Warden Attack Speed Percentage Coefficient", 0.10f, "The additional attack speed gained during FARM-R's Arbor Warden passive. The default increase is 10%");
            farmerAltPassiveArmorPerLevel = plugin.Config.Bind<float>(passiveSection, "SPRT-N Plating Armor", 5f, "The amount of Armor gained per level due to FARM-R's SPRT-N Plating passive.");


            #endregion


            #region Primary

            shotgunDamageCoefficient = plugin.Config.Bind<float>(primarySection, "Seed Shotgun Damage Coefficient", 1.2f, "Damage coefficient per pellet of the Seed Shotgun");
            superShotgunDamageCoefficient = plugin.Config.Bind<float>(primarySection, "Enhanced Seed Shotgun Damage Coefficient", 1.2f, "Damage coefficient per pellet of the Seed Shotgun after using Fertilizer");


            cannonDamageCoefficient = plugin.Config.Bind<float>(primarySection, "Spud Launcher Damage Coefficient", 2.4f, "Damage coefficient per shot of the cannon");
            cannonExplosionRadius = plugin.Config.Bind<float>(primarySection, "Spud Launcher Explosion Radius", 2.5f, "The radius of the attack explosion");
            superCannonDamageCoefficient = plugin.Config.Bind<float>(primarySection, "Enhanced Spud Launcher Damage Coefficiant", 2.4f, "Damage coefficient per shot of the cannon after using Fertilizer");

            pulseRifleDamageCoefficient = plugin.Config.Bind<float>(primarySection, "Pulse Rifle Damage Coefficient", 1f, "Damage coefficient per bullet of the 3 shot burst");
            //pulseRifleInterval = plugin.Config.Bind<float>(primarySection, "Pulse Rifle Interval", 0.125f, "Time in between the bullets leaving the weapon in seconds")
            pulseRifleBulletCount = plugin.Config.Bind<int>(primarySection, "Pulse Rifle Burst Bullet Count", 3, "The number of bullets in a burst");



            #endregion

            #region Secondary

            shovelDamageCoefficient = plugin.Config.Bind<float>(secondarySection, "Shovel Damage Coefficient", 3f, "Damage coefficient the shovel toss");
            forkDamageCoefficient = plugin.Config.Bind<float>(secondarySection, "Pitchfork Damage Coefficient", 3f, "Damage coefficient the pitchfork");

            #endregion

            #region Utility

            bungalHealingCoefficient = plugin.Config.Bind<float>(utilitySection, "Bungal Grove Healing Coefficient", 2.5f, "The coefficient for the bungal grove ability at level 1");
            bungalAdditionalHealingCoefficient = plugin.Config.Bind<float>(utilitySection, "Bungal Grove Additional Healing Coefficiant", 2.5f, "The value that increases the healing coefficient per level");

            mortarDamageCoefficient = plugin.Config.Bind<float>(utilitySection, "Mobile Mortar Damage Coefficient", 4f, "The damage coefficient for the mobile mortar skill");

            #endregion


            #endregion
        }


        internal static void BeekeeperConfig(FirstLightPlugin plugin)
        {
            ReconfigureSections(keeperPrefix);

            assaultRifleDamageCoefficient = plugin.Config.Bind<float>(primarySection, "Assault Rifle Damage Coefficient", 1.0f, "Damage coefficient is multiplied by 100 to calculate base damage.");

            jarRadius = plugin.Config.Bind<float>(secondarySection, "Jar Radius", 25.0f, "What is the radius of beekeeper's jar abiltieis. Note: this affects both jar abilities.");

            honeyHealPercentage = plugin.Config.Bind<float>(utilitySection, "HON-Y Heal Percentage", 10.0f, "What is the percentage of beekeeper's hit points that will be healed by Natural Medicine");

            beeDamageCoefficient = plugin.Config.Bind<float>(specialSection, "Bee Damage Coefficient", 2.0f, "Damage coefficient of bee bullets");
            hornetDamageCoefficient = plugin.Config.Bind<float>(specialSection, "Hornet Damage Coefficient", 10.0f, "Damage coefficient for damage per tick of the Hornet Laser");

        }

        // this helper automatically makes config entries for disabling survivors
        public static ConfigEntry<bool> CharacterEnableConfig(string characterName, string description = "Set to false to disable this character", bool enabledDefault = true) {

            return FirstLightPlugin.instance.Config.Bind<bool>("General",
                                                          "Enable " + characterName,
                                                          enabledDefault,
                                                          description);
        }
    }
}