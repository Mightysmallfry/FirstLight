using BepInEx.Configuration;
using FirstLightMod.Modules.Items;
using R2API;
using RoR2;
using UnityEngine.AddressableAssets;
using UnityEngine;
using TMPro;

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

        public int triggerQuakeCount;

        public static BuffDef glacialBuff;
        public static BuffDef quakeBuff;


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

            triggerQuakeCount = config.Bind<int>(
                "Item: " + ItemName,
                "Quake Trigger Count",
                10,
                "How much glacial does the target need before they cause a quake?"
            ).Value;
        }

        private void CreateBuffs()
        {
            glacialBuff = Modules.Content.CreateAndAddBuff("Glacial Quake",
                LegacyResourcesAPI.Load<BuffDef>("BuffDefs/HiddenInvincibility").iconSprite,
                new Color(104f, 135f, 247f),
                true,
                true);

            quakeBuff = Modules.Content.CreateAndAddBuff("Glacial Quake",
                LegacyResourcesAPI.Load<BuffDef>("BuffDefs/HiddenInvincibility").iconSprite,
                new Color(146f, 198f, 241f),
                false,
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
            if (damageInfo.attacker && damageInfo.attacker.TryGetComponent(out CharacterBody attackerBody) && attackerBody.inventory)
            {
                if (victim && victim.TryGetComponent(out CharacterBody victimBody))
                {
                    if (GetCount(attackerBody) > 0)
                    {
                        if (victimBody.GetBuffCount(glacialBuff) < triggerQuakeCount)
                        {
                            victimBody.AddBuff(glacialBuff);

                        }
                        else
                        {
                            victimBody.AddBuff(quakeBuff);
                        }
                    }


                    //Deal with the quake
                    if (victimBody.GetBuffCount(quakeBuff) > 0)
                    {

                        //Do the blast damage to nearby creatures
                        new BlastAttack
                        {
                            radius = 12f,
                            baseDamage = 4f * victimBody.GetBuffCount(glacialBuff),
                            procCoefficient = 0f,
                            crit = Util.CheckRoll(attackerBody.crit),
                            damageColorIndex = DamageColorIndex.Item,
                            attackerFiltering = AttackerFiltering.Default,
                            falloffModel = BlastAttack.FalloffModel.None,
                            attacker = damageInfo.attacker,
                            teamIndex = attackerBody.teamComponent.teamIndex,
                            position = victimBody.corePosition
                        }.Fire();
                        
                        //Remove glacial

                        for (int i = 0; i < triggerQuakeCount; i++)
                        {
                            victimBody.RemoveBuff(glacialBuff);
                        }
                    }





                }
            }



            orig(self, damageInfo, victim);
        }

    }
}
