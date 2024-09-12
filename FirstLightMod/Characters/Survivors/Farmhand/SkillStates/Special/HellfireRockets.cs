using EntityStates;
using R2API;
using RoR2;
using RoR2.Projectile;
using UnityEngine;
using UnityEngine.UIElements;

namespace FirstLightMod.Survivors.Farmer.SkillStates
{
    public class HellFireRockets : BaseSkillState
    {
        public static float damageCoefficient = FarmerStaticValues.hellfireDamageCoefficient;
        public static float explosionRadius = FarmerStaticValues.hellfireDamageCoefficient;
        public static float procCoefficient = 1f;
        public static float baseDuration = 0.5f;
        public static float force = 800f;
        public static float recoil = 3f;
        public static float range = 256f;
        public static GameObject tracerEffectPrefab = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/Tracers/TracerGoldGat");

        private float duration;
        private float fireDuration;
        private bool hasFired;
        private string muzzleString;


        public override void OnEnter()
        {
            base.OnEnter();
            this.duration = Cannon.baseDuration / this.attackSpeedStat;
            this.fireDuration = 0.0f; //0.2f * this.duration;
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

                base.characterBody.AddSpreadBloom(0.0f);
                EffectManager.SimpleMuzzleFlash(EntityStates.Commando.CommandoWeapon.FirePistol2.muzzleEffectPrefab, base.gameObject, this.muzzleString, false);
                Util.PlaySound(EntityStates.EngiTurret.EngiTurretWeapon.FireGauss.attackSoundString, base.gameObject);
                //Util.PlaySound(EntityStates.Engi.EngiWeapon.FireConcussionBlast.attackSoundString, base.gameObject);

                if (base.isAuthority)
                {
                    Ray aimRay = base.GetAimRay();
                    base.AddRecoil(-1f * Cannon.recoil, -2f * Cannon.recoil, -0.5f * Cannon.recoil, 0.5f * Cannon.recoil);


                    new BulletAttack
                    {
                        bulletCount = 1,
                        aimVector = aimRay.direction,
                        origin = aimRay.origin,
                        damage = Cannon.damageCoefficient * this.damageStat,
                        damageColorIndex = DamageColorIndex.Default,
                        damageType = DamageType.PercentIgniteOnHit,
                        falloffModel = BulletAttack.FalloffModel.DefaultBullet,
                        maxDistance = Cannon.range,
                        force = Cannon.force,
                        hitMask = LayerIndex.CommonMasks.bullet,
                        minSpread = 0f,
                        maxSpread = 0f,
                        isCrit = base.RollCrit(),
                        owner = base.gameObject,
                        muzzleName = muzzleString,
                        smartCollision = false,
                        procChainMask = default(ProcChainMask),
                        procCoefficient = procCoefficient,
                        radius = explosionRadius,
                        sniper = false,
                        stopperMask = LayerIndex.CommonMasks.bullet,
                        weapon = null,
                        tracerEffectPrefab = Cannon.tracerEffectPrefab,
                        spreadPitchScale = 0f,
                        spreadYawScale = 0f,
                        queryTriggerInteraction = QueryTriggerInteraction.UseGlobal,
                        //hitEffectPrefab = RoR2.LegacyResourcesAPI.Load<GameObject>("prefabs/effects/bombexplosion"),
                        //hitEffectPrefab = EntityStates.Engi.EngiWeapon.FireSeekerGrenades.hitEffectPrefab, 
                        hitEffectPrefab = EntityStates.Commando.CommandoWeapon.FirePistol2.hitEffectPrefab,
                    }.Fire();
                }
            }
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (base.fixedAge >= this.fireDuration)
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