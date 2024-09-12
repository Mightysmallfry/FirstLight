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
    public class BloodSword : ItemBase<BloodSword>
    {
        public override string ItemName => "Chipped Longsword"; 
        public override string ItemNameToken => "BLOOD_BLADE";
        public override string ItemPickupDescription => "A tireless blade with an unending thirst."; 
        public override string ItemFullDescription => $"Upon damaging an enemy with bleed heal hit points equivalent to the amount of bleed inflicted.";
        public override string ItemLore => ".";
        public override ItemTier Tier => ItemTier.Tier2;

        //public override ItemTag[] ItemTags => new ItemTag[] { ItemTag.Damage }
        public override GameObject ItemModel => Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Mystery/PickupMystery.prefab").WaitForCompletion(); //Again tri-tip dagger
        public override Sprite ItemIcon => Addressables.LoadAssetAsync<Sprite>("RoR2/Base/Common/MiscIcons/texMysteryIcon.png").WaitForCompletion(); //Could use tri-tip dagger for now

        //public static GameObject ItemBodyModelPrefab;

        public float AmountHealPerBleedTick;

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
            AmountHealPerBleedTick = config.Bind<float>(
                "Item: " + ItemName,
                "Initial Heal Amount",
                1f,
                "How many hit points should this restore per stack of bleed?"
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

            if (damageInfo.attacker.TryGetComponent(out CharacterBody attackerBody) && attackerBody && GetCount(attackerBody) > 0) //If creature has items
            {
                //Something like this?
                if ((damageInfo.damageType & DamageType.BleedOnHit) != 0)
                {
                    attackerBody.healthComponent.Heal(attackerBody.GetBuffCount(RoR2Content.Buffs.Bleeding.buffIndex), damageInfo.procChainMask, true);
                }
                

            }

            orig(self, damageInfo, victim);
        }
    }
}
