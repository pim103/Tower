using Games.Global.Entities;
using Games.Global.Spells;
using Games.Global.TreeBehavior.TestTreeBehavior;
using UnityEngine;

namespace Games.Global.TreeBehavior.LeafBehavior
{
    public class CheckHeal : Leaf
    {
        private SpellTag wantedTag;
        public CheckHeal(SpellTag tag)
        {
            wantedTag = tag;
        }
        public override TreeStatus OnExecute(BehaviorStatus behaviorStatus)
        {
            Monster monster = (behaviorStatus as GameContext).CurrentMonster;
            Spell wantedSpell = null;
            wantedSpell = UtilsLeaf.HasSpellFromTag(wantedTag,monster);
            if (wantedSpell != null)
            {
                if (UtilsLeaf.CheckCanLaunchSpell(wantedSpell, monster))
                {
                    return TreeStatus.SUCCESS;
                }
            }

            if (wantedSpell == null)
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
