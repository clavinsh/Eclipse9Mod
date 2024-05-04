using System.Collections.Generic;
using System.Linq;
using RoR2;

namespace ExamplePlugin
{
    internal class DeBuffPersister
    {
        // curse stacks will be tied to the specific player,
        // we can simply tie the stacks (integer), to the player's name (string)
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
            // when stats are recalculated for a player's body, the username is retrieved and stacks are checked against the dictionary (playersCurseStacks)
            if (self.isPlayerControlled && self.teamComponent.teamIndex == TeamIndex.Player)
            {
                string name = self.GetUserName();
                int savedCurseStacks = playersCurseStacks[name];
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
