using BepInEx.Configuration;
using FirstLightMod.Modules.Items;
using R2API;
using RoR2;
using static FirstLightMod.Modules.Helpers;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace FirstLightMod.Content.Items
{
    public class KnuckleDusters : ItemBase<KnuckleDusters>
    {
        public override string ItemName => "Knuckle Dusters";
        public override string ItemNameToken => "KNUCKLE_DUSTERS";
        public override string ItemPickupDescription => "Gain increased crit damage for with each knuckle.";
        public override string ItemFullDescription => $"Gain <style=cIsDamage>{CritDamageGainPercentage}% Critical Damage</style> with each knuckle duster acquired.";
        public override string ItemLore => "";
        public override ItemTier Tier => ItemTier.Tier1;


        //Use Addressables not Resources as it is more up to date
        public override GameObject ItemModel => Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Thorns/PickupRazorwire.prefab").WaitForCompletion(); 
        public override Sprite ItemIcon => Addressables.LoadAssetAsync<Sprite>("RoR2/Base/Thorns/texRazorwireIcon.png").WaitForCompletion(); 



        public float CritDamageGainPercentage; //Percentage of Crit Damage gained for having the item

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
            CritDamageGainPercentage = config.Bind<float>(
                "Item: " + ItemName,
                "Critical Strike Damage Gained",
                .10f,
                "What is percentage of damage that your critical damage increases by having one of these items?").Value;

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
            RecalculateStatsAPI.GetStatCoefficients += RecalculateStatsAPI_GetStatCoefficients;
        }

        private void RecalculateStatsAPI_GetStatCoefficients(CharacterBody sender, RecalculateStatsAPI.StatHookEventArgs args)
        {

            var itemCount = GetCount(sender);
            if (itemCount > 0)
            {
                args.critDamageMultAdd += itemCount * CritDamageGainPercentage;
            }
        }


    }
}
