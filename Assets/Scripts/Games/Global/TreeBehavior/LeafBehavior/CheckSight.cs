using System.Linq;
using Games.Global.Entities;
using Games.Global.TreeBehavior.TestTreeBehavior;
using UnityEngine;

namespace Games.Global.TreeBehavior.LeafBehavior
{
    public class CheckSight : Leaf
    {
        private bool useTargetLock;
        
        // Check if target is in sight, if bool is false, target is the player
        public CheckSight(bool useTarget = false)
        {
            useTargetLock = useTarget;
        }

        public override TreeStatus OnExecute(BehaviorStatus behaviorStatus)
        {
            Monster monster = (behaviorStatus as GameContext).CurrentMonster;

            EntityPrefab target = useTargetLock ? monster.entityPrefab.target.entityPrefab : DataObject.playerInScene.First().Value;
            
            Ray ray = new Ray(monster.entityPrefab.transform.position + Vector3.up * 1,
                (target.transform.position - monster.entityPrefab.transform.position));

            monster.entityPrefab.transform.LookAt(target.transform);

            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo, 1000, ~LayerMask.GetMask("Weapon")))
            {
                if (hitInfo.collider.CompareTag("Player") && !useTargetLock)
                {
                    return TreeStatus.SUCCESS;
                }

                if (hitInfo.collider.gameObject.layer == LayerMask.NameToLayer("Monster") && useTargetLock)
                {
                    return TreeStatus.SUCCESS;
                }
            }

            return TreeStatus.FAILURE;
        }

        protected override void OnReset()
        {
        }
    }
}
