using System.Collections.Generic;
using System.Linq;
using Games.Global.Entities;
using Games.Global.TreeBehavior.TestTreeBehavior;
using Games.Players;

namespace Games.Global.TreeBehavior.LeafBehavior
{
    public class EnemyIsBuff : Leaf
    {
        public override TreeStatus OnExecute(BehaviorStatus behaviorStatus)
        {
            if (DataObject.playerInScene != null && DataObject.playerInScene.Count > 0)
            {
                Monster monster = (behaviorStatus as GameContext)?.CurrentMonster;
                PlayerPrefab player = DataObject.playerInScene.First().Value;

                foreach (var effect in player.entity.GetUnderEffects())
                {
                    List<TypeEffect> goodEffects = new List<TypeEffect>();
                    goodEffects.AddRange(EffectController.BuffEffect);
                    
                    foreach (var goodEffect in goodEffects)
                    {
                        if (effect.typeEffect == goodEffect)
                        {
                            monster.entityPrefab.target = player.entity;
                            return TreeStatus.SUCCESS;
                        }
                    }
                }
            }
            
            return TreeStatus.FAILURE;
        }

        protected override void OnReset()
        {
            
        }
    }
}