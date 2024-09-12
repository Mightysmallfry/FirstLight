using BepInEx.Configuration;
using FirstLightMod.Modules.Items;
using R2API;
using RoR2;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace FirstLightMod.Items
{
    public class Stiletto : ItemBase<Stiletto>
    {
        public override string ItemName => "Stiletto Blade"; //I Still like the stiletto dagger theme, blood stone is a hard thing to change it to
        public override string ItemNameToken => "STILETTO";
        public override string ItemPickupDescription => "Stab and twist."; //"Deal massively increased bleed damage"
        public override string ItemFullDescription => $"Gain {InitialBleedChance}% bleed chance, all bleed damage is also increased by <style=cIsDamage>{100f* InitialBleedPercentage}%</style>.";
        public override string ItemLore => "A finely made blade that retains its edge. Viscera beware.";
        public override ItemTier Tier => ItemTier.Tier3;

        public override ItemTag[] ItemTags => new ItemTag[] { ItemTag.Damage }; // I can uncomment this once I figure out what is happening with the load issue
        public override GameObject ItemModel => Addressables.LoadAssetAsync<GameObject>("RoR2/Base/BleedOnHit/PickupTriTip.prefab").WaitForCompletion(); //Again tri-tip dagger
        public override Sprite ItemIcon => Addressables.LoadAssetAsync<Sprite>("RoR2/Base/BleedOnHit/texTriTipIcon.png").WaitForCompletion(); //Could use tri-tip dagger for now

        //public static GameObject ItemBodyModelPrefab;

        public float InitialBleedChance;
        public float InitialBleedPercentage;
        public float AdditionalBleedPercentage;



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
            InitialBleedChance = config.Bind<float>(
                "Item: " + ItemName, 
                "Initial Bleed Chance",
                10f,
                "How much bleed chance should 1 item give?"
                ).Value;

            InitialBleedPercentage = config.Bind<float>(
                "Item: " + ItemName,
                "Bleed damage initial increase percentage", 
                1.0f,
                "How much damage should the bleed damage be initially increased?"
                ).Value;

            AdditionalBleedPercentage = config.Bind<float>(
                "Item: " + ItemName,
                "Bleed damage additional increase percentage",
                1.0f,
                "How much damage should the bleed damage be increased per additional item?"
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
            On.RoR2.CharacterBody.RecalculateStats += CharacterBody_RecalculateStats;
            On.RoR2.DotController.AddDot += DotController_AddDot;

        }


        private void CharacterBody_RecalculateStats(On.RoR2.CharacterBody.orig_RecalculateStats orig, CharacterBody self)
        {
            orig(self);

            if (GetCount(self) > 0)
            {
                self.bleedChance += InitialBleedChance;
            }
        }
        private void DotController_AddDot(On.RoR2.DotController.orig_AddDot orig, DotController self, GameObject attackerObject, float duration, DotController.DotIndex dotIndex, float damageMultiplier, uint? maxStacksFromAttacker, float? totalDamage, DotController.DotIndex? preUpgradeDotIndex)
        {
            var itemCount = GetCount(attackerObject.GetComponent<CharacterBody>());

            if (itemCount > 0 && dotIndex == DotController.DotIndex.Bleed)
            {
                damageMultiplier = 1f + InitialBleedPercentage + ((itemCount - 1) * AdditionalBleedPercentage);
            }

            orig(self, attackerObject, duration, dotIndex, damageMultiplier, maxStacksFromAttacker, totalDamage, preUpgradeDotIndex);
        }

    }
}
