using System;
using System.Collections.Generic;
using System.Text;
using BepInEx.Configuration;
using FirstLightMod.Modules.Items;
using R2API;
using RoR2;
using UnityEngine.AddressableAssets;
using UnityEngine;

namespace FirstLightMod.Items
{
    public class FryingPan : ItemBase<FryingPan>
    {
        public override string ItemName => "Cast Iron Frying Pan"; 
        public override string ItemNameToken => "FRYING_PAN";
        public override string ItemPickupDescription => "Bonk."; 
        public override string ItemFullDescription => $"Gain {stunProcChance}% chance to stun, on hitting a stunned target, trigger active enemy debuffs {additionalDebuffStacks} times.";
        public override string ItemLore => ".";
        public override ItemTier Tier => ItemTier.Tier2;

        //public override ItemTag[] ItemTags => new ItemTag[] { ItemTag.Damage }
        public override GameObject ItemModel => Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Mystery/PickupMystery.prefab").WaitForCompletion(); //Again tri-tip dagger
        public override Sprite ItemIcon => Addressables.LoadAssetAsync<Sprite>("RoR2/Base/Common/MiscIcons/texMysteryIcon.png").WaitForCompletion(); //Could use tri-tip dagger for now

        //public static GameObject ItemBodyModelPrefab;

        public float stunProcChance;
        public float additionalDebuffStacks;

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

            stunProcChance = config.Bind<float>(
                "Item: " + ItemName,
                "stunProcChance",
                5f,
                "What percentage of your shots should stun on hit?"
            ).Value;


            additionalDebuffStacks = config.Bind<float>(
                "Item: " + ItemName,
                "additionalDebuffStacks",
                1f,
                "How many additional stacks of debuffs should this item apply per hit?"
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
            if (damageInfo.attacker.TryGetComponent(out CharacterBody attackerBody) && GetCount(attackerBody) > 0 && damageInfo.damageType == DamageType.Stun1s)
            {
                foreach (BuffIndex buff in self.body.activeBuffsList)
                {
                    if (BuffCatalog.GetBuffDef(buff).isDebuff && BuffCatalog.GetBuffDef(buff).canStack)
                    {
                        self.body.AddBuff(buff);
                    }
                }
            }

            orig(self, damageInfo);
        }

    }
}
