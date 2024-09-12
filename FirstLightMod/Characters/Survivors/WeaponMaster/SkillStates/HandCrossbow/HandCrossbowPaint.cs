using System.Collections.Generic;
using EntityStates;
using FirstLightMod.Survivors.WeaponMaster;
using FirstLightMod.Survivors.WeaponMaster.SkillStates.HandCrossbow;
using RoR2.Skills;
using RoR2;
using RoR2.UI;
using UnityEngine;

namespace FirstLightMod.Survivors.WeaponMaster.SkillStates
{
    public class HandCrossbowPaint : BaseSkillState
    {

        public static GameObject crosshairOverridePrefab = LegacyResourcesAPI.Load<GameObject>("Prefabs/EngiPaintCrosshair");
        public static GameObject targetIndicatorPrefab = LegacyResourcesAPI.Load<GameObject>("Prefabs/EngiPaintingIndicator");
        public static float stackInterval;

        public static float maxAngle;
        public static float maxDistance;

        private List<HurtBox> targetList;
        private Dictionary<HurtBox, HandCrossbowPaint.CrossbowIndicatorInfo> targetIndicators;

        private Indicator targetIndicator;

        private SkillDef confirmSkillDef;
        private SkillDef cancelSkillDef;

        private bool releasedKeyOnce;

        private float stackStopwatch;

        private CrosshairUtils.OverrideRequest crosshairOverrideRequest;

        private BullseyeSearch search;
        private bool queuedFiringState;

        private HealthComponent previouslyHighlightedTargetHealthComponent;

        private HurtBox previousHighlightTargetHurtBox;

        private float duration;

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Pain;
        }

        private struct CrossbowIndicatorInfo
        {
            public int refCount;

            public HandCrossbowBoltIndicator indicator;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            if (base.isAuthority)
            {
                this.targetList = new List<HurtBox>();
                this.targetIndicators = new Dictionary<HurtBox, HandCrossbowPaint.CrossbowIndicatorInfo>();
                this.targetIndicator = new Indicator(base.gameObject, HandCrossbowPaint.targetIndicatorPrefab);
                this.search = new BullseyeSearch();
            }
            base.PlayCrossfade("Gesture, Additive", "PrepHarpoons", 0.1f);
            //Util.PlaySound(Paint.enterSoundString, base.gameObject);
            //this.loopSoundID = Util.PlaySound(Paint.loopSoundString, base.gameObject);

            if (HandCrossbowPaint.crosshairOverridePrefab)
            {
                this.crosshairOverrideRequest = CrosshairUtils.RequestOverrideForBody(base.characterBody, HandCrossbowPaint.crosshairOverridePrefab, CrosshairUtils.OverridePriority.Skill);
            }

            this.confirmSkillDef = SkillCatalog.GetSkillDef(SkillCatalog.FindSkillIndexByName("EngiConfirmTargetDummy"));
            this.cancelSkillDef = SkillCatalog.GetSkillDef(SkillCatalog.FindSkillIndexByName("EngiCancelTargetingDummy"));

            base.skillLocator.primary.SetSkillOverride(this, this.confirmSkillDef, GenericSkill.SkillOverridePriority.Contextual);
            base.skillLocator.secondary.SetSkillOverride(this, this.cancelSkillDef, GenericSkill.SkillOverridePriority.Contextual);
        }

        public override void OnExit()
        {
            if (base.isAuthority && !this.outer.destroying && !this.queuedFiringState)
            {
                for (int i = 0; i < this.targetList.Count; i++)
                {
                    base.activatorSkillSlot.AddOneStock();
                }
            }

            base.skillLocator.primary.UnsetSkillOverride(this, this.confirmSkillDef, GenericSkill.SkillOverridePriority.Contextual);
            base.skillLocator.secondary.UnsetSkillOverride(this, this.cancelSkillDef, GenericSkill.SkillOverridePriority.Contextual);

            if (this.targetIndicators != null)
            {
                foreach (KeyValuePair<HurtBox, HandCrossbowPaint.CrossbowIndicatorInfo> keyValuePair in this.targetIndicators)
                {
                    keyValuePair.Value.indicator.active = false;
                }
            }
            if (this.targetIndicator != null)
            {
                this.targetIndicator.active = false;
            }
            CrosshairUtils.OverrideRequest overrideRequest = this.crosshairOverrideRequest;
            if (overrideRequest != null)
            {
                overrideRequest.Dispose();
            }

            base.PlayCrossfade("Gesture, Additive", "ExitHarpoons", 0.1f);
            //Util.PlaySound(Paint.exitSoundString, base.gameObject);
            //Util.PlaySound(Paint.stopLoopSoundString, base.gameObject);
            base.OnExit();
        }

