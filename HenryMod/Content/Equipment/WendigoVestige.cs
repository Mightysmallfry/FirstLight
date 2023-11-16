using BepInEx.Configuration;
using FirstLightMod.Modules;
using FirstLightMod.Modules.Equipment;
using R2API;
using RoR2;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;

namespace FirstLightMod.Content.Equipment
{
    class WendigoVestige : EquipmentBase<WendigoVestige>
    {
        public override string EquipmentName => "Wendigo Vestige";

        public override string EquipmentLangTokenName => "WENDIGO_VESTIGE";

        public override string EquipmentPickupDesc => "Let the hunt begin...";

        public override string EquipmentFullDescription => $"Mark a target for {duration} seconds, during that time they receive {damageIncrease}% more damage from all sources, upon killing that target within {duration} seconds, will reward {bountyReward} gold pieces.";

        public override string EquipmentLore => "";

        public override GameObject EquipmentModel => Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Mystery/PickupMystery.prefab").WaitForCompletion();

        public override Sprite EquipmentIcon => Addressables.LoadAssetAsync<Sprite>("RoR2/Base/Common/MiscIcons/texMysteryIcon.png").WaitForCompletion();

        /// <summary>
        /// How long the debuff lasts on the target
        /// </summary>
        public static float duration;

        /// <summary>
        /// What percentage is the damage being increased by?
        /// </summary>
        public static float damageIncrease;


        /// <summary>
        /// How much money should the player receive upon killing the target?
        /// Unsigned due to no possible negative money.
        /// </summary>
        public static uint bountyReward;


        public override void Init(ConfigFile config)
        {
            //CreateConfig(config);
            //CreateLang();
            //CreateTargetingIndicator();
            //CreateEquipment();
            //Hooks();
        }

        protected override void CreateConfig(ConfigFile config)
        {
            duration = config.Bind<float>(
                "Equipment: " + EquipmentName,
                "Debuff Duration",
                30f,
                "How long does the target remain debuffed?").Value;

            damageIncrease = config.Bind<float>(
                "Equipment: " + EquipmentName,
                "Damage Increase",
                15f,
                "How much will damage be increased against the marked target?").Value;

            bountyReward = config.Bind<uint>(
                "Equipment: " + EquipmentName,
                "Bounty Reward",
                100,
                "How much money will the player make after killing the marked target?").Value;

        }

        /// <summary>
        /// An example targeting indicator implementation. We clone the woodsprite's indicator, but we edit it to our liking. We'll use this in our activate equipment.
        /// We shouldn't need to network this as this only shows for the player with the Equipment.
        /// </summary>
        private void CreateTargetingIndicator()
        {
            //Double this in the future when Assets have become a focus.
            TargetingIndicatorPrefabBase = Resources.Load<GameObject>("Prefabs/WoodSpriteIndicator").InstantiateClone("ExampleIndicator", false);
            TargetingIndicatorPrefabBase.GetComponentInChildren<SpriteRenderer>().sprite = Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("ExampleReticuleIcon.png");
            //We can actually turn this into a really cool honeycomb hexagon
            // MainAssets.LoadAsset<Sprite>("ExampleReticuleIcon.png");
            TargetingIndicatorPrefabBase.GetComponentInChildren<SpriteRenderer>().color = Color.white;
            TargetingIndicatorPrefabBase.GetComponentInChildren<SpriteRenderer>().transform.rotation = Quaternion.identity;
            TargetingIndicatorPrefabBase.GetComponentInChildren<TMPro.TextMeshPro>().color = new Color(0.423f, 1, 0.749f);
        }

        public override ItemDisplayRuleDict CreateItemDisplayRules()
        {
            return new ItemDisplayRuleDict();
        }

        protected override bool ActivateEquipment(EquipmentSlot slot)
        {
            //We check for the characterbody, and if that has an inputbank that we'll be getting our aimray from. If we don't have it, we don't continue.
            if (!slot.characterBody || !slot.characterBody.inputBank) { return false; }

            //Check for our targeting controller that we attach to the object if we have "Use Targeting" enabled.
            var targetComponent = slot.GetComponent<TargetingControllerComponent>();

            //Ensure we have a target component, and that component is reporting that we have an object targeted.
            if (targetComponent && targetComponent.TargetObject)
            {
                var chosenHurtbox = targetComponent.TargetFinder.GetResults().First();

                //Here we would use said hurtbox for something. Could be anything from firing a projectile towards it, applying a buff/debuff to it. Anything you can think of.
                if (chosenHurtbox)
                {
                    //stuff
                    chosenHurtbox.healthComponent.body.AddTimedBuff(Buffs.WendigoVestigeDebuff, duration);
                }

                return true;
            }
            return false;
        }

        public override void Hooks()
        {
            On.RoR2.HealthComponent.TakeDamage += HealthComponent_TakeDamage;

            GlobalEventManager.onCharacterDeathGlobal += GlobalEventManager_onCharacterDeathGlobal;
       
        }

        private void GlobalEventManager_onCharacterDeathGlobal(DamageReport damageReport)
        {
            if (NetworkServer.active)
            {
                if (damageReport.victimBody.HasBuff(Buffs.WendigoVestigeDebuff))
                {
                    damageReport.attackerMaster.GiveMoney(bountyReward);
                }
            }
        }

        private void HealthComponent_TakeDamage(On.RoR2.HealthComponent.orig_TakeDamage orig, HealthComponent self, DamageInfo damageInfo)
        {

            if (self.body.GetBuffCount(Buffs.WendigoVestigeDebuff) > 0)
            {
                damageInfo.damage += damageInfo.damage * damageIncrease;
            }

            orig(self, damageInfo);
        }


        
    }
}
