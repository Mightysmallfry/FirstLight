using BepInEx.Configuration;
using FirstLightMod.Modules;
using FirstLightMod.Modules.Characters;
using FirstLightMod.Modules.Survivors;
using FirstLightMod.SkillStates.Farmer;
using On.EntityStates.CaptainSupplyDrop;
using RoR2;
using RoR2.Skills;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using static RoR2.SkillLocator;

namespace FirstLightMod.Content
{
    internal class FarmerCharacter : SurvivorBase
    {
        //used when building your character using the prefabs you set up in unity
        //don't upload to thunderstore without changing this
        public override string prefabBodyName => "Henry";

        public const string FARMER_PREFIX = FirstLightPlugin.DEVELOPER_PREFIX + "_HENRY_BODY_";

        //used when registering your survivor's language tokens
        public override string survivorTokenPrefix => FARMER_PREFIX;

        #region SkillDefs

        //used when boosting skills to their "super" versions
        public static SkillDef shotgunSkillDef;
        public static SkillDef superShotgunSkillDef;

        public static SkillDef cannonSkillDef;
        public static SkillDef superCannonSkillDef;
        public static SkillDef pulseRifleSkillDef;

        public static SkillDef shovelSkillDef;
        public static SkillDef pitchforkSkillDef;
        public static SkillDef razorWireGrenadeSkillDef;
        public static SkillDef reapSkillDef;

        public static SkillDef groveSkillDef;
        public static SkillDef superGroveSkillDef;
        public static SkillDef mortarSkillDef;

        #endregion

        public override BodyInfo bodyInfo { get; set; } = new BodyInfo
        {
            bodyName = "HenryTutorialBody",
            bodyNameToken = FARMER_PREFIX + "NAME",
            subtitleNameToken = FARMER_PREFIX + "SUBTITLE",

            characterPortrait = Assets.mainAssetBundle.LoadAsset<Texture>("texHenryIcon"),
            bodyColor = Color.green,

            crosshair = Assets.LoadCrosshair("Standard"),
            podPrefab = LegacyResourcesAPI.Load<GameObject>("Prefabs/NetworkedObjects/SurvivorPod"),

            maxHealth = 110f,
            healthRegen = 1.5f,
            armor = 0f,
            jumpCount = 1,
        };

        public override CustomRendererInfo[] customRendererInfos { get; set; } = new CustomRendererInfo[]
        {
                new CustomRendererInfo
                {
                    childName = "SwordModel",
                    material = Materials.CreateHopooMaterial("matHenry"),
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

        public override UnlockableDef characterUnlockableDef => FLUnlockables.farmerUnlockableDef;

        public override Type characterMainState => typeof(EntityStates.GenericCharacterMain);

        public override ItemDisplaysBase itemDisplays => new FarmerItemDisplays();

        //if you have more than one character, easily create a config to enable/disable them like this
        public override ConfigEntry<bool> characterEnabledConfig => null; //Modules.Config.CharacterEnableConfig(bodyName);

        public override void InitializeCharacter()
        {
            base.InitializeCharacter();
            Tokens.AddFarmer(FARMER_PREFIX);
        }

        public override void InitializeUnlockables()
        {
            //Moved to FLUnlockables
            //uncomment this when you have a mastery skin. when you do, make sure you have an icon too
            //masterySkinUnlockableDef = Modules.Unlockables.AddUnlockable<Modules.Achievements.MasteryAchievement>();
        }

        public override void InitializeHitboxes()
        {
            ChildLocator childLocator = bodyPrefab.GetComponentInChildren<ChildLocator>();

            //example of how to create a hitbox
            //Transform hitboxTransform = childLocator.FindChild("SwordHitbox");
            //Modules.Prefabs.SetupHitbox(prefabCharacterModel.gameObject, hitboxTransform, "Sword");
        }

        public override void InitializeSkills()
        {
            Skills.CreateSkillFamilies(bodyPrefab);
            string prefix = FirstLightPlugin.DEVELOPER_PREFIX;

            InitializePassiveSkills(prefix);
            InitializePrimarySkills(prefix);
            InitializeSecondarySkills(prefix);
            InitializeUtilitySkills(prefix);
            InitializeSpecialSkills(prefix);
            InitializeHooks();


            //skillLocator.passiveSkill.skillNameToken = FARMER_PREFIX + "ALT_PASSIVE_NAME";
            //skillLocator.passiveSkill.skillDescriptionToken = FARMER_PREFIX + "ALT_PASSIVE_DESCRIPTION";
            //skillLocator.passiveSkill.icon = Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("texSecondaryIcon");
            //skillLocator.passiveSkill.enabled = false; //hopefully this is not used by others.




        }



        private void InitializePassiveSkills(string prefix)
        {
            SkillLocator skillLocator = bodyPrefab.GetComponent<SkillLocator>();

            SkillDef arborPassive = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillNameToken = FARMER_PREFIX + "PASSIVE_NAME",
                skillDescriptionToken = FARMER_PREFIX + "PASSIVE_DESCRIPTION",
                skillIcon = Assets.mainAssetBundle.LoadAsset<Sprite>("texSecondaryIcon"),
                activationStateMachineName = "Body",
                isCombatSkill = false,
            });

            Skills.AddPassiveSkills(bodyPrefab, arborPassive);



            SkillDef spartanPassive = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillNameToken = FARMER_PREFIX + "ALT_PASSIVE_NAME",
                skillDescriptionToken = FARMER_PREFIX + "ALT_PASSIVE_DESCRIPTION",
                skillIcon = Assets.mainAssetBundle.LoadAsset<Sprite>("texSecondaryIcon"),
                activationStateMachineName = "Body",
                isCombatSkill = false,
            });

