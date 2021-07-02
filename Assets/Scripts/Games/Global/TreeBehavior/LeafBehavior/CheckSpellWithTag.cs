using System.Collections.Generic;
using System.Linq;
using Games.Global.Entities;
using Games.Global.Spells;
using Games.Global.TreeBehavior.TestTreeBehavior;
using UnityEngine;
using UnityEngine.Rendering;

namespace Games.Global.TreeBehavior.LeafBehavior
{
    public class CheckSpellWithTag : Leaf
    {
        private List<SpellTag> wantedTag;

        public CheckSpellWithTag(params SpellTag[] tags)
        {
            wantedTag = tags.ToList();
        }

        public override TreeStatus OnExecute(BehaviorStatus behaviorStatus)
        {
            Monster monster = (behaviorStatus as GameContext).CurrentMonster;
            List<Spell> wantedSpell = UtilsLeaf.HasSpellFromTag(monster, wantedTag);

            if (wantedTag.Contains(SpellTag.HealHimself))
            {
                Debug.Log("Just check");
            }

            if (wantedSpell != null && wantedSpell.Count > 0)
            {
                if (UtilsLeaf.CheckCanLaunchSpell(wantedSpell, monster) != null)
                {
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
