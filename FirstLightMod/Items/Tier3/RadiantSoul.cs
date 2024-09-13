using BepInEx.Configuration;
using FirstLightMod.Modules.Items;
using R2API;
using RoR2;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace FirstLightMod.Items
{
    public class RadiantSoul : ItemBase<RadiantSoul>
    {
        public override string ItemName => "Radiant Soul";
        public override string ItemNameToken => "RADIANT_SOUL";
        public override string ItemPickupDescription => "Bring to heel the lords of this world.";
        public override string ItemFullDescription => $"Upon defeating a boss, recruit them into your service. Up to a maximum of {maxNumberOfBossCompanions} bosses may be recruited at any time.";
        public override string ItemLore => "";
        public override ItemTier Tier => ItemTier.Tier3;


        public override ItemTag[] ItemTags => new ItemTag[] { ItemTag.Damage };
        //Use Addressables not Resources as it is more up to date
        public override GameObject ItemModel => Addressables.LoadAssetAsync<GameObject>("RoR2/DLC1/ElementalRingVoid/PickupVoidRing.prefab").WaitForCompletion(); //use singularity band
        public override Sprite ItemIcon => Addressables.LoadAssetAsync<Sprite>("RoR2/DLC1/ElementalRingVoid/texVoidRingIcon.png").WaitForCompletion(); //use singularity band


        public float bossCompanionSizeMultiplier;
        public int maxNumberOfBossCompanions;
        public int additionalBossCompanions;

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
            bossCompanionSizeMultiplier = config.Bind<float>(
                "Item: " + ItemName,
                "Boss Companion Conversion Factor",
                .10f,
                "What should the boss companions size and damage output be relative to when the player fights them?").Value;


            maxNumberOfBossCompanions = config.Bind<int>(
                "Item: " + ItemName,
                "Maximum Number of Boss Companions",
                5,
                "What is the maximum number of boss companions gained when in possession of one item?").Value;

            additionalBossCompanions = config.Bind<int>(
                "Item: " + ItemName,
                "Additional Companions",
                2,
                "How many additional bosses should appear per additional item.?").Value;


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
                if (appliedProcChance < maxNumberOfBossCompanions)
                {
                    appliedProcChance += bossCompanionSizeMultiplier;
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
