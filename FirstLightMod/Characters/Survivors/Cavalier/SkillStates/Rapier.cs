using EntityStates;
using FirstLightMod.Modules.BaseStates;
using RoR2;
using RoR2.Skills;
using UnityEngine;
using UnityEngine.Networking;

namespace FirstLightMod.Survivors.Cavalier.SkillStates
{
    public class Rapier : BasicMeleeAttack, SteppedSkillDef.IStepSetter
    {


        public static GameObject comboFinisherSwingEffectPrefab;
        public static float comboFinisherHitPauseDuration;
        public static float comboFinisherDamageCoefficient;
        public static float comboFinisherBloom;

        public static float comboFinisherBaseDurationBeforeInterruptable;
        public static float baseDurationBeforeInterruptable;

        private string animationStateName;
        public float bloom;
        public float durationBeforeInterruptable;

        public BuffDef debuffVulnerable;
        public float debuffDuration;
        public bool hasGrantedDebuff;


        public int step;
        protected string hitboxGroupName = "SwordGroup";
        //protected string muzzleString =  % 2 == 0 ? "SwingLeft" : "SwingRight";

        public static string rapier1Sound = "HenrySwordSwing";
        public static string rapier2Sound = "HenrySwordSwing";
        public static string rapier3Sound = "HenrySwordSwing";


        private bool isComboFinisher
        {
            get
            {
                return this.step == 2;
            }
        }


        void SteppedSkillDef.IStepSetter.SetStep(int i)
        {
            this.step = i;
        }


        public override bool allowExitFire
        {
            get
            {
                return base.characterBody && !base.characterBody.isSprinting;
            }
        }



        public override void OnEnter()
        {

            if (this.isComboFinisher)
            {
                this.swingEffectPrefab = Rapier.comboFinisherSwingEffectPrefab;
                this.hitPauseDuration = Rapier.comboFinisherHitPauseDuration;
                this.damageCoefficient = Rapier.comboFinisherDamageCoefficient;
                this.bloom = Rapier.comboFinisherBloom;
            }
            base.OnEnter();
            base.characterDirection.forward = base.GetAimRay().direction;
            this.durationBeforeInterruptable = (this.isComboFinisher ? (Rapier.comboFinisherBaseDurationBeforeInterruptable / this.attackSpeedStat) : (Rapier.baseDurationBeforeInterruptable / this.attackSpeedStat));

        }

        public override void OnExit()
        {
            base.OnExit();
        }
        
        public override void AuthorityModifyOverlapAttack(OverlapAttack overlapAttack)
        {
            base.AuthorityModifyOverlapAttack(overlapAttack);
            if (this.isComboFinisher)
            {
                overlapAttack.damageType = DamageType.ApplyMercExpose;
            }
        }

        public override void PlayAnimation()
        {
            this.animationStateName = "";
            string soundString = null;
            switch (this.step)
            {
                case 0:
                    this.animationStateName = "SwingLeft";
                    soundString = Rapier.rapier1Sound;
                    break;
                case 1:
                    this.animationStateName = "SwingRight";
                    soundString = Rapier.rapier2Sound;
                    break;
                case 2:
                    this.animationStateName = "SwingRight";
                    soundString = Rapier.rapier3Sound;
                    break;
            }
            float duration = Mathf.Max(this.duration, 0.2f);
            base.PlayCrossfade("Gesture, Additive", this.animationStateName, "Slash.playbackRate", duration, 0.05f);
            base.PlayCrossfade("Gesture, Override", this.animationStateName, "Slash.playbackRate", duration, 0.05f);
            Util.PlaySound(soundString, base.gameObject);
        }


        public override void OnMeleeHitAuthority()
        {
            base.OnMeleeHitAuthority();
            base.characterBody.AddSpreadBloom(this.bloom);
            if (this.hasGrantedDebuff && this.isComboFinisher)
            {
                this.hasGrantedDebuff = true;
                base.characterBody.AddTimedBuff(CavalierBuffs.vulnerableBuff, debuffDuration);
            }
        }



        public override void OnSerialize(NetworkWriter writer)
        {
            base.OnSerialize(writer);
            writer.Write((byte)this.step);
        }


        public override void OnDeserialize(NetworkReader reader)
        {
            base.OnDeserialize(reader);
            this.step = (int)reader.ReadByte();
        }



        public override InterruptPriority GetMinimumInterruptPriority()
        {
            if (base.fixedAge >= this.durationBeforeInterruptable)
            {
                return InterruptPriority.Skill;
            }
            return InterruptPriority.PrioritySkill;
        }


    }
}