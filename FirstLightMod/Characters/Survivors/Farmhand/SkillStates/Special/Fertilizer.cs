using EntityStates;
using FirstLightMod.Survivors.Farmer;
using RoR2;
using RoR2.Skills;
using RoR2.Projectile;
using UnityEngine;

namespace FirstLightMod.Survivors.Farmer.SkillStates
{
    public class Fertilizer : BaseSkillState
    {
        #region Initial setup for skill


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

        private int superPrimary = 0;
        private int superSecondary = 0;
        private int superUtility = 0;

        private SkillStatus primaryStatus;
        private SkillStatus secondaryStatus;
        private SkillStatus utilityStatus;

        public bool playSound = true;


        #endregion

        public override void OnEnter()
        {
            //Replace the skills with the Super Versions
            if (base.isAuthority)
            {
                //Primaries
                if (skillLocator.primary.baseSkill == FarmerSurvivor.cannonSkillDef)
                {
                    primaryStatus = new SkillStatus(skillLocator.primary.stock, skillLocator.primary.rechargeStopwatch);
                    base.skillLocator.primary.SetSkillOverride(this, FarmerSurvivor.superCannonSkillDef, GenericSkill.SkillOverridePriority.Contextual);
                    superPrimary = 1;
                }
                else if (skillLocator.primary.baseSkill == FarmerSurvivor.shotgunSkillDef)
                {
                    primaryStatus = new SkillStatus(skillLocator.primary.stock, skillLocator.primary.rechargeStopwatch);
                    base.skillLocator.primary.SetSkillOverride(this, FarmerSurvivor.superShotgunSkillDef, GenericSkill.SkillOverridePriority.Contextual);
                    superPrimary = 1;
                }

                //Secondary
                if (skillLocator.secondary.baseSkill == FarmerSurvivor.shovelSkillDef)
                {
                    secondaryStatus = new SkillStatus(skillLocator.secondary.stock, skillLocator.secondary.rechargeStopwatch);
                    base.skillLocator.secondary.SetSkillOverride(this, FarmerSurvivor.pitchforkSkillDef, GenericSkill.SkillOverridePriority.Contextual);
                    superSecondary = 1;
                }
                else if (skillLocator.secondary.baseSkill == FarmerSurvivor.grenadeSkillDef)
                {
                    secondaryStatus = new SkillStatus(skillLocator.secondary.stock, skillLocator.secondary.rechargeStopwatch);
                    base.skillLocator.secondary.SetSkillOverride(this, FarmerSurvivor.grenadeSuperSkillDef, GenericSkill.SkillOverridePriority.Contextual);
                    superSecondary = 1;
                }
                else if (skillLocator.secondary.baseSkill == FarmerSurvivor.reapSkillDef)
                {
                    secondaryStatus = new SkillStatus(skillLocator.secondary.stock, skillLocator.secondary.rechargeStopwatch);
                    base.skillLocator.secondary.SetSkillOverride(this, FarmerSurvivor.reapSkillDef, GenericSkill.SkillOverridePriority.Contextual);
                    superSecondary = 1;
                }

                //Utility
                if (skillLocator.utility.baseSkill == FarmerSurvivor.groveSkillDef)
                {
                    utilityStatus = new SkillStatus(skillLocator.utility.stock, skillLocator.utility.rechargeStopwatch);
                    base.skillLocator.utility.SetSkillOverride(this, FarmerSurvivor.groveSuperSkillDef, GenericSkill.SkillOverridePriority.Contextual);
                    superUtility = 1;
                }
                else if (skillLocator.utility.baseSkill == FarmerSurvivor.mortarSkillDef)
                {
                    utilityStatus = new SkillStatus(skillLocator.utility.stock, skillLocator.utility.rechargeStopwatch);
                    base.skillLocator.utility.SetSkillOverride(this, FarmerSurvivor.mortarSuperSkillDef, GenericSkill.SkillOverridePriority.Contextual);
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

            //Check if a super ability is used
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
            //Revert the abilities back to the normal versions
            if (base.isAuthority)
            {
                //Primary
                if (superPrimary != 0)
                {
                    if (skillLocator.primary.baseSkill == FarmerSurvivor.cannonSkillDef)
                    {
                        base.skillLocator.primary.UnsetSkillOverride(this, FarmerSurvivor.superCannonSkillDef, GenericSkill.SkillOverridePriority.Contextual);
                    }
                    else if (skillLocator.primary.baseSkill == FarmerSurvivor.shotgunSkillDef)
                    {
                        base.skillLocator.primary.UnsetSkillOverride(this, FarmerSurvivor.superShotgunSkillDef, GenericSkill.SkillOverridePriority.Contextual);
                    }
                base.skillLocator.primary.stock = primaryStatus.stock;
                base.skillLocator.primary.rechargeStopwatch = primaryStatus.stopwatch;
                }

                //Secondary
                if (superSecondary != 0)
                {
                    if (skillLocator.secondary.baseSkill == FarmerSurvivor.shovelSkillDef)
                    {
                        base.skillLocator.secondary.UnsetSkillOverride(this, FarmerSurvivor.pitchforkSkillDef, GenericSkill.SkillOverridePriority.Contextual);
                    }
                    else if (skillLocator.secondary.baseSkill == FarmerSurvivor.grenadeSkillDef)
                    {
                        base.skillLocator.secondary.UnsetSkillOverride(this, FarmerSurvivor.grenadeSuperSkillDef, GenericSkill.SkillOverridePriority.Contextual);
                    }
                    else if (skillLocator.secondary.baseSkill == FarmerSurvivor.reapSkillDef)
                    {
                        base.skillLocator.secondary.UnsetSkillOverride(this, FarmerSurvivor.reapSkillDef, GenericSkill.SkillOverridePriority.Contextual);
                    }

                    base.skillLocator.secondary.stock = secondaryStatus.stock;
                    base.skillLocator.secondary.rechargeStopwatch = secondaryStatus.stopwatch;
                }

                //Utility
                if (superUtility != 0)
                {
                    if (skillLocator.utility.baseSkill == FarmerSurvivor.groveSkillDef)
                    {
                        base.skillLocator.utility.UnsetSkillOverride(this, FarmerSurvivor.groveSuperSkillDef, GenericSkill.SkillOverridePriority.Contextual);
                    }
                    else if (skillLocator.utility.baseSkill == FarmerSurvivor.mortarSkillDef)
                    {
                        base.skillLocator.utility.UnsetSkillOverride(this, FarmerSurvivor.mortarSuperSkillDef, GenericSkill.SkillOverridePriority.Contextual);
                    }

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