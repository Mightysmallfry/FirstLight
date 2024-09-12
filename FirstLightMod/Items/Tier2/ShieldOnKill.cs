using BepInEx.Configuration;
using FirstLightMod.Modules.Items;
using R2API;
using RoR2;
using UnityEngine.AddressableAssets;
using UnityEngine;

namespace FirstLightMod.Items
{
    public class ShieldOnKill : ItemBase<ShieldOnKill>
    {
        public override string ItemName => "Sapphire Shield"; 
        public override string ItemNameToken => "SHIELD_ON_KILL";
        public override string ItemPickupDescription => "May their blades shatter on your sapphire mane."; 
        public override string ItemFullDescription => $"On pickup receive {ShieldFromHitpointPercentage}% of your current hit points as additional shield. On kill restore {ShieldOnKillPercentage}% of your hit points to your shields.";
        public override string ItemLore => "A glass of ice cold clean water, barely touched.";
        public override ItemTier Tier => ItemTier.Tier2;

        //public override ItemTag[] ItemTags => new ItemTag[] { ItemTag.Damage }
        public override GameObject ItemModel => Addressables.LoadAssetAsync<GameObject>("RoR2/Base/BleedOnHit/PickupTriTip.prefab").WaitForCompletion(); //Again tri-tip dagger
        public override Sprite ItemIcon => Addressables.LoadAssetAsync<Sprite>("RoR2/Base/SprintBonus/texSodaIcon.png").WaitForCompletion(); //Could use tri-tip dagger for now

        //public static GameObject ItemBodyModelPrefab;

        public float ShieldOnKillPercentage;
        public float ShieldFromHitpointPercentage;

        private bool hasGrantedShieldFromHitPoints = false; //default to false rather than true

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
            ShieldOnKillPercentage = config.Bind<float>(
                "Item: " + ItemName,
                "On Kill Shield Restore",
                10f,
                "How much of your shields should the item restore per kill?"
                ).Value;

            ShieldFromHitpointPercentage = config.Bind<float>(
                "Item: " + ItemName,
                "On Pickup, HP to Shield Percentage",
                10f,
                "What percentage of your hitpoints should be converted to additional shields on pickup."
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
            On.RoR2.GlobalEventManager.OnCharacterDeath += GlobalEventManager_OnCharacterDeath;
            On.RoR2.CharacterBody.OnInventoryChanged += CharacterBody_OnInventoryChanged;
        }

        private void CharacterBody_OnInventoryChanged(On.RoR2.CharacterBody.orig_OnInventoryChanged orig, CharacterBody self)
        {
            orig(self);

            if (self && self.inventory && !hasGrantedShieldFromHitPoints)
            {
                if (GetCount(self) > 0)
                {

                    float ShieldFromHitpoints = (ShieldFromHitpointPercentage / 100f) * self.maxHealth;
                    self.maxShield += ShieldFromHitpoints;
                    hasGrantedShieldFromHitPoints = true;
                }
            }

        }

        private void GlobalEventManager_OnCharacterDeath(On.RoR2.GlobalEventManager.orig_OnCharacterDeath orig, GlobalEventManager self, DamageReport damageReport)
        {
            orig(self, damageReport);

            if (damageReport.attackerBody && damageReport.attackerBody.inventory && GetCount(damageReport.attackerBody) > 0)
            {
                CharacterBody attackerBody = damageReport.attackerBody;
                
                attackerBody.healthComponent.shield += (ShieldOnKillPercentage * ((float)GetCount(attackerBody)) / 100f) * attackerBody.maxHealth;
            }

        }
    }
}
