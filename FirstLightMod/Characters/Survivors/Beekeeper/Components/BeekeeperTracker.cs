using RoR2;
using R2API;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using System.Linq;

namespace FirstLightMod.Survivors.Beekeeper.Components
{
    public class BeekeeperTracker : MonoBehaviour
    {

        //Put this on pause, Equipment is not working right.


        Indicator indicator;

        HurtBox trackedHurtBox;

        CharacterBody characterBody;

        TeamComponent teamComponent;

        InputBankTest inputBank;

        private readonly BullseyeSearch search = new BullseyeSearch();

        private float trackerUpdateStopwatch;

        public float maxTrackingDistance = 60f;

        public float maxTrackingAngle = 20f;

        public float trackerUpdateFrequency = 10f;


        private void Awake()
        {
            //indicator = new Indicator(gameObject, LegacyResourcesAPI.Load<GameObject>("RoR2/Base/Lightning/LightningIndicator.prefab"));
            indicator = new Indicator(gameObject, LegacyResourcesAPI.Load<GameObject>("Prefabs/HuntressTrackingIndicator"));

        }

        private void Start()
        {
            characterBody = GetComponent<CharacterBody>();
            inputBank = GetComponent<InputBankTest>();
            teamComponent = GetComponent<TeamComponent>();
        }

        public HurtBox GetTrackingTarget()
        {
            return trackedHurtBox;
        }

        private void OnEnable()
        {
            indicator.active = true;
        }

        private void OnDisable()
        {
            indicator.active = false;
        }

        private void FixedUpdate()
        {
            trackerUpdateStopwatch += Time.fixedDeltaTime;
            if (trackerUpdateStopwatch >= 1f / trackerUpdateFrequency)
            {
                trackerUpdateStopwatch -= 1f / trackerUpdateFrequency;
                HurtBox hurtBox = trackedHurtBox;
                Ray aimRay = new Ray(inputBank.aimOrigin, inputBank.aimDirection);
                SearchForTarget(aimRay);
                indicator.targetTransform = trackedHurtBox ? trackedHurtBox.transform : null;
            }
        }

        private void SearchForTarget(Ray aimRay)
        {
            search.teamMaskFilter = TeamMask.GetUnprotectedTeams(teamComponent.teamIndex);
            search.filterByLoS = true;
            search.searchOrigin = aimRay.origin;
            search.searchDirection = aimRay.direction;
            search.sortMode = BullseyeSearch.SortMode.Distance;
            search.maxDistanceFilter = maxTrackingDistance;
            search.maxAngleFilter = maxTrackingAngle;
            search.RefreshCandidates();
            search.FilterOutGameObject(gameObject);
            trackedHurtBox = search.GetResults().FirstOrDefault();
        }

    }
}
