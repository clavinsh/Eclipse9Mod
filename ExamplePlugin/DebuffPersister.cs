using System.Collections.Generic;
using System.Linq;
using RoR2;

namespace ExamplePlugin
{
    internal class DeBuffPersister
    {
        private readonly List<int> playersCurseStacks;
        private readonly int DefaultMaxPlayerCount = 4;

        public DeBuffPersister()
        {
            playersCurseStacks = Enumerable.Repeat(0, DefaultMaxPlayerCount).ToList();
        }

        public void GetBuffStacks()
        {
            var networkUsers = NetworkUser.readOnlyInstancesList;

            for (int i = 0; i < networkUsers.Count; i++)
            {
                int currentCurseStacks = networkUsers[i]
                    .GetCurrentBody()
                    .GetBuffCount(RoR2Content.Buffs.PermanentCurse);

                if (i >= playersCurseStacks.Count)
                {
                    playersCurseStacks.Add(currentCurseStacks);
                }
                else
                {
                    playersCurseStacks[i] = currentCurseStacks;
                }
            }
        }

        public void SetBuffStacks()
        {
            var networkUsers = NetworkUser.readOnlyInstancesList;

            for (int i = 0; i < networkUsers.Count; i++)
            {
                if (i < playersCurseStacks.Count)
                {
                    networkUsers[i]
                        .GetCurrentBody()
                        .SetBuffCount(
                            RoR2Content.Buffs.PermanentCurse.buffIndex,
                            playersCurseStacks[i]
                        );
                }
                // curse stacks list didn't account for this user
                else
                {
                    Log.Warning(
                        $"There were more network users ({networkUsers.Count}) than this mod accounted for ({playersCurseStacks.Count}) at the end of the stage"
                    );
                    playersCurseStacks.Add(0);
                }
            }
        }
    }
}
