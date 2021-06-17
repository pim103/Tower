using System.Collections.Generic;
using Games.Global.Entities;
using Games.Global.Spells;
using Games.Global.Spells.SpellsController;
using Games.Global.TreeBehavior.TestTreeBehavior;

namespace Games.Global.TreeBehavior.LeafBehavior
{
    public class LaunchSpellWithTag : Leaf
    {
        private SpellTag wantedTag;
        public LaunchSpellWithTag(SpellTag tag)
        {
            wantedTag = tag;
        }
        public override TreeStatus OnExecute(BehaviorStatus behaviorStatus)
        {
            Monster monster = (behaviorStatus as GameContext).CurrentMonster;
            List<Spell> wantedSpells = UtilsLeaf.HasSpellFromTag(wantedTag,monster);
            Spell spell = UtilsLeaf.CheckCanLaunchSpell(wantedSpells,monster);

            if (SpellController.CastSpell(monster, spell))
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
