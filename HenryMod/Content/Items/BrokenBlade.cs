using BepInEx.Configuration;
using FirstLightMod.Modules.Items;
using R2API;
using RoR2;
using UnityEngine;
using static FirstLightMod.FirstLightPlugin;
using static FirstLightMod.Modules.Helpers;
using UnityEngine.Networking;
using System;
using UnityEngine.AddressableAssets;

namespace FirstLightMod.Content.Items
{
    public class BrokenBlade : ItemBase<BrokenBlade>
    {
        public override string ItemName => "Broken Blade";
        public override string ItemNameToken => "BROKEN_BLADE";
        public override string ItemPickupDescription => "Deal extra damage to elite monsters."; 
        public override string ItemFullDescription => $"Deal an additional <style=cIsDamage>{EliteDamageIncrease * 100}%</style> damage to elite monsters";
        public override string ItemLore => "";
        public override ItemTier Tier => ItemTier.Tier1;

        //public override ItemTag[] ItemTags => new ItemTag[] { ItemTag.Damage }; // I can uncomment this once I figure out what is happening with the load issue
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

            EliteDamageIncrease = config.Bind<float>(
                "Item: " + ItemName,
                "Elite Damage Increase",
                .10f,
                "How much damage should damage towards elite enemies be increased per additional item?"
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
            On.RoR2.GlobalEventManager.OnHitEnemy += GlobalEventManager_OnHitEnemy;
        }

        private void GlobalEventManager_OnHitEnemy(On.RoR2.GlobalEventManager.orig_OnHitEnemy orig, GlobalEventManager self, DamageInfo damageInfo, GameObject victim)
        {
            orig(self, damageInfo, victim);

            CharacterBody attackerBody = damageInfo.attacker.GetComponent<CharacterBody>();

            if (damageInfo.attacker)
            {
                var count = GetCount(attackerBody);

                if (count > 0 && victim.GetComponent<CharacterBody>().isElite && !damageInfo.rejected)
                {
                    damageInfo.damage *= (float)(1 + (damageInfo.damage * (EliteDamageIncrease * count)));
                }

            }
        }

    }
}
