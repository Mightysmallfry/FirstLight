using BepInEx.Configuration;
using R2API;
using RoR2;
using RoR2.Skills;
using System.Collections.Generic;
using FirstLightMod.Modules;
using FirstLightMod.Modules.Characters;
using FirstLightMod.Survivors.Beekeeper.SkillStates;
using FirstLightMod.Survivors.Beekeeper.Components;
using UnityEngine;



namespace FirstLightMod.Survivors.Beekeeper
{
    internal class BeekeeperSurvivor : SurvivorBase<BeekeeperSurvivor>
    {
        //used to load the assetbundle for this character. must be unique
        public override string assetBundleName => "banneretassetbundle"; //if you do not change this, you are giving permission to deprecate the mod

        //the name of the prefab we will create. conventionally ending in "Body". must be unique
        public override string bodyName => "BanneretBody"; //if you do not change this, you get the point by now

        //name of the ai master for vengeance and goobo. must be unique
        public override string masterName => "BeekeeperMonsterMaster"; //if you do not
        //the names of the prefabs you set up in unity that we will use to build your character

        //TODO: Make a model for a character, Keep it basic and simple right now.
        public override string modelPrefabName => "mdlBanneret";
        public override string displayPrefabName => "BanneretDisplay";

        public const string BEEKEEPER_PREFIX = FirstLightPlugin.DEVELOPER_PREFIX + "_BEEKEEPER_";

        //used when registering your survivor's language tokens
        public override string survivorTokenPrefix => BEEKEEPER_PREFIX;
        
        public override BodyInfo bodyInfo => new BodyInfo
        {
            bodyName = bodyName,
            bodyNameToken = BEEKEEPER_PREFIX + "NAME",
            subtitleNameToken = BEEKEEPER_PREFIX + "SUBTITLE",

            characterPortrait = assetBundle.LoadAsset<Texture>("texHenryIcon"),
            bodyColor = Color.yellow,

            crosshair = Assets.LoadCrosshair("Standard"),
            podPrefab = LegacyResourcesAPI.Load<GameObject>("Prefabs/NetworkedObjects/SurvivorPod"),

            maxHealth = 110f,
            healthRegen = 1.5f,
            armor = 0f,

            jumpCount = 1,
        };

        public override CustomRendererInfo[] customRendererInfos => new CustomRendererInfo[]
        {
            new CustomRendererInfo
            {
                childName = "SwordModel",
                material = assetBundle.LoadMaterial("matHenry"),
            },
            new CustomRendererInfo
            {
                childName = "GunModel",
            },
            new CustomRendererInfo
            {
                childName = "Model",
            }
        };

        public override UnlockableDef characterUnlockableDef => BeekeeperUnlockables.characterUnlockableDef;

        public override ItemDisplaysBase itemDisplays => new BeekeeperItemDisplays();

        //set in base classes
        public override AssetBundle assetBundle { get; protected set; }

        public override GameObject bodyPrefab { get; protected set; }
        public override CharacterBody prefabCharacterBody { get; protected set; }
        public override GameObject characterModelObject { get; protected set; }
        public override CharacterModel prefabCharacterModel { get; protected set; }
        public override GameObject displayPrefab { get; protected set; }


        public override void Initialize()
        {
            //uncomment if you have multiple characters
            ConfigEntry<bool> characterEnabled = Config.CharacterEnableConfig("Survivors", "Beekeeper");

            if (!characterEnabled.Value)
            {
                return;
            }

            base.Initialize();
        }



        public override void InitializeCharacter()
        {
            BeekeeperUnlockables.Init();

            base.InitializeCharacter();


            BeekeeperConfig.Init();
            BeekeeperStates.Init();
            BeekeeperTokens.Init();

            BeekeeperAssets.Init(assetBundle);
            BeekeeperBuffs.Init(assetBundle);

            InitializeEntityStateMachines();
            InitializeSkills();
            InitializeSkins();
            InitializeCharacterMaster();

            AdditionalBodySetup();

            AddHooks();
        }

