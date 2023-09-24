using BepInEx;
using FirstLightMod.Content.Items;
using FirstLightMod.Modules;
using FirstLightMod.Modules.Survivors;
using R2API;
using R2API.Utils;
using RoR2;
using System.Security;
using System.Security.Permissions;
using UnityEngine;
using UnityEngine.AddressableAssets;

[module: UnverifiableCode]
[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]


namespace FirstLightMod
{
    [BepInDependency("com.bepis.r2api", BepInDependency.DependencyFlags.HardDependency)]
    [NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.EveryoneNeedSameModVersion)]
    [BepInPlugin(MODUID, MODNAME, MODVERSION)]
    [R2APISubmoduleDependency(new string[]
    {
        "PrefabAPI",
        "LanguageAPI",
        "ItemAPI",
        "SoundAPI",
        "UnlockableAPI",
        "RecalculateStatsAPI",
        "DeployablesAPI"
    })]

    public class FirstLightPlugin : BaseUnityPlugin
    {
        public const string MODUID = "com.Mighty.FirstLightMod";
        public const string MODNAME = "FirstLightMod";
        public const string MODVERSION = "0.0.2";
        public const string DEVELOPER_PREFIX = "FLP";

        internal static BepInEx.Configuration.ConfigFile mainConfig;


        public static FirstLightPlugin instance;

        //public KrisBlade KrisBlade;
        
        private void Awake()
        {
            mainConfig = Config;
            instance = this;
            Helpers helper = new Helpers(mainConfig);

            Log.Init(Logger);
            Modules.Assets.Initialize(); // load assets and read config
            Modules.Config.ReadConfig(this);
            Modules.States.RegisterStates(); // register states for networking, These are also the entity states for skills that need registering
            Modules.Buffs.RegisterBuffs(); // add and register custom buffs/debuffs
            Modules.Projectiles.RegisterProjectiles(); // add and register custom projectiles
            Modules.Tokens.AddTokens(); // register name tokens
            Modules.ItemDisplays.PopulateDisplays(); // collect item display prefabs for use in our display rules




            //----------------------------------------------- UNLOCKABLES/ACHIEVEMENTS
            //Modules.FLUnlockables.RegisterUnlockables(); // Out of commission due to updating nuget

            //----------------------------------------------- SURVIVORS

            //Can't have them using the same prefab, make a copy and make sure to name them seperately then.

            //new FarmerCharacter().Initialize();
            new BeekeeperCharacter().Initialize();

            //----------------------------------------------- ITEMS

            // Problem Occurs within loading the items
            // - See if this fixes it, separated the functions and gave it a proper configFile var
            // - It did not, something is wrong with loading the items
            // - Current thoughts: are getting help
            //KrisBlade = new KrisBlade();
            //KrisBlade.Init(mainConfig);

            //Items have been fixed!!!!
            helper.LoadAllItems();

            //----------------------------------------------- HOOKS


            Hooks(); // Hooks should be in the file that they are relevant to.

            //----------------------------------------------- EXPANSION

            //Add the Expansion definition to the content Pack
            ApplyExpansion();


            //----------------------------------------------- CONTENT PACKS
            // now make a content pack and add it- this part will change with the next update
            new Modules.ContentPacks().Initialize();


        }

        private void Update()
        {
            // This if statement checks if the player has currently pressed F2.
            if (Input.GetKeyDown(KeyCode.F2))
            {
                // Get the player body to use a position:
                var transform = PlayerCharacterMasterController.instances[0].master.GetBodyObject().transform;

                // And then drop our defined item in front of the player.
                Log.Info($"Player pressed F2. Spawning our custom item at coordinates {transform.position}");
                //KrisBlade krisBlade = new KrisBlade(); // remember, it is singleton, must get the one from the content pack
                //foreach (ItemDef itemDef in ContentPacks.itemDefs)
                //{
                //    PickupIndex index = itemDef.CreatePickupDef().pickupIndex;
                //    index.value = 999;

                //    PickupDropletController.CreatePickupDroplet(index, transform.position, transform.forward * 20f);
                //}
            }
        }

        private void ApplyExpansion()
        {
            string prefix = DEVELOPER_PREFIX + "_EXPANSION_";
            //RoR2.ExpansionManagement.ExpansionDef expansionDef = new RoR2.ExpansionManagement.ExpansionDef();
            RoR2.ExpansionManagement.ExpansionDef expansionDef = ScriptableObject.CreateInstance<RoR2.ExpansionManagement.ExpansionDef>();
            expansionDef.name = "First Light";
            // Is there a nicer way to do the tokens?
            expansionDef.nameToken = prefix + "FIRST_LIGHT_NAME";
            expansionDef.descriptionToken = prefix + "FIRST_LIGHT_DESC";
            //
            expansionDef.iconSprite = Addressables.LoadAssetAsync<Sprite>("RoR2/Base/Common/MiscIcons/texMysteryIcon.png").WaitForCompletion();
            expansionDef.disabledIconSprite = Addressables.LoadAssetAsync<Sprite>("3ec13f47b775f5d478c8a844fa28fdc0").WaitForCompletion();

            LanguageAPI.Add(expansionDef.nameToken, expansionDef.name);
            LanguageAPI.Add(expansionDef.descriptionToken, "Adds content from the '" + expansionDef.name + "' mod to the game.");

            Modules.Content.AddExpansionDef(expansionDef);
        }

        private void Hooks()
        {


            // run hooks here, disabling one is as simple as commenting out the line
            // Make sure to place hooks where they are relevant

            
        }

       
    }
}