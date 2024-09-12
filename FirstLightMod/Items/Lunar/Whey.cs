using BepInEx.Configuration;
using FirstLightMod.Modules.Items;
using R2API;
using RoR2;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace FirstLightMod.Items
{
    public class Whey : ItemBase<Whey>
    {
        public override string ItemName => "Moondust Whey";
        public override string ItemNameToken => "WHEY";
        public override string ItemPickupDescription => $"Grow stronger, but <style=cDeath>skip leg day.</style>";
        public override string ItemFullDescription => $"Gain <style=cIsDamage>{DamageBoostMultiplier * 100}% base damage </style> but receive <style=cDeath>{MovementSpeedMultiplier * 100}% movement speed</style>.";
        public override string ItemLore => "";
        public override ItemTier Tier => ItemTier.Lunar;


        public override ItemTag[] ItemTags => new ItemTag[] { ItemTag.Damage };
        //Use Addressables not Resources as it is more up to date
        public override GameObject ItemModel => Addressables.LoadAssetAsync<GameObject>("RoR2/DLC1/ElementalRingVoid/PickupVoidRing.prefab").WaitForCompletion(); //use singularity band
        public override Sprite ItemIcon => Addressables.LoadAssetAsync<Sprite>("RoR2/DLC1/ElementalRingVoid/texVoidRingIcon.png").WaitForCompletion(); //use singularity band


        public float DamageBoostMultiplier; //increases damage
        public float MovementSpeedMultiplier; //slows the player



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
            DamageBoostMultiplier = config.Bind<float>(
                "Item: " + ItemName,
                "Damage Multiplier Per Item",
                2f,
                "How much is your damage multiplied by?").Value;

            MovementSpeedMultiplier = config.Bind<float>(
                "Item: " + ItemName,
                "Slow Multiplier Per Item",
                0.5f,
                "How much slower should you move for each item.").Value;


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
            On.RoR2.CharacterBody.RecalculateStats += CharacterBody_RecalculateState;
        }

        private void CharacterBody_RecalculateState(On.RoR2.CharacterBody.orig_RecalculateStats orig, RoR2.CharacterBody self)
        {
            orig(self);

            if (self && GetCount(self) > 0)
            {
                self.baseDamage *= DamageBoostMultiplier * GetCount(self);
                self.moveSpeed *= MovementSpeedMultiplier * (GetCount(self));
            }
        }
    }
}
