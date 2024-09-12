using System;
using System.Collections.Generic;
using System.Text;
using RoR2;
using UnityEngine;

namespace FirstLightMod.Survivors.WeaponMaster.SkillStates.HandCrossbow
{
    public class HandCrossbowBoltIndicator : Indicator
    {

        public int boltCount;

        public HandCrossbowBoltIndicator(GameObject owner, GameObject visualizerPrefab) : base(owner, visualizerPrefab)
        {
        }

        public override void UpdateVisualizer()
        {
            base.UpdateVisualizer();
            Transform transform = base.visualizerTransform.Find("DotOrigin");
            for (int i = transform.childCount - 1; i >= this.boltCount; i--)
            {
                EntityStates.EntityState.Destroy(transform.GetChild(i));
            }
            for (int j = transform.childCount; j < this.boltCount; j++)
            {
                UnityEngine.Object.Instantiate<GameObject>(base.visualizerPrefab.transform.Find("DotOrigin/DotTemplate").gameObject, transform);
            }
            if (transform.childCount > 0)
            {
                float num = 360f / (float)transform.childCount;
                float num2 = (float)(transform.childCount - 1) * 90f;
                for (int k = 0; k < transform.childCount; k++)
                {
                    Transform child = transform.GetChild(k);
                    child.gameObject.SetActive(true);
                    child.localRotation = Quaternion.Euler(0f, 0f, num2 + (float)k * num);
                }
            }
        }

    }
}
