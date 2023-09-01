using RoR2;
using R2API;
using UnityEngine;
using FirstLightMod.Modules.Unlocks;

namespace FirstLightMod.Content.Achievements
{
    public class FarmerUnlockAchievement : GenericModdedUnlockable
    {
        public override string AchievementTokenPrefix => FirstLightPlugin.DEVELOPER_PREFIX + "_HENRY_BODY_MASTERY";

        public override string AchievementSpriteName => "texMasteryAchievement";

        public override string PrerequisiteUnlockableIdentifier => FirstLightPlugin.DEVELOPER_PREFIX + "_HENRY_BODY_UNLOCKABLE_REWARD_ID";

        public class FarmerUnlockAchievementServer : RoR2.Achievements.BaseServerAchievement
        {
            public bool hasMushroom; //Bungus
            public bool hasVoidMushroom;
            public bool hasLeechingSeed;
            //public bool hasInterstellarDeskPlant;
            public bool hasClover;
            public bool hasVoidClover;
            public bool hasCorpseBloom;
            public bool hasForeignFruit;

            public override void OnInstall()
            {
                base.OnInstall();
                On.RoR2.CharacterMaster.OnInventoryChanged += this.CheckItems;
                Run.onRunStartGlobal += ResetOnRunStart;
            }

            public override void OnUninstall()
            {
                base.OnUninstall();
                On.RoR2.CharacterMaster.OnInventoryChanged -= this.CheckItems;
                Run.onRunStartGlobal -= ResetOnRunStart;
            }

            private void CheckItems(On.RoR2.CharacterMaster.orig_OnInventoryChanged orig, CharacterMaster self)
            {
                orig(self);

                if (self.teamIndex != TeamIndex.Player)
                {
                    if (!hasMushroom && self.inventory.GetItemCount(RoR2Content.Items.Mushroom) > 0 )
                    {
                        hasMushroom = true;
                    }

                    if (!hasVoidMushroom && self.inventory.GetItemCount(RoR2.DLC1Content.Items.MushroomVoid) > 0 )
                    {
                        hasVoidMushroom = true;
                    }

                    if (!hasLeechingSeed && self.inventory.GetItemCount(RoR2Content.Items.Seed) > 0 )
                    {
                        hasLeechingSeed = true;
                    }

                    if (!hasClover && self.inventory.GetItemCount(RoR2Content.Items.Clover) > 0 )
                    {
                        hasClover = true;
                    }

                    if (!hasVoidClover && self.inventory.GetItemCount(RoR2.DLC1Content.Items.CloverVoid) > 0 )
                    {
                        hasVoidClover = true;
                    }

                    if (!hasCorpseBloom && self.inventory.GetItemCount(RoR2Content.Items.RepeatHeal) > 0 )
                    {
                        hasCorpseBloom = true;
                    }

                    if (!hasForeignFruit && self.inventory.GetEquipmentIndex() ==  RoR2Content.Equipment.Fruit.equipmentIndex)
                    {
                        hasForeignFruit = true;
                    }

                    if (hasMushroom && hasVoidMushroom && hasCorpseBloom && hasLeechingSeed && hasClover && hasVoidClover && hasForeignFruit)
                    {
                        base.Grant();
                    }
                }
            }

            private void ResetOnRunStart(Run run)
            {
                this.ResetItems();
            }

            private void ResetItems()
            {
                this.hasMushroom = false;
                this.hasVoidMushroom = false;
                this.hasLeechingSeed = false;
                this.hasClover = false;
                this.hasVoidClover = false;
                this.hasCorpseBloom = false;
                this.hasForeignFruit = false;
            }

        }
    }
}