        private void AuthorityFixedUpdate()
        {
            this.CleanTargetList();
            bool flag = false;
            HurtBox hurtBox;
            HealthComponent y;
            this.GetCurrentTargetInfo(out hurtBox, out y);
            if (hurtBox)
            {
                this.stackStopwatch += Time.fixedDeltaTime;
                if (base.inputBank.skill1.down && (this.previouslyHighlightedTargetHealthComponent != y || this.stackStopwatch > HandCrossbowPaint.stackInterval / this.attackSpeedStat || base.inputBank.skill1.justPressed))
                {
                    this.stackStopwatch = 0f;
                    this.AddTargetAuthority(hurtBox);
                }
            }
            if (base.inputBank.skill1.justReleased)
            {
                flag = true;
            }
            if (base.inputBank.skill2.justReleased)
            {
                this.outer.SetNextStateToMain();
                return;
            }
            if (base.inputBank.skill3.justReleased)
            {
                if (this.releasedKeyOnce)
                {
                    flag = true;
                }
                this.releasedKeyOnce = true;
            }
            if (hurtBox != this.previousHighlightTargetHurtBox)
            {
                this.previousHighlightTargetHurtBox = hurtBox;
                this.previouslyHighlightedTargetHealthComponent = y;
                this.targetIndicator.targetTransform = ((hurtBox && base.activatorSkillSlot.stock != 0) ? hurtBox.transform : null);
                this.stackStopwatch = 0f;
            }
            this.targetIndicator.active = this.targetIndicator.targetTransform;
            if (flag)
            {
                this.queuedFiringState = true;
                this.outer.SetNextState(new HandCrossbowFire
                {
                    targetList = this.targetList,
                    activatorSkillSlot = base.activatorSkillSlot
                });
            }

        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            base.characterBody.SetAimTimer(3f);
            if (base.isAuthority)
            {
                this.AuthorityFixedUpdate();
            }
        }

        private void GetCurrentTargetInfo(out HurtBox currentTargetHurtBox, out HealthComponent currentTargetHealthComponent)
        {
            Ray aimRay = base.GetAimRay();
            this.search.filterByDistinctEntity = true;
            this.search.filterByLoS = true;
            this.search.minDistanceFilter = 0f;
            this.search.maxDistanceFilter = HandCrossbowPaint.maxDistance;
            this.search.minAngleFilter = 0f;
            this.search.maxAngleFilter = HandCrossbowPaint.maxAngle;
            this.search.viewer = base.characterBody;
            this.search.searchOrigin = aimRay.origin;
            this.search.searchDirection = aimRay.direction;
            this.search.sortMode = BullseyeSearch.SortMode.DistanceAndAngle;
            this.search.teamMaskFilter = TeamMask.GetUnprotectedTeams(base.GetTeam());
            this.search.RefreshCandidates();
            this.search.FilterOutGameObject(base.gameObject);
            foreach (HurtBox hurtBox in this.search.GetResults())
            {
                if (hurtBox.healthComponent && hurtBox.healthComponent.alive)
                {
                    currentTargetHurtBox = hurtBox;
                    currentTargetHealthComponent = hurtBox.healthComponent;
                    return;
                }
            }
            currentTargetHurtBox = null;
            currentTargetHealthComponent = null;
        }

        private void CleanTargetList()
        {
            for (int i = this.targetList.Count - 1; i >= 0; i--)
            {
                HurtBox hurtBox = this.targetList[i];
                if (!hurtBox.healthComponent || !hurtBox.healthComponent.alive)
                {
                    this.RemoveTargetAtAuthority(i);
                    base.activatorSkillSlot.AddOneStock();
                }
            }
            for (int j = this.targetList.Count - 1; j >= base.activatorSkillSlot.maxStock; j--)
            {
                this.RemoveTargetAtAuthority(j);
            }
        }


        private void AddTargetAuthority(HurtBox hurtBox)
        {
            if (base.activatorSkillSlot.stock == 0)
            {
                return;
            }
            //Util.PlaySound(HandCrossbowPaint.lockOnSoundString, base.gameObject);
            this.targetList.Add(hurtBox);
            HandCrossbowPaint.CrossbowIndicatorInfo indicatorInfo;
            if (!this.targetIndicators.TryGetValue(hurtBox, out indicatorInfo))
            {
                indicatorInfo = new HandCrossbowPaint.CrossbowIndicatorInfo
                {
                    refCount = 0,
                    indicator = new HandCrossbowBoltIndicator(base.gameObject, LegacyResourcesAPI.Load<GameObject>("Prefabs/EngiMissileTrackingIndicator"))
                };
                indicatorInfo.indicator.targetTransform = hurtBox.transform;
                indicatorInfo.indicator.active = true;
            }
            indicatorInfo.refCount++;
            indicatorInfo.indicator.boltCount = indicatorInfo.refCount;
            this.targetIndicators[hurtBox] = indicatorInfo;
            base.activatorSkillSlot.DeductStock(1);
        }

        private void RemoveTargetAtAuthority(int i)
        {
            HurtBox key = this.targetList[i];
            this.targetList.RemoveAt(i);
            HandCrossbowPaint.CrossbowIndicatorInfo indicatorInfo;
            if (this.targetIndicators.TryGetValue(key, out indicatorInfo))
            {
                indicatorInfo.refCount--;
                indicatorInfo.indicator.boltCount = indicatorInfo.refCount;
                this.targetIndicators[key] = indicatorInfo;
                if (indicatorInfo.refCount == 0)
                {
                    indicatorInfo.indicator.active = false;
                    this.targetIndicators.Remove(key);
                }
            }
        }

    }
}