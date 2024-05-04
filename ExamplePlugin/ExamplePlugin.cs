using BepInEx;
using RoR2;
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

        private readonly DeBuffPersister buffPersister = new();

        public void Awake()
        {
            Log.Init(Logger);

            RoR2.Stage.onStageStartGlobal += Stage_onStageStartGlobal;

            On.RoR2.SceneExitController.SetState += SceneExitController_SetState;
        }

        private void SceneExitController_SetState(On.RoR2.SceneExitController.orig_SetState orig, SceneExitController self, SceneExitController.ExitState newState)
        {
            if (newState == SceneExitController.ExitState.TeleportOut)
            {
                buffPersister.GetBuffStacks();
            }

            orig(self, newState);
        }

        private void Stage_onStageStartGlobal(Stage obj)
        {
            buffPersister.SetBuffStacks();
        }
    }
}
