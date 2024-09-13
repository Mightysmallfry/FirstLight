using BepInEx.Configuration;
using FirstLightMod.Modules.Items;
using R2API;
using RoR2;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace FirstLightMod.Items
{
    public class BearTrap : ItemBase<BearTrap>
    {
        public override string ItemName => "Bear Trap";
        public override string ItemNameToken => "BEAR_TRAP";
        public override string ItemPickupDescription => "Gain increased damage against slowed and rooted enemies.";
        public override string ItemFullDescription => $"Deal an additional <style=cIsDamage>{DamageBonus}% damage</style> towards slowed and rooted enemies";
        public override string ItemLore => "";
        public override ItemTier Tier => ItemTier.Tier2;


        //Use Addressables not Resources as it is more up to date
        public override GameObject ItemModel => Addressables.LoadAssetAsync<GameObject>("RoR2/DLC1/CritDamage/PickupLaserSight.prefab").WaitForCompletion(); 
        public override Sprite ItemIcon => Addressables.LoadAssetAsync<Sprite>("RoR2/DLC1/CritDamage/texLaserSightIcon.png").WaitForCompletion();

        /// <summary>
        /// Percentage of which damage will be increased
        /// </summary>
        public float DamageBonus;

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

            DamageBonus = config.Bind<float>(
                "Item: " + ItemName,
                "Bonus Damage Percentage Gained",
                25f,
                "What is the percent of damage gained for having at least one copy of this item").Value;
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
            On.RoR2.HealthComponent.TakeDamage += HealthComponent_TakeDamage;
        }

        private void HealthComponent_TakeDamage(On.RoR2.HealthComponent.orig_TakeDamage orig, HealthComponent self, DamageInfo damageInfo)
        {

            if (damageInfo.attacker && damageInfo.attacker.TryGetComponent(out CharacterBody attackerBody) && attackerBody.inventory)
            {
                if (GetCount(attackerBody) > 0)
                {
                    if (attackerBody.HasBuff(RoR2Content.Buffs.Slow50) || attackerBody.HasBuff(RoR2Content.Buffs.Slow60) || attackerBody.HasBuff(RoR2Content.Buffs.Slow80) ||
                        attackerBody.HasBuff(RoR2Content.Buffs.Nullified) || attackerBody.HasBuff(RoR2Content.Buffs.LunarSecondaryRoot) || attackerBody.HasBuff(RoR2Content.Buffs.Entangle) ||
                        attackerBody.HasBuff(RoR2Content.Buffs.ClayGoo))
                    {
                        damageInfo.damage += (float)(1 + damageInfo.damage * ((DamageBonus/100f) * (float)GetCount(attackerBody)));
                    }
                }
            }

            orig(self, damageInfo);
        }


    }
}
