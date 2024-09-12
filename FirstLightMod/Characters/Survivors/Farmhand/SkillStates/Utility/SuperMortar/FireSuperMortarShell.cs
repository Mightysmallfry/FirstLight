using RoR2;
using R2API;
using EntityStates;
using System;
using System.Collections.Generic;
using System.Text;
using EntityStates.Huntress;
using UnityEngine;
using RoR2.Projectile;

namespace FirstLightMod.Survivors.Farmer.SkillStates
{
    public class FireSuperMortarShell : BaseState
    {
        public static string fireSound = EntityStates.Treebot.Weapon.FireMortar2.fireSound;
        public static float baseDuration = 0.5f;

        public static GameObject projectilePrefab;
        private float duration;

        private static float damageCoefficient = FarmerStaticValues.mortarSuperDamageCoefficient;
        private static float maxDistance = 100f;
        private static float force = 10f;

        //EntityStates.Treebot.Weapon.AimMortar2 mortar = new EntityStates.Treebot.Weapon.AimMortarRain();


        public override void OnEnter()
        {
            //projectilePrefab = mortar.projectilePrefab;
            projectilePrefab = ArrowRain.projectilePrefab;

            base.OnEnter();
            this.duration = FireSuperMortarShell.baseDuration / this.attackSpeedStat;
            Util.PlaySound(FireSuperMortarShell.fireSound, base.gameObject);

            if (base.isAuthority)
            {
                this.Fire();
            }
        }

        private void Fire()
        {
            RaycastHit raycastHit;
            Vector3 aimPoint;
            if (base.inputBank.GetAimRaycast(FireSuperMortarShell.maxDistance, out raycastHit))
            {
                aimPoint = raycastHit.point;
            }
            else
            {
                aimPoint = base.inputBank.GetAimRay().GetPoint(FireSuperMortarShell.maxDistance);
            }
            FireProjectileInfo fireProjectileInfo = new FireProjectileInfo
            {
                projectilePrefab = FireSuperMortarShell.projectilePrefab,
                position = aimPoint,
                rotation = Quaternion.identity,
                owner = base.gameObject,
                damage = FireSuperMortarShell.damageCoefficient * this.damageStat,
                force = FireSuperMortarShell.force,
                crit = base.RollCrit(),
                damageColorIndex = DamageColorIndex.Default,
                damageTypeOverride = DamageType.Freeze2s,
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
