using Games.Global.Entities;
using Games.Global.Spells;
using Games.Global.Spells.SpellsController;
using Games.Global.TreeBehavior.TestTreeBehavior;

namespace Games.Global.TreeBehavior.LeafBehavior
{
    public class UseDispel : Leaf
    {
        private SpellTag spellTag;
        
        public UseDispel(SpellTag spellTag)
        {
            this.spellTag = spellTag;
        }
        
        public override TreeStatus OnExecute(BehaviorStatus behaviorStatus)
        {
            Monster monster = (behaviorStatus as GameContext)?.CurrentMonster;
            Spell dispel = UtilsLeaf.HasSpellFromTag(spellTag, monster);

            if (dispel != null)
            {
                if (SpellController.CastSpell(monster, dispel))
                    return TreeStatus.SUCCESS;
            }

            return TreeStatus.FAILURE;
        }

        protected override void OnReset()
        {
            
        }
    }
}