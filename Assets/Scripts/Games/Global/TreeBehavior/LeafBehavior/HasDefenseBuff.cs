using Games.Global.Entities;
using Games.Global.Spells;
using Games.Global.TreeBehavior.TestTreeBehavior;
using Games.Global.TreeBehavior.Utils;
using UnityEngine;

namespace Games.Global.TreeBehavior.LeafBehavior
{
    public class HasDefenseBuff : Leaf
    {
        public override TreeStatus OnExecute(BehaviorStatus behaviorStatus)
        {
            Monster monster = (behaviorStatus as GameContext).CurrentMonster;
            Spell wantedSpell = null;
            wantedSpell = UtilsLeaf.HasSpellFromTag(SpellTag.DefensiveBuff, monster);
            bool canLaunchDash = wantedSpell != null && UtilsLeaf.CheckCanLaunchSpell(wantedSpell, monster);
            if (wantedSpell != null && canLaunchDash)
            {
                Debug.Log("Oh oh, I can use my defense buff, JoJo!");
                return TreeStatus.SUCCESS;
            }

            Debug.Log("I can't defend myself, YAMERO !");
            return TreeStatus.FAILURE;
        }

        protected override void OnReset()
        {
            throw new System.NotImplementedException();
        }
    }
}