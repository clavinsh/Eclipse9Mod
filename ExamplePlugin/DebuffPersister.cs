using RoR2;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ExamplePlugin
{
    internal class DebuffPersister
    {
        private readonly List<int> playersCurseStacks = [];

        public void UpdateStacks(ReadOnlyCollection<PlayerCharacterMasterController> controllerInstances)
        {
            for (int i = 0; i < controllerInstances.Count; i++)
            {
                playersCurseStacks[i] = controllerInstances[i].body.GetBuffCount(RoR2Content.Buffs.PermanentCurse.buffIndex);
            }
        }

        public void SetStacks(ReadOnlyCollection<PlayerCharacterMasterController> controllerInstances)
        {
            for (int i = 0; i < controllerInstances.Count; i++)
            {
                PlayerCharacterMasterController.instances[i].body.SetBuffCount(RoR2Content.Buffs.PermanentCurse.buffIndex, playersCurseStacks[i]);
            }
        }
    }
}
