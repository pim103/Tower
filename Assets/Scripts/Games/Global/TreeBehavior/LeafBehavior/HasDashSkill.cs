using Games.Global.Entities;
using Games.Global.Spells;
using Games.Global.TreeBehavior.TestTreeBehavior;
using Games.Global.TreeBehavior.Utils;
using UnityEngine;

namespace Games.Global.TreeBehavior.LeafBehavior
{
    public class HasDashSkill : Leaf
    {
        public override TreeStatus OnExecute(BehaviorStatus behaviorStatus)
        {
            Monster monster = (behaviorStatus as GameContext).CurrentMonster;
            Spell wantedSpell = null;
            wantedSpell = UtilsLeaf.HasSpellFromTag(SpellTag.Movement, monster);
            bool canLaunchDash = wantedSpell != null && UtilsLeaf.CheckCanLaunchSpell(wantedSpell, monster);
            if (wantedSpell != null && canLaunchDash)
            {
                Debug.Log("Can Dash, Ikuso!");
                return TreeStatus.SUCCESS;
            }

            Debug.Log("Can't Dash, Life is Not Daijobu");
            return TreeStatus.FAILURE;
        }

        protected override void OnReset()
        {
            throw new System.NotImplementedException();
        }
    }
}