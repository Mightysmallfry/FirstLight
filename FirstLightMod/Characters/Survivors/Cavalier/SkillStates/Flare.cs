using EntityStates;
using FirstLightMod.Survivors.Cavalier;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace FirstLightMod.Survivors.Cavalier.SkillStates
{
    public class Flare : BaseSkillState
    {
        public static float duration = 5f;
        //delay on firing is usually ass-feeling. only set this if you know what you're doing
        public static float firePercentTime = 0.0f;
        public static float radius = 10f;
        public static float interval = 1.0f;
        public static GameObject tracerEffectPrefab = LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/Tracers/TracerGoldGat");
        public static Transform rangeIndicator; //For playtest we can check for the great banner ward.

        private float fireTime;
        private bool hasFired;
        private string muzzleString;
        private float healAmount;

        private BuffWard riposteBuffWard;
        private BuffWard offGuardBuffWard;


        public override void OnEnter()
        {
            base.OnEnter();
            characterBody.SetAimTimer(2f);
            muzzleString = "Muzzle";

            PlayAnimation("LeftArm, Override", "ShootGun", "ShootGun.playbackRate", 1.8f);

            CreateWards();

        }

        private void CreateWards()
        {
            riposteBuffWard.interval = Flare.interval;
            riposteBuffWard.buffDef = CavalierBuffs.riposteBuff;
            riposteBuffWard.buffDuration = riposteBuffWard.interval;
            riposteBuffWard.expires = true;
            riposteBuffWard.floorWard = true;
            riposteBuffWard.invertTeamFilter = false;

            riposteBuffWard.shape = BuffWard.BuffWardShape.VerticalTube;
            riposteBuffWard.radius = Flare.radius;
            riposteBuffWard.teamFilter.teamIndex = base.GetTeam();

            riposteBuffWard.expireDuration = Flare.duration;
            riposteBuffWard.rangeIndicator = Flare.rangeIndicator;


            offGuardBuffWard.interval = Flare.interval;
            offGuardBuffWard.buffDef = CavalierBuffs.offGuardBuff;
            offGuardBuffWard.buffDuration = offGuardBuffWard.interval;
            offGuardBuffWard.expires = true;
            offGuardBuffWard.floorWard = true;
            offGuardBuffWard.invertTeamFilter = true;

            offGuardBuffWard.shape = BuffWard.BuffWardShape.VerticalTube;
            offGuardBuffWard.radius = Flare.radius;
            offGuardBuffWard.teamFilter.teamIndex = base.GetTeam();

            offGuardBuffWard.expireDuration = Flare.duration;
            offGuardBuffWard.rangeIndicator = Flare.rangeIndicator;
        }



        public override void OnExit()
        {
            base.OnExit();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (fixedAge >= fireTime)
            {
                Fire();
            }

            if (fixedAge >= duration && isAuthority)
            {
                outer.SetNextStateToMain();
                return;
            }
        }

        private void Fire()
        {
            if (!hasFired)
            {
                hasFired = true;

                characterBody.AddSpreadBloom(1.5f);
                EffectManager.SimpleMuzzleFlash(EntityStates.Commando.CommandoWeapon.FirePistol2.muzzleEffectPrefab, gameObject, muzzleString, false);
                Util.PlaySound("HenryShootPistol", gameObject);



                if (NetworkServer.active)
                {
                    characterBody.AddTimedBuff(CavalierBuffs.riposteBuff, 5f);
                    characterBody.healthComponent.HealFraction(healAmount, new ProcChainMask());
                }
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }
}