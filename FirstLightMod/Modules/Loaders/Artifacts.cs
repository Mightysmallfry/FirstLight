using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using BepInEx.Configuration;
using FirstLightMod.Modules.BaseContent.Artifacts;
using FirstLightMod.Modules.Items;

namespace FirstLightMod.Modules.Loaders
{
    internal static class Artifacts
    {
        public static int numberOfLoadedArtifacts = 0;
        public static List<ArtifactBase> artifactBases = new List<ArtifactBase>();
        public static ConfigFile config = Config.MyConfig;


        public static void Init()
        {
            Log.Info("-------------------Artifacts----------------------");

            //This section automatically scans the project for all items
            var ArtifactTypes = Assembly.GetExecutingAssembly().GetTypes().Where(type => !type.IsAbstract && type.IsSubclassOf(typeof(ArtifactBase)));


            foreach (var artifactType in ArtifactTypes)
            {
                ArtifactBase artifact = (ArtifactBase)System.Activator.CreateInstance(artifactType);

                if (ValidateItem(artifact, artifactBases))
                {
                    Log.Info("Started Loading Item : " + artifact.ArtifactName);
                    artifact.Init(config);
                    numberOfLoadedArtifacts++;
                    Log.Info("Finished Item : " + artifact.ArtifactName);
                }
            }
        }

        /// <summary>
        /// A helper to easily set up and initialize an artifact from your item classes if the user has it enabled in their configuration files.
        /// </summary>
        /// <param name="artifact">A new instance of an ArtifactBase class."</param>
        /// <param name="artifactList">The list you would like to add this to if it passes the config check.</param>
        public static bool ValidateItem(ArtifactBase artifact, List<ArtifactBase> artifactList)
        {
            var enabled = config.Bind("Artifact: " + artifact.ArtifactName, "Enable", true, "Should this artifact appear?").Value;
            if (enabled)
            {
                artifactList.Add(artifact);
            }
            return enabled;
        }
    }
}
