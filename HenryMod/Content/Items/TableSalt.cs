using BepInEx.Configuration;
using FirstLightMod.Modules.Items;
using R2API;
using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace FirstLightMod.Content.Items
{
    public class TableSalt : ItemBase
    {
        public override string ItemName => throw new NotImplementedException();
        public override string ItemNameToken => throw new NotImplementedException();
        public override string ItemPickupDescription => throw new NotImplementedException();
        public override string ItemFullDescription => throw new NotImplementedException();
        public override string ItemLore => throw new NotImplementedException();
        public override ItemTier Tier => throw new NotImplementedException();
        public override GameObject ItemModel => throw new NotImplementedException();
        public override Sprite ItemIcon => throw new NotImplementedException();

        public float DamageIncreaseCoefficient;
        public float DamageIncreaseCoefficitentPerStack;

        public override void Init(ConfigFile config)
        {
            throw new NotImplementedException();
        }
        public override ItemDisplayRuleDict CreateItemDisplayRules()
        {
            throw new NotImplementedException();
        }

        public override void Hooks()
        {
            throw new NotImplementedException();
        }

    }
}
