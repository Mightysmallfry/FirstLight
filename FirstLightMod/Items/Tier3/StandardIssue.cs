using BepInEx.Configuration;
using FirstLightMod.Modules.Items;
using R2API;
using RoR2;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace FirstLightMod.Items
{
    public class StandardIssue : ItemBase<StandardIssue>
    {
        public override string ItemName => "Standard Issue";
        public override string ItemNameToken => "STANDARD_ISSUE";
        public override string ItemPickupDescription => "Bring to heel the lords of this world.";
        public override string ItemFullDescription => $"Gain an additional {procChancePerBossKilled}% proc chance on all abilities upon defeating a boss, up to a maximum of {maxProcChance}%.";
        public override string ItemLore => "";
        public override ItemTier Tier => ItemTier.Tier3;


        public override ItemTag[] ItemTags => new ItemTag[] { ItemTag.Damage };
        //Use Addressables not Resources as it is more up to date
        public override GameObject ItemModel => Addressables.LoadAssetAsync<GameObject>("RoR2/DLC1/ElementalRingVoid/PickupVoidRing.prefab").WaitForCompletion(); //use singularity band
        public override Sprite ItemIcon => Addressables.LoadAssetAsync<Sprite>("RoR2/DLC1/ElementalRingVoid/texVoidRingIcon.png").WaitForCompletion(); //use singularity band


        public float procChancePerBossKilled;
        public float maxProcChance;
        public float additionalMaxProcChance;

        public float appliedProcChance = 0f;


        public override void Init(ConfigFile config)
        {
            CreateConfig(config);
            CreateItemDisplayRules();
            CreateLang();
            CreateItem();
            Hooks();
        }
        
        private void CreateConfig(ConfigFile config)
        {
            procChancePerBossKilled = config.Bind<float>(
                "Item: " + ItemName,
                "Proc Chance Per Boss Killed",
                5f,
                "How much proc chance should killing a boss give?").Value;


            maxProcChance = config.Bind<float>(
                "Item: " + ItemName,
                "maxProcChancePerItem",
                25f,
                "What is the maximum proc chance one item may give?").Value;

            additionalMaxProcChance = config.Bind<float>(
                "Item: " + ItemName,
                "additionalMaxProcChance",
                10f,
                "How much should additional copies of this item increase the maximum given proc chance?").Value;


        }



        public override ItemDisplayRuleDict CreateItemDisplayRules()
        {
            //ItemBodyModelPrefab = Modules.Assets.mainAssetBundle.LoadAsset<GameObject>(ItemModelPath);
            ItemBodyModelPrefab = ItemModel;
            var itemDisplay = ItemBodyModelPrefab.AddComponent<ItemDisplay>();
            itemDisplay.rendererInfos = ItemDisplaySetup(ItemBodyModelPrefab);

            ItemDisplayRuleDict rules = new ItemDisplayRuleDict(new RoR2.ItemDisplayRule[]
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
            On.RoR2.GlobalEventManager.OnCharacterDeath += GlobalEventManager_OnCharacterDeath;
            On.RoR2.CharacterBody.RecalculateStats += CharacterBody_RecalculateStats;
        }

        private void GlobalEventManager_OnCharacterDeath(On.RoR2.GlobalEventManager.orig_OnCharacterDeath orig, GlobalEventManager self, DamageReport damageReport)
        {
            if (damageReport.victimBody && damageReport.victimBody.isBoss && damageReport.attackerBody && damageReport.attackerBody.inventory && GetCount(damageReport.attackerBody) > 0)
            {
                if (appliedProcChance < maxProcChance)
                {
                    appliedProcChance += procChancePerBossKilled;
                }
            }
        }

        private void CharacterBody_RecalculateStats(On.RoR2.CharacterBody.orig_RecalculateStats orig, CharacterBody self)
        {

            if (self && GetCount(self) > 0)
            {
                self.maxHealth = self.maxHealth * (appliedProcChance/100f);
            }


            orig(self);
        }
    }
}
