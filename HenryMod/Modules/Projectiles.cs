using FirstLightMod.Components;
using IL.RoR2.Items;
using R2API;
using RoR2;
using RoR2.Projectile;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;

namespace FirstLightMod.Modules
{
    internal static class Projectiles
    {
        
        //Deployables may also be treated as new projectiles?
        internal static GameObject bombPrefab;
        //internal static GameObject shovelPrefab;
        internal static GameObject grovePrefab;

        //internal static GameObject RazorGrenadePrefab;
        //internal static GameObject RazorGrenadeGhost;



        internal static void RegisterProjectiles() 
        {
            CreateBomb();
            //CreateGrenades();
            //CreateShovel();
            //CreateGrove();

            AddProjectile(bombPrefab);
            //AddProjectile(shovelPrefab);
            //AddProjectile(grovePrefab);
        }

        internal static void AddProjectile(GameObject projectileToAdd)
        {
            Modules.Content.AddProjectilePrefab(projectileToAdd);
        }


        //private static void CreateGrenades()
        //{
        //    RazorGrenadePrefab = CloneProjectilePrefab("CryoCannisterProjectile", "RazorGrenade");
        //    RazorGrenadeGhost = CreateGhostPrefab("CryoCannisterGhost");



        //}


        private static void CreateShovel()
        {
            //shovelPrefab = CloneProjectilePrefab("ToolbotGrenadeLauncherProjectile", "shovel");
            //Rigidbody rigidbody = shovelPrefab.GetComponent<Rigidbody>();
            //rigidbody.useGravity = true;

            //shovelPrefab.transform.localScale = new Vector3(2.5f, 2.5f, 2.5f);

            //ProjectileController shovelController = shovelPrefab.GetComponent<ProjectileController>();
            //shovelController.ghostPrefab = CreateGhostPrefab("ShovelGhost");

            //ProjectileSimple ps = shovelPrefab.GetComponent<ProjectileSimple>();
            //ps.desiredForwardSpeed = 200f;

            //ProjectileImpactExplosion impactExplosion = shovelPrefab.GetComponent<ProjectileImpactExplosion>();
            //impactExplosion.blastRadius = 20;
            //impactExplosion.falloffModel = BlastAttack.FalloffModel.SweetSpot;

            //impactExplosion.impactEffect = LegacyResourcesAPI.Load<GameObject>("prefabs/effects/omnieffect/OmniExplosionVFX");

        }

        private static void CreateGrove()
        {
            //grovePrefab = CloneProjectilePrefab("EngiMine", "FarmGrove");
       


            //grovePrefab = CloneProjectilePrefab("EngiBubbleShield", "FarmGrove");
            //UnityEngine.GameObject.Destroy(grovePrefab.GetComponent<ProjectileController>());


            grovePrefab = PrefabAPI.InstantiateClone(LegacyResourcesAPI.Load<GameObject>("Prefabs/NetworkedObjects/MushroomWard"), "FarmGrove", true); //the mushrooom field
            //MushroomWard has a healingWard

            ////These components should already be a part of the prefab but this is for safety

            if (grovePrefab.GetComponent<TeamFilter>() == null)
            {
                grovePrefab.AddComponent<TeamFilter>();
            }
            HealingWard groveHealingWard = grovePrefab.GetComponent<HealingWard>();


            Components.BungalGroveController groveController = grovePrefab.AddComponent<BungalGroveController>();

            groveController.groveHealingWard = groveHealingWard;
            groveController.owner = grovePrefab.gameObject;

            grovePrefab.AddComponent<DestroyOnTimer>().duration = 10f;
            
            
        }


        #region Bomb

        private static void CreateBomb()
        {
            bombPrefab = CloneProjectilePrefab("CommandoGrenadeProjectile", "HenryBombProjectile");

            ProjectileImpactExplosion bombImpactExplosion = bombPrefab.GetComponent<ProjectileImpactExplosion>();
            InitializeImpactExplosion(bombImpactExplosion);

            bombImpactExplosion.blastRadius = 16f;
            bombImpactExplosion.destroyOnEnemy = true;
            bombImpactExplosion.lifetime = 12f;
            bombImpactExplosion.impactEffect = Modules.Assets.bombExplosionEffect;
            //bombImpactExplosion.lifetimeExpiredSound = Modules.Assets.CreateNetworkSoundEventDef("HenryBombExplosion");
            bombImpactExplosion.timerAfterImpact = true;
            bombImpactExplosion.lifetimeAfterImpact = 0.1f;

            ProjectileController bombController = bombPrefab.GetComponent<ProjectileController>();
            if (Modules.Assets.mainAssetBundle.LoadAsset<GameObject>("HenryBombGhost") != null) bombController.ghostPrefab = CreateGhostPrefab("HenryBombGhost");
            bombController.startSound = "";
        }

        private static void InitializeImpactExplosion(ProjectileImpactExplosion projectileImpactExplosion)
        {
            projectileImpactExplosion.blastDamageCoefficient = 1f;
            projectileImpactExplosion.blastProcCoefficient = 1f;
            projectileImpactExplosion.blastRadius = 1f;
            projectileImpactExplosion.bonusBlastForce = Vector3.zero;
            projectileImpactExplosion.childrenCount = 0;
            projectileImpactExplosion.childrenDamageCoefficient = 0f;
            projectileImpactExplosion.childrenProjectilePrefab = null;
            projectileImpactExplosion.destroyOnEnemy = false;
            projectileImpactExplosion.destroyOnWorld = false;
            projectileImpactExplosion.falloffModel = RoR2.BlastAttack.FalloffModel.None;
            projectileImpactExplosion.fireChildren = false;
            projectileImpactExplosion.impactEffect = null;
            projectileImpactExplosion.lifetime = 0f;
            projectileImpactExplosion.lifetimeAfterImpact = 0f;
            projectileImpactExplosion.lifetimeRandomOffset = 0f;
            projectileImpactExplosion.offsetForLifetimeExpiredSound = 0f;
            projectileImpactExplosion.timerAfterImpact = false;

            projectileImpactExplosion.GetComponent<ProjectileDamage>().damageType = DamageType.Generic;
        }

        private static GameObject CreateGhostPrefab(string ghostName)
        {
            GameObject ghostPrefab = Modules.Assets.mainAssetBundle.LoadAsset<GameObject>(ghostName);
            if (!ghostPrefab.GetComponent<NetworkIdentity>()) ghostPrefab.AddComponent<NetworkIdentity>();
            if (!ghostPrefab.GetComponent<ProjectileGhostController>()) ghostPrefab.AddComponent<ProjectileGhostController>();

            Modules.Assets.ConvertAllRenderersToHopooShader(ghostPrefab);

            return ghostPrefab;
        }


        #endregion

        private static GameObject CloneProjectilePrefab(string prefabName, string newPrefabName)
        {
            GameObject newPrefab = PrefabAPI.InstantiateClone(RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/Projectiles/" + prefabName), newPrefabName);
            return newPrefab;
        }
    }
}