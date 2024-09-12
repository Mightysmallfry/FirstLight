using BepInEx.Configuration;
using FirstLightMod.Modules.Items;
using R2API;
using RoR2;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace FirstLightMod.Items
{
    public class Antimatter : ItemBase<Antimatter>
    {
        public override string ItemName => "Contained Antimatter";
        public override string ItemNameToken => "ANTIMATTER";
        public override string ItemPickupDescription => "Punch through and annihilate.";
        public override string ItemFullDescription => $"Have a {TrueDamageProcChance}% chance to deal true damage.";
        public override string ItemLore => "";
        public override ItemTier Tier => ItemTier.Tier3;


        public override ItemTag[] ItemTags => new ItemTag[] { ItemTag.Damage };
        //Use Addressables not Resources as it is more up to date
        public override GameObject ItemModel => Addressables.LoadAssetAsync<GameObject>("RoR2/DLC1/ElementalRingVoid/PickupVoidRing.prefab").WaitForCompletion(); //use singularity band
        public override Sprite ItemIcon => Addressables.LoadAssetAsync<Sprite>("RoR2/DLC1/ElementalRingVoid/texVoidRingIcon.png").WaitForCompletion(); //use singularity band


        public float TrueDamageProcChance;
        //public float TrueDamagePercentage;


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
            TrueDamageProcChance = config.Bind<float>(
                "Item: " + ItemName,
                "True Damage Proc Chance",
                10f,
                "What percent chance should each item give to deal true damage?").Value;


            //TrueDamagePercentage = config.Bind<float>(
            //    "Item: " + ItemName,
            //    "True Damage Percentage",
            //    25f,
            //    "What percentage of your damage deal should be true damage?").Value;


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

            if (self && damageInfo.attacker.TryGetComponent(out CharacterBody attackerBody) && attackerBody.inventory)
            {
                if (GetCount(attackerBody) > 0 && Util.CheckRoll((TrueDamageProcChance * GetCount(attackerBody)), attackerBody.master))
                {
                    damageInfo.damageType = damageInfo.damageType & DamageType.BypassArmor;
                }
            }

            orig(self, damageInfo);
        }
    }
}
