using RoR2;
using R2API;
using UnityEngine;
using EntityStates;
using FirstLightMod.Modules.BaseStates;

namespace FirstLightMod.Survivors.Beekeeper.SkillStates
{
    public class TargetJar : BaseTimedSkillState
    {

       //public static GameObject bigZapEffectPrefab = RoR2.LegacyResourcesAPI.Load<GameObject>("prefabs/effects/magelightningbombexplosion");
       //public static GameObject bigZapEffectPrefabArea = RoR2.LegacyResourcesAPI.Load<GameObject>("prefabs/effects/lightningstakenova");
       //public static GameObject bigZapEffectFlashPrefab = RoR2.LegacyResourcesAPI.Load<GameObject>("prefabs/effects/omnieffect/omniimpactvfxlightning");

        public static float DamageCoefficient = 6.9f;
        public static float ProcCoefficient = 1f;
        public static float BaseAttackRadius = 10;

        public static float BaseDuration = 0.6f;
        public static float BaseCastTime = 0;

        public float skillsPlusAreaMulti = 1f;
        public float skillsPlusDamageMulti = 1f;
        public float attackRadius;
        public Vector3 aimPoint;


        public override float TimedBaseDuration { get; }
        public override float TimedBaseCastStartPercentTime { get; }

        public override void OnEnter()
        {
            base.OnEnter();

            InitDurationValues();

            attackRadius = BaseAttackRadius * skillsPlusAreaMulti;

            base.characterBody.AddSpreadBloom(1);
        }

        protected override void OnCastEnter()
        {
            base.OnCastEnter();

            bool isCrit = RollCrit();

            if (base.isAuthority)
            {

                BlastAttack blast = new BlastAttack
                {
                    attacker = gameObject,
                    inflictor = gameObject,
                    teamIndex = teamComponent.teamIndex,

                    position = aimPoint,
                    radius = attackRadius,
                    falloffModel = BlastAttack.FalloffModel.None,

                    baseDamage = damageStat * DamageCoefficient * skillsPlusDamageMulti,
                    crit = isCrit,
                    damageType = DamageType.Stun1s,

                    procCoefficient = 1,

                    baseForce = -5, 

                };
                blast.Fire();
            }
        }

        public override void OnExit()
        {
            base.OnExit();
        }



        public override void FixedUpdate()
        {
            base.FixedUpdate();
        }



        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }
}
