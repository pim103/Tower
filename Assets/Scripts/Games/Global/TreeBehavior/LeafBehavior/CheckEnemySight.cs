using System.Linq;
using Games.Global.Entities;
using Games.Global.TreeBehavior.TestTreeBehavior;
using Games.Players;
using UnityEngine;

namespace Games.Global.TreeBehavior.LeafBehavior
{
    public class CheckEnemySight : Leaf
    {
        public override TreeStatus OnExecute(BehaviorStatus behaviorStatus)
        {
            Monster monster = (behaviorStatus as GameContext).CurrentMonster;
            PlayerPrefab player = DataObject.playerInScene.First().Value;
            
            Ray ray = new Ray(monster.entityPrefab.transform.position, player.transform.forward);

            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo))
            {
                if (hitInfo.collider.CompareTag("Player"))
                {
                    float distanceFromPlayer = Vector3.Distance(monster.entityPrefab.transform.position, player.transform.forward);
                    if (monster.monsterType == MonsterType.Distance || monster.monsterType == MonsterType.Support)
                    {
                        if (distanceFromPlayer < 30.0f)
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
                        if (distanceFromPlayer < 5.0f)
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
                    return TreeStatus.FAILURE;
                }
            }
            else
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
