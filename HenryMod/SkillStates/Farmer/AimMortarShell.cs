using RoR2;
using R2API;
using EntityStates;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using RoR2.Projectile;

namespace FirstLightMod.SkillStates
{
    public class AimMortarShell : AimThrowableBase
    {
        
        public override void OnEnter()
        {
            //All serialized fields
            this.maxDistance = 100f;
            this.rayRadius = 5f;
            this.arcVisualizerPrefab = RoR2.LegacyResourcesAPI.Load<GameObject>("RoR2/Base/Common/VFX/BasicThrowableVisualizer");
            this.endpointVisualizerPrefab = RoR2.LegacyResourcesAPI.Load<GameObject>("RoR2/Base/Treebot/TreebotMortarAreaIndicator");

            //this.endpointVisualizerPrefab = RoR2.LegacyResourcesAPI.Load<GameObject>("RoR2/Base/Common/TeamIndicator, FullSphere");
            //this.endpointVisualizerPrefab.GetComponent<TeamComponent>().teamIndex = this.teamComponent.teamIndex;

            this.endpointVisualizerRadiusScale = 7f;
            this.setFuse = false;
            //this.damageCoefficient = Modules.Config.mortarDamageCoefficient.Value;
            this.damageCoefficient = 4f;
            this.baseMinimumDuration = 4f;

            this.projectilePrefab = RoR2.LegacyResourcesAPI.Load<GameObject>("RoR2/Base/Treebot/TreebotFlowerSeed");
            this.projectilePrefab.GetComponent<ProjectileImpactExplosion>().impactEffect = RoR2.LegacyResourcesAPI.Load<GameObject>("RoR2/Base/Treebot/OmniExplosionVFXTreebot");
            this.projectilePrefab.GetComponent<ProjectileController>().ghostPrefab = RoR2.LegacyResourcesAPI.Load<GameObject>("RoR2/Base/Treebot/SeedpodMortarGhost");

            Chat.AddMessage("AimMortar has entered");

            //BUG HAS BEEN FOUND, THERE IS A NULL OBJECT REFERENCE OCCURING
            base.OnEnter();
            this.detonationRadius = 10f;

        }

        public override void OnExit()
        {
            Chat.AddMessage("AimMortar is leaving");

            //Now the null reference is occuring here
            //ERROR : Arguement exception, the object I am instatiating is null
            base.OnExit();
            //this.outer.SetNextState(new AimMortarShell());
            Chat.AddMessage("AimMortar has left");
        }



        public override EntityState PickNextState()
        {
            return new MortarShell();
        }

        public override void FixedUpdate()
        {

            Chat.AddMessage("Fixed Update tic");

            base.FixedUpdate();

        }
    }
}
