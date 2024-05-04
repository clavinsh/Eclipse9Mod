using System.Security.Permissions;
using BepInEx;
using RoR2;

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

        private DeBuffPersister deBuffPersister;
        public static DifficultyIndex Eclipse9;

        public void Awake()
        {
            Log.Init(Logger);

            DifficultyDef eclipse9def =
                new(
                    3f, //This is the scaling factor, and decides how quickly the difficulty ramps up. drizzle is 1, rainstorm=2, monsoon=3.
                    "Eclipse", //The name token. consider using AssetPlus.Language to add your tokens.
                    "", //The iconPath, You can use a vanilla icon, or with use of AssetAPI/ResourceAPI use your own custom one.
                    "Damage is actually permanent", //The description token. consider using AssetPlus.Language to add your tokens.
                    new UnityEngine.Color(0.5f, 0.1f, 0.2f), //The color that appears when hovering over this in the rulebook.
                    "eclipse9", //serverTag
                    true // if beaten, unlocks Survivor's Mastery skin
                );

            Eclipse9 = R2API.DifficultyAPI.AddDifficulty(eclipse9def, true); // true variable (prefer positive) will add eclipse levels

            RoR2.Run.onRunStartGlobal += Run_onRunStartGlobal;

            RoR2.Run.onRunDestroyGlobal += Run_onRunDestroyGlobal;
        }

        private void Run_onRunStartGlobal(Run run)
        {
            if (run.selectedDifficulty == Eclipse9)
            {
                deBuffPersister = new();
            }
        }

        private void Run_onRunDestroyGlobal(Run run)
        {
            if (run.selectedDifficulty == Eclipse9)
            {
                deBuffPersister.Unsubscribe();
            }
        }
    }
}
