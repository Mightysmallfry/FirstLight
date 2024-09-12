using EntityStates;
using EntityStates.Engi.EngiWeapon;
using FirstLightMod.Modules;
using RoR2;
using RoR2.Projectile;
using System;
using UnityEngine;

namespace FirstLightMod.Survivors.Farmer.SkillStates
{
    public class Shovel : BaseSkillState
    {
        public static float damageCoefficient = FarmerStaticValues.shovelBaseDamageCoefficient;
        public static float procCoefficient = 1f;
        public static float baseDuration = 0.5f;
        public static float force = 400f;
        public static float recoil = 3f;
        public static float range = 256f;

        private float duration;
        private float fireTime;
        private bool hasFired;
        private string muzzleString;

        private ChildLocator childLocator;
        private GameObject shovel;

        public override void OnEnter()
        {
            base.OnEnter();
            this.duration = Shovel.baseDuration / this.attackSpeedStat;
            this.fireTime = 0.2f * this.duration;
            base.characterBody.SetAimTimer(2f);
            this.muzzleString = "Muzzle";

            childLocator = base.GetModelChildLocator();
            //shovel = childLocator.FindChild("Shovel").gameObject;  // This was causing an issue

            base.PlayAnimation("LeftArm, Override", "ShootGun", "ShootGun.playbackRate", 1.8f);
        }

        public override void OnExit()
        {
            shovel.SetActive(true);

            base.OnExit();
        }

        private void FireShovel()
        {
            if (!this.hasFired)
            {
                //Ray aimRay = base.GetAimRay();
                this.hasFired = true;
                //Util.PlaySound(FireMines.throwMineSoundString, base.gameObject);
                //FireProjectileInfo fireProjectileInfo = new FireProjectileInfo();
                //fireProjectileInfo.damage = Shovel.damageCoefficient * base.damageStat;
                //fireProjectileInfo.force = Shovel.force;
                //fireProjectileInfo.owner = base.gameObject;
                //fireProjectileInfo.crit = base.RollCrit();
                //fireProjectileInfo.damageColorIndex = DamageColorIndex.Default;
                //fireProjectileInfo.position = aimRay.origin;
                //fireProjectileInfo.rotation = Util.QuaternionSafeLookRotation(aimRay.direction);
                //fireProjectileInfo.projectilePrefab = Projectiles.shovelPrefab; //Projectiles.javelinPrefab;
                //fireProjectileInfo.target = null;
                //fireProjectileInfo.speedOverride = 16f;
                //fireProjectileInfo.fuseOverride = -1f;

                //ProjectileManager.instance.FireProjectile(fireProjectileInfo);
            }
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (base.fixedAge >= this.fireTime) // && !hasFired //You can have this either in or out of the function
            {
                shovel.SetActive(false);
                //this.FireShovel();
            }

            if (base.fixedAge >= this.duration && base.isAuthority)
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

