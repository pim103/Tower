using System.Collections.Generic;
using Games.Global.Entities;
using Games.Global.Spells;
using Games.Global.TreeBehavior.TestTreeBehavior;
using UnityEngine;

namespace Games.Global.TreeBehavior.LeafBehavior
{
    public class CheckSpellWithTag : Leaf
    {
        private SpellTag wantedTag;
        public CheckSpellWithTag(SpellTag tag)
        {
            wantedTag = tag;
        }

        public override TreeStatus OnExecute(BehaviorStatus behaviorStatus)
        {
            Monster monster = (behaviorStatus as GameContext).CurrentMonster;
            List<Spell> wantedSpell = UtilsLeaf.HasSpellFromTag(wantedTag,monster);

            if (wantedSpell != null)
            {
                if (UtilsLeaf.CheckCanLaunchSpell(wantedSpell, monster) != null)
                {
                    return TreeStatus.SUCCESS;
                }
            }

            if (wantedSpell.Count == 0)
            {
                return TreeStatus.FAILURE;
            }

            return TreeStatus.RUNNING;
        }

        protected override void OnReset()
        {
        }
    }
}
