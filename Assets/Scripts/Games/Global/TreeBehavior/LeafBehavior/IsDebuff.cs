using System.Collections.Generic;
using Games.Global.Entities;
using Games.Global.TreeBehavior.TestTreeBehavior;

namespace Games.Global.TreeBehavior.LeafBehavior
{
    public class IsDebuff : Leaf
    {
        public override TreeStatus OnExecute(BehaviorStatus behaviorStatus)
        {
            Monster monster = (behaviorStatus as GameContext)?.CurrentMonster;
            
            foreach (var effect in monster.GetUnderEffects())
            {
                List<TypeEffect> badEffects = new List<TypeEffect>();
                badEffects.AddRange(EffectController.DebuffEffect);
                badEffects.AddRange(EffectController.MovementControlEffect);
                
                foreach (var badEffect in badEffects)
                {
                    if (effect.typeEffect == badEffect)
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