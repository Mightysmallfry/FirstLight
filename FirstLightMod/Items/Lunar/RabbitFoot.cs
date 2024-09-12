using BepInEx.Configuration;
using FirstLightMod.Modules.Items;
using R2API;
using RoR2;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace FirstLightMod.Items
{
    public class RabbitFoot : ItemBase<RabbitFoot>
    {
        public override string ItemName => "Rabbits Foot";
        public override string ItemNameToken => "RABBIT_FOOT";
        public override string ItemPickupDescription => $"Feel lucky, but <style=cDeath>invoke more challenge.</style>";
        public override string ItemFullDescription => $"Gain luck but <style=cDeath>harder enemies spawn</style>.";
        public override string ItemLore => "";
        public override ItemTier Tier => ItemTier.Lunar;


        public override ItemTag[] ItemTags => new ItemTag[] { ItemTag.Damage };
        //Use Addressables not Resources as it is more up to date
        public override GameObject ItemModel => Addressables.LoadAssetAsync<GameObject>("RoR2/DLC1/ElementalRingVoid/PickupVoidRing.prefab").WaitForCompletion(); //use singularity band
        public override Sprite ItemIcon => Addressables.LoadAssetAsync<Sprite>("RoR2/DLC1/ElementalRingVoid/texVoidRingIcon.png").WaitForCompletion(); //use singularity band

        public float LuckAmountGranted;



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
            LuckAmountGranted = config.Bind<float>(
                "Item: " + ItemName,
                "Luck Granted",
                1f,
                "How much luck should be granted?").Value;


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
            //Hook onto the director, make elites more common, grant all enemies 1 irradiant pearl.
        }
        
    }
}
