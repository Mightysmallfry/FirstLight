using BepInEx.Configuration;
using FirstLightMod.Modules.Items;
using R2API;
using RoR2;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace FirstLightMod.Items
{
    public class Mixtape : ItemBase<Mixtape>
    {
        public override string ItemName => "Mixtape"; 
        public override string ItemNameToken => "MIXTAPE";
        public override string ItemPickupDescription => "and his words were fire."; //"Deal massively increased bleed damage"
        public override string ItemFullDescription => $"Burning enemies have a chance to erupt, spreading fire to nearby enemies.";
        public override string ItemLore => ".";
        public override ItemTier Tier => ItemTier.Tier3;

        public override ItemTag[] ItemTags => new ItemTag[] { ItemTag.Damage }; // I can uncomment this once I figure out what is happening with the load issue
        public override GameObject ItemModel => Addressables.LoadAssetAsync<GameObject>("RoR2/Base/BleedOnHit/PickupTriTip.prefab").WaitForCompletion(); //Again tri-tip dagger
        public override Sprite ItemIcon => Addressables.LoadAssetAsync<Sprite>("RoR2/Base/BleedOnHit/texTriTipIcon.png").WaitForCompletion(); //Could use tri-tip dagger for now

        public static GameObject EruptionEffectPrefab; // = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Items/IgniteExplosionVFX.prefab").WaitForCompletion();
        private static readonly List<HurtBox> EruptionHurtBoxBuffer = new List<HurtBox>();

        public static float Radius = 5f;

        public float InitialEruptionChance;
        public float AdditionalEruptionChance;
        
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
            InitialEruptionChance = config.Bind<float>(
                "Item: " + ItemName, 
                "Initial Boom Chance",
                10f,
                "What are the chances that the burn tick explodes?"
                ).Value;
            


            AdditionalEruptionChance = config.Bind<float>(
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
            On.RoR2.HealthComponent.TakeDamage += HealthComponent_TakeDamage;
        }

        private void HealthComponent_TakeDamage(On.RoR2.HealthComponent.orig_TakeDamage orig, HealthComponent self, DamageInfo damageInfo)
        {

            if (damageInfo.attacker && damageInfo.attacker.TryGetComponent(out CharacterBody attackerBody) && attackerBody.master)
            {
                if (self.body && self.TryGetComponent(out CharacterBody victimBody))
                {
                    //By including the master into the check roll, we also account for luck
                    if (GetCount(attackerBody.master) > 0 && Util.CheckRoll(InitialEruptionChance + (AdditionalEruptionChance * ((float)GetCount(attackerBody) - 1f)), attackerBody.master))
                    {
                        //Calculate range for the attack based off of itemCount and enemy size
                        float itemRange = 12f;

                        //radius of the enemy that erupts
                        float enemyRadius = victimBody.radius;
                        float finalRadius = itemRange + enemyRadius;

                        //Calculate the damage that the explosion will deal
                        float DamageCoefficient = 1.5f;
                        float explosionDamage = attackerBody.damage * DamageCoefficient;

                        //Calculate our burn damage per tick
                        float burnDamage = 0.75f * attackerBody.damage;

                        //Create out Sphere Search for finding people
                        Vector3 corePosition = victimBody.corePosition;
                        Mixtape.IgniteSphereSearch.origin = corePosition;
                        Mixtape.IgniteSphereSearch.mask = LayerIndex.entityPrecise.mask;
                        Mixtape.IgniteSphereSearch.radius = Mixtape.Radius;
                        Mixtape.IgniteSphereSearch.RefreshCandidates();
                        Mixtape.IgniteSphereSearch.FilterCandidatesByHurtBoxTeam(TeamMask.GetUnprotectedTeams(attackerBody.teamComponent.teamIndex));
                        Mixtape.IgniteSphereSearch.FilterCandidatesByDistinctHurtBoxEntities();
                        Mixtape.IgniteSphereSearch.OrderCandidatesByDistance();

                        //Add all the hurtboxes in the search to our list
                        Mixtape.IgniteSphereSearch.GetHurtBoxes(Mixtape.EruptionHurtBoxBuffer);
                        Mixtape.IgniteSphereSearch.ClearCandidates();


                        //Look through the hurtboxes in the area
                        for (int i = 0; i < Mixtape.EruptionHurtBoxBuffer.Count; i++)
                        {
                            HurtBox hurtBox = Mixtape.EruptionHurtBoxBuffer[i];


                            //Check if the target can be dealt damage and inflict our burn dot
                            if (hurtBox.healthComponent)
                            {
                                InflictDotInfo inflictDotInfo = new InflictDotInfo
                                {
                                    victimObject = hurtBox.healthComponent.gameObject,
                                    attackerObject = damageInfo.attacker,
                                    totalDamage = new float?(burnDamage),
                                    dotIndex = DotController.DotIndex.Burn,
                                    damageMultiplier = 1f
                                };

                                //Check if we have an item that improves our burns
                                if (attackerBody.master)
                                {
                                    StrengthenBurnUtils.CheckDotForUpgrade(attackerBody.master.inventory, ref inflictDotInfo);
                                }

                                //Inflict the burn dot
                                DotController.InflictDot(ref inflictDotInfo);
                            }

                        }

                        //Clean out our buffer so that it can be used next time.
                        Mixtape.EruptionHurtBoxBuffer.Clear();

                        //Do the blast damage to nearby creatures
                        new BlastAttack
                        {
                            radius = finalRadius,
                            baseDamage = explosionDamage,
                            procCoefficient = 0f,
                            crit = Util.CheckRoll(attackerBody.crit),
                            damageColorIndex = DamageColorIndex.Item,
                            attackerFiltering = AttackerFiltering.Default,
                            falloffModel = BlastAttack.FalloffModel.None,
                            attacker = damageInfo.attacker,
                            teamIndex = attackerBody.teamComponent.teamIndex,
                            position = corePosition
                        }.Fire();



                        Mixtape.EruptionEffectPrefab = GlobalEventManager.CommonAssets.igniteOnKillExplosionEffectPrefab;
                        //Spawn the effect for the eruption
                        EffectManager.SpawnEffect(Mixtape.EruptionEffectPrefab,
                            new EffectData
                            {
                                origin = corePosition,
                                scale = finalRadius,
                                rotation = Util.QuaternionSafeLookRotation(damageInfo.force)
                            }, true);
                    }

                }

            }

            orig(self, damageInfo);
        }


    }
}
