using RoR2;
using FirstLightMod.Modules.Equipment;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.AddressableAssets;
using R2API;
using BepInEx.Configuration;
using FirstLightMod.Modules;
using static RoR2.MasterSpawnSlotController;

namespace FirstLightMod.Content.Equipment
{
    public class MicrobotMatrix : EquipmentBase<MicrobotMatrix>
    {
        public override string EquipmentName => "Microbot Matrix";

        public override string EquipmentLangTokenName => "MICROBOT_MATRIX";

        public override string EquipmentPickupDesc => "Create a swarm of temporary Defense Microbots to defend you";

        public override string EquipmentFullDescription => "";

        public override string EquipmentLore => "";

        public override GameObject EquipmentModel => Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Mystery/PickupMystery.prefab").WaitForCompletion(); //Use fuel Array

        public override Sprite EquipmentIcon => Addressables.LoadAssetAsync<Sprite>("RoR2/Base/Common/MiscIcons/texMysteryIcon.png").WaitForCompletion(); //Use fuel Array



        ///<summary>
        /// The number of items that this equipment gives
        ///</summary>
        public int microCount; 

        ///<summary>
        /// The amount of items that the captain specific interaction saves
        ///</summary>
        public int captainSave; //How many does the captain get to save?


        ///<summary>
        /// The duration that the items remain with the player
        ///</summary>
        public float buffDuration;

        ///<summary>
        /// The Item given by this equipment
        ///</summary>
        ItemDef itemDef = RoR2Content.Items.CaptainDefenseMatrix;


        /// <summary>
        /// The buff used to track when the duration ends
        /// </summary>
        BuffDef matrixBuff = Buffs.MicrobotMatrixBuff;


        public override ItemDisplayRuleDict CreateItemDisplayRules()
        {
            return new ItemDisplayRuleDict();
        }

        public override void Init(ConfigFile config)
        {

            CreateConfig(config);
            CreateLang();
            CreateEquipment();
            Hooks();

        }

        protected override void CreateConfig(ConfigFile config)
        {

            microCount = config.Bind<int>(
                "Equipment: " + EquipmentName,
                "Microbot Count",
                10,
                "How many Defense Microbots are given upon activation?").Value;

            captainSave = config.Bind<int>(
                "Equipment: " + EquipmentName,
                "Captain Save",
                1,
                "How many Defense Microbots does Captain get to save?").Value;

            buffDuration = config.Bind<float>(
                "Equipment: " + EquipmentName,
                "Duration",
                15f,
                "How long will the Defense Microbots last, once activated?").Value;
        }

        protected override bool ActivateEquipment(EquipmentSlot equipmentSlot)
        {
            if (!equipmentSlot.characterBody) { return false; } // Check to see if you have a character body
            if (!equipmentSlot.characterBody.inventory) { return false; } // Make sure that body has an inventory


            
            //Give Items
            equipmentSlot.characterBody.inventory.GiveItem(itemDef, microCount);
            //Start Timer
            equipmentSlot.characterBody.AddTimedBuff(matrixBuff, buffDuration);
            //Notify Player
            Util.PlaySound("Play_item_proc_healingPotion", equipmentSlot.gameObject);
            return true;

        }
        public override void Hooks()
        {
            On.RoR2.CharacterBody.RemoveBuff_BuffDef += CharacterBody_RemoveBuff_BuffDef;
        }

        private void CharacterBody_RemoveBuff_BuffDef(On.RoR2.CharacterBody.orig_RemoveBuff_BuffDef orig, CharacterBody self, BuffDef buffDef)
        {
            //This might work, double check removal but otherwise should be good.

            if (buffDef == matrixBuff)
            {
                //Remove Items
                if (self.bodyIndex == SurvivorCatalog.GetBodyIndexFromSurvivorIndex(RoR2Content.Survivors.Captain.survivorIndex))
                {
                    self.inventory.RemoveItem(itemDef, microCount);
                }
                else
                {
                    self.inventory.RemoveItem(itemDef, microCount - captainSave);
                }
            }

            orig(self, buffDef);
        }
    }
}