        private void AddHooks()
        {

        }

        private void AdditionalBodySetup()
        {
            AddHitboxes();
            bodyPrefab.AddComponent<BeekeeperWeaponComponent>();
            //bodyPrefab.AddComponent<HuntressTrackerComopnent>();
            //anything else here

            //Should add the Tracker to our character
            bodyPrefab.AddComponent<BeekeeperTracker>();
            bodyPrefab.AddComponent<BeekeeperDroneController>();
        }

        public void AddHitboxes()
        {
            //example of how to create a HitBoxGroup. see summary for more details
            Prefabs.SetupHitBoxGroup(characterModelObject, "SwordGroup", "SwordHitbox");
        }

        public override void InitializeEntityStateMachines()
        {
            //clear existing state machines from your cloned body (probably commando)
            //omit all this if you want to just keep theirs
            Prefabs.ClearEntityStateMachines(bodyPrefab);

            //the main "Body" state machine has some special properties
            Prefabs.AddMainEntityStateMachine(bodyPrefab, "Body", typeof(EntityStates.GenericCharacterMain), typeof(EntityStates.SpawnTeleporterState));
            //if you set up a custom main characterstate, set it up here
            //don't forget to register custom entitystates in your HenryStates.cs

            Prefabs.AddEntityStateMachine(bodyPrefab, "Weapon");
            Prefabs.AddEntityStateMachine(bodyPrefab, "Weapon2");
        }

        public override void InitializeSkills()
        {
            Skills.ClearGenericSkills(bodyPrefab);

            //add our own
            //AddPassiveSkill();
            InitializePassiveSkills();
            InitializePrimarySkills();
            InitializeSecondarySkills();
            InitializeUtilitySkills();
            InitializeSpecialSkills();
        }




        private void InitializePassiveSkills()
        {
            #region passiveDef
            //option 1. fake passive icon just to describe functionality we will implement elsewhere
            bodyPrefab.GetComponent<SkillLocator>().passiveSkill = new SkillLocator.PassiveSkill
            {
                enabled = true,
                skillNameToken = BEEKEEPER_PREFIX + "PASSIVE_NAME",
                skillDescriptionToken = BEEKEEPER_PREFIX + "PASSIVE_DESCRIPTION",
                keywordToken = "KEYWORD_AGILE",
                icon = assetBundle.LoadAsset<Sprite>("texPassiveIcon"),
            };

            #endregion

        }

        private void InitializePrimarySkills()
        {
            SkillDef assaultRifeDef = Skills.CreateSkillDef(new SkillDefInfo(
                "BeekeeperPassive",
                BEEKEEPER_PREFIX + "PRIMARY_RIFLE_NAME",
                BEEKEEPER_PREFIX + "PRIMARY_RIFLE_DESCRIPTION",
                assetBundle.LoadAsset<Sprite>("texUziIcon"),
                new EntityStates.SerializableEntityStateType(typeof(AssaultRifle)), //change to AssaultRifle skill state
                "Weapon",
                true));

            Skills.AddPrimarySkills(bodyPrefab, assaultRifeDef);
        }


        private void InitializeSecondarySkills()
        {
            SkillDef targetJarDef = Skills.CreateSkillDef(new SkillDefInfo()
            {
                skillName = "TargetJar",
                skillNameToken = BEEKEEPER_PREFIX + "SECONDARY_TARGET_JAR_NAME",
                skillDescriptionToken = BEEKEEPER_PREFIX + "SECONDARY_TARGET_JAR_DESCRIPTION",
                keywordTokens = new string[] { "KEYWORD_AGILE" },
                skillIcon = assetBundle.LoadAsset<Sprite>("texSecondaryIcon"),

                activationState = new EntityStates.SerializableEntityStateType(typeof(AimTargetJar)),
                activationStateMachineName = "Weapon2",
                interruptPriority = EntityStates.InterruptPriority.Skill,

                baseRechargeInterval = 1f,
                baseMaxStock = 1,

                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,

                resetCooldownTimerOnUse = false,
                fullRestockOnAssign = true,
                dontAllowPastMaxStocks = false,
                mustKeyPress = false,
                beginSkillCooldownOnSkillEnd = false,

                isCombatSkill = true,
                canceledFromSprinting = false,
                cancelSprintingOnActivation = false,
                forceSprintDuringState = false,
            });

            Skills.AddSecondarySkills(bodyPrefab, targetJarDef);
        }



