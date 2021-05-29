using Games.Global.Entities;
using Games.Global.Spells;
using Games.Global.TreeBehavior.TestTreeBehavior;

namespace Games.Global.TreeBehavior.LeafBehavior
{
    public class CanUseDispel : Leaf
    {
        private SpellTag spellTag;
        
        public CanUseDispel(SpellTag spellTag)
        {
            this.spellTag = spellTag;
        }
        
        public override TreeStatus OnExecute(BehaviorStatus behaviorStatus)
        {
            Monster monster = (behaviorStatus as GameContext)?.CurrentMonster;
            Spell dispel = UtilsLeaf.HasSpellFromTag(spellTag, monster);

            if (dispel != null && UtilsLeaf.CheckCanLaunchSpell(dispel, monster))
            {
                return TreeStatus.SUCCESS;
            }

            return TreeStatus.FAILURE;
        }

        protected override void OnReset()
        {
            
        }
    }
}