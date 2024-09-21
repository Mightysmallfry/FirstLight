using BepInEx.Configuration;
using FirstLightMod.Modules.Items;
using R2API;
using RoR2;
using UnityEngine.AddressableAssets;
using UnityEngine;

namespace FirstLightMod.Items
{
    public class IceWater : ItemBase<IceWater>
    {
        public override string ItemName => "Ice Water"; 
        public override string ItemNameToken => "ICE_WATER";
        public override string ItemPickupDescription => "Refreshing as always."; 
        public override string ItemFullDescription => $"On killing an elite, have a refreshing drink and heal for {HealOnKillAmount}% maximum health";
        public override string ItemLore => "A glass of ice cold clean water, barely touched.";
        public override ItemTier Tier => ItemTier.Tier2;

        //Enemies get stacks, if 10 stacks, quake
        //quake would chain react to all enemies with at least 1 stack,
        //dealing damage depending on the number of stacks
        //chain reaction does not remove stacks other than detonator.

        //public override ItemTag[] ItemTags => new ItemTag[] { ItemTag.Damage }
        public override GameObject ItemModel => Addressables.LoadAssetAsync<GameObject>("RoR2/Base/BleedOnHit/PickupTriTip.prefab").WaitForCompletion(); //Again tri-tip dagger
        public override Sprite ItemIcon => Addressables.LoadAssetAsync<Sprite>("RoR2/Base/SprintBonus/texSodaIcon.png").WaitForCompletion(); //Could use tri-tip dagger for now

        //public static GameObject ItemBodyModelPrefab;

        public float HealOnKillAmount;

        public static BuffDef glacialQuakeBuff;


        public override void Init(ConfigFile config)
        {
            CreateConfig(config);
            CreateItemDisplayRules();
            CreateLang();
            CreateBuffs();
            CreateItem();
            Hooks();
        }

        public void CreateConfig(ConfigFile config)
        {
            HealOnKillAmount = config.Bind<float>(
                "Item: " + ItemName,
                "On Kill Heal Amount",
                5f,
                "How much healing should the item give?"
                ).Value;


        }

        private void CreateBuffs()
        {
            glacialQuakeBuff = Modules.Content.CreateAndAddBuff("Glacial Quake",
                LegacyResourcesAPI.Load<BuffDef>("BuffDefs/HiddenInvincibility").iconSprite,
                new Color(20f, 13f, 149f),
                true,
                true);

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





            orig(self, damageInfo, victim);
        }

    }
}
