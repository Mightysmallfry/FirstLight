using EntityStates;
using On.RoR2.EntityLogic;
using R2API;
using RoR2;
using RoR2.EntityLogic;
using RoR2.Projectile;
using UnityEngine;
using UnityEngine.UIElements;

namespace FirstLightMod.Survivors.Farmer.SkillStates
{
    public class PulseRifle : BaseSkillState
    {
        private static float damageCoefficient = FarmerStaticValues.assaultRifleDamageCoefficient;


        [Tooltip("The number of bullets in a burst")]
        private static int maximumBulletCount = 3;
        private static float bulletStopwatch;

        public static float procCoefficient = 1f;
        public static float baseDuration = 1.0f;
        public static float force = 100f;
        public static float recoil = 1f;
        public static float range = 256f;
        public static GameObject tracerEffectPrefab = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/Tracers/TracerGoldGat");


        [Tooltip("The time between each bullet firing")]
        private float timeBetweenShots;
        private float duration;
        private string muzzleString;
        private bool hasFired;
        private int bulletsFired;


        public override void OnEnter()
        {
            base.OnEnter();
            this.duration = baseDuration / this.attackSpeedStat;
            this.timeBetweenShots = (baseDuration/maximumBulletCount) / this.attackSpeedStat;
            base.characterBody.SetAimTimer(2f);
            this.muzzleString = "Muzzle";

            bulletsFired = 0;
            PulseRifle.bulletStopwatch = 0f;

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


                base.characterBody.AddSpreadBloom(0.1f);
                EffectManager.SimpleMuzzleFlash(EntityStates.Commando.CommandoWeapon.FirePistol2.muzzleEffectPrefab, base.gameObject, this.muzzleString, false);
                Util.PlaySound(EntityStates.EngiTurret.EngiTurretWeapon.FireGauss.attackSoundString, base.gameObject);

                if (base.isAuthority)
                {
                    Ray aimRay = base.GetAimRay();
                    base.AddRecoil(-1f * PulseRifle.recoil, -2f * PulseRifle.recoil, -0.5f * PulseRifle.recoil, 0.5f * PulseRifle.recoil);


                    new BulletAttack
                    {
                        bulletCount = 1,
                        aimVector = aimRay.direction,
                        origin = aimRay.origin,
                        damage = PulseRifle.damageCoefficient * this.damageStat,
                        damageColorIndex = DamageColorIndex.Default,
                        damageType = DamageType.Generic,
                        falloffModel = BulletAttack.FalloffModel.DefaultBullet,
                        maxDistance = PulseRifle.range,
                        force = PulseRifle.force,
                        hitMask = LayerIndex.CommonMasks.bullet,
                        minSpread = 0f,
                        maxSpread = 0f,
                        isCrit = base.RollCrit(),
                        owner = base.gameObject,
                        muzzleName = muzzleString,
                        smartCollision = false,
                        procChainMask = default(ProcChainMask),
                        procCoefficient = procCoefficient,
                        radius = 0.75f,
                        sniper = false,
                        stopperMask = LayerIndex.CommonMasks.bullet,
                        weapon = null,
                        tracerEffectPrefab = PulseRifle.tracerEffectPrefab,
                        spreadPitchScale = 0f,
                        spreadYawScale = 0f,
                        queryTriggerInteraction = QueryTriggerInteraction.UseGlobal,
                        //hitEffectPrefab = EntityStates.Engi.EngiWeapon.FireSeekerGrenades.hitEffectPrefab,
                        hitEffectPrefab = EntityStates.Commando.CommandoWeapon.FirePistol2.hitEffectPrefab,
                    }.Fire();
                }

                this.bulletsFired++;

            }
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            PulseRifle.bulletStopwatch += UnityEngine.Time.fixedDeltaTime;
            if (PulseRifle.bulletStopwatch >= this.timeBetweenShots && this.bulletsFired < PulseRifle.maximumBulletCount)
            {
                this.Fire();
                PulseRifle.bulletStopwatch -= this.timeBetweenShots;
            }

            
            if (base.fixedAge >= this.duration && PulseRifle.maximumBulletCount == this.bulletsFired && base.isAuthority)
            {
                this.bulletsFired = 0;

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