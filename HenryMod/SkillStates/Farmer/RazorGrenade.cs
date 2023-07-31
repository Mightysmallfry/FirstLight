using EntityStates;
using EntityStates.Toolbot;
using R2API;
using R2API.ContentManagement;
using RoR2;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace FirstLightMod.SkillStates
{
    public class RazorGrenade : AimGrenade
    {

       

        public override void OnEnter()
        {
            base.OnEnter();

            //this.projectilePrefab = Modules.Projectiles.RazorGrenadePrefab;
            ////this.arcVisualizerPrefab = null;

            //this.damageCoefficient = 4f;

            
            ////this.endpointVisualizerPrefab = null;
            //this.endpointVisualizerRadiusScale = detonationRadius;

            //this.setFuse = false;


            //this.detonationRadius = 7f;
            //this.maxDistance = 100f;
            //this.minimumDuration = 0f;



        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
        }

        public override void OnExit()
        {
            //this.outer.SetNextState(new NEXTSKILLSTATECLASSNAME());
            this.outer.SetNextStateToMain();
            base.OnExit();
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }
}
