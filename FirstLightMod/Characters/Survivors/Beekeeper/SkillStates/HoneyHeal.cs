using RoR2;
using R2API;
using UnityEngine;
using EntityStates;

namespace FirstLightMod.Survivors.Beekeeper.SkillStates
{
    public class HoneyHeal : BaseSkillState
    {

        private float healAmount = BeekeeperStaticValues.honeyHealPercentage/100f;
        private float duration = 1f;

        public override void OnEnter()
        {
            base.OnEnter();
            
            if (healAmount > 0)
            {
                ProcChainMask procChainMask = new ProcChainMask();
                procChainMask.AddProc(ProcType.VoidSurvivorCrush);
                base.healthComponent.HealFraction(healAmount, procChainMask);
            }
        }


        public override void OnExit()
        {
            base.OnExit();
        }



        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (base.isAuthority && base.fixedAge >= this.duration)
            {
                this.outer.SetNextStateToMain();
            }
        }



        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }
}
