using RoR2;
using R2API;
using EntityStates;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Events;
using FirstLightMod.Modules;

namespace FirstLightMod.Survivors.Beekeeper.SkillStates
{
    public class SummonHornet : BaseSkillState
    {
        private int sliceCount = 1; // also effects number of drones created.

        private GameObject summonDronePrefab = LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterMasters/DroneBackupMaster");




        public override void OnEnter()
        {
            base.OnEnter();

            if (NetworkServer.active)
            {
                float y = Quaternion.LookRotation(this.GetAimRay().direction).eulerAngles.y;
                float d = 3f;
                foreach (float num2 in new DegreeSlices(sliceCount, 0.5f))
                {
                    Quaternion rotation = Quaternion.Euler(-30f, y + num2, 0f);
                    Quaternion rotation2 = Quaternion.Euler(0f, y + num2 + 180f, 0f);
                    Vector3 position = base.transform.position + rotation * (Vector3.forward * d);
                    CharacterMaster characterMaster = this.SummonMaster(this.summonDronePrefab, position, rotation2);

                }
            }
        }


        public override void OnExit()
        {
            base.OnExit();
        }



        public override void FixedUpdate()
        {
            base.FixedUpdate();

            this.outer.SetNextStateToMain();
        }



        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }


        internal CharacterMaster SummonMaster(GameObject masterPrefab, Vector3 position, Quaternion rotation)
        {
            if (!NetworkServer.active)
            {
                Debug.LogWarning("[Server] function 'RoR2.CharacterMaster Beekeeper.SummonHornet::SummonMaster(UnityEngine.GameObject,UnityEngine.Vector3,UnityEngine.Quaternion)' called on client");
                return null;
            }
            return new MasterSummon
            {
                masterPrefab = masterPrefab,
                position = position,
                rotation = rotation,
                summonerBodyObject = base.gameObject,
                ignoreTeamMemberLimit = false
            }.Perform();
        }



    }
}
