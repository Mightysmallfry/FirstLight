using RoR2;
using R2API;
using EntityStates;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace FirstLightMod.SkillStates
{
    public class MortarShell : BaseState
    {
        public static float baseDuration = 5f;
        private float duration;

        public override void OnEnter()
        {
            base.OnEnter();
            this.duration = MortarShell.baseDuration / this.attackSpeedStat;

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
