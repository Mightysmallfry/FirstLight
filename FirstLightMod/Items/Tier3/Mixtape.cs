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

        public static GameObject ExplosionEffectPrefab = GlobalEventManager.CommonAssets.igniteOnKillExplosionEffectPrefab;

        public static float Radius = 5f;

        public float InitialBoomChance;
        public float AdditionalBoomChance;
        public float InitialExecuteAmount;
        
        private static SphereSearch IgniteSphereSearch = new SphereSearch();


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
            On.RoR2.GlobalEventManager.OnHitEnemy += GlobalEventManager_OnHitEnemy;

        }

        private void GlobalEventManager_OnHitEnemy(On.RoR2.GlobalEventManager.orig_OnHitEnemy orig, GlobalEventManager self, DamageInfo damageInfo, GameObject victim)
        {


            //Null checking

            //Check for item and proc

            // float num = 8f + 4f * (float)igniteOnKillCount;
            // float radius = victimBody.radius;
            // float num2 = num + radius;
            // float num3 = 1.5f;
            // float baseDamage = damageReport.attackerBody.damage * num3;
            // Vector3 corePosition = victimBody.corePosition;

            //Scan for enemies
            // Mixtape.IgniteSphereSearch.origin = corePosition;
            // GlobalEventManager.igniteOnKillSphereSearch.mask = LayerIndex.entityPrecise.mask;
            // GlobalEventManager.igniteOnKillSphereSearch.radius = Mixtape.Radius;
            // GlobalEventManager.igniteOnKillSphereSearch.RefreshCandidates();
            // GlobalEventManager.igniteOnKillSphereSearch.FilterCandidatesByHurtBoxTeam(TeamMask.GetUnprotectedTeams(attackerTeamIndex));
            // GlobalEventManager.igniteOnKillSphereSearch.FilterCandidatesByDistinctHurtBoxEntities();
            // GlobalEventManager.igniteOnKillSphereSearch.OrderCandidatesByDistance();
            // GlobalEventManager.igniteOnKillSphereSearch.GetHurtBoxes(GlobalEventManager.igniteOnKillHurtBoxBuffer);
            // GlobalEventManager.igniteOnKillSphereSearch.ClearCandidates();
            // float value = (float)(1 + igniteOnKillCount) * 0.75f * damageReport.attackerBody.damage;
            // for (int i = 0; i < GlobalEventManager.igniteOnKillHurtBoxBuffer.Count; i++)
            // {
            // HurtBox hurtBox = GlobalEventManager.igniteOnKillHurtBoxBuffer[i];
            // if (hurtBox.healthComponent)
            // {
            // InflictDotInfo inflictDotInfo = new InflictDotInfo
            // {
            // victimObject = hurtBox.healthComponent.gameObject,
            // attackerObject = damageReport.attacker,
            // totalDamage = new float?(value),
            // dotIndex = DotController.DotIndex.Burn,
            // damageMultiplier = 1f
            // };
            // UnityEngine.Object exists;
            // if (damageReport == null)
            // {
            // exists = null;
            // }
            // else
            // {
            // CharacterMaster attackerMaster = damageReport.attackerMaster;

            // Check for upgrading the fire 

            // exists = ((attackerMaster != null) ? attackerMaster.inventory : null);
            // }
            // if (exists)
            // {
            // StrengthenBurnUtils.CheckDotForUpgrade(damageReport.attackerMaster.inventory, ref inflictDotInfo);
            // }
            // DotController.InflictDot(ref inflictDotInfo);
            // }
            // }
            // GlobalEventManager.igniteOnKillHurtBoxBuffer.Clear();




            //Create a new blast attack     

            //new BlastAttack
            // {
            // radius = Mixtape.Radius,
            // baseDamage = baseDamage,
            // procCoefficient = 0f,
            // crit = Util.CheckRoll(damageReport.attackerBody.crit, damageReport.attackerMaster),
            // damageColorIndex = DamageColorIndex.Item,
            // attackerFiltering = AttackerFiltering.Default,
            // falloffModel = BlastAttack.FalloffModel.None,
            // attacker = damageReport.attacker,
            // teamIndex = attackerTeamIndex,
            // position = corePosition
            // }.Fire();
            // EffectManager.SpawnEffect(GlobalEventManager.CommonAssets.igniteOnKillExplosionEffectPrefab, new EffectData
            // {
            // origin = corePosition,
            // scale = Mixtape.Radius,
            // rotation = Util.QuaternionSafeLookRotation(damageReport.damageInfo.force)
            // }, true);


            orig(self, damageInfo, victim);
        }
    }
}
