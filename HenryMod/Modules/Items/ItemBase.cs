using UnityEngine;
using BepInEx.Configuration;
using RoR2;
using R2API;
using System;
using System.Collections.Generic;
using System.Text;
using System.Net.NetworkInformation;

namespace FirstLightMod.Modules.Items
{
    public abstract class ItemBase
    {
        string prefix = FirstLightPlugin.DEVELOPER_PREFIX + "_ITEM_";

        public abstract string ItemName { get; }
        public abstract string ItemNameToken { get; }
        public abstract string ItemPickupDescription { get; }
        public abstract string ItemFullDescription { get; }
        public abstract string ItemLore { get; }

        public abstract ItemTier Tier { get; }
        public virtual ItemTag[] ItemTags { get; } 

        public abstract GameObject ItemModel { get; }
        public abstract Sprite ItemIcon { get; }

        public virtual bool CanRemove { get; } = true;
        public virtual bool Hidden { get; } = false;

        public ItemDef ItemDef;
        public abstract void Init(ConfigFile config);


        protected void CreateLang()
        {
            LanguageAPI.Add(prefix + ItemNameToken + "_NAME", ItemName);
            LanguageAPI.Add(prefix + ItemNameToken + "_PICKUP", ItemPickupDescription);
            LanguageAPI.Add(prefix + ItemNameToken + "_DESCRIPTION", ItemFullDescription);
            LanguageAPI.Add(prefix + ItemNameToken + "_LORE", ItemLore);
        }

        public abstract ItemDisplayRuleDict CreateItemDisplayRules();


        protected void CreateItem()
        {
            ItemDef = ScriptableObject.CreateInstance<ItemDef>();
            ItemDef.name = prefix + ItemNameToken;
            ItemDef.nameToken = prefix + ItemNameToken + "_Name";
            ItemDef.pickupToken = prefix + ItemNameToken + "_PICKUP";
            ItemDef.descriptionToken = prefix + ItemNameToken + "_DESCRIPTION";
            ItemDef.loreToken = prefix + ItemNameToken + "_LORE";
            ItemDef.pickupModelPrefab = ItemModel;
            ItemDef.pickupIconSprite = ItemIcon;
            ItemDef.hidden = false;
            ItemDef.canRemove = CanRemove;
            ItemDef.tier = Tier;
            ItemDef.tags = ItemTags;

            var itemDisplayRulesDict = CreateItemDisplayRules();
            ItemAPI.Add(new CustomItem(ItemDef, itemDisplayRulesDict));
        }

        public abstract void Hooks();
  

        public int GetCount(CharacterBody body)
        {
            if (!body || !body.inventory) { return 0; }

            return body.inventory.GetItemCount(ItemDef);
        }

        public int GetCount(CharacterMaster master)
        {
            if (!master || !master.inventory) { return 0; }

            return master.inventory.GetItemCount(ItemDef);  
        }

    }
}
