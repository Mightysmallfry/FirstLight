using EntityStates;
using FirstLightMod.Modules.Survivors;
using RoR2;
using RoR2.Skills;
using RoR2.Projectile;
using UnityEngine;

namespace FirstLightMod.SkillStates
{
    public class Fertilizer : BaseSkillState
    {
        #region Initial setup for skill

        private int superPrimary = 0;
        private int superSecondary = 0;
        private int superUtility = 0;

        private SkillStatus primaryStatus, secondaryStatus, utilityStatus;

        public bool playSound = true;

        private class SkillStatus
        {
            public int stock;
            public float stopwatch;
            public SkillStatus(int stock, float stopwatch)
            {
                this.stock = stock;
                this.stopwatch = stopwatch;
            }
        }

        #endregion

        public override void OnEnter()
        {

            if (base.isAuthority)
            {
                if (skillLocator.primary.baseSkill == FarmerCharacter.cannonSkillDef)
                {
                    primaryStatus = new SkillStatus(skillLocator.primary.stock, skillLocator.primary.rechargeStopwatch);
                    base.skillLocator.primary.SetSkillOverride(this, FarmerCharacter.superCannonSkillDef, GenericSkill.SkillOverridePriority.Contextual);
                    superPrimary = 1;
                }
                else if (skillLocator.primary.baseSkill == FarmerCharacter.shotgunSkillDef)
                {
                    primaryStatus = new SkillStatus(skillLocator.primary.stock, skillLocator.primary.rechargeStopwatch);
                    base.skillLocator.primary.SetSkillOverride(this, FarmerCharacter.superShotgunSkillDef, GenericSkill.SkillOverridePriority.Contextual);
                    superPrimary = 1;
                }

                //if (skillLocator.secondary.baseSkill == ChefMod.ChefPlugin.secondaryDef)
                //{
                //    secondaryStatus = new SkillStatus(skillLocator.secondary.stock, skillLocator.secondary.rechargeStopwatch);
                //    base.skillLocator.secondary.SetSkillOverride(this, ChefMod.ChefPlugin.boostedSecondaryDef, GenericSkill.SkillOverridePriority.Contextual);
                //    boostSecondary = 1;
                //}
                //else if (skillLocator.secondary.baseSkill == ChefMod.ChefPlugin.altSecondaryDef)
                //{
                //    secondaryStatus = new SkillStatus(skillLocator.secondary.stock, skillLocator.secondary.rechargeStopwatch);
                //    base.skillLocator.secondary.SetSkillOverride(this, ChefMod.ChefPlugin.boostedAltSecondaryDef, GenericSkill.SkillOverridePriority.Contextual);
                //    boostSecondary = 1;
                //}

                if (skillLocator.utility.baseSkill == FarmerCharacter.groveSkillDef)
                {
                    utilityStatus = new SkillStatus(skillLocator.utility.stock, skillLocator.utility.rechargeStopwatch);
                    base.skillLocator.utility.SetSkillOverride(this, FarmerCharacter.superGroveSkillDef, GenericSkill.SkillOverridePriority.Contextual);
                    superUtility = 1;
                }

                //Fix skill stocks
                base.skillLocator.primary.stock = base.skillLocator.primary.maxStock;
                base.skillLocator.secondary.stock = base.skillLocator.secondary.maxStock;
                base.skillLocator.utility.stock = base.skillLocator.utility.maxStock;
            }


            base.OnEnter();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            int maxStock = (superPrimary * skillLocator.primary.maxStock) + (superSecondary * skillLocator.secondary.maxStock) + (superUtility * skillLocator.utility.maxStock);
            int currentStock = (superPrimary * skillLocator.primary.stock) + (superSecondary * skillLocator.secondary.stock) + (superUtility * skillLocator.utility.stock);
            if ((currentStock) < maxStock && base.isAuthority)
            {
                NextState();
                return;
            }
        }

        public virtual void NextState()
        {
            this.outer.SetNextStateToMain();
        }

        public override void OnExit()
        {
            if (base.isAuthority)
            {
                if (superPrimary != 0)
                {
                    if (skillLocator.primary.baseSkill == FarmerCharacter.cannonSkillDef)
                    {
                        base.skillLocator.primary.UnsetSkillOverride(this, FarmerCharacter.superCannonSkillDef, GenericSkill.SkillOverridePriority.Contextual);
                    }
                    else if (skillLocator.primary.baseSkill == FarmerCharacter.shotgunSkillDef)
                    {
                        base.skillLocator.primary.UnsetSkillOverride(this, FarmerCharacter.superShotgunSkillDef, GenericSkill.SkillOverridePriority.Contextual);
                    }
                base.skillLocator.primary.stock = primaryStatus.stock;
                base.skillLocator.primary.rechargeStopwatch = primaryStatus.stopwatch;
                }
                //if (boostSecondary != 0)
                //{
                //    if (skillLocator.secondary.baseSkill == ChefMod.ChefPlugin.secondaryDef)
                //    {
                //        base.skillLocator.secondary.UnsetSkillOverride(this, ChefMod.ChefPlugin.boostedSecondaryDef, GenericSkill.SkillOverridePriority.Contextual);
                //    }
                //    else if (skillLocator.secondary.baseSkill == ChefMod.ChefPlugin.altSecondaryDef)
                //    {
                //        base.skillLocator.secondary.UnsetSkillOverride(this, ChefMod.ChefPlugin.boostedAltSecondaryDef, GenericSkill.SkillOverridePriority.Contextual);
                //    }
                //    base.skillLocator.secondary.stock = secondaryStatus.stock;
                //    base.skillLocator.secondary.rechargeStopwatch = secondaryStatus.stopwatch;
                //}
                if (superUtility != 0)
                {
                    base.skillLocator.utility.UnsetSkillOverride(this, FarmerCharacter.superGroveSkillDef, GenericSkill.SkillOverridePriority.Contextual);
                    base.skillLocator.utility.stock = utilityStatus.stock;
                    base.skillLocator.utility.rechargeStopwatch = utilityStatus.stopwatch;
                }
            }

            base.OnExit();
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Skill;
        }

    }
}