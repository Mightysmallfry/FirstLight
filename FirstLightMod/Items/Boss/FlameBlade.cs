using BepInEx.Configuration;
using FirstLightMod.Modules.Items;
using R2API;
using RoR2;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace FirstLightMod.Content.Items
{
    public class FlameBlade : ItemBase<FlameBlade>
    {
        public override string ItemName => "Flamberge";
        public override string ItemNameToken => "FLAME_BLADE";
        public override string ItemPickupDescription => "Let thy flames grant simple respite"; 
        public override string ItemFullDescription => $"Gain a <style=cIsDamage>{IgnitionChance * 100}%</style> to ignite enemies on hit. " +
                                                      $"Burning enemies now grants <style=cShrine>barrier</style>. " +
                                                      $"Ignite effects deal <style=cIsDamage>+{IgnitionDamageIncrease * 100}% damage.</style>"; //Apply an execute to burning targets?
        public override string ItemLore => "";
        public override ItemTier Tier => ItemTier.Boss;

        public override ItemTag[] ItemTags => new ItemTag[] { ItemTag.Damage }; 
        public override GameObject ItemModel => Addressables.LoadAssetAsync<GameObject>("RoR2/Base/BleedOnHit/PickupTriTip.prefab").WaitForCompletion(); //Again tri-tip dagger
        public override Sprite ItemIcon => Addressables.LoadAssetAsync<Sprite>("RoR2/Base/BleedOnHit/texTriTipIcon.png").WaitForCompletion(); //Could use tri-tip dagger for now

        //public static GameObject ItemBodyModelPrefab;

        public float IgnitionChance;
        public float BarrierPerTick;
        public float IgnitionDamageIncrease;



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
            IgnitionChance = config.Bind<float>(
                "Item: " + ItemName, 
                "Initial Ignition Chance",
                20f,
                "What is the chance that an attack causes an ignition."
                ).Value;

            BarrierPerTick = config.Bind<float>(
                "Item: " + ItemName,
                "Barrier Per Ignition Tick", 
                5.0f,
                "How much barrier should be given upon dealing ignition damage?"
                ).Value;

            IgnitionDamageIncrease = config.Bind<float>(
                "Item: " + ItemName,
                "Ignition Damage Increase",
                6.0f,
                "How much damage should the ignition damage be increased per additional item?"
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
            On.RoR2.GlobalEventManager.OnHitEnemy += GlobalEventManager_OnHitEnemy;
            On.RoR2.StrengthenBurnUtils.CheckDotForUpgrade += StrengthenBurnUtils_CheckDotForUpgrade; //Check when this is being called.
            DotController.onDotInflictedServerGlobal += DotController_onDotInflictedServerGlobal;
        }
        

        //Does it apply a burn on hit
        private void GlobalEventManager_OnHitEnemy(On.RoR2.GlobalEventManager.orig_OnHitEnemy orig, GlobalEventManager self, DamageInfo damageInfo, GameObject victim)
        {
            orig(self, damageInfo, victim);

            CharacterBody attackerBody = damageInfo.attacker.GetComponent<CharacterBody>();

            if (damageInfo.attacker)
            {
                var count = GetCount(attackerBody);

                if (count > 0 && Util.CheckRoll(IgnitionChance / 100, attackerBody.master))
                {
                    DamageInfo burnAttackDamageInfo = new DamageInfo
                    {
                        attacker = damageInfo.attacker,
                        canRejectForce = false,
                        crit = true,
                        damage = 1f,
                        damageColorIndex = DamageColorIndex.CritHeal,
                        damageType = DamageType.IgniteOnHit,
                        dotIndex = DotController.DotIndex.StrongerBurn,
                        force = Vector3.zeroVector,
                        rejected = false,
                        procChainMask = default,
                        procCoefficient = 0,
                        inflictor = damageInfo.inflictor,
                        position = Vector3.zeroVector
                    };

                    victim.GetComponent<CharacterBody>().healthComponent.TakeDamage(damageInfo);

                }

            }
        }


        //Act as a different kind of ignition tank aka StrengthenBurn
        private void StrengthenBurnUtils_CheckDotForUpgrade(On.RoR2.StrengthenBurnUtils.orig_CheckDotForUpgrade orig, Inventory inventory, ref InflictDotInfo dotInfo)
        {

            if (dotInfo.dotIndex == DotController.DotIndex.Burn || dotInfo.dotIndex == DotController.DotIndex.Helfire)
            {
                int flameBladeCount = inventory.GetItemCount(FlameBlade.instance.ItemDef);
                if (flameBladeCount > 0)
                {
                    dotInfo.preUpgradeDotIndex = new DotController.DotIndex?(dotInfo.dotIndex);
                    dotInfo.dotIndex = DotController.DotIndex.StrongerBurn;
                    float damage = (float)(1 + IgnitionDamageIncrease * flameBladeCount);
                    dotInfo.damageMultiplier *= damage;
                    dotInfo.totalDamage *= damage;
                }
            }

            orig(inventory, ref dotInfo);
        }

        //Granting barrier on burn ticks
        private void DotController_onDotInflictedServerGlobal(DotController dotController, ref InflictDotInfo inflictDotInfo)
        {
            if (inflictDotInfo.dotIndex == DotController.DotIndex.StrongerBurn)
            {

                CharacterBody attackerBody = inflictDotInfo.attackerObject.GetComponent<CharacterBody>();
                int itemCount = attackerBody.inventory.GetItemCount(FlameBlade.instance.ItemDef);

                if (itemCount > 0)
                {
                    attackerBody.healthComponent.AddBarrier(BarrierPerTick);
                }
            };
        }


    }
}
