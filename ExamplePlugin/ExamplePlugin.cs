using BepInEx;
using R2API;
using R2API.Utils;
using RoR2;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Permissions;
using UnityEngine;

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

        private List<int> curseStacks = [];

        public void Awake()
        {
            Log.Init(Logger);

            Logger.LogMessage($"Mod loaded yipee v2");


            RoR2.Run.onRunStartGlobal += Run_onRunStartGlobal;

            On.RoR2.Run.BeginStage += Run_BeginStage;
            On.RoR2.Run.EndStage += Run_EndStage;

            On.RoR2.Run.AdvanceStage += Run_AdvanceStage;

            Logger.LogMessage($"Awake completed, hopefully");
        }

        private void Run_AdvanceStage(On.RoR2.Run.orig_AdvanceStage orig, Run self, SceneDef nextScene)
        {
            Logger.LogMessage($"Run_AdvanceStage");
        }

        private void Run_onRunStartGlobal(Run obj)
        {
            Logger.LogMessage($"Run_onRunStartGlobal");
        }

        private void Run_Start(On.RoR2.Run.orig_Start orig, Run self)
        {
            Logger.LogMessage($"Run_Start function executed");
            orig(self);

            // initialize curse stacks for each player of this run
            var playerInstances = PlayerCharacterMasterController.instances;
            curseStacks = playerInstances.Select(p => p.body.GetBuffCount(RoR2Content.Buffs.PermanentCurse.buffIndex)).ToList();
            Logger.LogMessage($"Run started, there are {playerInstances.Count} player(-s) playing");
        }

        private void Run_EndStage(On.RoR2.Run.orig_EndStage orig, Run self)
        {
            Logger.LogMessage($"Run_EndStage function executed");
            // save the gathered curse stacks at the end of the run
            curseStacks = PlayerCharacterMasterController.instances.Select(p => p.body.GetBuffCount(RoR2Content.Buffs.PermanentCurse.buffIndex)).ToList();

            for (int i = 0; i < curseStacks.Count; i++)
            {
                Logger.LogMessage($"Saved {curseStacks[i]} Permanent Curse stacks at end of stage for player {i}");
            }

            orig(self);
        }

        private void Run_BeginStage(On.RoR2.Run.orig_BeginStage orig, Run self)
        {
            Logger.LogMessage($"Run_BeginStage function executed");
            orig(self);


            for (int i = 0; i < curseStacks.Count; i++)
            {
                PlayerCharacterMasterController.instances[i].body.SetBuffCount(RoR2Content.Buffs.PermanentCurse.buffIndex, curseStacks[i]);

                Logger.LogMessage($"Set {curseStacks[i]} Permanent Curse stacks at the start of stage for player {i}");
            }
        }
    }
}
