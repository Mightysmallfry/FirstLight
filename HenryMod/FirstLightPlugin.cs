using BepInEx;
using R2API;
using R2API.Utils;
using RoR2;
using System;
using System.Reflection;
using System.Security;
using System.Security.Permissions;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using FirstLightMod.Content;
using FirstLightMod.Content.Beekeeper;
using FirstLightMod.Modules;
using FirstLightMod.Modules.Items;
using IL.RoR2.ExpansionManagement;

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
            Log.Info("Beekepeper has finished Initializing");

            //----------------------------------------------- ITEMS

            // Problem Occurs within loading the items
            // - See if this fixes it, separated the functions and gave it a proper configFile var
            // - It did not, something is wrong with loading the items
            // - Current thoughts: are getting help
            //KrisBlade = new KrisBlade();
            //KrisBlade.Init(mainConfig);

            //Items have been fixed!!!!
            helper.LoadAll();

            //----------------------------------------------- HOOKS


            Hooks(); // Hooks should be in the file that they are relevant to.

            //----------------------------------------------- EXPANSION

            //Add the Expansion definition to the content Pack
            ApplyExpansion();


            //----------------------------------------------- CONTENT PACKS
            // now make a content pack and add it- this part will change with the next update
            new Modules.ContentPacks().Initialize();

            //----------------------------------------------- CONSOLE COMMANDS
            R2API.Utils.CommandHelper.AddToConsoleWhenReady();

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
            expansionDef.nameToken = prefix + "FIRST_LIGHT_NAME";
            expansionDef.descriptionToken = prefix + "FIRST_LIGHT_DESC";
            //
            expansionDef.iconSprite = Addressables.LoadAssetAsync<Sprite>("RoR2/Base/Common/MiscIcons/texWIPIcon.png").WaitForCompletion();
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

        /// <summary>
        /// Gives one of each item from the FirsLight Mod to the player
        /// </summary>
        /// <param name="args"></param> 
        [ConCommand(commandName = "GiveAllItems_FLP", flags = ConVarFlags.None, helpText = "Gives one of each item from the FirstLight Mod to the player. args\\[0\\]=(int)value) ]")]
        private static void GiveModdedItems(ConCommandArgs args)
        {
            try
            {
                //This works but ideally change this to work with multiplayer and character indices in the future
                CharacterBody characterBody = args.TryGetSenderBody();

                //I want this loop to give the item by accessing each items instance
                foreach (ItemDef itemDef in ContentPacks.itemDefs)
                {
                    characterBody.inventory.GiveItem(itemDef);
                }
                
            }
            catch (Exception ex)
            {
                Debug.LogError(ex);
            }
        }



        [ConCommand(commandName = "GiveAllEquipment_FLP", flags = ConVarFlags.None, helpText = "Spawns a droplet of all the equipment available in the FirstLightMod, near the player.")]
        private static void GiveModdedEquipment(ConCommandArgs args)
        {
            try
            {
                CharacterBody characterBody = args.TryGetSenderBody();
                var transform = PlayerCharacterMasterController.instances[0].master.GetBodyObject().transform;

                foreach (EquipmentDef equipmentDef in ContentPacks.equipmentDefs)
                {
                    //I should add rotation so that they don't all pile up in the same location.
                    PickupDropletController.CreatePickupDroplet(PickupCatalog.FindPickupIndex(equipmentDef.equipmentIndex), transform.position, transform.forward * 20f);
                }
                
            }
            catch (Exception ex)
            {
                Debug.LogError(ex);
            }
        }



    }

}