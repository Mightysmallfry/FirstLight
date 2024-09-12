using EntityStates;
using EntityStates.Toolbot;
using R2API;
using R2API.ContentManagement;
using RoR2;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;

namespace FirstLightMod.Survivors.Farmer.SkillStates
{
    public class ThrowRazorGrenade : BaseState
    {
        //public static float damageCoefficient = 6.9f;
        //public static float procCoefficient = 1f;
        public static float duration = 0.6f;
        public Vector3 aimPoint;


        public override void OnEnter()
        {
            base.OnEnter();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (base.isAuthority && base.fixedAge >= ThrowRazorGrenade.duration)
            {
                this.outer.SetNextStateToMain();
            }
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }
}
