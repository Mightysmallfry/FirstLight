using System;
using System.Collections.Generic;
using System.Text;
using BepInEx.Configuration;
using FirstLightMod.Modules.Items;
using IL.RoR2.Skills;
using R2API;
using RoR2;
using UnityEngine.AddressableAssets;
using UnityEngine;

namespace FirstLightMod.Items
{
    public class VolatileAugment : ItemBase<VolatileAugment>
    {
        public override string ItemName => "Volatile Augment"; 
        public override string ItemNameToken => "VOLATILE_AUGMENT";
        public override string ItemPickupDescription => "Violent deconstruction."; 
        public override string ItemFullDescription => $"Replaces your secondary attack with DEMON CORE";
        public override string ItemLore => "Oops.";
        public override ItemTier Tier => ItemTier.Lunar;

        public override ItemTag[] ItemTags => new ItemTag[]
        {
            ItemTag.WorldUnique,
            ItemTag.BrotherBlacklist,
            ItemTag.CannotSteal,
            ItemTag.CannotDuplicate
        };
        public override GameObject ItemModel => Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Mystery/PickupMystery.prefab").WaitForCompletion(); //Again tri-tip dagger
        public override Sprite ItemIcon => Addressables.LoadAssetAsync<Sprite>("RoR2/Base/Common/MiscIcons/texMysteryIcon.png").WaitForCompletion(); //Could use tri-tip dagger for now

        //public static GameObject ItemBodyModelPrefab;

        public GenericSkill DemonCoreSkill;

        public override void Init(ConfigFile config)
        {
            CreateConfig(config);
            CreateItemDisplayRules();
            CreateLang();
            CreateItem();
            Hooks();
        }

        public void CreateConfig(ConfigFile config)
        {
            //LOOK AT LunarSecondaryReplacementSkill under ROR2.Skills
            //Idea for the skill, fire off a single shot that gives a timed buff - jumps to non-buffed creatures (may change) as long as there is another creature within range.
            //Each of the attacks has proc ~0.5

        //    HealChanceInitial = config.Bind<float>(
        //        "Item: " + ItemName,
        //        "Initial Heal Chance",
        //        10f,
        //        "How much heal chance should the first item give?"
        //        ).Value;


        }


        public override ItemDisplayRuleDict CreateItemDisplayRules()
        {
            ItemBodyModelPrefab = ItemModel;
            var itemDisplay = ItemBodyModelPrefab.AddComponent<ItemDisplay>();
            itemDisplay.rendererInfos = ItemDisplaySetup(ItemBodyModelPrefab);

            ItemDisplayRuleDict rules = new ItemDisplayRuleDict(new RoR2.ItemDisplayRule[]
            {
                new RoR2.ItemDisplayRule{
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemBodyModelPrefab,
                    childName = "Chest",
                    localPos = new Vector3(0, 0, 0),
                    localAngles = new Vector3(0, 0, 0),
                    localScale = new Vector3(1, 1, 1)
                }
            });

            rules.Add("mdlHuntress", new RoR2.ItemDisplayRule[]
            {
                new RoR2.ItemDisplayRule
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = ItemBodyModelPrefab,
                    childName = "Chest",
                    localPos = new Vector3(0, 0, 0),
                    localAngles = new Vector3(0, 0, 0),
                    localScale = new Vector3(1, 1, 1)
                }
            });

            return rules;
        }

        public override void Hooks()
        {

        }


    }
}
