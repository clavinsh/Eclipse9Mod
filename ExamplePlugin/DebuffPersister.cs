using System.Collections.Generic;
using System.Linq;
using RoR2;

namespace ExamplePlugin
{
    internal class DeBuffPersister
    {
        readonly Dictionary<string, int> playersCurseStacks;

        readonly BuffDef permanentCurse = RoR2Content.Buffs.PermanentCurse;

        public DeBuffPersister()
        {
            playersCurseStacks = [];

            On.RoR2.CharacterBody.GetBuffCount_BuffDef += CharacterBody_GetBuffCount_BuffDef;
        }

        private int CharacterBody_GetBuffCount_BuffDef(
            On.RoR2.CharacterBody.orig_GetBuffCount_BuffDef orig,
            CharacterBody self,
            BuffDef buffDef
        )
        {
            if (!self.isPlayerControlled)
            {
                return orig(self, buffDef);
            }

            if (buffDef != permanentCurse)
            {
                return orig(self, buffDef);
            }

            string name = self.GetUserName(); // should find a better player identifier than the username

            // if no value is found, it gets the default 0, which is fine
            playersCurseStacks.TryGetValue(name, out int savedCurseStacks);
            int currentCurseStacks = self.GetBuffCount(permanentCurse);

            if (currentCurseStacks > savedCurseStacks)
            {
                playersCurseStacks[name] = currentCurseStacks;
            }
            else
            {
                self.SetBuffCount(permanentCurse.buffIndex, savedCurseStacks);
            }

            return orig(self, buffDef);
        }

        public void Unsubscribe()
        {
            playersCurseStacks.Clear();
            On.RoR2.CharacterBody.GetBuffCount_BuffDef -= CharacterBody_GetBuffCount_BuffDef;
        }
    }
}
