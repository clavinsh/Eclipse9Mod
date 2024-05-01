using BepInEx;
using RoR2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;

[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]
namespace ExamplePlugin
{
    [BepInDependency("com.bepis.r2api")]
    [BepInPlugin(PluginGUID, PluginName, PluginVersion)]
    public class ExamplePlugin : BaseUnityPlugin
    {
        public const string PluginGUID = PluginAuthor + "." + PluginName;
        public const string PluginAuthor = "hoozy";
        public const string PluginName = "PersistentDebuffMod";
        public const string PluginVersion = "1.0.0";

        private readonly DebuffPersister persister;

        public void Awake()
        {
            Log.Init(Logger);

            On.RoR2.Run.BeginStage += Run_BeginStage;
            On.RoR2.Run.AdvanceStage += Run_AdvanceStage;
        }

        private void Run_BeginStage(On.RoR2.Run.orig_BeginStage orig, Run self)
        {
            orig(self);
            persister.SetStacks(PlayerCharacterMasterController.instances);
        }

        private void Run_AdvanceStage(On.RoR2.Run.orig_AdvanceStage orig, Run self, SceneDef nextScene)
        {
            persister.UpdateStacks(PlayerCharacterMasterController.instances);
            orig(self, nextScene);
        }
    }
}
