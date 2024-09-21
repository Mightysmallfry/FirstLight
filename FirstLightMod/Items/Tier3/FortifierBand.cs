using BepInEx.Configuration;
using FirstLightMod.Modules.Items;
using R2API;
using RoR2;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace FirstLightMod.Items
{
    public class FortifierBand : ItemBase<FortifierBand>
    {
        public override string ItemName => "Fortifier Band";
        public override string ItemNameToken => "FORTIFIER_BAND";
        public override string ItemPickupDescription => "Make real the will of others.";
        public override string ItemFullDescription => $"Gain <style=cShrine>{ArmorFlatGain} armor</style>. Gain an additional <style=cShrine>{ArmorPerBand} armor</style> for each non {ItemName} item in your inventory.";
        public override string ItemLore => "";
        public override ItemTier Tier => ItemTier.Tier3;


        public override ItemTag[] ItemTags => new ItemTag[] { ItemTag.Utility };
        //Use Addressables not Resources as it is more up to date
        public override GameObject ItemModel => Addressables.LoadAssetAsync<GameObject>("RoR2/DLC1/ElementalRingVoid/PickupVoidRing.prefab").WaitForCompletion(); //use singularity band
        public override Sprite ItemIcon => Addressables.LoadAssetAsync<Sprite>("RoR2/DLC1/ElementalRingVoid/texVoidRingIcon.png").WaitForCompletion(); //use singularity band


        //More non-Fortifier bands = more armor
        //More Fortifier bands = more armor per other band.
        public float ArmorFlatGain; //amount of armor gained for having the item
        public float ArmorPerBand; //increases armor for non Fortifier bands
        public float ArmorPerCopy; //amount of armor gained per band due to number of fortifier's bands



        public static BuffDef fortifiedBuff;


        public override void Init(ConfigFile config)
        {
            CreateConfig(config);
            CreateItemDisplayRules();
            CreateLang();
            CreateBuffs();
            CreateItem();
            Hooks();
        }
        
        private void CreateConfig(ConfigFile config)
        {
            ArmorFlatGain = config.Bind<float>(
                "Item: " + ItemName,
                "Armor Gained",
                25f,
                "What is the armor gained for having at least one copy of this item").Value;

            ArmorPerBand = config.Bind<float>(
                "Item: " + ItemName,
                "Armor Per Band",
                75f,
                "What is the additional armor gained per other Band?").Value;

            ArmorPerCopy = config.Bind<float>(
                "Item: " + ItemName,
                "Armor Per Copy",
                25f,
                "How much is the increase in armor for getting other bands, increased by having additional copies of this item.").Value;

        }

        private void CreateBuffs()
        {
            fortifiedBuff = Modules.Content.CreateAndAddBuff("Fortified",
                LegacyResourcesAPI.Load<BuffDef>("BuffDefs/HiddenInvincibility").iconSprite,
                new Color(20f, 13f, 149f),
                true,
                false);

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

            if (GetCount(self) > 0)
            {
                self.armor += ArmorFlatGain;

                float armorFromBands = GetCountBands(self) * (ArmorPerBand + (ArmorPerCopy * (GetCount(self) - 1)));
                self.armor += armorFromBands;

                if (self.GetBuffCount(fortifiedBuff) < armorFromBands + ArmorFlatGain)
                {
                    self.AddBuff(fortifiedBuff);
                }
                
            }
        }


        private int GetCountBands(RoR2.CharacterBody self)
        {
            int bandCount = 0;

            // FireRing - Kjaro's Band
            // IceRing - Runald's Band
            // ElementalRingVoid - Singularity Band
            // I need their ItemDefs.

            if (self.inventory.GetItemCount(RoR2Content.Items.FireRing.itemIndex) > 0)
            {
                bandCount += self.inventory.GetItemCount(RoR2Content.Items.FireRing.itemIndex);
            }
            if (self.inventory.GetItemCount(RoR2Content.Items.IceRing.itemIndex) > 0)
            {
                bandCount += self.inventory.GetItemCount(RoR2Content.Items.IceRing.itemIndex);
            }
            if (self.inventory.GetItemCount(DLC1Content.Items.ElementalRingVoid.itemIndex) > 0)
            {
                bandCount += self.inventory.GetItemCount(DLC1Content.Items.ElementalRingVoid.itemIndex);
            }

            return bandCount;
        }
    }
}
