using System;
using System.Collections.Generic;
using System.Text;
using BepInEx.Configuration;
using FirstLightMod.Modules.Items;
using R2API;
using RoR2;
using UnityEngine.AddressableAssets;
using UnityEngine;

namespace FirstLightMod.Items
{
    public class FryingPan : ItemBase<FryingPan>
    {
        public override string ItemName => "Cast Iron Frying Pan"; 
        public override string ItemNameToken => "FRYING_PAN";
        public override string ItemPickupDescription => "Bonk."; 
        public override string ItemFullDescription => $"Gain {StunProcChance}% chance to stun, on hitting a stunned target, trigger active enemy debuffs {AdditionalDebuffStacks} times.";
        public override string ItemLore => ".";
        public override ItemTier Tier => ItemTier.Tier2;

        //public override ItemTag[] ItemTags => new ItemTag[] { ItemTag.Damage }
        public override GameObject ItemModel => Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Mystery/PickupMystery.prefab").WaitForCompletion(); //Again tri-tip dagger
        public override Sprite ItemIcon => Addressables.LoadAssetAsync<Sprite>("RoR2/Base/Common/MiscIcons/texMysteryIcon.png").WaitForCompletion(); //Could use tri-tip dagger for now

        //public static GameObject ItemBodyModelPrefab;

        public static float StunProcChance;
        public static int AdditionalDebuffStacks;

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

            StunProcChance = config.Bind<float>(
                "Item: " + ItemName,
                "stunProcChance",
                5f,
                "What percentage of your shots should stun on hit?"
            ).Value;