        private void InitializeUtilitySkills()
        {
            SkillDef honeyHealDef = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "BeekeeperHeal",
                skillNameToken = BEEKEEPER_PREFIX + "UTILITY_HEAL_NAME",
                skillDescriptionToken = BEEKEEPER_PREFIX + "UTILITY_HEAL_DESCRIPTION",
                skillIcon = assetBundle.LoadAsset<Sprite>("texSpecialIcon"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(HoneyHeal)), //Replace type with HoneyHeal class
                activationStateMachineName = "Weapon",
                baseMaxStock = 1,
                baseRechargeInterval = 1f,
                beginSkillCooldownOnSkillEnd = false,
                canceledFromSprinting = true,
                forceSprintDuringState = false,
                fullRestockOnAssign = true,
                interruptPriority = EntityStates.InterruptPriority.Skill,
                resetCooldownTimerOnUse = false,
                isCombatSkill = true,
                mustKeyPress = true,
                cancelSprintingOnActivation = true,
                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,
            });

            Skills.AddUtilitySkills(bodyPrefab, honeyHealDef);

        }


        private void InitializeSpecialSkills()
        {
            SkillDef beeDef = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName =  BEEKEEPER_PREFIX + "SPECIAL_BEE_NAME",
                skillNameToken = BEEKEEPER_PREFIX + "SPECIAL_BEE_NAME",
                skillDescriptionToken = BEEKEEPER_PREFIX + "SPECIAL_BEE_DESCRIPTION",
                skillIcon = assetBundle.LoadAsset<Sprite>("texBazookaIconScepter"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(SummonBee)),
                activationStateMachineName = "Weapon",
                baseMaxStock = 4,
                baseRechargeInterval = 1f,
                beginSkillCooldownOnSkillEnd = false,
                canceledFromSprinting = true,
                forceSprintDuringState = false,
                fullRestockOnAssign = true,
                interruptPriority = EntityStates.InterruptPriority.Skill,
                resetCooldownTimerOnUse = false,
                isCombatSkill = true,
                mustKeyPress = true,
                cancelSprintingOnActivation = true,
                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,
            });

            Skills.AddSpecialSkills(bodyPrefab, beeDef);

            SkillDef hornetDef = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = BEEKEEPER_PREFIX + "SPECIAL_HORNET_NAME",
                skillNameToken = BEEKEEPER_PREFIX + "SPECIAL_HORNET_NAME",
                skillDescriptionToken = BEEKEEPER_PREFIX + "SPECIAL_HORNET_DESCRIPTION",
                skillIcon = assetBundle.LoadAsset<Sprite>("texBazookaIconScepter"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(SummonHornet)),
                activationStateMachineName = "Weapon",
                baseMaxStock = 2,
                baseRechargeInterval = .2f,
                beginSkillCooldownOnSkillEnd = false,
                canceledFromSprinting = true,
                forceSprintDuringState = false,
                fullRestockOnAssign = true,
                interruptPriority = EntityStates.InterruptPriority.Skill,
                resetCooldownTimerOnUse = false,
                isCombatSkill = true,
                mustKeyPress = true,
                cancelSprintingOnActivation = true,
                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,
            });

            Skills.AddSpecialSkills(bodyPrefab, hornetDef);

        }


        public override void InitializeSkins()
        {
            ModelSkinController skinController = prefabCharacterModel.gameObject.AddComponent<ModelSkinController>();
            ChildLocator childLocator = prefabCharacterModel.GetComponent<ChildLocator>();

            CharacterModel.RendererInfo[] defaultRendererinfos = prefabCharacterModel.baseRendererInfos;

            List<SkinDef> skins = new List<SkinDef>();

            #region DefaultSkin
            //this creates a SkinDef with all default fields
            SkinDef defaultSkin = Skins.CreateSkinDef("DEFAULT_SKIN",
                assetBundle.LoadAsset<Sprite>("texMainSkin"),
                defaultRendererinfos,
                prefabCharacterModel.gameObject);

            //these are your Mesh Replacements. The order here is based on your CustomRendererInfos from earlier
            //pass in meshes as they are named in your assetbundle
            //currently not needed as with only 1 skin they will simply take the default meshes
            //uncomment this when you have another skin
            //defaultSkin.meshReplacements = Modules.Skins.getMeshReplacements(assetBundle, defaultRendererinfos,
            //    "meshHenrySword",
            //    "meshHenryGun",
            //    "meshHenry");

            //add new skindef to our list of skindefs. this is what we'll be passing to the SkinController
            skins.Add(defaultSkin);
            #endregion

            //uncomment this when you have a mastery skin
            #region MasterySkin

            ////creating a new skindef as we did before
            //SkinDef masterySkin = Modules.Skins.CreateSkinDef(HENRY_PREFIX + "MASTERY_SKIN_NAME",
            //    assetBundle.LoadAsset<Sprite>("texMasteryAchievement"),
            //    defaultRendererinfos,
            //    prefabCharacterModel.gameObject,
            //    HenryUnlockables.masterySkinUnlockableDef);

            ////adding the mesh replacements as above. 
            ////if you don't want to replace the mesh (for example, you only want to replace the material), pass in null so the order is preserved
            //masterySkin.meshReplacements = Modules.Skins.getMeshReplacements(assetBundle, defaultRendererinfos,
            //    "meshHenrySwordAlt",
            //    null,//no gun mesh replacement. use same gun mesh
            //    "meshHenryAlt");

            ////masterySkin has a new set of RendererInfos (based on default rendererinfos)
            ////you can simply access the RendererInfos' materials and set them to the new materials for your skin.
            //masterySkin.rendererInfos[0].defaultMaterial = assetBundle.LoadMaterial("matHenryAlt");
            //masterySkin.rendererInfos[1].defaultMaterial = assetBundle.LoadMaterial("matHenryAlt");
            //masterySkin.rendererInfos[2].defaultMaterial = assetBundle.LoadMaterial("matHenryAlt");

            ////here's a barebones example of using gameobjectactivations that could probably be streamlined or rewritten entirely, truthfully, but it works
            //masterySkin.gameObjectActivations = new SkinDef.GameObjectActivation[]
            //{
            //    new SkinDef.GameObjectActivation
            //    {
            //        gameObject = childLocator.FindChildGameObject("GunModel"),
            //        shouldActivate = false,
            //    }
            //};
            ////simply find an object on your child locator you want to activate/deactivate and set if you want to activate/deacitvate it with this skin

            //skins.Add(masterySkin);

            #endregion

            skinController.skins = skins.ToArray();
        }

        //Character Master is what governs the AI of your character when it is not controlled by a player (artifact of vengeance, goobo)
        public override void InitializeCharacterMaster()
        {
            //you must only do one of these. adding duplicate masters breaks the game.

            //if you're lazy or prototyping you can simply copy the AI of a different character to be used
            //Modules.Prefabs.CloneDopplegangerMaster(bodyPrefab, masterName, "Merc");

            //how to set up AI in code
            BeekeeperAI.Init(bodyPrefab, masterName);

            //how to load a master set up in unity, can be an empty gameobject with just AISkillDriver components
            //assetBundle.LoadMaster(bodyPrefab, masterName);
        }

    }
}
