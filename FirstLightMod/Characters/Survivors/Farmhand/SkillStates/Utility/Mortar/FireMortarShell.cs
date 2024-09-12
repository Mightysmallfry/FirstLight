using RoR2;
using R2API;
using EntityStates;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using RoR2.Projectile;

namespace FirstLightMod.Survivors.Farmer.SkillStates
{
    public class FireMortarShell : BaseState
    {
        public static string fireSound = EntityStates.Treebot.Weapon.FireMortar2.fireSound;
        public static float baseDuration = 0.5f;

        public static GameObject projectilePrefab;
        private float duration;

        private static float damageCoefficient = FarmerStaticValues.mortarDamageCoefficient;
        private static float maxDistance = 100f;
        private static float force = 10f;

        EntityStates.Treebot.Weapon.AimMortar2 mortar = new EntityStates.Treebot.Weapon.AimMortar2();


        public override void OnEnter()
        {
            projectilePrefab = mortar.projectilePrefab;

            base.OnEnter();
            this.duration = FireMortarShell.baseDuration / this.attackSpeedStat;
            Util.PlaySound(FireMortarShell.fireSound, base.gameObject);

            if (base.isAuthority)
            {
                this.Fire();
            }
        }

        private void Fire()
        {
            RaycastHit raycastHit;
            Vector3 aimPoint;
            if (base.inputBank.GetAimRaycast(FireMortarShell.maxDistance, out raycastHit))
            {
                aimPoint = raycastHit.point;
            }
            else
            {
                aimPoint = base.inputBank.GetAimRay().GetPoint(FireMortarShell.maxDistance);
            }
            FireProjectileInfo fireProjectileInfo = new FireProjectileInfo
            {
                projectilePrefab = FireMortarShell.projectilePrefab,
                position = aimPoint,
                rotation = Quaternion.identity,
                owner = base.gameObject,
                damage = FireMortarShell.damageCoefficient * this.damageStat,
                force = FireMortarShell.force,
                crit = base.RollCrit(),
                damageColorIndex = DamageColorIndex.Default,
                target = null,
                speedOverride = 0f,
                fuseOverride = -1f
            };
            ProjectileManager.instance.FireProjectile(fireProjectileInfo);

        }
  

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (base.isAuthority && base.fixedAge >= this.duration)
            {
                this.outer.SetNextStateToMain();
                return;
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
     
        }
    }
}
