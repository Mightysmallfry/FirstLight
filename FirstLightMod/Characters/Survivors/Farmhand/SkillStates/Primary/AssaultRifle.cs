using EntityStates;
using RoR2;
using UnityEngine;

namespace FirstLightMod.Survivors.Farmer.SkillStates
{
    public class AssaultRifle : BaseSkillState
    {
        private static float damageCoefficient = FarmerStaticValues.assaultRifleDamageCoefficient;


        public static float procCoefficient = 1f;
        public static float baseDuration = .2f;
        public static float force = 100f;
        public static float recoil = .1f;
        public static float range = 256f;
        public static GameObject tracerEffectPrefab = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/Tracers/TracerGoldGat");

        private float duration;
        private float fireTime;
        private string muzzleString;
        private bool hasFired;


        public override void OnEnter()
        {
            base.OnEnter();
            this.duration = AssaultRifle.baseDuration / this.attackSpeedStat;
            this.fireTime = 0.05f * this.duration;
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


                base.characterBody.AddSpreadBloom(0.1f);
                EffectManager.SimpleMuzzleFlash(EntityStates.Commando.CommandoWeapon.FirePistol2.muzzleEffectPrefab, base.gameObject, this.muzzleString, false);
                Util.PlaySound(EntityStates.EngiTurret.EngiTurretWeapon.FireGauss.attackSoundString, base.gameObject);

                if (base.isAuthority)
                {
                    Ray aimRay = base.GetAimRay();
                    base.AddRecoil(-1f * AssaultRifle.recoil, -2f * AssaultRifle.recoil, -0.5f * AssaultRifle.recoil, 0.5f * AssaultRifle.recoil);


                    new BulletAttack
                    {
                        bulletCount = 1,
                        aimVector = aimRay.direction,
                        origin = aimRay.origin,
                        damage = AssaultRifle.damageCoefficient * this.damageStat,
                        damageColorIndex = DamageColorIndex.Default,
                        damageType = DamageType.Generic,
                        falloffModel = BulletAttack.FalloffModel.DefaultBullet,
                        maxDistance = AssaultRifle.range,
                        force = AssaultRifle.force,
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
                        tracerEffectPrefab = AssaultRifle.tracerEffectPrefab,
                        spreadPitchScale = 0f,
                        spreadYawScale = 0f,
                        queryTriggerInteraction = QueryTriggerInteraction.UseGlobal,
                        //hitEffectPrefab = EntityStates.Engi.EngiWeapon.FireSeekerGrenades.hitEffectPrefab,
                        hitEffectPrefab = EntityStates.Commando.CommandoWeapon.FirePistol2.hitEffectPrefab,
                    }.Fire();
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
