using BepInEx;
using R2API;
using R2API.Utils;
using RoR2;
using System;
using System.Security.Permissions;


[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]
namespace ExamplePlugin
{
    [BepInDependency(LanguageAPI.PluginGUID)]
    [BepInPlugin(PluginGUID, PluginName, PluginVersion)]
    public class ExamplePlugin : BaseUnityPlugin
    {
        // The Plugin GUID should be a unique ID for this plugin,
        // which is human readable (as it is used in places like the config).
        // If we see this PluginGUID as it is on thunderstore,
        // we will deprecate this mod.
        // Change the PluginAuthor and the PluginName !
        public const string PluginGUID = PluginAuthor + "." + PluginName;
        public const string PluginAuthor = "AuthorName";
        public const string PluginName = "ExamplePlugin";
        public const string PluginVersion = "1.0.0";

        public static DifficultyIndex myIndex;

        public void Awake()
        {

            DifficultyDef myDef = new(
            10f, //This is the scaling factor, and decides how quickly the difficulty ramps up. drizzle is 1, rainstorm=2, monsoon=3.
            "Lobotomy",//The name token. consider using AssetPlus.Language to add your tokens.
            "", //The iconPath, You can use a vanilla icon, or with use of AssetAPI/ResourceAPI use your own custom one.
            "Lobotomy difficulty, totally unfair scaling, 10x monsoon",//The description token. consider using AssetPlus.Language to add your tokens.
            new UnityEngine.Color(0.5f, 0.1f, 0.2f), //The color that appears when hovering over this in the rulebook.
            "serverTag??",
            true // if beaten, unlocks Survivor's Mastery skin

            );

            //sets the diff index to positive, will position this custom diff after eclipse diffs, applies the eclipse modifiers to it

            bool preferPositive = true;

            myIndex = R2API.DifficultyAPI.AddDifficulty(myDef, preferPositive);

            RoR2.Run.onRunStartGlobal += (RoR2.Run run) =>
            {
                if (run.selectedDifficulty == myIndex)
                {
                    ChatMessage.Send("Lobotomy difficulty selected"!);
                    //Ideally you'd hook your methods here, so you don't need to check in your changes if the current run is selected. 
                }
            };
            RoR2.Run.onRunDestroyGlobal += (RoR2.Run run) =>
            {
                if (run.selectedDifficulty == myIndex)
                {
                    //If you made changes to the game, specifically for your own difficulty, this would be the ideal moment to undo them and return the game to a vanilla state.
                }
            };

            SceneDirector.onPreGeneratePlayerSpawnPointsServer += GlobalEventManager_onPreGeneratePlayerSpawnPointsServer;

            Run.instance.
        }

        private void GlobalEventManager_onPreGeneratePlayerSpawnPointsServer(SceneDirector sceneDirector, ref Action generationMethod)
        {
            ChatMessage.Send("player spawnpoint event triggered");

            throw new NotImplementedException();
        }

    }
}
