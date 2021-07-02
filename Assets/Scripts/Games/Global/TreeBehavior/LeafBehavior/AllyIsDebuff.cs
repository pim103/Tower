using System.Collections.Generic;
using Games.Global.Entities;
using Games.Global.TreeBehavior.TestTreeBehavior;

namespace Games.Global.TreeBehavior.LeafBehavior
{
    public class AllyIsDebuff : Leaf
    {
        public override TreeStatus OnExecute(BehaviorStatus behaviorStatus)
        {
            if (DataObject.monsterInScene != null && DataObject.monsterInScene.Count > 0)
            {
                Monster currentMonster = (behaviorStatus as GameContext)?.CurrentMonster;
                List<Monster> monsters = DataObject.monsterInScene;

                foreach (var monster in monsters)
                {
                    if (monster == currentMonster)
                        continue;

                    foreach (var effect in monster.GetUnderEffects())
                    {
                        List<TypeEffect> badEffects = new List<TypeEffect>();
                        badEffects.AddRange(EffectController.DebuffEffect);
                        badEffects.AddRange(EffectController.IncapacitateEffect);
                        badEffects.AddRange(EffectController.MovementControlEffect);
                        
                        foreach (var badEffect in badEffects)
                        {
                            if (effect.typeEffect == badEffect)
                            {
                                currentMonster.entityPrefab.target = monster;
                                return TreeStatus.SUCCESS;
                            }
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