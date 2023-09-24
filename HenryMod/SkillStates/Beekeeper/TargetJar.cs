using RoR2;
using R2API;
using UnityEngine;
using EntityStates;

namespace FirstLightMod.SkillStates.Beekeeper
{
    public class TargetJar : BaseSkillState
    {



        public override void OnEnter()
        {
            base.OnEnter();


        }


        public override void OnExit()
        {
            base.OnExit();
        }



        public override void FixedUpdate()
        {
            base.FixedUpdate();
            this.outer.SetNextStateToMain();
        }



        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }
}
