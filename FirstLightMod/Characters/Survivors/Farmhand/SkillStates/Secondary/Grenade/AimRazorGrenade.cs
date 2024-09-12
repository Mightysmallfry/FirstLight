using System;
using System.Collections.Generic;
using System.Text;
using RoR2.Projectile;
using UnityEngine;
using EntityStates;

namespace FirstLightMod.Survivors.Farmer.SkillStates
{
    public class AimRazorGrenade : AimThrowableBase 
    {
        EntityStates.Toolbot.AimStunDrone aimStunDrone = new EntityStates.Toolbot.AimStunDrone();

        public override void OnEnter()
        {
            projectilePrefab = aimStunDrone.projectilePrefab;
            endpointVisualizerPrefab = aimStunDrone.endpointVisualizerPrefab;

            //These are all AimThrowableBase variables
            this.endpointVisualizerRadiusScale = FarmerStaticValues.grenadeRazorRadius;
            this.damageCoefficient = FarmerStaticValues.grenadeRazorDamageCoefficient;
            this.detonationRadius = FarmerStaticValues.grenadeRazorRadius;
            this.rayRadius = FarmerStaticValues.grenadeRazorRadius;

            this.setFuse = false;
            this.baseMinimumDuration = 0.0f; //Pretty sure this is the same as a windup. We don't want this for the most part
            this.maxDistance = 50f;

            base.OnEnter();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            StartAimMode();

            endpointVisualizerRadiusScale = Mathf.Lerp(endpointVisualizerRadiusScale, rayRadius, 0.5f);
        }

        
        public override EntityState PickNextState()
        {
            return new ThrowRazorGrenade() { aimPoint = currentTrajectoryInfo.hitPoint };
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }

    }
}
