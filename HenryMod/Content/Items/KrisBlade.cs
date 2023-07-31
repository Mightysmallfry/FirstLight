using BepInEx.Configuration;
using FirstLightMod.Modules.Items;
using R2API;
using RoR2;
using UnityEngine;
using static FirstLightMod.FirstLightPlugin;
using static FirstLightMod.Modules.Items.ItemHelper;
using UnityEngine.Networking;


namespace FirstLightMod.Content.Items
{
    public class KrisBlade : ItemBase
    {
        public override string ItemName => "Kris Blade";
        public override string ItemNameToken => "KRIS_BLADE";
        public override string ItemPickupDescription => "Deal massively increased bleed damage";
        public override string ItemFullDescription => $"Gain {initialBleedChance}% bleed chance, all bleed damage is also increased by <style=cIsDamage>{100f* InitialBleedPercentage}</style>, this percentage is increased by <style=cStack>(+{100f * AdditionalBleedPercentage}) per additional item.";
        public override string ItemLore => "A finely made blade that retains its edge. Viscera beware.";
        public override ItemTier Tier => ItemTier.Tier2;

        //public override ItemTag[] ItemTags => new ItemTag[] { ItemTag.Damage }; // I can uncomment this once I figure out what is happening with the load issue
        public override GameObject ItemModel => Resources.Load<GameObject>("Prefabs/PickupModels/PickupMystery"); //Again tri-tip dagger
        public override Sprite ItemIcon => Resources.Load<Sprite>("Textures/MiscIcons/texMysteryIcon"); //Could use tri-tip dagger for now

        public static GameObject ItemBodyModelPrefab;

        public float initialBleedChance;
        public float InitialBleedPercentage;
        public float AdditionalBleedPercentage;



        public override void Init(ConfigFile config)
        {
            CreateConfig(config);
            CreateLang();
            CreateItem();
            Hooks();
        }

        public void CreateConfig(ConfigFile config)
        {
            initialBleedChance = config.Bind<float>(
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
            //ItemBodyModelPrefab = ItemModel;
            //var itemDisplay = ItemBodyModelPrefab.AddComponent<ItemDisplay>();
            //itemDisplay.rendererInfos = ItemDisplaySetup(ItemBodyModelPrefab);

            //ItemDisplayRuleDict rules = new ItemDisplayRuleDict(new RoR2.ItemDisplayRule[]
            //{
            //    new RoR2.ItemDisplayRule{
            //        ruleType = ItemDisplayRuleType.ParentedPrefab,
            //        followerPrefab = ItemBodyModelPrefab,
            //        childName = "Chest",
            //        localPos = new Vector3(0, 0, 0),
            //        localAngles = new Vector3(0, 0, 0),
            //        localScale = new Vector3(1, 1, 1)
            //    }
            //});

            //rules.Add("mdlHuntress", new RoR2.ItemDisplayRule[]
            //{
            //    new RoR2.ItemDisplayRule
            //    {
            //        ruleType = ItemDisplayRuleType.ParentedPrefab,
            //        followerPrefab = ItemBodyModelPrefab,
            //        childName = "Chest",
            //        localPos = new Vector3(0, 0, 0),
            //        localAngles = new Vector3(0, 0, 0),
            //        localScale = new Vector3(1, 1, 1)
            //    }
            //});

            //return rules;

            return new ItemDisplayRuleDict();
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
                self.bleedChance += initialBleedChance;
            }
        }


        private void DotController_AddDot(On.RoR2.DotController.orig_AddDot orig, DotController self, GameObject attackerObject, float duration, DotController.DotIndex dotIndex, float damageMultiplier, uint? maxStacksFromAttacker, float? totalDamage)
        {
            var itemCount = GetCount(attackerObject.GetComponent<CharacterBody>());

            if (itemCount > 0 && dotIndex == DotController.DotIndex.Bleed)
            {
                damageMultiplier = 1f + InitialBleedPercentage + ((itemCount - 1) * AdditionalBleedPercentage);
            }

            orig(self, attackerObject, duration, dotIndex, damageMultiplier, maxStacksFromAttacker, totalDamage);
        }
    }
}
