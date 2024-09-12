using EntityStates;
using R2API;
using RoR2;
using RoR2.Projectile;
using UnityEngine;

namespace FirstLightMod.Survivors.Farmer.SkillStates
{
    public class LightningGrove : BaseSkillState
    {
        // This is the empowered bungal grove
        //
        // May need a deploying state and a deployed stated. One would throw out the projectile and create it and the other handles the healing effect and radius
        //

        public static float healCoefficient = FarmerStaticValues.groveHealCoefficient;
        public static float procCoefficient = 1f;
        public static float baseDuration = 0f;
        public static float force = 200f;
        public static float recoil = 3f;
        public static float range = 64f;
        public static GameObject tracerEffectPrefab = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/Tracers/TracerGoldGat");

        private float duration;
        private float fireTime;
        private bool hasFired;
        private string muzzleString;

        public override void OnEnter()
        {
            base.OnEnter();
            this.duration = LightningGrove.baseDuration; // / this.attackSpeedStat // should not have to worry about this attack speed stat unless we split deploying and deployed states
            this.fireTime = 0.2f * this.duration;
            base.characterBody.SetAimTimer(2f);
            this.muzzleString = "Muzzle";


            base.PlayAnimation("LeftArm, Override", "ShootGun", "ShootGun.playbackRate", 1.8f);
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        private void Fire()
        {
            if (!this.hasFired)
            {
                this.hasFired = true;

                
                EffectManager.SimpleMuzzleFlash(EntityStates.Commando.CommandoWeapon.FirePistol2.muzzleEffectPrefab, base.gameObject, this.muzzleString, false);
                Util.PlaySound("HenryShootPistol", base.gameObject);

                //This should fire a projectile now that is the prefab of the engiMine... Hopefully

                if (base.isAuthority)
                {
                    Ray aimRay = base.GetAimRay();

                    FireProjectileInfo pinfo = new FireProjectileInfo
                    {
                        projectilePrefab = FarmerAssets.bombProjectilePrefab,
                        position = aimRay.origin,
                        rotation = Util.QuaternionSafeLookRotation(aimRay.direction),
                        owner = base.gameObject,
                        damage = BungalGrove.healCoefficient,
                        force = 10f,
                        crit = base.RollCrit(),
                        damageColorIndex = DamageColorIndex.Heal,
                        target = null,
                        speedOverride = 16f,
                        fuseOverride = -1f,
                    };

                    ProjectileManager.instance.FireProjectile(pinfo);

                }
            }
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (base.fixedAge >= this.fireTime)
            {
                this.Fire();
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