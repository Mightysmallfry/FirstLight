using RoR2;
using FirstLightMod.Modules.Items;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using R2API;
using BepInEx.Configuration;

namespace FirstLightMod.Content.Equipment
{
    public class MicrobotMatrix : EquipmentBase
    {
        public override string EquipmentName => "Microbot Matrix";

        public override string EquipmentNameToken => "MICROBOT_MATRIX";

        public override string EquipmentPickupDesc => "Create a swarm of temporary Defense Microbots to defend you";

        public override string EquipmentFullDescription => "";

        public override string EquipmentLore => "";

        public override GameObject EquipmentModel => throw new NotImplementedException(); //Use fuel Array

        public override Sprite EquipmentIcon => throw new NotImplementedException(); //Use fuel Array


        public int microCount; //How many microbots
        public int captainSave; //How many does the captain get to save?
        public float duration; //How long should these remain on the field

        public float age; //How long has the microbots been on the field


        public override ItemDisplayRuleDict CreateItemDisplayRules()
        {
            return new ItemDisplayRuleDict();
        }

        public override void Hooks()
        {
            //Shouldn't need any hooks
        }

        public override void Init(ConfigFile config)
        {

            //CreateConfig(config);
            //CreateLang();
            //CreateEquipment();
            //Hooks();

        }

        private void CreateConfig(ConfigFile config)
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

            duration = config.Bind<float>(
                "Equipment: " + EquipmentName,
                "Duration",
                15f,
                "How long will the Defense Microbots last once activated?").Value;
        }

        protected override bool ActivateEquipment(EquipmentSlot equipmentSlot)
        {
            if (!equipmentSlot.characterBody) { return false; } // Check to see if you have a character body
            if (!equipmentSlot.characterBody.inventory) { return false; } // Make sure that body has an inventory
                                                                          //var body = equipmentSlot.characterBody;
                                                                          //int removeCount = microCount;
                                                                          ////Add the items to the inventory
                                                                          //body.inventory.GiveItem(RoR2Content.Items.CaptainDefenseMatrix, microCount);

            //SurvivorDef survivorDef;
            ////Start a stopwatch

            ////Once stopwatch is complete, check characterbody to see if it is Captain

            //if (survivorDef == RoR2Content.Survivors.Captain)
            //{
            //    removeCount = microCount - captainSave;
            //}

            //for ()
            //{

            //    body.inventory.RemoveItem(RoR2Content.Items.CaptainDefenseMatrix, 1);

            //}

            //Remove Appropriate amount of Microbots

            return true;

        }
    }
}
