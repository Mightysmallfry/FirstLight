using BepInEx.Configuration;
using RoR2;
using RoR2.Skills;
using System;
using System.Collections.Generic;
using FirstLightMod.Modules;
using FirstLightMod.Modules.Characters;
//using FirstLightMod.Survivors.Farmer.Components;
using FirstLightMod.Survivors.Farmer.SkillStates;
using UnityEngine;

namespace FirstLightMod.Survivors.Farmer
{
    internal class FarmerSurvivor : SurvivorBase<FarmerSurvivor>
    {
        //used to load the assetbundle for this character. must be unique
        public override string assetBundleName => "flpassetbundle"; //if you do not change this, you are giving permission to deprecate the mod

        //the name of the prefab we will create. conventionally ending in "Body". must be unique
        public override string bodyName => "FarmerBody"; //if you do not change this, you get the point by now

        //name of the ai master for vengeance and goobo. must be unique
        public override string masterName => "FarmerMonsterMaster"; //if you do not
        //the names of the prefabs you set up in unity that we will use to build your character

        //TODO: Make a model for a character, Keep it basic and simple right now.
        public override string modelPrefabName => "mdlFarmer";
        public override string displayPrefabName => "FarmerDisplay";

        public const string FARMER_PREFIX = FirstLightPlugin.DEVELOPER_PREFIX + "_FARMER_";

        //used when registering your survivor's language tokens
        public override string survivorTokenPrefix => FARMER_PREFIX;

        public override BodyInfo bodyInfo => new BodyInfo
        {
            bodyName = bodyName,
            bodyNameToken = FARMER_PREFIX + "NAME",
            subtitleNameToken = FARMER_PREFIX + "SUBTITLE",

            characterPortrait = assetBundle.LoadAsset<Texture>("texHenryIcon"),
            bodyColor = Color.green,
            sortPosition = 100,

            crosshair = FirstLightAssets.LoadCrosshair("Standard"),
            podPrefab = LegacyResourcesAPI.Load<GameObject>("Prefabs/NetworkedObjects/SurvivorPod"),

            maxHealth = 110f,
            healthGrowth = 48f,
            healthRegen = 1.5f,
            armor = 12f,

            damage = 12f,
            damageGrowth = 2.4f,
            
            crit = 1f,

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

        public override UnlockableDef characterUnlockableDef => FarmerUnlockables.characterUnlockableDef;

        public override ItemDisplaysBase itemDisplays => new FarmerItemDisplays();

        //set in base classes
        public override AssetBundle assetBundle { get; protected set; }

        public override GameObject bodyPrefab { get; protected set; }
        public override CharacterBody prefabCharacterBody { get; protected set; }
        public override GameObject characterModelObject { get; protected set; }
        public override CharacterModel prefabCharacterModel { get; protected set; }
        public override GameObject displayPrefab { get; protected set; }
        
        //SkillDefs made public and static for special skill

        //Passive
        public GenericSkill passiveGenericSkillSlot; //Not every farmer has the same passive in a game

        public static SkillDef arborPassive;
        public static SkillDef spartanPassive;

        //Primary
        public static SkillDef shotgunSkillDef;
        public static SkillDef superShotgunSkillDef;
        public static SkillDef cannonSkillDef;
        public static SkillDef superCannonSkillDef;

        public static SkillDef pulseRifleSkillDef;
        public static SkillDef assaultRifleSkillDef;

        //Secondary
        public static SkillDef shovelSkillDef;
        public static SkillDef pitchforkSkillDef;
        public static SkillDef reapSkillDef;
        public static SkillDef grenadeSkillDef;
        public static SkillDef grenadeSuperSkillDef;

        //Utility
        public static SkillDef groveSkillDef;
        public static SkillDef groveSuperSkillDef;
        public static SkillDef mortarSkillDef;
        public static SkillDef mortarSuperSkillDef;

        //Special
        public static SkillDef fertilizerSkillDef;
        public static SkillDef hellfireRocketsSkillDef;


        public override void Initialize()
        {
            //uncomment if you have multiple characters
            ConfigEntry<bool> characterEnabled = Config.CharacterEnableConfig("Survivors", "FARM-R");

            if (!characterEnabled.Value)
            {
                return;
            }
            
            base.Initialize();
        }



        public override void InitializeCharacter()
        {
            //need the character unlockable before you initialize the survivordef
            FarmerUnlockables.Init();

            base.InitializeCharacter();

            FarmerConfig.Init();
            FarmerStates.Init();
            FarmerTokens.Init();

            FarmerAssets.Init(assetBundle);
            FarmerBuffs.Init(assetBundle);

            InitializeEntityStateMachines();
            InitializeSkills();
            InitializeSkins();
            InitializeCharacterMaster();

            AdditionalBodySetup();

            AddHooks();
        }

        private void AdditionalBodySetup()
        {
            AddHitboxes();
            //bodyPrefab.AddComponent<FarmerWeaponComponent>();
            //bodyPrefab.AddComponent<HuntressTrackerComponent>();
            //anything else here
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
            Prefabs.AddEntityStateMachine(bodyPrefab, "Special");
            Prefabs.AddEntityStateMachine(bodyPrefab, "Fertilizer");
        }


        public override void InitializeSkills()
        {
            //remove the genericskills from the commando body we cloned
            Skills.ClearGenericSkills(bodyPrefab);

            //add our own
            AddPassiveSkills();
            AddPrimarySkills();
            AddSecondarySkills();
            AddUtilitySkills();
            AddSpecialSkills();

        }



        private void AddPassiveSkills()
        {


            passiveGenericSkillSlot = Skills.CreateGenericSkillWithSkillFamily(bodyPrefab, "PassiveSkill");

            arborPassive = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "Arbor Warden",
                skillNameToken = FARMER_PREFIX + "PASSIVE_ARBOR_NAME",
                skillDescriptionToken = FARMER_PREFIX + "PASSIVE_ARBOR_DESCRIPTION",
                //keywordTokens = new string[] { "KEYWORD_AGILE" },
                skillIcon = assetBundle.LoadAsset<Sprite>("texPassiveIcon"),
            });

