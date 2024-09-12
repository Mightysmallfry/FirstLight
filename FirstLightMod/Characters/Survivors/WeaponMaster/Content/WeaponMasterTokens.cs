using System;
using FirstLightMod.Modules;
using FirstLightMod.Survivors.WeaponMaster.Achievements;

namespace FirstLightMod.Survivors.WeaponMaster
{
    public static class WeaponMasterTokens
    {
        public static void Init()
        {
            AddBanneretTokens();

            ////uncomment this to spit out a language file with all the above tokens that people can translate
            ////make sure you set Language.usingLanguageFolder and printingEnabled to true
            //Language.PrintOutput("Henry.txt");
            ////refer to guide on how to build and distribute your mod with the proper folders
        }

        public static void AddBanneretTokens()
        {
            string prefix = WeaponMasterSurvivor.WEAPON_MASTER_PREFIX;

            string desc = "Henry is a skilled fighter who makes use of a wide arsenal of weaponry to take down his foes.<color=#CCD3E0>" + Environment.NewLine + Environment.NewLine
             + "< ! > Sword is a good all-rounder while Boxing Gloves are better for laying a beatdown on more powerful foes." + Environment.NewLine + Environment.NewLine
             + "< ! > Pistol is a powerful anti air, with its low cooldown and high damage." + Environment.NewLine + Environment.NewLine
             + "< ! > Roll has a lingering armor buff that helps to use it aggressively." + Environment.NewLine + Environment.NewLine
             + "< ! > Bomb can be used to wipe crowds with ease." + Environment.NewLine + Environment.NewLine;

            string outro = "..and so he left, searching for a new identity.";
            string outroFailure = "..and so he vanished, forever a blank slate.";

            Language.Add(prefix + "NAME", "Weapon Master");
            Language.Add(prefix + "DESCRIPTION", desc);
            Language.Add(prefix + "SUBTITLE", "The Master of None");
            Language.Add(prefix + "LORE", "sample lore");
            Language.Add(prefix + "OUTRO_FLAVOR", outro);
            Language.Add(prefix + "OUTRO_FAILURE", outroFailure);

            #region Skins
            Language.Add(prefix + "MASTERY_SKIN_NAME", "Alternate");
            #endregion

            #region Passive
            Language.Add(prefix + "PASSIVE_NAME", "Flag Bearer");
            Language.Add(prefix + "PASSIVE_DESCRIPTION", "Upon defeating an opponent, gain an aura that boosts melee damage and occasionally heals.");
            #endregion

            #region Primary
            Language.Add(prefix + "PRIMARY_SLASH_NAME", "Flamberge");
            Language.Add(prefix + "PRIMARY_SLASH_DESCRIPTION", Tokens.agilePrefix + $"Swing forward for <style=cIsDamage>{100f * WeaponMasterStaticValues.flambergeDamageCoefficient}% damage</style>.");
            #endregion

            #region Secondary
            Language.Add(prefix + "SECONDARY_GUN_NAME", "Hand Crossbow");
            Language.Add(prefix + "SECONDARY_GUN_DESCRIPTION", Tokens.agilePrefix + $"Fire a 3 bolt volley from your crossbow for <style=cIsDamage>{100f * WeaponMasterStaticValues.handcrossbowDamageCoefficient}% damage</style>.");
            #endregion

            #region Utility
            Language.Add(prefix + "UTILITY_ROLL_NAME", "Uppercut");
            Language.Add(prefix + "UTILITY_ROLL_DESCRIPTION", Tokens.agilePrefix + $"Raise your blade in a defensive stance, storing up blocked damage before later releasing it dealing <style=cIsDamage>{100f * WeaponMasterStaticValues.handcrossbowDamageCoefficient}% times damage</style> back in a scorching wave.");
            #endregion

            #region Special
            Language.Add(prefix + "SPECIAL_BOMB_NAME", "Trebuchet");
            Language.Add(prefix + "SPECIAL_BOMB_DESCRIPTION", $"Call in a trebuchet shot for <style=cIsDamage>{100f * WeaponMasterStaticValues.trebuchetDamageCoefficient}% damage</style>.");
            #endregion

            #region Achievements
            Language.Add(Tokens.GetAchievementNameToken(WeaponMasterMasteryAchievement.identifier), "Banneret: Mastery");
            Language.Add(Tokens.GetAchievementDescriptionToken(WeaponMasterMasteryAchievement.identifier), "As the Banneret, beat the game or obliterate on Monsoon.");
            #endregion
        }
    }
}
