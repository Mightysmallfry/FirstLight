using EntityStates;
using RoR2;
using UnityEngine;

namespace FirstLightMod.Survivors.Farmer.SkillStates
{
    public class SuperShotgun : BaseSkillState
    {
        public static float damageCoefficient = FarmerStaticValues.superShotGunDamageCoefficient;
        public static int bulletShellCount = 5;

        public static float procCoefficient = 1f;
        public static float baseDuration = 0.1f;
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
            this.duration = SuperShotgun.baseDuration / this.attackSpeedStat;
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

                //base.characterBody.AddSpreadBloom(0.0f);
                EffectManager.SimpleMuzzleFlash(EntityStates.Commando.CommandoWeapon.FirePistol2.muzzleEffectPrefab, base.gameObject, this.muzzleString, false);
                Util.PlaySound("HenryShootPistol", base.gameObject);

                if (base.isAuthority)
                {
                    Ray aimRay = base.GetAimRay();
                    base.AddRecoil(-1f * SuperShotgun.recoil, -2f * SuperShotgun.recoil, -0.5f * SuperShotgun.recoil, 0.5f * SuperShotgun.recoil);

                    float minFixedSpreadYaw = 1f;
                    float maxFixedSpreadYaw = 1f;

                    Vector3 rhs = Vector3.Cross(Vector3.up, aimRay.direction);
                    Vector3 axis = Vector3.Cross(aimRay.direction, rhs);

                    float spreadRange = UnityEngine.Random.Range(minFixedSpreadYaw, maxFixedSpreadYaw) * 2f;
                    float angleBetweenBullets = spreadRange / (float)(SuperShotgun.bulletShellCount - 1);


                    //This section rotates the vector so that the bullets are centered
                    Vector3 direction = Quaternion.AngleAxis(-spreadRange * 0.5f, axis) * aimRay.direction;
                    Quaternion rotation = Quaternion.AngleAxis(angleBetweenBullets, axis);
                    Ray adjustedAimRay = new Ray(aimRay.origin, direction); //From here on out we want to use this Ray since it has been adjusted for bullet spread


                    //Build the shotgun bullets, make a bullet then rotate, the make bullet etc.
                    for (int i = 0; i < SuperShotgun.bulletShellCount; i++)
                    {
                        new BulletAttack
                        {
                            bulletCount = 1,
                            aimVector = adjustedAimRay.direction,
                            origin = aimRay.origin,
                            damage = SuperShotgun.damageCoefficient * this.damageStat,
                            damageColorIndex = DamageColorIndex.Default,
                            damageType = DamageType.Nullify,
                            falloffModel = BulletAttack.FalloffModel.DefaultBullet,
                            maxDistance = SuperShotgun.range,
                            force = SuperShotgun.force,
                            hitMask = LayerIndex.CommonMasks.bullet,
                            minSpread = 1f,
                            maxSpread = 1f,
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
                            tracerEffectPrefab = SuperShotgun.tracerEffectPrefab, // May want to take a look at this
                            spreadPitchScale = 0.0f,
                            spreadYawScale = 0.0f,
                            queryTriggerInteraction = QueryTriggerInteraction.UseGlobal,
                            hitEffectPrefab = EntityStates.Commando.CommandoWeapon.FirePistol2.hitEffectPrefab
                        }.Fire();

                        adjustedAimRay.direction = rotation * adjustedAimRay.direction;
                    }
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