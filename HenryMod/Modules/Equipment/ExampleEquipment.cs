using BepInEx.Configuration;
using R2API;
using RoR2;
using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace FirstLightMod.Modules.Equipment
{
    public class ExampleEquipment : EquipmentBase
    {
        public override string EquipmentName => "Deprecate Me Equipment";

        public override string EquipmentLangTokenName => "DEPRECATE_ME_EQUIPMENT";

        public override string EquipmentPickupDesc => "";

        public override string EquipmentFullDescription => "";

        public override string EquipmentLore => "";

        public override GameObject EquipmentModel => Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Mystery/PickupMystery.prefab").WaitForCompletion();

        public override Sprite EquipmentIcon => Addressables.LoadAssetAsync<Sprite>("RoR2/Base/Common/MiscIcons/texMysteryIcon.png").WaitForCompletion();

        public override void Init(ConfigFile config)
        {
            CreateConfig(config);
            CreateLang();
            CreateEquipment();
            Hooks();
        }

        protected override void CreateConfig(ConfigFile config)
        {

        }

        public override ItemDisplayRuleDict CreateItemDisplayRules()
        {
            return new ItemDisplayRuleDict();
        }

        protected override bool ActivateEquipment(EquipmentSlot slot)
        {
            return false;
        }


    }
}