            Skills.AddPassiveSkills(bodyPrefab, spartanPassive);
        }






        private void InitializePrimarySkills(string prefix)
        {

            #region Example Sword
            //Creates a skilldef for a typical primary 
            //SkillDef primarySkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo(prefix + "_HENRY_BODY_PRIMARY_SLASH_NAME",
            //                                                                          prefix + "_HENRY_BODY_PRIMARY_SLASH_DESCRIPTION",
            //                                                                          Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("texPrimaryIcon"),
            //                                                                          new EntityStates.SerializableEntityStateType(typeof(SkillStates.SlashCombo)),
            //                                                                          "Weapon",
            //                                                                          true));


            //Modules.Skills.AddPrimarySkills(bodyPrefab, primarySkillDef);
            #endregion

            #region SeedShotgun


            shotgunSkillDef = Skills.CreateSkillDef(new SkillDefInfo(
                prefix + "_HENRY_BODY_PRIMARY_SHOTGUN_NAME",
                prefix + "_HENRY_BODY_PRIMARY_SHOTGUN_DESCRIPTION",
                Assets.mainAssetBundle.LoadAsset<Sprite>("texPrimaryIcon"),
                new EntityStates.SerializableEntityStateType(typeof(SkillStates.Shotgun)),
                "Weapon",
                true));

            Skills.AddPrimarySkills(bodyPrefab, shotgunSkillDef);


            superShotgunSkillDef = Skills.CreateSkillDef(new SkillDefInfo(
                prefix + "_HENRY_BODY_PRIMARY_SUPER_SHOTGUN_NAME",
                prefix + "_HENRY_BODY_PRIMARY_SUPER_SHOTGUN_DESCRIPTION",
                Assets.mainAssetBundle.LoadAsset<Sprite>("texSpecialIcon"),
                new EntityStates.SerializableEntityStateType(typeof(SkillStates.Shotgun)),
                "Weapon",
                true));

            //Modules.Skills.AddPrimarySkills(bodyPrefab, superShotgunSkillDef);


            #endregion

            #region SpudLauncher

            cannonSkillDef = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = prefix + "_HENRY_BODY_PRIMARY_CANNON_NAME",
                skillNameToken = prefix + "_HENRY_BODY_PRIMARY_CANNON_NAME",
                skillDescriptionToken = prefix + "_HENRY_BODY_PRIMARY_CANNON_DESCRIPTION",
                skillIcon = Assets.mainAssetBundle.LoadAsset<Sprite>("texSpecialIcon"),
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
                skillName = prefix + "_HENRY_BODY_PRIMARY_SUPER_CANNON_NAME",
                skillNameToken = prefix + "_HENRY_BODY_PRIMARY_SUPER_CANNON_NAME",
                skillDescriptionToken = prefix + "_HENRY_BODY_PRIMARY_SUPER_CANNON_DESCRIPTION",
                skillIcon = Assets.mainAssetBundle.LoadAsset<Sprite>("texPrimaryIcon"),
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
            //Keep it for now just in case of bugs
            //Modules.Skills.AddPrimarySkills(bodyPrefab, superCannonSkillDef);



            #endregion

            #region PulseRifle

            pulseRifleSkillDef = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = prefix + "_HENRY_BODY_PRIMARY_PULSE_RIFLE_NAME",
                skillNameToken = prefix + "_HENRY_BODY_PRIMARY_PULSE_RIFLE_NAME",
                skillDescriptionToken = prefix + "_HENRY_BODY_PRIMARY_PULSE_RIFLE_DESCRIPTION",
                skillIcon = Assets.mainAssetBundle.LoadAsset<Sprite>("texSpecialIcon"),
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


            #endregion

        }

        private void InitializeSecondarySkills(string prefix)
        {

            #region Example Shoot
            //SkillDef shootSkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo
            //{
            //    skillName = prefix + "_HENRY_BODY_SECONDARY_GUN_NAME",
            //    skillNameToken = prefix + "_HENRY_BODY_SECONDARY_GUN_NAME",
            //    skillDescriptionToken = prefix + "_HENRY_BODY_SECONDARY_GUN_DESCRIPTION",
            //    skillIcon = Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("texSecondaryIcon"),
            //    activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.Shoot)),
            //    activationStateMachineName = "Slide",
            //    baseMaxStock = 1,
            //    baseRechargeInterval = 1f,
            //    beginSkillCooldownOnSkillEnd = false,
            //    canceledFromSprinting = false,
            //    forceSprintDuringState = false,
            //    fullRestockOnAssign = true,
            //    interruptPriority = EntityStates.InterruptPriority.Skill,
            //    resetCooldownTimerOnUse = false,
            //    isCombatSkill = true,
            //    mustKeyPress = false,
            //    cancelSprintingOnActivation = false,
            //    rechargeStock = 1,
            //    requiredStock = 1,
            //    stockToConsume = 1,
            //    keywordTokens = new string[] { "KEYWORD_AGILE" }
            //});
            //
            //Modules.Skills.AddSecondarySkills(bodyPrefab, shootSkillDef);
            #endregion

            #region Shovel Toss

            shovelSkillDef = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = prefix + "_HENRY_BODY_SECONDARY_SHOVEL_NAME",
                skillNameToken = prefix + "_HENRY_BODY_SECONDARY_SHOVEL_NAME",
                skillDescriptionToken = prefix + "_HENRY_BODY_SECONDARY_SHOVEL_DESCRIPTION",
                skillIcon = Assets.mainAssetBundle.LoadAsset<Sprite>("texSecondaryIcon"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.Shovel)),
                activationStateMachineName = "Slide",
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
                skillName = prefix + "_HENRY_BODY_SECONDARY_FORK_NAME",
                skillNameToken = prefix + "_HENRY_BODY_SECONDARY_FORK_NAME",
                skillDescriptionToken = prefix + "_HENRY_BODY_SECONDARY_FORK_DESCRIPTION",
                skillIcon = Assets.mainAssetBundle.LoadAsset<Sprite>("texSpecialIcon"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.Shovel)),
                activationStateMachineName = "Slide",
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

            Skills.AddSecondarySkills(bodyPrefab, pitchforkSkillDef);


            #endregion



            #region Reap

            reapSkillDef = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = prefix + "_HENRY_BODY_SECONDARY_REAP_NAME",
                skillNameToken = prefix + "_HENRY_BODY_SECONDARY_REAP_NAME",
                skillDescriptionToken = prefix + "_HENRY_BODY_SECONDARY_REAP_DESCRIPTION",
                skillIcon = Assets.mainAssetBundle.LoadAsset<Sprite>("texPrimaryIcon"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(Reap)),
                activationStateMachineName = "Weapon",
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

            razorWireGrenadeSkillDef = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = prefix + "_HENRY_BODY_SECONDARY_RAZOR_GRENADE_NAME",
                skillNameToken = prefix + "_HENRY_BODY_SECONDARY_RAZOR_GRENADE_NAME",
                skillDescriptionToken = prefix + "_HENRY_BODY_SECONDARY_RAZOR_GRENADE_DESCRIPTION",
                skillIcon = Assets.mainAssetBundle.LoadAsset<Sprite>("texSecondaryIcon"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.RazorGrenade)),
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

            Skills.AddSecondarySkills(bodyPrefab, razorWireGrenadeSkillDef);


            #endregion

        }

        private void InitializeUtilitySkills(string prefix)
        {

            #region roll
            //SkillDef rollSkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo
            //{
            //    skillName = prefix + "_HENRY_BODY_UTILITY_ROLL_NAME",
            //    skillNameToken = prefix + "_HENRY_BODY_UTILITY_ROLL_NAME",
            //    skillDescriptionToken = prefix + "_HENRY_BODY_UTILITY_ROLL_DESCRIPTION",
            //    skillIcon = Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("texUtilityIcon"),
            //    activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.Roll)),
            //    activationStateMachineName = "Body",
            //    baseMaxStock = 1,
            //    baseRechargeInterval = 4f,
            //    beginSkillCooldownOnSkillEnd = false,
            //    canceledFromSprinting = false,
            //    forceSprintDuringState = true,
            //    fullRestockOnAssign = true,
            //    interruptPriority = EntityStates.InterruptPriority.PrioritySkill,
            //    resetCooldownTimerOnUse = false,
            //    isCombatSkill = false,
            //    mustKeyPress = false,
            //    cancelSprintingOnActivation = false,
            //    rechargeStock = 1,
            //    requiredStock = 1,
            //    stockToConsume = 1
            //});

            //Modules.Skills.AddUtilitySkills(bodyPrefab, rollSkillDef);
            #endregion

            #region Bungal Grove



            groveSkillDef = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = prefix + "_HENRY_BODY_UTILITY_BUNGAL_GROVE_NAME",
                skillNameToken = prefix + "_HENRY_BODY_UTILITY_BUNGAL_GROVE_NAME",
                skillDescriptionToken = prefix + "_HENRY_BODY_UTILITY_BUNGAL_GROVE_DESCRIPTION",
                skillIcon = Assets.mainAssetBundle.LoadAsset<Sprite>("texUtilityIcon"),
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


            superGroveSkillDef = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = prefix + "_HENRY_BODY_UTILITY_LIGHTNING_GROVE_NAME",
                skillNameToken = prefix + "_HENRY_BODY_UTILITY_LIGHTNING_GROVE_NAME",
                skillDescriptionToken = prefix + "_HENRY_BODY_UTILITY_LIGHTNING_GROVE_DESCRIPTION",
                skillIcon = Assets.mainAssetBundle.LoadAsset<Sprite>("texPrimaryIcon"),
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

            //Modules.Skills.AddUtilitySkills(bodyPrefab, superGroveSkillDef);



            #endregion

            #region Mortar
            //need aim and fire states

            mortarSkillDef = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = prefix + "_HENRY_BODY_UTILITY_MORTAR_NAME",
                skillNameToken = prefix + "_HENRY_BODY_UTILITY_MORTAR_NAME",
                skillDescriptionToken = prefix + "_HENRY_BODY_UTILITY_MORTAR_DESCRIPTION",
                skillIcon = Assets.mainAssetBundle.LoadAsset<Sprite>("texSpecialIcon"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.AimMortarShell)),
                activationStateMachineName = "Body",
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


            Skills.AddUtilitySkills(bodyPrefab, mortarSkillDef);

            #endregion

        }


        private void InitializeSpecialSkills(string prefix)
        {

            #region Bomb
            //SkillDef bombSkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo
            //{
            //    skillName = prefix + "_HENRY_BODY_SPECIAL_BOMB_NAME",
            //    skillNameToken = prefix + "_HENRY_BODY_SPECIAL_BOMB_NAME",
            //    skillDescriptionToken = prefix + "_HENRY_BODY_SPECIAL_BOMB_DESCRIPTION",
            //    skillIcon = Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("texSpecialIcon"),
            //    activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.ThrowBomb)),
            //    activationStateMachineName = "Slide",
            //    baseMaxStock = 1,
            //    baseRechargeInterval = 10f,
            //    beginSkillCooldownOnSkillEnd = false,
            //    canceledFromSprinting = false,
            //    forceSprintDuringState = false,
            //    fullRestockOnAssign = true,
            //    interruptPriority = EntityStates.InterruptPriority.Skill,
            //    resetCooldownTimerOnUse = false,
            //    isCombatSkill = true,
            //    mustKeyPress = false,
            //    cancelSprintingOnActivation = true,
            //    rechargeStock = 1,
            //    requiredStock = 1,
            //    stockToConsume = 1
            //});

            //Modules.Skills.AddSpecialSkills(bodyPrefab, bombSkillDef);
            #endregion

            #region Fertilizer

            SkillDef fertilizerSkillDef = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = prefix + "_HENRY_BODY_SPECIAL_FERTILIZER_NAME",
                skillNameToken = prefix + "_HENRY_BODY_SPECIAL_FERTILIZER_NAME",
                skillDescriptionToken = prefix + "_HENRY_BODY_SPECIAL_FERTILIZER_DESCRIPTION",
                skillIcon = Assets.mainAssetBundle.LoadAsset<Sprite>("texUtilityIcon"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.Fertilizer)),
                activationStateMachineName = "Body",
                baseMaxStock = 1,
                baseRechargeInterval = 12f,
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
        }


        public override void InitializeSkins()
        {
            ModelSkinController skinController = prefabCharacterModel.gameObject.AddComponent<ModelSkinController>();
            ChildLocator childLocator = prefabCharacterModel.GetComponent<ChildLocator>();

            CharacterModel.RendererInfo[] defaultRendererinfos = prefabCharacterModel.baseRendererInfos;

            List<SkinDef> skins = new List<SkinDef>();

            #region DefaultSkin
            //this creates a SkinDef with all default fields
            SkinDef defaultSkin = Skins.CreateSkinDef(FARMER_PREFIX + "DEFAULT_SKIN_NAME",
                Assets.mainAssetBundle.LoadAsset<Sprite>("texMainSkin"),
                defaultRendererinfos,
                prefabCharacterModel.gameObject);

            //these are your Mesh Replacements. The order here is based on your CustomRendererInfos from earlier
            //pass in meshes as they are named in your assetbundle
            //defaultSkin.meshReplacements = Modules.Skins.getMeshReplacements(defaultRendererinfos,
            //    "meshHenrySword",
            //    "meshHenryGun",
            //    "meshHenry");

            //add new skindef to our list of skindefs. this is what we'll be passing to the SkinController
            skins.Add(defaultSkin);
            #endregion

            //uncomment this when you have a mastery skin
            #region MasterySkin
            /*
            //creating a new skindef as we did before
            SkinDef masterySkin = Modules.Skins.CreateSkinDef(HenryPlugin.DEVELOPER_PREFIX + "_HENRY_BODY_MASTERY_SKIN_NAME",
                Assets.mainAssetBundle.LoadAsset<Sprite>("texMasteryAchievement"),
                defaultRendererinfos,
                prefabCharacterModel.gameObject,
                masterySkinUnlockableDef);

            //adding the mesh replacements as above. 
            //if you don't want to replace the mesh (for example, you only want to replace the material), pass in null so the order is preserved
            masterySkin.meshReplacements = Modules.Skins.getMeshReplacements(defaultRendererinfos,
                "meshHenrySwordAlt",
                null,//no gun mesh replacement. use same gun mesh
                "meshHenryAlt");

            //masterySkin has a new set of RendererInfos (based on default rendererinfos)
            //you can simply access the RendererInfos defaultMaterials and set them to the new materials for your skin.
            masterySkin.rendererInfos[0].defaultMaterial = Modules.Materials.CreateHopooMaterial("matHenryAlt");
            masterySkin.rendererInfos[1].defaultMaterial = Modules.Materials.CreateHopooMaterial("matHenryAlt");
            masterySkin.rendererInfos[2].defaultMaterial = Modules.Materials.CreateHopooMaterial("matHenryAlt");

            //here's a barebones example of using gameobjectactivations that could probably be streamlined or rewritten entirely, truthfully, but it works
            masterySkin.gameObjectActivations = new SkinDef.GameObjectActivation[]
            {
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("GunModel"),
                    shouldActivate = false,
                }
            };
            //simply find an object on your child locator you want to activate/deactivate and set if you want to activate/deacitvate it with this skin

            skins.Add(masterySkin);
            */
            #endregion

            skinController.skins = skins.ToArray();
        }


        //Need a controller? Based off which generic skill is selected we want to 

        private void InitializeHooks()
        {
            On.RoR2.HealthComponent.Heal += HealthComponent_Heal;
            On.RoR2.CharacterBody.RecalculateStats += CharacterBody_RecalculateStats;

            On.RoR2.CharacterBody.OnLevelUp += CharacterBody_OnLevelUp;
        }

        private void CharacterBody_OnLevelUp(On.RoR2.CharacterBody.orig_OnLevelUp orig, CharacterBody self)
        {
            orig(self);

            string farmerBaseNameToken = FirstLightPlugin.DEVELOPER_PREFIX + "_HENRY_BODY_NAME";
            string farmerAltPassiveNameToken = FirstLightPlugin.DEVELOPER_PREFIX + "_HENRY_BODY_ALT_PASSIVE_NAME";
            SkillLocator skillLocator = self.GetComponent<SkillLocator>();

            //If also has SPRTN Passive, do this
            if (self.baseNameToken == farmerBaseNameToken && skillLocator.passiveSkill.skillNameToken == farmerAltPassiveNameToken)
            {
                skillLocator.passiveSkill.enabled = true;
            }
        }

        private float HealthComponent_Heal(On.RoR2.HealthComponent.orig_Heal orig, HealthComponent self, float amount, ProcChainMask procChainMask, bool nonRegen)
        {
            CharacterBody body = self.body.GetComponent<CharacterBody>();


            string farmerBaseNameToken = FirstLightPlugin.DEVELOPER_PREFIX + "_HENRY_BODY_NAME";



            if (amount > 0 && nonRegen && body.baseNameToken == farmerBaseNameToken && body.GetBuffCount(Buffs.farmerPassive) < 1f)
            {
                body.AddBuff(Buffs.farmerPassive);
                //body.AddTimedBuff(Buffs.farmerPassive, 1.3f);
            }

            return orig(self, amount, procChainMask, nonRegen);
        }

        private void CharacterBody_RecalculateStats(On.RoR2.CharacterBody.orig_RecalculateStats orig, CharacterBody self)
        {
            orig(self);

            string farmerBaseNameToken = FirstLightPlugin.DEVELOPER_PREFIX + "_HENRY_BODY_NAME";


            if (self)
            {

                if (self.baseNameToken == farmerBaseNameToken)
                {
                    if (self.HasBuff(Buffs.farmerPassive))
                    {
                        float attackSpeedIncrease = self.attackSpeed * Config.farmerPassiveAttackSpeedCoefficient.Value;
                        self.attackSpeed += attackSpeedIncrease;
                    }

                    if (self.GetComponent<SkillLocator>().passiveSkill.enabled)
                    {
                        self.armor += Config.farmerAltPassiveArmorPerLevel.Value;
                    }
                }
            }
        }
    }
}