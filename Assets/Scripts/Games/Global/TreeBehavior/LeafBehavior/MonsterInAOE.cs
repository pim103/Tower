using Games.Global.Entities;
using Games.Global.TreeBehavior.TestTreeBehavior;
using UnityEngine;

namespace Games.Global.TreeBehavior.LeafBehavior
{
    public class MonsterInAOE : Leaf
    {
        public override TreeStatus OnExecute(BehaviorStatus behaviorStatus)
        {
            if (DataObject.monsterInScene != null && DataObject.monsterInScene.Count > 0)
            {
                Monster monster = (behaviorStatus as GameContext).CurrentMonster;

                if (UtilsLeaf.IsInAOE(monster))
                {
                    Debug.Log("Monster is in AOE!");
                    return TreeStatus.SUCCESS;
                }
            }

            Debug.Log("No monster or no monster in AOE");
            return TreeStatus.FAILURE;
        }

        protected override void OnReset()
        {
        }
    }
}