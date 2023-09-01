using UnityEngine;
using BepInEx.Configuration;
using RoR2;
using R2API;
using System;

using HarmonyLib;


namespace FirstLightMod.Modules.Items
{

    // The directly below is entirely from TILER2 API (by ThinkInvis) specifically the Item module. Utilized to keep instance checking functionality as I migrate off TILER2.
    // TILER2 API can be found at the following places:
    // https://github.com/ThinkInvis/RoR2-TILER2
    // https://thunderstore.io/package/ThinkInvis/TILER2/

    public abstract class ItemBase<T> : ItemBase where T : ItemBase<T>
    {
        public static T instance { get; private set; }

        public ItemBase()
        {
            if (instance != null) throw new InvalidOperationException("Singleton class \"" + typeof(T).Name + "\" inheriting ItemBase was instantiated twice");
            instance = this as T;
        }
    }


    public abstract class ItemBase
    {
        string prefix = "ITEM_";
        //string prefix = FirstLightPlugin.DEVELOPER_PREFIX + "_ITEM_";

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
            ItemDef.tags = ItemTags;
            ItemDef.deprecatedTier = Tier;


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
