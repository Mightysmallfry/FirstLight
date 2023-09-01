using BepInEx.Configuration;
using FirstLightMod.Modules.Items;
using R2API;
using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace FirstLightMod.Content.Items
{
    public class FortifierBand : ItemBase<FortifierBand>
    {
        public override string ItemName => "Fortifier Band";
        public override string ItemNameToken => "FORTIFIER_BAND";
        public override string ItemPickupDescription => "Gain increased armor for each band acquired.";
        public override string ItemFullDescription => $"Gain <style=cShrine>{armorGain} armor</style>. Gain an additional <style=cShrine>{armorPerBand} armor</style> for each non {ItemName} item in your inventory.";
        public override string ItemLore => "";
        public override ItemTier Tier => ItemTier.Tier2;

        //Use Addressables not Resources as it is more up to date
        public override GameObject ItemModel => Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Mystery/PickupMystery.prefab").WaitForCompletion(); //use singularity band
        public override Sprite ItemIcon => Addressables.LoadAssetAsync<Sprite>("RoR2/Base/Common/MiscIcons/texMysteryIcon.png").WaitForCompletion(); //use singularity band


        //More non-Fortifier bands = more armor
        //More Fortifier bands = more armor per other band.
        public float armorGain; //amount of armor gained for having the item
        public float armorPerBand; //increases armor for non Fortifier bands
        public float armorPerCopy; //amount of armor gained per band due to number of fortifier's bands

        public override void Init(ConfigFile config)
        {
            CreateConfig(config);
            CreateLang();
            CreateItem();
            Hooks();
        }
        
        private void CreateConfig(ConfigFile config)
        {
            armorGain = config.Bind<float>(
                "Item: " + ItemName,
                "Armor Gained",
                20f,
                "What is the armor gained for having at least one copy of this item").Value;

            armorPerBand = config.Bind<float>(
                "Item: " + ItemName,
                "Armor Per Band",
                75f,
                "What is the additional armor gained per other Band?").Value;

            armorPerCopy = config.Bind<float>(
                "Item: " + ItemName,
                "Armor Per Copy",
                25f,
                "How much is the increase in armor for getting other bands, increased by having additional copies of this item.").Value;

        }

        public override ItemDisplayRuleDict CreateItemDisplayRules()
        {
            return new ItemDisplayRuleDict();
        }

        public override void Hooks()
        {
            On.RoR2.CharacterBody.RecalculateStats += CharacterBody_RecalculateState;
        }

        private void HealthComponent_TakeDamage(On.RoR2.HealthComponent.orig_TakeDamage orig, HealthComponent self, DamageInfo damageInfo)
        {
            throw new NotImplementedException();
        }

        private void CharacterBody_RecalculateState(On.RoR2.CharacterBody.orig_RecalculateStats orig, RoR2.CharacterBody self)
        {
            orig(self);

            if (GetCount(self) > 0)
            {
                self.armor += armorGain;

                if (GetCountBands(self) > 0)
                {
                    self.armor += GetCountBands(self) * (armorPerBand + (armorPerCopy * (GetCount(self) - 1)));
                }
            }
        }


        private int GetCountBands(RoR2.CharacterBody self)
        {
            int bandCount = 0;

            // FireRing - Kjaro's Band
            // IceRing - Runald's Band
            // ElementalRingVoid - Singularity Band
            // I need their ItemDefs.

            if (self.inventory.GetItemCount(RoR2Content.Items.FireRing.itemIndex) > 0)
            {
                bandCount += self.inventory.GetItemCount(RoR2Content.Items.FireRing.itemIndex);
            }
            if (self.inventory.GetItemCount(RoR2Content.Items.IceRing.itemIndex) > 0)
            {
                bandCount += self.inventory.GetItemCount(RoR2Content.Items.IceRing.itemIndex);
            }
            if (self.inventory.GetItemCount(DLC1Content.Items.ElementalRingVoid.itemIndex) > 0)
            {
                bandCount += self.inventory.GetItemCount(DLC1Content.Items.ElementalRingVoid.itemIndex);
            }

            return bandCount;
        }
    }
}
