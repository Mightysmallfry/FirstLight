using System;
using System.Collections.Generic;
using System.Text;
using EntityStates;
using RoR2;

namespace FirstLightMod.Survivors.WeaponMaster.SkillStates
{
    public class HandCrossbowStart : BaseSkillState
    {

        public static float baseDuration = 3f; // Later Make it a static value
        public float duration;

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Pain;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            this.duration = HandCrossbowStart.baseDuration / this.attackSpeedStat;
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (base.isAuthority && this.duration <= base.fixedAge)
            {
                this.outer.SetNextState(new HandCrossbowPaint());
            }
        }

    }
}
