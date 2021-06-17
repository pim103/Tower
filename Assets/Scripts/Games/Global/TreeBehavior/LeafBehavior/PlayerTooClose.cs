using System.Linq;
using Games.Global.Entities;
using Games.Global.TreeBehavior.TestTreeBehavior;
using Games.Players;
using UnityEngine;

namespace Games.Global.TreeBehavior.LeafBehavior
{
    public class PlayerTooClose : Leaf
    {
        public override TreeStatus OnExecute(BehaviorStatus behaviorStatus)
        {
            if (DataObject.playerInScene != null && DataObject.playerInScene.Count > 0 && DataObject.monsterInScene != null && DataObject.monsterInScene.Count > 0)
            {
                Monster monster = (behaviorStatus as GameContext)?.CurrentMonster;
                PlayerPrefab player = DataObject.playerInScene.First().Value;

                if (monster != null && Vector3.Distance(player.transform.position, monster.entityPrefab.transform.position) < 5)
                {
                    Debug.Log("Player too close, warning !!!");
                    return TreeStatus.SUCCESS;
                }
                else
                {
                    return TreeStatus.FAILURE;
                }
            }

            Debug.Log("Pas de joueur ou monstre trouvé");
            return TreeStatus.FAILURE;
        }

        protected override void OnReset()
        {
        }
    }
}