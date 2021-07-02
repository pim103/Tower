using System.Linq;
using Games.Global.Entities;
using Games.Global.Spells.SpellBehavior;
using Games.Global.TreeBehavior.TestTreeBehavior;
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
                    return TreeStatus.SUCCESS;
                }
                monster.entityPrefab.MoveOutFromAOE(monster.inNefastSpells.First());
            }

            return TreeStatus.RUNNING;
        }

        protected override void OnReset()
        {
        }
    }
}