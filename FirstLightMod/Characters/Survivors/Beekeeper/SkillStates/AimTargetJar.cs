using RoR2;
using R2API;
using UnityEngine;
using EntityStates;

namespace FirstLightMod.Survivors.Beekeeper.SkillStates
{
    public class AimTargetJar : AimThrowableBase
    {


        public static string EnterSoundString = EntityStates.Treebot.Weapon.AimMortar.enterSoundString;
        public static string ExitSoundString = EntityStates.Treebot.Weapon.AimMortar.exitSoundString;

        private float viewRadius;

        public override void OnEnter()
        {           
            EntityStates.Toolbot.AimStunDrone aimStunDrone = new EntityStates.Toolbot.AimStunDrone();
            projectilePrefab = aimStunDrone.projectilePrefab;
            endpointVisualizerPrefab = aimStunDrone.endpointVisualizerPrefab;
            endpointVisualizerRadiusScale = TargetJar.BaseAttackRadius;
            maxDistance = 60;
            rayRadius = 1.6f;
            setFuse = false;
            damageCoefficient = 0f;
            baseMinimumDuration = 0.2f;
            base.OnEnter();

            Util.PlaySound(EnterSoundString, gameObject);

        }


        public override void OnExit()
        {   
            base.OnExit();
        }



        public override void FixedUpdate()
        {
            base.FixedUpdate();
            StartAimMode();

            viewRadius = TargetJar.BaseAttackRadius;
            maxDistance = 30;

            endpointVisualizerRadiusScale = Mathf.Lerp(endpointVisualizerRadiusScale, viewRadius, 0.5f);
        }

        public override EntityState PickNextState()
        {
            return new TargetJar() { aimPoint = currentTrajectoryInfo.hitPoint };
        } 

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }
}
