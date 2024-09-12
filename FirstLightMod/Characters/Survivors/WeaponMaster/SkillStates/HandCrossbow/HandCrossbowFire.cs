using System;
using System.Collections.Generic;
using System.Text;
using EntityStates;
using RoR2;
using UnityEngine;

namespace FirstLightMod.Survivors.WeaponMaster.SkillStates.HandCrossbow
{
    public class HandCrossbowFire : BaseSkillState
    {

        public static float baseDurationPerBolt;

        public static float damageCoefficient;

        public static GameObject projectilePrefab;

        public static GameObject muzzleflashEffectPrefab;

        public List<HurtBox> targetList;

        private int fireIndex;

        private float durationPerBolt;

        private float stopwatch;

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Pain;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            this.durationPerBolt = HandCrossbowFire.baseDurationPerBolt / this.attackSpeedStat;
            this.PlayAnimation("Gesture, Additive", "IdleHarpoons");
        }

        public override void FixedUpdate()
        {

            base.FixedUpdate();
            bool boltFired = false;
            if (base.isAuthority)
            {
                this.stopwatch += Time.fixedDeltaTime;
                if (this.stopwatch >= this.durationPerBolt)
                {
                    this.stopwatch -= this.durationPerBolt;
                    while (this.fireIndex < this.targetList.Count)
                    {
                        List<HurtBox> list = this.targetList;
                        int num = this.fireIndex;
                        this.fireIndex = num + 1;
                        HurtBox hurtBox = list[num];
                        if (hurtBox.healthComponent && hurtBox.healthComponent.alive)
                        {
                            string text = (this.fireIndex % 2 == 0) ? "MuzzleLeft" : "MuzzleRight";
                            Vector3 position = base.inputBank.aimOrigin;
                            Transform transform = base.FindModelChild(text);
                            if (transform != null)
                            {
                                position = transform.position;
                            }
                            EffectManager.SimpleMuzzleFlash(HandCrossbowFire.muzzleflashEffectPrefab, base.gameObject, text, true);
                            this.FireMissile(hurtBox, position);
                            boltFired = true;
                            break;
                        }
                        base.activatorSkillSlot.AddOneStock();
                    }
                    if (this.fireIndex >= this.targetList.Count)
                    {
                        this.outer.SetNextState(new HandCrossbowFinish());
                    }
                }
            }
            if (boltFired)
            {
                this.PlayAnimation((this.fireIndex % 2 == 0) ? "Gesture Left Cannon, Additive" : "Gesture Right Cannon, Additive", "FireHarpoon");
            }
        }


        private void FireMissile(HurtBox target, Vector3 position)
        {
            MissileUtils.FireMissile(
                base.inputBank.aimOrigin, base.characterBody,
                default(ProcChainMask),
                target.gameObject,
                this.damageStat * HandCrossbowFire.damageCoefficient,
                base.RollCrit(), 
                HandCrossbowFire.projectilePrefab,
                DamageColorIndex.Default,
                Vector3.up,
                0f, 
                false);
        }

        public override void OnExit()
        {
            base.OnExit();
            //base.PlayCrossfade("Gesture, Additive", "ExitHarpoons", 0.1f);
        }

    }
}
