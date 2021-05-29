using System.Collections.Generic;
using Games.Global.Entities;
using Games.Global.TreeBehavior.TestTreeBehavior;
using UnityEditor.Experimental.GraphView;

namespace Games.Global.TreeBehavior.LeafBehavior
{
    public class CheckHPPercentage : Leaf
    {
        private float wantedPercentage;
        private bool wantSelf;
        public CheckHPPercentage(float percentage, bool self)
        {
            wantedPercentage = percentage;
            wantSelf = self;
        }
        public override TreeStatus OnExecute(BehaviorStatus behaviorStatus)
        {
            Monster monster = (behaviorStatus as GameContext).CurrentMonster;
            if (wantSelf && monster.hp < monster.initialHp*wantedPercentage)
            {
                return TreeStatus.SUCCESS;
            }
            else if(wantSelf)
            {
                return TreeStatus.FAILURE;
            }
            else if(!wantSelf)
            {
                List<Monster> monstersToHeal = new List<Monster>();
                foreach (Monster currentMonster in DataObject.monsterInScene)
                {
                    if (currentMonster != monster && currentMonster.hp < currentMonster.initialHp * wantedPercentage)
                    {
                        monstersToHeal.Add(currentMonster);
                    }
                }

                float minHp = 10000;
                Monster prioritisedMonster = null;
                foreach (Monster currentMonster in monstersToHeal)
                {
                    if (currentMonster.hp < minHp)
                    {
                        minHp = currentMonster.hp;
                        prioritisedMonster = currentMonster;
                    }
                }

                if (prioritisedMonster != null)
                {
                    monster.entityPrefab.target = prioritisedMonster;
                    return TreeStatus.SUCCESS;
                }
                else
                {
                    return TreeStatus.FAILURE;
                }
            }

            return TreeStatus.RUNNING;
        }
        

        protected override void OnReset()
        {
        }
    }
}
