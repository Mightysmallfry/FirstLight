using RoR2;
using R2API;
using EntityStates;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using RoR2.Projectile;

namespace FirstLightMod.Survivors.Farmer.SkillStates
{
    public class AimMortarShell : AimThrowableBase
    {

        EntityStates.Treebot.Weapon.AimMortar2 mortar = new EntityStates.Treebot.Weapon.AimMortar2();

        public override void OnEnter()
        {

            //Ideally I just load my own projectile prefab instead.
            projectilePrefab = mortar.projectilePrefab; 
            endpointVisualizerPrefab = mortar.endpointVisualizerPrefab;

            //These are all AimThrowableBase variables
            this.endpointVisualizerRadiusScale = FarmerStaticValues.mortarRadius;
            this.damageCoefficient = FarmerStaticValues.mortarDamageCoefficient;
            this.detonationRadius = FarmerStaticValues.mortarRadius;
            this.rayRadius = FarmerStaticValues.mortarRadius;

            this.setFuse = false;
            this.baseMinimumDuration = 0.0f; //Pretty sure this is the same as a windup. We don't want this for the most part
            this.maxDistance = 100f;

            base.OnEnter();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            StartAimMode();

            endpointVisualizerRadiusScale = Mathf.Lerp(endpointVisualizerRadiusScale, rayRadius, 0.5f);
        }

        public override void ModifyProjectile(ref FireProjectileInfo fireProjectileInfo)
        {
            base.ModifyProjectile(ref fireProjectileInfo);
            fireProjectileInfo.position = this.currentTrajectoryInfo.hitPoint;
            fireProjectileInfo.rotation = Quaternion.identity;
            fireProjectileInfo.speedOverride = 0f;
        }


        public override EntityState PickNextState()
        {
            return new FireMortarShell();
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }
}
