using BepInEx.Configuration;
using FirstLightMod.Modules.Items;
using R2API;
using RoR2;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace FirstLightMod.Items
{
    public class BrokenBlade : ItemBase<BrokenBlade>
    {
        public override string ItemName => "Broken Blade";
        public override string ItemNameToken => "BROKEN_BLADE";
        public override string ItemPickupDescription => "Die fighting."; 
        public override string ItemFullDescription => $"Deal an additional <style=cIsDamage>{EliteDamageIncrease}% damage</style> to elite monsters";
        public override string ItemLore => "";
        public override ItemTier Tier => ItemTier.Tier1;

        public override ItemTag[] ItemTags => new ItemTag[] { ItemTag.Damage }; 
        public override GameObject ItemModel => Addressables.LoadAssetAsync<GameObject>("RoR2/Base/BleedOnHit/PickupTriTip.prefab").WaitForCompletion(); //Again tri-tip dagger
        public override Sprite ItemIcon => Addressables.LoadAssetAsync<Sprite>("RoR2/Base/BleedOnHit/texTriTipIcon.png").WaitForCompletion(); //Could use tri-tip dagger for now

        //public static GameObject ItemBodyModelPrefab;

        public float EliteDamageIncrease;


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
            //Remember to change damage buff back down to 10%
            EliteDamageIncrease = config.Bind<float>(
                "Item: " + ItemName,
                "Elite Damage Increase",
                10f,
                "What percentage should damage towards elite enemies be increased per additional item?"
                ).Value;

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
            On.RoR2.HealthComponent.TakeDamage += HealthComponent_TakeDamage;
        }

        private void HealthComponent_TakeDamage(On.RoR2.HealthComponent.orig_TakeDamage orig, HealthComponent self, DamageInfo damageInfo)
        {
            if (damageInfo.attacker.TryGetComponent(out CharacterBody attackerBody) && GetCount(attackerBody) > 0)
            {
                if (self.body.isChampion || self.body.isElite)
                {
                    damageInfo.damageColorIndex = DamageColorIndex.WeakPoint;
                    damageInfo.damage = damageInfo.damage * (1.0f + (EliteDamageIncrease / 100f) * (float)GetCount(attackerBody));
                }
            }

            orig(self, damageInfo);
        }

    }
}