            AdditionalDebuffStacks = config.Bind<int>(
                "Item: " + ItemName,
                "additionalDebuffStacks",
                1,
                "How many additional stacks of debuffs should this item apply per hit?"
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
            if (damageInfo.attacker.TryGetComponent(out CharacterBody attackerBody) && GetCount(attackerBody) > 0 && damageInfo.damageType == DamageType.Stun1s)
            {
                int itemCount = GetCount(attackerBody);


                //Burning
                int buffCount = self.body.GetBuffCount(RoR2Content.Buffs.OnFire);
                if (buffCount > 0)
                {
                    float damageMultiplier = 0.5f;
                    int additionalDebuffCount = FryingPan.AdditionalDebuffStacks * itemCount;
                    InflictDotInfo inflictDotInfo = new InflictDotInfo
                    {
                        attackerObject = damageInfo.attacker,
                        victimObject = self.body.gameObject,
                        totalDamage = new float?(damageInfo.damage * damageMultiplier),
                        damageMultiplier = 1f,
                        dotIndex = DotController.DotIndex.Burn
                    };
                    for (int j = 0; j < additionalDebuffCount; j++)
                    {
                        DotController.InflictDot(ref inflictDotInfo);
                    }
                }

                //Beetlejuice
                buffCount = self.body.GetBuffCount(RoR2Content.Buffs.BeetleJuice);
                if (buffCount > 0)
                {
                    int newCount = buffCount + FryingPan.AdditionalDebuffStacks * itemCount;
                    self.body.SetBuffCount(RoR2Content.Buffs.BeetleJuice.buffIndex, newCount);
                }

                //Nullify
                buffCount = self.body.GetBuffCount(RoR2Content.Buffs.NullifyStack);
                if (buffCount > 0)
                {
                    int newCount = buffCount + FryingPan.AdditionalDebuffStacks * itemCount;
                    self.body.SetBuffCount(RoR2Content.Buffs.NullifyStack.buffIndex, newCount);
                }

                //Bleed
                buffCount = self.body.GetBuffCount(RoR2Content.Buffs.Bleeding);
                if (buffCount > 0)
                {
                    int additionalDebuffCount = FryingPan.AdditionalDebuffStacks * itemCount;
                    for (int k = 0; k < additionalDebuffCount; k++)
                    {
                        DotController.InflictDot(self.body.gameObject, damageInfo.attacker, DotController.DotIndex.Bleed, 3f * damageInfo.procCoefficient, 1f, null);
                    }
                }
                
                //SuperBleed
                buffCount = self.body.GetBuffCount(RoR2Content.Buffs.SuperBleed);
                if (buffCount > 0)
                {
                    int additionalDebuffCount = FryingPan.AdditionalDebuffStacks * itemCount;
                    for (int l = 0; l < additionalDebuffCount; l++)
                    {
                        DotController.InflictDot(self.body.gameObject, damageInfo.attacker, DotController.DotIndex.SuperBleed, 15f * damageInfo.procCoefficient, 1f, null);
                    }
                }
                
                //Blight
                buffCount = self.body.GetBuffCount(RoR2Content.Buffs.Blight);
                if (buffCount > 0)
                {
                    int additionalDebuffCount = FryingPan.AdditionalDebuffStacks * itemCount;
                    for (int m = 0; m < additionalDebuffCount; m++)
                    {
                        DotController.InflictDot(self.body.gameObject, damageInfo.attacker, DotController.DotIndex.Blight, 5f * damageInfo.procCoefficient, 1f, null);
                    }
                }
                
                //Overheat
                buffCount = self.body.GetBuffCount(RoR2Content.Buffs.Overheat);
                if (buffCount > 0)
                {
                    int newCount = buffCount + FryingPan.AdditionalDebuffStacks * itemCount;
                    self.body.SetBuffCount(RoR2Content.Buffs.Overheat.buffIndex, newCount);
                }

                //Pulverize
                buffCount = self.body.GetBuffCount(RoR2Content.Buffs.PulverizeBuildup);
                if (buffCount > 0)
                {
                    int newCount = buffCount + FryingPan.AdditionalDebuffStacks * itemCount;
                    self.body.SetBuffCount(RoR2Content.Buffs.PulverizeBuildup.buffIndex, newCount);
                }

                //Stronger Burn/Ignition tanks
                buffCount = self.body.GetBuffCount(DLC1Content.Buffs.StrongerBurn);
                if (buffCount > 0)
                {
                    int additionalDebuffCount = FryingPan.AdditionalDebuffStacks * itemCount;
                    for (int n = 0; n < additionalDebuffCount; n++)
                    {
                        DotController.InflictDot(self.body.gameObject, damageInfo.attacker, DotController.DotIndex.StrongerBurn, 3f * damageInfo.procCoefficient, 1f, null);
                    }
                }

                //Fracture
                buffCount = self.body.GetBuffCount(DLC1Content.Buffs.Fracture);
                if (buffCount > 0)
                {
                    int additionalDebuffCount = FryingPan.AdditionalDebuffStacks * itemCount;
                    DotController.DotDef dotDef = DotController.GetDotDef(DotController.DotIndex.Fracture);
                    for (int i = 0; i < additionalDebuffCount; i++)
                    {
                        DotController.InflictDot(self.body.gameObject, damageInfo.attacker, DotController.DotIndex.Fracture, dotDef.interval, 1f, null);
                    }
                }
                
                //Curse /Permanent Debuff
                buffCount = self.body.GetBuffCount(DLC1Content.Buffs.PermanentDebuff);
                if (buffCount > 0)
                {
                    int newCount = buffCount + FryingPan.AdditionalDebuffStacks * itemCount;
                    self.body.SetBuffCount(DLC1Content.Buffs.PermanentDebuff.buffIndex, newCount);
                }

                //LunarRuin
                buffCount = self.body.GetBuffCount(DLC2Content.Buffs.lunarruin);
                if (buffCount > 0)
                {
                    int additionalDebuffCount = buffCount + FryingPan.AdditionalDebuffStacks * itemCount;
                    self.body.SetBuffCount(DLC2Content.Buffs.lunarruin.buffIndex, additionalDebuffCount);
                    for (int i = 0; i < additionalDebuffCount; i++)
                    {
                        DotController.InflictDot(self.body.gameObject, damageInfo.attacker, DotController.DotIndex.LunarRuin, 5f, 1f, null);
                    }
                }



            } 

            orig(self, damageInfo);
        }

    }
}
