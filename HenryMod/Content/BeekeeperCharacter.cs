using BepInEx.Configuration;
using FirstLightMod.Modules;
using FirstLightMod.Modules.Characters;
using FirstLightMod.Modules.Survivors;
using FirstLightMod.SkillStates.Beekeeper;
using RoR2;
using RoR2.Skills;
using System;
using System.Collections.Generic;
using UnityEngine;



namespace FirstLightMod.Content
{
    internal class BeekeeperCharacter : SurvivorBase
    {
        //used when building your character using the prefabs you set up in unity
        //don't upload to thunderstore without changing this
        public override string prefabBodyName => "Henry";

        public const string BEEKEEPER_PREFIX = FirstLightPlugin.DEVELOPER_PREFIX + "_HENRY_BODY_";

        //used when registering your survivor's language tokens
        public override string survivorTokenPrefix => BEEKEEPER_PREFIX;

        public override UnlockableDef characterUnlockableDef => FLUnlockables.farmerUnlockableDef;


        #region SkillDefs
        public static SkillDef assaultRifeDef;
        public static SkillDef targetJarDef;
        public static SkillDef honeyHealDef;
        public static SkillDef beeDef;
        public static SkillDef hornetDef;
        #endregion

        public override BodyInfo bodyInfo { get; set; } = new BodyInfo
        {
            bodyName = "HenryTutorialBody",
            bodyNameToken = BEEKEEPER_PREFIX + "NAME",
            subtitleNameToken = BEEKEEPER_PREFIX + "SUBTITLE",

            characterPortrait = Assets.mainAssetBundle.LoadAsset<Texture>("texHenryIcon"),
            bodyColor = Color.yellow,

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

        public override Type characterMainState => typeof(EntityStates.GenericCharacterMain);

        public override ItemDisplaysBase itemDisplays => new BeekeeperItemDisplays();

        //if you have more than one character, easily create a config to enable/disable them like this
        public override ConfigEntry<bool> characterEnabledConfig => null; //Modules.Config.CharacterEnableConfig(bodyName);

        public override void InitializeCharacter()
        {
            base.InitializeCharacter();
            Tokens.AddBeekeeper(BEEKEEPER_PREFIX);
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
        }

        private void InitializePassiveSkills(string prefix)
        {
            SkillDef suppressiveFirePassive = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillNameToken = BEEKEEPER_PREFIX + "PASSIVE_NAME",
                skillDescriptionToken = BEEKEEPER_PREFIX + "PASSIVE_DESCRIPTION",
                skillIcon = Assets.mainAssetBundle.LoadAsset<Sprite>("texPassiveIcon"),
                activationStateMachineName = "Body",
                isCombatSkill = false,
            });

            Skills.AddPassiveSkills(bodyPrefab, suppressiveFirePassive);
        }

        private void InitializePrimarySkills(string prefix)
        {
            assaultRifeDef = Skills.CreateSkillDef(new SkillDefInfo(
                prefix + "_HENRY_BODY_PRIMARY_RIFLE_NAME",
                prefix + "_HENRY_BODY_PRIMARY_RIFLE_DESCRIPTION",
                Assets.mainAssetBundle.LoadAsset<Sprite>("texUziIcon"),
                new EntityStates.SerializableEntityStateType(typeof(AssaultRifle)), //change to AssaultRifle skill state
                "Weapon",
                true));

            Skills.AddPrimarySkills(bodyPrefab, assaultRifeDef);
        }
        private void InitializeSecondarySkills(string prefix)
        {
            targetJarDef = Skills.CreateSkillDef(new SkillDefInfo(
                prefix + "_HENRY_BODY_SECONDARY_TARGET_JAR_NAME",
                prefix + "_HENRY_BODY_SECONDARY_TARGET_JAR_DESCRIPTION",
                Assets.mainAssetBundle.LoadAsset<Sprite>("texBazookaFireIcon"),
                new EntityStates.SerializableEntityStateType(typeof(TargetJar)), //change to AssaultRifle skill state
                "Weapon",
                true));

            Skills.AddSecondarySkills(bodyPrefab, targetJarDef);
        }
        private void InitializeUtilitySkills(string prefix)
        {
            honeyHealDef = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = prefix + "_HENRY_BODY_UTILITY_HEAL_NAME",
                skillNameToken = prefix + "_HENRY_BODY_UTILITY_HEAL_NAME",
                skillDescriptionToken = prefix + "_HENRY_BODY_UTILITY_HEAL_DESCRIPTION",
                skillIcon = Assets.mainAssetBundle.LoadAsset<Sprite>("texSpecialIcon"),
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
        private void InitializeSpecialSkills(string prefix)
        {
            beeDef = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = prefix + "_HENRY_BODY_SPECIAL_BEE_NAME",
                skillNameToken = prefix + "_HENRY_BODY_SPECIAL_BEE_NAME",
                skillDescriptionToken = prefix + "_HENRY_BODY_SPECIAL_BEE_DESCRIPTION",
                skillIcon = Assets.mainAssetBundle.LoadAsset<Sprite>("texBazookaIconScepter"),
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

            hornetDef = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = prefix + "_HENRY_BODY_SPECIAL_HORNET_NAME",
                skillNameToken = prefix + "_HENRY_BODY_SPECIAL_HORNET_NAME",
                skillDescriptionToken = prefix + "_HENRY_BODY_SPECIAL_HORNET_DESCRIPTION",
                skillIcon = Assets.mainAssetBundle.LoadAsset<Sprite>("texBazookaIconScepter"),
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
            SkinDef defaultSkin = Skins.CreateSkinDef(BEEKEEPER_PREFIX + "DEFAULT_SKIN_NAME",
                Assets.mainAssetBundle.LoadAsset<Sprite>("texMainSkin"),
                defaultRendererinfos,
                prefabCharacterModel.gameObject);

            //these are your Mesh Replacements. The order here is based on your CustomRendererInfos from earlier
            //pass in meshes as they are named in your assetbundle
            defaultSkin.meshReplacements = Skins.getMeshReplacements(defaultRendererinfos,
                "meshHenrySword",
                "meshUzi",
                "meshHenry");

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

    }
}
