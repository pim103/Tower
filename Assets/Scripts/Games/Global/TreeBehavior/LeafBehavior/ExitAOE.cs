using System.Linq;
using Games.Global.Entities;
using Games.Global.Spells.SpellBehavior;
using Games.Global.TreeBehavior.TestTreeBehavior;
using Games.Global.TreeBehavior.Utils;
using UnityEngine;

namespace Games.Global.TreeBehavior.LeafBehavior
{
    public class ExitAOE : Leaf
    {
        public override TreeStatus OnExecute(BehaviorStatus behaviorStatus)
        {
            if (DataObject.monsterInScene != null && DataObject.monsterInScene.Count > 0)
            {
                Monster monster = (behaviorStatus as GameContext).CurrentMonster;

                if (!UtilsLeaf.IsInAOE(monster))
                {
                    Debug.Log("Imma head out!");
                    return TreeStatus.SUCCESS;
                }
                monster.entityPrefab.MoveOutFromAOE(monster.inNefastSpells.First());
            }

            Debug.Log("Nigerundayoooooooooooooo !");
            return TreeStatus.RUNNING;
        }

        protected override void OnReset()
        {
            throw new System.NotImplementedException();
        }
    }
}