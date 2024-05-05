using System.Collections.Generic;
using System.Linq;
using HG;
using RoR2;

namespace Eclipse9
{
    internal class DeBuffPersister
    {
        readonly Dictionary<string, int> playersCurseStacks;

        readonly BuffDef permanentCurse = RoR2Content.Buffs.PermanentCurse;

        public DeBuffPersister()
        {
            playersCurseStacks = [];

            On.RoR2.CharacterBody.GetBuffCount_BuffIndex += CharacterBody_GetBuffCount_BuffIndex;
        }
        public void Unsubscribe()
        {
            playersCurseStacks.Clear();
            On.RoR2.CharacterBody.GetBuffCount_BuffIndex -= CharacterBody_GetBuffCount_BuffIndex;
        }

        private int CharacterBody_GetBuffCount_BuffIndex(On.RoR2.CharacterBody.orig_GetBuffCount_BuffIndex orig, CharacterBody self, BuffIndex buffType)
        {
            if(!self.isPlayerControlled)
            {
                return orig(self, buffType);
            }

            if (buffType != permanentCurse.buffIndex)
            {
                return orig(self, buffType);
            }

            UpdateStacks(self);

            return orig(self, buffType);
        }

        private void UpdateStacks(CharacterBody characterBody)
        { 
            string name = characterBody.GetUserName(); // should find a better player identifier than the username
            var hmm = characterBody.netId;

            playersCurseStacks.TryGetValue(name, out int savedCurseStacks);
            int currentCurseStacks = NonInvokingGetBuffCount(characterBody);

            if(currentCurseStacks == savedCurseStacks)
            {
                return;
            }
            if (currentCurseStacks > savedCurseStacks)
            {
                playersCurseStacks[name] = currentCurseStacks;
            }
            else
            {
                characterBody.SetBuffCount(permanentCurse.buffIndex, savedCurseStacks);
            }
        }

        private int NonInvokingGetBuffCount(CharacterBody characterBody)
        {
            return ArrayUtils.GetSafe<int>(characterBody.buffs, (int)permanentCurse.buffIndex);
        }
    }
}
