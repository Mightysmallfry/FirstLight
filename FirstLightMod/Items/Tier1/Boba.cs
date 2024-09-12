using BepInEx.Configuration;
using FirstLightMod.Modules.Items;
using R2API;
using RoR2;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace FirstLightMod.Items
{
    public class Boba : ItemBase<Boba>
    {
        public override string ItemName => "Sunlight Strawberry Milk Tea";
        public override string ItemNameToken => "BOBA";
        public override string ItemPickupDescription => "A sweet strawberry delight."; 
        public override string ItemFullDescription => $"Increases all of your base stats by {StatIncreasePercentage}%.";
        public override string ItemLore => "";
        public override ItemTier Tier => ItemTier.Tier1;

        public override ItemTag[] ItemTags => new ItemTag[] { ItemTag.Damage }; 
        public override GameObject ItemModel => Addressables.LoadAssetAsync<GameObject>("RoR2/Base/BleedOnHit/PickupTriTip.prefab").WaitForCompletion(); //Again tri-tip dagger
        public override Sprite ItemIcon => Addressables.LoadAssetAsync<Sprite>("RoR2/Base/BleedOnHit/texTriTipIcon.png").WaitForCompletion(); //Could use tri-tip dagger for now

        //public static GameObject ItemBodyModelPrefab;

        public float StatIncreasePercentage;


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
            StatIncreasePercentage = config.Bind<float>(
                "Item: " + ItemName,
                "Stat Increase Percentage",
                1f,
                "What percentage should all of your base stats be increased per item?"
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
        }

        private void CharacterBody_RecalculateStats(On.RoR2.CharacterBody.orig_RecalculateStats orig, CharacterBody self)
        {
            orig(self);

            if (self && self.inventory && GetCount(self) > 0)
            {
                self.baseMaxHealth += self.baseMaxHealth * ((StatIncreasePercentage/100f) * (float)GetCount(self));
                self.baseArmor += self.baseArmor * ((StatIncreasePercentage / 100f) * (float)GetCount(self));
                self.baseCrit += self.baseCrit * ((StatIncreasePercentage / 100f) * (float)GetCount(self));
                self.baseAttackSpeed += self.baseAttackSpeed * ((StatIncreasePercentage / 100f) * (float)GetCount(self));
                self.baseMoveSpeed += self.baseMoveSpeed * ((StatIncreasePercentage / 100f) * (float)GetCount(self));
                self.baseRegen += self.baseRegen * ((StatIncreasePercentage / 100f) * (float)GetCount(self));
                self.baseDamage += self.baseDamage * ((StatIncreasePercentage / 100f) * (float)GetCount(self));
            }
        }



    }
}
