using BepInEx.Configuration;
using FirstLightMod.Modules.Items;
using R2API;
using RoR2;
using RoR2.Projectile;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace FirstLightMod.Items
{
    public class Matchbox : ItemBase<Matchbox>
    {
        public override string ItemName => "Matchbox";
        public override string ItemNameToken => "MATCHBOX";
        public override string ItemPickupDescription => "Strike anywhere."; 
        public override string ItemFullDescription => $"Gain a {IgnitePercentChance}% chance to ignite targets on hit";
        public override string ItemLore => "";
        public override ItemTier Tier => ItemTier.Tier1;

        public override ItemTag[] ItemTags => new ItemTag[] { ItemTag.Damage }; 
        public override GameObject ItemModel => Addressables.LoadAssetAsync<GameObject>("RoR2/Base/BleedOnHit/PickupTriTip.prefab").WaitForCompletion(); //Again tri-tip dagger
        public override Sprite ItemIcon => Addressables.LoadAssetAsync<Sprite>("RoR2/Base/BleedOnHit/texTriTipIcon.png").WaitForCompletion(); //Could use tri-tip dagger for now

        //public static GameObject ItemBodyModelPrefab;

        public float IgnitePercentChance;


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
            IgnitePercentChance = config.Bind<float>(
                "Item: " + ItemName,
                "Elite Damage Increase",
                10f,
                "What percentage of your attacks will ignite the target?"
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

            if (victim && damageInfo.attacker.TryGetComponent(out CharacterBody attackerBody) && attackerBody.master && attackerBody.master.inventory)
            {
                if (GetCount(attackerBody.master) > 0 && Util.CheckRoll((IgnitePercentChance * GetCount(attackerBody.master)), attackerBody.master))
                {
                    InflictDotInfo inflictDotInfo = new InflictDotInfo
                    {
                        attackerObject = damageInfo.attacker,
                        victimObject = victim,
                        totalDamage = new float?(damageInfo.damage / 2f),
                        damageMultiplier = 1f,
                        dotIndex = DotController.DotIndex.Burn
                    };
                    StrengthenBurnUtils.CheckDotForUpgrade(attackerBody.master.inventory, ref inflictDotInfo);
                    DotController.InflictDot(ref inflictDotInfo);
                }
            }

            orig(self, damageInfo, victim);

        }
    }
}
