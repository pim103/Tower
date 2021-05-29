using Games.Global.Entities;
using Games.Global.Spells;
using Games.Global.TreeBehavior.TestTreeBehavior;
using UnityEngine;

namespace Games.Global.TreeBehavior.LeafBehavior
{
    public class CheckHeal : Leaf
    {
        public override TreeStatus OnExecute(BehaviorStatus behaviorStatus)
        {
            Monster monster = (behaviorStatus as GameContext).CurrentMonster;
            Spell wantedSpell = null;
            wantedSpell = UtilsLeaf.HasSpellFromTag(SpellTag.HealHimself,monster);
            if (wantedSpell != null)
            {
                UtilsLeaf.CheckCanLaunchSpell(wantedSpell, monster);
                Debug.Log("yes");
            }

            if (wantedSpell == null)
            {
                Debug.Log("no");
                return TreeStatus.FAILURE;
            }

            return TreeStatus.RUNNING;
        }

        protected override void OnReset()
        {
        }
    }
}
