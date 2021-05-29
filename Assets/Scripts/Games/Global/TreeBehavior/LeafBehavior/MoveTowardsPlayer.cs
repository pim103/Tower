using System.Linq;
using Games.Global.Entities;
using Games.Global.TreeBehavior.TestTreeBehavior;
using Games.Players;
using UnityEngine;

namespace Games.Global.TreeBehavior.LeafBehavior
{
    public class MoveTowardsPlayer : Leaf
    {
        public override TreeStatus OnExecute(BehaviorStatus behaviorStatus)
        {
            Monster monster = (behaviorStatus as GameContext).CurrentMonster;
            PlayerPrefab player = DataObject.playerInScene.First().Value;
            
            float distanceFromPlayer = Vector3.Distance(monster.entityPrefab.transform.position, player.transform.forward);
            if (monster.monsterType == MonsterType.Distance || monster.monsterType == MonsterType.Support)
            {
                if (distanceFromPlayer > 30.0f)
                {
                    if (monster.entityPrefab.navMeshAgent.SetDestination(player.transform.position))
                    {
                        return TreeStatus.SUCCESS;
                    }
                    else
                    {
                        return TreeStatus.FAILURE;
                    }
                }
                else
                {
                    if (monster.entityPrefab.navMeshAgent.SetDestination(monster.entityPrefab.transform.position))
                    {
                        return TreeStatus.SUCCESS;
                    }
                    else
                    {
                        return TreeStatus.FAILURE;
                    }
                }
            }
            else
            {
                if (distanceFromPlayer > 5.0f)
                {
                    if (monster.entityPrefab.navMeshAgent.SetDestination(player.transform.position))
                    {
                        return TreeStatus.SUCCESS;
                    }
                    else
                    {
                        return TreeStatus.FAILURE;
                    }
                }
                else
                {
                    if (monster.entityPrefab.navMeshAgent.SetDestination(monster.entityPrefab.transform.position))
                    {
                        return TreeStatus.SUCCESS;
                    }
                    else
                    {
                        return TreeStatus.FAILURE;
                    }
                }
            }

            return TreeStatus.RUNNING;
        }

        protected override void OnReset()
        {
        }
    }
}
