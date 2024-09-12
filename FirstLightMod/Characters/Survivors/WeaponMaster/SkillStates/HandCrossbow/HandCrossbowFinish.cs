using System;
using System.Collections.Generic;
using System.Text;
using EntityStates;
using RoR2;
using UnityEngine;

namespace FirstLightMod.Survivors.WeaponMaster.SkillStates.HandCrossbow
{
    public class HandCrossbowFinish : BaseSkillState
    {
        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Pain;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            if (base.isAuthority)
            {
                this.outer.SetNextState(new Idle());
            }
        }
    }
}
