using System.Collections.Generic;
using System.Linq;
using RoR2;

namespace ExamplePlugin
{
    internal class DeBuffPersister
    {
        // curse stacks will be tied to the specific player,
        // we can simply tie the stacks (integer), to the player's name (string)
        // player's name is the simplest unique identifier
        readonly Dictionary<string, int> playersCurseStacks;

        readonly BuffDef permanentCurse = RoR2Content.Buffs.PermanentCurse;

        public DeBuffPersister()
        {
            playersCurseStacks = [];
            On.RoR2.CharacterBody.RecalculateStats += CharacterBody_RecalculateStats;
        }

        private void CharacterBody_RecalculateStats(
            On.RoR2.CharacterBody.orig_RecalculateStats orig,
            CharacterBody self
        )
        {
            if (self.isPlayerControlled)
            {
                bool real = PlayerCharacterMasterController.instances.Any(a => a.body.Equals(self));

                //PlayerCharacterMasterController.instances[i].playerControllerId

                string name = self.GetUserName();

                // if no value is found, it gets the default 0, which is fine
                playersCurseStacks.TryGetValue(name, out int savedCurseStacks);
                int currentCurseStacks = self.GetBuffCount(permanentCurse);

                if (currentCurseStacks < savedCurseStacks)
                {
                    self.SetBuffCount(
                        RoR2Content.Buffs.PermanentCurse.buffIndex,
                        playersCurseStacks[name]
                    );
                }
                else if (currentCurseStacks > savedCurseStacks)
                {
                    playersCurseStacks[name] = currentCurseStacks;
                }
            }

            orig(self);
        }

        public void Unsubscribe()
        {
            playersCurseStacks.Clear();
            On.RoR2.CharacterBody.RecalculateStats -= CharacterBody_RecalculateStats;
        }
    }
}
