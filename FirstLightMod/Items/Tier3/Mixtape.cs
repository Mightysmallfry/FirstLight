using BepInEx.Configuration;
using FirstLightMod.Modules.Items;
using R2API;
using RoR2;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace FirstLightMod.Items
{
    public class Mixtape : ItemBase<Mixtape>
    {
        public override string ItemName => "Mixtape"; 
        public override string ItemNameToken => "MIXTAPE";
        public override string ItemPickupDescription => "and his words were fire."; //"Deal massively increased bleed damage"
        public override string ItemFullDescription => $"Enemies that are burning can now be executed at low health, burning enemies have a chance to spread fire to nearby enemies.";
        public override string ItemLore => ".";
        public override ItemTier Tier => ItemTier.Tier3;

        public override ItemTag[] ItemTags => new ItemTag[] { ItemTag.Damage }; // I can uncomment this once I figure out what is happening with the load issue
        public override GameObject ItemModel => Addressables.LoadAssetAsync<GameObject>("RoR2/Base/BleedOnHit/PickupTriTip.prefab").WaitForCompletion(); //Again tri-tip dagger
        public override Sprite ItemIcon => Addressables.LoadAssetAsync<Sprite>("RoR2/Base/BleedOnHit/texTriTipIcon.png").WaitForCompletion(); //Could use tri-tip dagger for now

        //public static GameObject ItemBodyModelPrefab;

        public float InitialBoomChance;
        public float AdditionalBoomChance;
        public float InitialExecuteAmount;



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
            InitialBoomChance = config.Bind<float>(
                "Item: " + ItemName, 
                "Initial Boom Chance",
                10f,
                "What are the chances that the burn tick explodes?"
                ).Value;

            InitialExecuteAmount = config.Bind<float>(
                "Item: " + ItemName,
                "Low Health Execute Percentage", 
                20f,
                "What percentage of hit points should be left before the execute?"
                ).Value;

            AdditionalBoomChance = config.Bind<float>(
                "Item: " + ItemName,
                "Additional Boom Chance",
                5f,
                "By how much should an additional item increase the explosion chances?"
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


        }



    }
}
