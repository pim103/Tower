using System.Collections.Generic;
using System.Linq;
using Games.Global.Entities;
using Games.Global.Spells;
using Games.Global.Spells.SpellsController;
using Games.Global.TreeBehavior.TestTreeBehavior;

namespace Games.Global.TreeBehavior.LeafBehavior
{
    public class LaunchSpellWithTag : Leaf
    {
        private List<SpellTag> wantedTag;

        public LaunchSpellWithTag(params SpellTag[] tags)
        {
            wantedTag = tags.ToList();
        }

        public override TreeStatus OnExecute(BehaviorStatus behaviorStatus)
        {
            Monster monster = (behaviorStatus as GameContext).CurrentMonster;
            List<Spell> wantedSpells = UtilsLeaf.HasSpellFromTag(monster, wantedTag);
            Spell spell = UtilsLeaf.CheckCanLaunchSpell(wantedSpells, monster);

            if (SpellController.CastSpell(monster, spell))
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