            Skills.AddSkillsToFamily(passiveGenericSkillSlot.skillFamily, arborPassive);

            spartanPassive = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "SPRT-N",
                skillNameToken = FARMER_PREFIX + "PASSIVE_SPARTAN_NAME",
                skillDescriptionToken = FARMER_PREFIX + "PASSIVE_SPARTAN_DESCRIPTION",
                //keywordTokens = new string[] { "KEYWORD_AGILE" },
                skillIcon = assetBundle.LoadAsset<Sprite>("texPassiveIcon"),
            });
            Skills.AddSkillsToFamily(passiveGenericSkillSlot.skillFamily, spartanPassive);
            
        }




        private void AddPrimarySkills()
        {
            Skills.CreateSkillFamilies(bodyPrefab, SkillSlot.Primary);

            #region SeedShotgun

            //TODO: If I want the single fire when fertilized, I will have to do the skill def like the cannon
            //shotgunSkillDef = Skills.CreateSkillDef(new SkillDefInfo(
            //    "Shotgun",
            //    FARMER_PREFIX + "PRIMARY_SHOTGUN_NAME",
            //    FARMER_PREFIX + "PRIMARY_SHOTGUN_DESCRIPTION",
            //    assetBundle.LoadAsset<Sprite>("texFarmerShotgunIcon"),
            //    new EntityStates.SerializableEntityStateType(typeof(SkillStates.Shotgun)),
            //    "Weapon",
            //    true));

            shotgunSkillDef = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "Shotgun",
                skillNameToken = FARMER_PREFIX + "PRIMARY_SHOTGUN_NAME",
                skillDescriptionToken = FARMER_PREFIX + "PRIMARY_SHOTGUN_DESCRIPTION",
                skillIcon = assetBundle.LoadAsset<Sprite>("texFarmerShotgunIcon"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.Shotgun)),
                activationStateMachineName = "Weapon",
                baseMaxStock = 1,
                requiredStock = 1,
                rechargeStock = 1,
                stockToConsume = 1,
                fullRestockOnAssign = true,
                forceSprintDuringState = false,
                beginSkillCooldownOnSkillEnd = false,
                canceledFromSprinting = false,
                interruptPriority = EntityStates.InterruptPriority.Skill,
                resetCooldownTimerOnUse = false,
                isCombatSkill = true,
                mustKeyPress = false,
                cancelSprintingOnActivation = false,
            });



            Skills.AddPrimarySkills(bodyPrefab, shotgunSkillDef);


            //superShotgunSkillDef = Skills.CreateSkillDef(new SkillDefInfo(
            //    "Super Shotgun",
            //    FARMER_PREFIX + "PRIMARY_SUPER_SHOTGUN_NAME",
            //    FARMER_PREFIX + "PRIMARY_SUPER_SHOTGUN_DESCRIPTION",
            //    assetBundle.LoadAsset<Sprite>("texFarmerShotgunSuperIcon"),
            //    new EntityStates.SerializableEntityStateType(typeof(SkillStates.SuperShotgun)),
            //    "Weapon",
            //    true));

            superShotgunSkillDef = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "Super Shotgun",
                skillNameToken = FARMER_PREFIX + "PRIMARY_SUPER_SHOTGUN_NAME",
                skillDescriptionToken = FARMER_PREFIX + "PRIMARY_SUPER_SHOTGUN_DESCRIPTION",
                skillIcon = assetBundle.LoadAsset<Sprite>("texFarmerShotgunSuperIcon"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.SuperShotgun)),
                activationStateMachineName = "Weapon",
                baseMaxStock = 1,
                requiredStock = 1,
                rechargeStock = 1,
                stockToConsume = 1,
                fullRestockOnAssign = true,
                forceSprintDuringState = false,
                beginSkillCooldownOnSkillEnd = false,
                canceledFromSprinting = false,
                interruptPriority = EntityStates.InterruptPriority.Skill,
                resetCooldownTimerOnUse = false,
                isCombatSkill = true,
                mustKeyPress = false,
                cancelSprintingOnActivation = false,
            });

            //Skills.AddPrimarySkills(bodyPrefab, superShotgunSkillDef);


            #endregion

            #region SpudLauncher

            cannonSkillDef = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "Spud Launcher",
                skillNameToken = FARMER_PREFIX + "PRIMARY_CANNON_NAME",
                skillDescriptionToken = FARMER_PREFIX + "PRIMARY_CANNON_DESCRIPTION",
                skillIcon = assetBundle.LoadAsset<Sprite>("texFarmerCannonIcon"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.Cannon)),
                activationStateMachineName = "Weapon",
                baseMaxStock = 1,
                requiredStock = 1,
                rechargeStock = 1,
                stockToConsume = 1,
                fullRestockOnAssign = true,
                forceSprintDuringState = false,
                beginSkillCooldownOnSkillEnd = false,
                canceledFromSprinting = false,
                interruptPriority = EntityStates.InterruptPriority.Skill,
                resetCooldownTimerOnUse = false,
                isCombatSkill = false,
                mustKeyPress = false,
                cancelSprintingOnActivation = false,
            });

            Skills.AddPrimarySkills(bodyPrefab, cannonSkillDef);

            superCannonSkillDef = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "Super Spud Launcher",
                skillNameToken = FARMER_PREFIX + "PRIMARY_SUPER_CANNON_NAME",
                skillDescriptionToken = FARMER_PREFIX + "PRIMARY_SUPER_CANNON_DESCRIPTION",
                skillIcon = assetBundle.LoadAsset<Sprite>("texFarmerCannonSuperIcon"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.SuperCannon)),
                activationStateMachineName = "Weapon",
                baseMaxStock = 1,
                requiredStock = 1,
                rechargeStock = 0,
                stockToConsume = 1,
                fullRestockOnAssign = true,
                forceSprintDuringState = false,
                beginSkillCooldownOnSkillEnd = false,
                canceledFromSprinting = false,
                interruptPriority = EntityStates.InterruptPriority.Skill,
                resetCooldownTimerOnUse = false,
                isCombatSkill = false,
                mustKeyPress = false,
                cancelSprintingOnActivation = false,
            });

            //Don't add it as it should not be selectable from the menu
            //Skills.AddPrimarySkills(bodyPrefab, superCannonSkillDef);



            #endregion

            #region PulseRifle

            pulseRifleSkillDef = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "Pulse Rifle",
                skillNameToken = FARMER_PREFIX + "PRIMARY_PULSE_RIFLE_NAME",
                skillDescriptionToken = FARMER_PREFIX + "PRIMARY_PULSE_RIFLE_DESCRIPTION",
                skillIcon = assetBundle.LoadAsset<Sprite>("texUziIcon"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.PulseRifle)),
                activationStateMachineName = "Weapon",
                baseMaxStock = 1,
                requiredStock = 1,
                rechargeStock = 1,
                stockToConsume = 1,
                fullRestockOnAssign = true,
                forceSprintDuringState = false,
                beginSkillCooldownOnSkillEnd = false,
                canceledFromSprinting = false,
                interruptPriority = EntityStates.InterruptPriority.Skill,
                resetCooldownTimerOnUse = false,
                isCombatSkill = true,
                mustKeyPress = false,
                cancelSprintingOnActivation = false,
            });

            Skills.AddPrimarySkills(bodyPrefab, pulseRifleSkillDef);

            assaultRifleSkillDef = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "Assault Rifle",
                skillNameToken = FARMER_PREFIX + "PRIMARY_ASSAULT_RIFLE_NAME",
                skillDescriptionToken = FARMER_PREFIX + "PRIMARY_ASSAULT_RIFLE_DESCRIPTION",
                skillIcon = assetBundle.LoadAsset<Sprite>("texUziIcon"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.AssaultRifle)),
                activationStateMachineName = "Weapon",
                baseMaxStock = 1,
                requiredStock = 1,
                rechargeStock = 1,
                stockToConsume = 1,
                fullRestockOnAssign = true,
                forceSprintDuringState = false,
                beginSkillCooldownOnSkillEnd = false,
                canceledFromSprinting = false,
                interruptPriority = EntityStates.InterruptPriority.Skill,
                resetCooldownTimerOnUse = false,
                isCombatSkill = true,
                mustKeyPress = false,
                cancelSprintingOnActivation = false,
            });

            Skills.AddPrimarySkills(bodyPrefab, assaultRifleSkillDef);


            #endregion

        }

        private void AddSecondarySkills()
        {

            Skills.CreateSkillFamilies(bodyPrefab, SkillSlot.Secondary);

            #region Shovel Toss

            shovelSkillDef = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "Shovel Toss",
                skillNameToken = FARMER_PREFIX + "SECONDARY_SHOVEL_NAME",
                skillDescriptionToken = FARMER_PREFIX + "SECONDARY_SHOVEL_DESCRIPTION",
                skillIcon = assetBundle.LoadAsset<Sprite>("texPrimaryIcon"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.Shovel)),
                activationStateMachineName = "Weapon2",
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
                //keywordTokens = new string[] { "KEYWORD_PIERCING" }
            });

            Skills.AddSecondarySkills(bodyPrefab, shovelSkillDef);

            pitchforkSkillDef = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "Pitchfork Toss",
                skillNameToken = FARMER_PREFIX + "SECONDARY_FORK_NAME",
                skillDescriptionToken = FARMER_PREFIX + "SECONDARY_FORK_DESCRIPTION",
                skillIcon = assetBundle.LoadAsset<Sprite>("texStingerIcon"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.SuperShovel)),
                activationStateMachineName = "Weapon2",
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
                rechargeStock = 1, //Remember that this is an empowered ability, this should change to 0 after testing
                requiredStock = 1,
                stockToConsume = 1,
                //keywordTokens = new string[] { "KEYWORD_PIERCING" }
            });
            
            //Skills.AddSecondarySkills(bodyPrefab, pitchforkSkillDef);


            #endregion
            
            #region Reap

            reapSkillDef = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "Reap",
                skillNameToken = FARMER_PREFIX + "SECONDARY_REAP_NAME",
                skillDescriptionToken = FARMER_PREFIX + "SECONDARY_REAP_DESCRIPTION",
                skillIcon = assetBundle.LoadAsset<Sprite>("texPrimaryIcon"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.Reap)),
                activationStateMachineName = "Weapon2",
                baseMaxStock = 2,
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

            Skills.AddSecondarySkills(bodyPrefab, reapSkillDef);

            #endregion

            #region Grenade

            grenadeSkillDef = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "Razor Wire Grenade",
                skillNameToken = FARMER_PREFIX + "SECONDARY_RAZOR_GRENADE_NAME",
                skillDescriptionToken = FARMER_PREFIX + "SECONDARY_RAZOR_GRENADE_DESCRIPTION",
                skillIcon = assetBundle.LoadAsset<Sprite>("texFarmerGrenadeIcon"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.AimRazorGrenade)),
                activationStateMachineName = "Weapon2",
                keywordTokens = new string[] {"KEYWORD_AGILE"},
                baseMaxStock = 1,
                baseRechargeInterval = 1f,
                beginSkillCooldownOnSkillEnd = false,
                canceledFromSprinting = false,
                forceSprintDuringState = false,
                fullRestockOnAssign = true,
                interruptPriority = EntityStates.InterruptPriority.Skill,
                resetCooldownTimerOnUse = false,
                isCombatSkill = true,
                mustKeyPress = true,
                cancelSprintingOnActivation = false,
                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,
            });

            

            Skills.AddSecondarySkills(bodyPrefab, grenadeSkillDef);

            grenadeSuperSkillDef = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "Geothermic Grenade",
                skillNameToken = FARMER_PREFIX + "SECONDARY_THERMAL_GRENADE_NAME",
                skillDescriptionToken = FARMER_PREFIX + "SECONDARY_THERMAL_GRENADE_DESCRIPTION",
                skillIcon = assetBundle.LoadAsset<Sprite>("texFarmerGrenadeSuperIcon"), 
                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.AimThermalGrenade)),
                activationStateMachineName = "Weapon2",
                keywordTokens = new string[] { "KEYWORD_AGILE" },
                baseMaxStock = 1,
                baseRechargeInterval = 1f,
                beginSkillCooldownOnSkillEnd = false,
                canceledFromSprinting = false,
                forceSprintDuringState = false,
                fullRestockOnAssign = true,
                interruptPriority = EntityStates.InterruptPriority.Skill,
                resetCooldownTimerOnUse = false,
                isCombatSkill = true,
                mustKeyPress = true,
                cancelSprintingOnActivation = false,
                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,

            });

            #endregion

        }

        private void AddUtilitySkills()
        {
            Skills.CreateSkillFamilies(bodyPrefab, SkillSlot.Utility);

            #region Bungal Grove



            groveSkillDef = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "Bungal Grove",
                skillNameToken = FARMER_PREFIX + "UTILITY_BUNGAL_GROVE_NAME",
                skillDescriptionToken = FARMER_PREFIX + "UTILITY_BUNGAL_GROVE_DESCRIPTION",
                skillIcon = assetBundle.LoadAsset<Sprite>("texUtilityIcon"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.BungalGrove)),
                activationStateMachineName = "Body",
                baseMaxStock = 1,
                baseRechargeInterval = 12f,
                beginSkillCooldownOnSkillEnd = false,
                canceledFromSprinting = false,
                forceSprintDuringState = false,
                fullRestockOnAssign = true,
                interruptPriority = EntityStates.InterruptPriority.Skill,
                resetCooldownTimerOnUse = false,
                isCombatSkill = false,
                mustKeyPress = false,
                cancelSprintingOnActivation = false,
                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1
            });

            Skills.AddUtilitySkills(bodyPrefab, groveSkillDef);


            groveSuperSkillDef = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "Lightning Grove",
                skillNameToken = FARMER_PREFIX + "UTILITY_LIGHTNING_GROVE_NAME",
                skillDescriptionToken = FARMER_PREFIX + "UTILITY_LIGHTNING_GROVE_DESCRIPTION",
                skillIcon = assetBundle.LoadAsset<Sprite>("texPrimaryIcon"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.BungalGrove)),
                activationStateMachineName = "Body",
                baseMaxStock = 1,
                baseRechargeInterval = 12f,
                beginSkillCooldownOnSkillEnd = false,
                canceledFromSprinting = false,
                forceSprintDuringState = false,
                fullRestockOnAssign = true,
                interruptPriority = EntityStates.InterruptPriority.Skill,
                resetCooldownTimerOnUse = false,
                isCombatSkill = false,
                mustKeyPress = false,
                cancelSprintingOnActivation = false,
                rechargeStock = 0,
                requiredStock = 1,
                stockToConsume = 1
            });

            //Skills.AddUtilitySkills(bodyPrefab, superGroveSkillDef);



            #endregion

            #region Mortar
            //need aim and fire states

            mortarSkillDef = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "Mortar",
                skillNameToken = FARMER_PREFIX + "UTILITY_MORTAR_NAME",
                skillDescriptionToken = FARMER_PREFIX + "UTILITY_MORTAR_DESCRIPTION",
                skillIcon = assetBundle.LoadAsset<Sprite>("texFarmerMortarIcon"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.AimMortarShell)),
                activationStateMachineName = "Weapon2",
                baseMaxStock = 1,
                baseRechargeInterval = 1f,
                beginSkillCooldownOnSkillEnd = false,
                canceledFromSprinting = false,
                forceSprintDuringState = false,
                fullRestockOnAssign = true,
                interruptPriority = EntityStates.InterruptPriority.Skill,
                resetCooldownTimerOnUse = false,
                isCombatSkill = true,
                mustKeyPress = true,
                cancelSprintingOnActivation = false,
                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,
            });


            Skills.AddUtilitySkills(bodyPrefab, mortarSkillDef);

            mortarSuperSkillDef = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "Super Mortar",
                skillNameToken = FARMER_PREFIX + "UTILITY_SUPER_MORTAR_NAME",
                skillDescriptionToken = FARMER_PREFIX + "UTILITY_SUPER_MORTAR_DESCRIPTION",
                skillIcon = assetBundle.LoadAsset<Sprite>("texFarmerMortarSuperIcon"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.AimSuperMortarShell)),
                activationStateMachineName = "Weapon2",
                baseMaxStock = 1,
                baseRechargeInterval = 1f,
                beginSkillCooldownOnSkillEnd = false,
                canceledFromSprinting = false,
                forceSprintDuringState = false,
                fullRestockOnAssign = true,
                interruptPriority = EntityStates.InterruptPriority.Skill,
                resetCooldownTimerOnUse = false,
                isCombatSkill = true,
                mustKeyPress = true,
                cancelSprintingOnActivation = false,
                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,
            });

            #endregion

        }


        private void AddSpecialSkills()
        {
            Skills.CreateSkillFamilies(bodyPrefab, SkillSlot.Special);

            #region Fertilizer

            fertilizerSkillDef = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "Fertilizer",
                skillNameToken = FARMER_PREFIX + "SPECIAL_FERTILIZER_NAME",
                skillDescriptionToken = FARMER_PREFIX + "SPECIAL_FERTILIZER_DESCRIPTION",
                skillIcon = assetBundle.LoadAsset<Sprite>("texFarmerFertilizerIcon"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.Fertilizer)),
                activationStateMachineName = "Fertilizer",
                baseMaxStock = 1,
                baseRechargeInterval = 4f,
                beginSkillCooldownOnSkillEnd = true,
                canceledFromSprinting = false,
                forceSprintDuringState = false,
                fullRestockOnAssign = true,
                interruptPriority = EntityStates.InterruptPriority.Skill,
                resetCooldownTimerOnUse = false,
                isCombatSkill = false,
                mustKeyPress = false,
                cancelSprintingOnActivation = false,
                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1
            });

            Skills.AddSpecialSkills(bodyPrefab, fertilizerSkillDef);

            #endregion


            #region Hellfire Rockets

            hellfireRocketsSkillDef = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "Hellfire Rockets",
                skillNameToken = FARMER_PREFIX + "SPECIAL_HELLFIRE_NAME",
                skillDescriptionToken = FARMER_PREFIX + "SPECIAL_HELLFIRE_DESCRIPTION",
                skillIcon = assetBundle.LoadAsset<Sprite>("texBazookaIcon"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.HellFireRockets)),
                activationStateMachineName = "Special",
                baseMaxStock = FarmerStaticValues.hellfireBaseStock,
                baseRechargeInterval = 5f,
                beginSkillCooldownOnSkillEnd = true,
                canceledFromSprinting = false,
                forceSprintDuringState = false,
                fullRestockOnAssign = true,
                interruptPriority = EntityStates.InterruptPriority.Skill,
                resetCooldownTimerOnUse = false,
                isCombatSkill = false,
                mustKeyPress = false,
                cancelSprintingOnActivation = false,
                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1
            });

            Skills.AddSpecialSkills(bodyPrefab, hellfireRocketsSkillDef);

            #endregion


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
                assetBundle.LoadAsset<Sprite>("texFarmerDefaultPalette"),
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
            SkinDef masterySkin = Modules.Skins.CreateSkinDef(FARMER_PREFIX + "MASTERY_SKIN_NAME",
                assetBundle.LoadAsset<Sprite>("texFarmerAlternateMonsoonPalette"),
                defaultRendererinfos,
                prefabCharacterModel.gameObject,
                FarmerUnlockables.masterySkinUnlockableDef);

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

            skins.Add(masterySkin);

            ////creating a new skindef as we did before
            SkinDef meridianSkin = Modules.Skins.CreateSkinDef(FARMER_PREFIX + "MERIDIAN_SKIN_NAME",
                assetBundle.LoadAsset<Sprite>("texFarmerAlternateMeridianPalette"),
                defaultRendererinfos,
                prefabCharacterModel.gameObject,
                FarmerUnlockables.meridianSkinUnlockableDef);

            skins.Add(meridianSkin);

            #endregion

            skinController.skins = skins.ToArray();
        }

        public override void InitializeCharacterMaster()
        {
            //you must only do one of these. adding duplicate masters breaks the game.

            //if you're lazy or prototyping you can simply copy the AI of a different character to be used
            //Modules.Prefabs.CloneDopplegangerMaster(bodyPrefab, masterName, "Merc");

            //how to set up AI in code
            FarmerAI.Init(bodyPrefab, masterName);

            //how to load a master set up in unity, can be an empty gameobject with just AISkillDriver components
            //assetBundle.LoadMaster(bodyPrefab, masterName);
        }





        private void AddHooks()
        {

            On.RoR2.CharacterBody.RecalculateStats += CharacterBody_RecalculateStats;
        }


        private void CharacterBody_RecalculateStats(On.RoR2.CharacterBody.orig_RecalculateStats orig, CharacterBody self)
        {
            orig(self);

            if (self.baseNameToken == FARMER_PREFIX + "NAME")
            {
                GenericSkill passiveGenericSkillSlot = self.GetComponent<GenericSkill>();
                if (this.passiveGenericSkillSlot == FarmerSurvivor.arborPassive)
                {
                    Log.Info("Farmer: Arbor Passive Activated on CharacterBody Update");
                    for (int i = 0; i < FarmerStaticValues.farmerArborPassiveHealEffectivenessFraction; i++)
                    {
                        self.AddBuff(FarmerBuffs.farmerArborPassive);
                    }
                }

                if (this.passiveGenericSkillSlot == FarmerSurvivor.spartanPassive)
                {
                    Log.Info("Farmer: Spartan Passive Activated on CharacterBody Start");
                    self.baseArmor += FarmerStaticValues.farmerSpartanPassiveArmorPerLevel;
                    for (int i = 0; i < FarmerStaticValues.farmerSpartanPassiveArmorPerLevel; i++)
                    {
                        self.AddBuff(FarmerBuffs.farmerSpartanPassive);
                    }
                }
            }

        }

        //private void GlobalEventManager_OnCharacterLevelUp(On.RoR2.GlobalEventManager.orig_OnCharacterLevelUp orig, CharacterBody characterBody)
        //{
        //    throw new NotImplementedException();
        //}



    }
}