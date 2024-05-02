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
        public const string PluginName = "ExamplePlugin";
        public const string PluginVersion = "1.0.0";

        private int curseStacks = 0;

        public void Awake()
        {
            RoR2.Stage.onStageStartGlobal += Stage_onStageStartGlobal;

            On.RoR2.Run.AdvanceStage += Run_AdvanceStage;
        }

        private void Stage_onStageStartGlobal(Stage obj)
        {
            var networkCharMaybe = NetworkUser.readOnlyInstancesList[0];
            networkCharMaybe.GetCurrentBody().SetBuffCount(RoR2Content.Buffs.PermanentCurse.buffIndex, curseStacks);
        }

        private void Run_AdvanceStage(On.RoR2.Run.orig_AdvanceStage orig, Run self, SceneDef nextScene)
        {
            curseStacks = PlayerCharacterMasterController.instances[0].body.GetBuffCount(RoR2Content.Buffs.PermanentCurse);
            orig(self, nextScene);
        }
    }
}
