using Games.Global.Entities;
using Games.Global.TreeBehavior.TestTreeBehavior;
using UnityEngine;

namespace Games.Global.TreeBehavior.LeafBehavior
{
    public class MonsterFaceProjectile : Leaf
    {
        public override TreeStatus OnExecute(BehaviorStatus behaviorStatus)
        {
            if (DataObject.monsterInScene != null && DataObject.monsterInScene.Count > 0)
            {
                Monster monster = (behaviorStatus as GameContext).CurrentMonster;

                if (monster != null && monster.IsFacingProjectile())
                {
                    Debug.Log("Monster is facing a projectile NIGERUNDAYO !");
                    return TreeStatus.SUCCESS;
                }
            }

            Debug.Log("No monster or no monster projectile");
            return TreeStatus.FAILURE;
        }

        protected override void OnReset()
        {
        }
    }
}