using BepInEx;
using R2API;
using R2API.Utils;
using RoR2;
using System.Collections.Generic;
using System.Security;
using System.Security.Permissions;
using FirstLightMod.Items;
using FirstLightMod.Modules;
////using FirstLightMod.Survivors.Banneret;
using FirstLightMod.Survivors.Farmer;
using FirstLightMod.Survivors.Cavalier;
using RoR2.ExpansionManagement;
using UnityEngine;
using UnityEngine.AddressableAssets;
//using FirstLightMod.Survivors.WeaponMaster;


[module: UnverifiableCode]
[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]

//rename this namespace
namespace FirstLightMod
{
    //[BepInDependency("com.rune580.riskofoptions", BepInDependency.DependencyFlags.SoftDependency)]

    //Dependencies
    [BepInDependency(ItemAPI.PluginGUID)]
    [BepInDependency(LanguageAPI.PluginGUID)]
    [BepInDependency(RecalculateStatsAPI.PluginGUID)]

    //Compatibility
    [NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.EveryoneNeedSameModVersion)]
    [BepInPlugin(MODUID, MODNAME, MODVERSION)]

    public class FirstLightPlugin : BaseUnityPlugin
    {
        // if you do not change this, you are giving permission to deprecate the mod-
        //  please change the names to your own stuff, thanks
        //   this shouldn't even have to be said
        public const string MODUID = "com.Mighty.FirstLightMod";
        public const string MODNAME = "FirstLightMod";
        public const string MODVERSION = "0.0.4";

        // a prefix for name tokens to prevent conflicts- please capitalize all name tokens for convention
        public const string DEVELOPER_PREFIX = "FLP";
        public const string EXPANSION_PREFIX = DEVELOPER_PREFIX + "_EXPANSION_";

        public static FirstLightPlugin instance;

        public const string assetBundleName = "flpassetbundle";
        public static AssetBundle mainAssetBundle;

        public ExpansionDef expansionDef;

        void Awake()
        {
            instance = this;

            //easy to use logger
            Log.Init(Logger);

            mainAssetBundle = FirstLightAssets.LoadAssetBundle(assetBundleName);


            //Used when you want to properly set up language folders
            Modules.Language.Init();

            //Expansion Initialization
            InitExpansion();

            //Item Initialization
            Modules.Loaders.Items.Init();
            Log.Info("All items have been initialized");
            Modules.Loaders.Artifacts.Init();
            Log.Info("All artifacts have been initalized");



            //Character Initialization
            //Log.Info("-------------------Survivors----------------------");
            new FarmerSurvivor().Initialize();
            new CavalierSurvivor().Initialize();
            //new WeaponMasterSurvivor().Initialize();
            Log.Info("Survivors have been initialized");


            // make a content pack and add it. this has to be last
            new Modules.ContentPacks().Initialize();
        }



        void InitExpansion()
        {
            //Create Tokens
            LanguageAPI.Add(EXPANSION_PREFIX + "NAME", "First light");
            LanguageAPI.Add(EXPANSION_PREFIX + "DESCRIPTION", "Adds content from the \'First Light\' mod to the game.");
            
            //mainAssetBundle = FirstLightAssets.LoadAssetBundle(assetBundleName);

            //Create Expansion
            expansionDef = ScriptableObject.CreateInstance<RoR2.ExpansionManagement.ExpansionDef>();
            expansionDef.name = "First Light";
            expansionDef.nameToken = EXPANSION_PREFIX + "NAME";
            expansionDef.descriptionToken = EXPANSION_PREFIX + "DESCRIPTION";
            expansionDef.iconSprite = mainAssetBundle.LoadAsset<Sprite>("texFirstLightExpansionIcon");
            expansionDef.disabledIconSprite = Addressables.LoadAssetAsync<Sprite>("3ec13f47b775f5d478c8a844fa28fdc0").WaitForCompletion();
            
            Modules.Content.AddExpansionDef(expansionDef);

        }



    }
}
