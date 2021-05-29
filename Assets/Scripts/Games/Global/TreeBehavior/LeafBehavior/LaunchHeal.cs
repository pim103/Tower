using Games.Global.Entities;
using Games.Global.Spells;
using Games.Global.Spells.SpellsController;
using Games.Global.TreeBehavior.TestTreeBehavior;

namespace Games.Global.TreeBehavior.LeafBehavior
{
    public class LaunchHeal : Leaf
    {
        private SpellTag wantedTag;
        public LaunchHeal(SpellTag tag)
        {
            wantedTag = tag;
        }
        public override TreeStatus OnExecute(BehaviorStatus behaviorStatus)
        {
            Monster monster = (behaviorStatus as GameContext).CurrentMonster;
            Spell wantedSpell = null;
            wantedSpell = UtilsLeaf.HasSpellFromTag(wantedTag,monster);
            if (SpellController.CastSpell(monster, wantedSpell))
            {
                return TreeStatus.SUCCESS;
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
