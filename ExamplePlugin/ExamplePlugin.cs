using BepInEx;
using R2API;
using R2API.Utils;
using RoR2;
using System;
using System.Linq;
using System.Security.Permissions;
using UnityEngine;


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


        private const string PermanentCurseDeBuffInternalName = "PermanentCurse";

        public static DifficultyIndex myIndex;


        private int curseStacks = 0;


        public void Awake()
        {
            Logger.LogMessage("TESTING 123 my mod supposedly loaded");
        }

        public void Update()
        {
            // save permanent curse stacks
            if(Input.GetKeyDown(KeyCode.F2))
            {
                curseStacks = PlayerCharacterMasterController.instances[0].body.GetBuffCount(RoR2Content.Buffs.PermanentCurse.buffIndex);

                Logger.LogMessage($"{curseStacks} Permanent Curse stacks saved");
            }

            // load saved permanent curse stacks
            if (Input.GetKeyDown(KeyCode.F3))
            {
                PlayerCharacterMasterController.instances[0].body.SetBuffCount(RoR2Content.Buffs.PermanentCurse.buffIndex, curseStacks);

                Logger.LogMessage($"{curseStacks} Permanent Curse applied to player");
            }
        }
    }
}
