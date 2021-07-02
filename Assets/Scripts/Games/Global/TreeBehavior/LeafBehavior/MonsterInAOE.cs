using Games.Global.Entities;
using Games.Global.TreeBehavior.TestTreeBehavior;
using UnityEngine;

namespace Games.Global.TreeBehavior.LeafBehavior
{
    public class MonsterInAOE : Leaf
    {
        public override TreeStatus OnExecute(BehaviorStatus behaviorStatus)
        {
            Monster monster = (behaviorStatus as GameContext).CurrentMonster;

            if (UtilsLeaf.IsInAOE(monster))
            {
                Debug.Log("Monster is in AOE!");
                return TreeStatus.SUCCESS;
            }

            return TreeStatus.FAILURE;
        }

        protected override void OnReset()
        {
        }
    }
}