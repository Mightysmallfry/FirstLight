using FirstLightMod.Modules;
using FirstLightMod.Modules.Survivors;
using FirstLightMod.SkillStates;
using IL.RoR2.Projectile;
using R2API;
using RoR2;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.PlayerLoop;
using UnityEngine.UIElements;

namespace FirstLightMod.Components
{
    [RequireComponent(typeof(ProjectileController))]
    [RequireComponent(typeof(HealingWard))]
    [RequireComponent(typeof(TeamFilter))]
    public class BungalGroveController : NetworkBehaviour
    {

        //private static GameObject groveControllerPrefab = PrefabAPI.InstantiateClone(LegacyResourcesAPI.Load<GameObject>("Prefabs/NetworkedObjects/MushroomWard"), "FarmGrove", true);

        public GameObject groveGameObject = Assets.mainAssetBundle.LoadAsset<GameObject>("FarmGrove");

        public GameObject owner;


        private TeamFilter groveTeamFilter;
        public HealingWard groveHealingWard;
        private RoR2.Projectile.ProjectileController groveProjectController;

        public CharacterBody body { get; private set; }


        //Awake is like start but always is enabled
        private void Awake()
        {
            this.groveTeamFilter = this.groveGameObject.GetComponent<TeamFilter>();
            this.groveHealingWard = this.groveGameObject.GetComponent<HealingWard>();

            //groveProjectController = groveGameObject.GetComponent<RoR2.Projectile.ProjectileController>();

            //owner = groveProjectController.owner;

            if (owner)
            {
                body = owner.GetComponent<CharacterBody>();
            }

        }

        //Start is used when enabled
        private void Start()
        {
            //this.groveGameObject = UnityEngine.Object.Instantiate<GameObject>(groveControllerPrefab, body.footPosition, Quaternion.identity);
            //this.groveTeamFilter = this.groveGameObject.GetComponent<TeamFilter>();
            //this.groveHealingWard = this.groveGameObject.GetComponent<HealingWard>();


            this.groveGameObject = PrefabAPI.InstantiateClone(LegacyResourcesAPI.Load<GameObject>("Prefabs/NetworkedObjects/MushroomWard"), "FarmGrove", true);
            
            NetworkServer.Spawn(this.groveGameObject);


        }

        private void Update()
        {

        }

        private void FixedUpdate()
        {
            if (!NetworkServer.active)
            {
                return;
            }


            float networkRadius = body.radius + 3.0f;
            

            if (this.groveHealingWard)
            {
                this.groveHealingWard.interval = 0.25f;
                this.groveHealingWard.healFraction = (0.045f + 0.0225f) * this.groveHealingWard.interval; // in the parentheses, there should also be a multiplier corresponding to level
                this.groveHealingWard.healPoints = 5f; //Make sure to set this to 0 once working
                this.groveHealingWard.Networkradius = networkRadius;
            }
            if (this.groveTeamFilter)
            {
                this.groveTeamFilter.teamIndex = body.teamComponent.teamIndex;
            }

            this.groveGameObject = UnityEngine.Object.Instantiate<GameObject>(this.groveGameObject, body.footPosition, Quaternion.identity);





        }

        private void OnDisable()
        {
            if (this.groveGameObject)
            {
                UnityEngine.Object.Destroy(this.groveGameObject);
            }
        }

    }
}
