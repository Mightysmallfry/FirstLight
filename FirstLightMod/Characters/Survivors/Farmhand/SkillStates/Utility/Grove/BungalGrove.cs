using EntityStates;
using HG;
using On.RoR2.Items;
using R2API;
using RoR2;
using RoR2.Projectile;
using UnityEngine;
using UnityEngine.Networking;

namespace FirstLightMod.Survivors.Farmer.SkillStates
{
    public class BungalGrove : BaseSkillState
    {
        public static float healCoefficient = FarmerStaticValues.groveHealCoefficient;
        public static float procCoefficient = 1f;
        public static float baseDuration = 0f;

        private float duration;
        private float fireTime;
        public bool hasFired;
        private string muzzleString;


        //Healing Zone
        private static GameObject bungalFieldPrefab = FarmerAssets.bombProjectilePrefab;
        //private GameObject bungalFieldGameObject;
        //private HealingWard bungalHealingWard;
        //private TeamFilter bungalFilter;


        //
        // TeamIndex.Player exists
        //
        


        public override void OnEnter()
        {
            base.OnEnter();
            base.characterBody.SetAimTimer(2f);
            this.muzzleString = "Muzzle";
            base.PlayAnimation("LeftArm, Override", "ShootGun", "ShootGun.playbackRate", 1.8f);

        }

        public override void OnExit()
        {
            base.OnExit();

            this.Fire();
        }


        private void Fire()
        {
            if (!this.hasFired)
            {
                this.hasFired = true;


                EffectManager.SimpleMuzzleFlash(EntityStates.Commando.CommandoWeapon.FirePistol2.muzzleEffectPrefab, base.gameObject, this.muzzleString, false);
                //Util.PlaySound("HenryShootPistol", base.gameObject);

                //This should fire a projectile now that is the prefab of the engiMine... Hopefully

                if (base.isAuthority)
                {
                    Ray aimRay = base.GetAimRay();

                    FireProjectileInfo pinfo = new FireProjectileInfo
                    {
                        //projectilePrefab = UnityEngine.Object.Instantiate<GameObject>(bungalFieldPrefab, aimRay.origin, Util.QuaternionSafeLookRotation(aimRay.direction)), //This should create the object at the characters feet
                        projectilePrefab = bungalFieldPrefab, 
                        position = aimRay.origin,
                        rotation = Util.QuaternionSafeLookRotation(aimRay.direction),
                        owner = base.gameObject,
                        force = 10f,
                        crit = base.RollCrit(),
                        target = null,
                    };

                    ProjectileManager.instance.FireProjectile(pinfo);

                }
            }
        }
        public override void FixedUpdate()
        {
            base.FixedUpdate();


            //if (!NetworkServer.active)
            //{
            //    return;
            //}

            //if (base.fixedAge >= this.fireTime)
            //{
            //    Fire();
            //}

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