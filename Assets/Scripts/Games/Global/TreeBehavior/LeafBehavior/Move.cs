using UnityEngine;

namespace Games.Global.TreeBehavior.LeafBehavior
{
    public class Move : Leaf
    {
        public override TreeStatus OnExecute(BehaviorStatus behaviorStatus)
        {
            Debug.Log("Need to move");

            return TreeStatus.SUCCESS;
        }

        protected override void OnReset()
        {
            
        }
    }
}