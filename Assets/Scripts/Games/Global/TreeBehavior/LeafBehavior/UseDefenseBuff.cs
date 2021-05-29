using Games.Global.Entities;
using Games.Global.Spells;
using Games.Global.Spells.SpellsController;
using Games.Global.TreeBehavior.TestTreeBehavior;
using Games.Global.TreeBehavior.Utils;
using UnityEngine;

namespace Games.Global.TreeBehavior.LeafBehavior
{
    public class UseDefenseBuff : Leaf
    {
        public override TreeStatus OnExecute(BehaviorStatus behaviorStatus)
        {
            Monster monster = (behaviorStatus as GameContext).CurrentMonster;
            Spell wantedSpell = null;
            wantedSpell = UtilsLeaf.HasSpellFromTag(SpellTag.DefensiveBuff, monster);
            if (SpellController.CastSpell(monster, wantedSpell))
            {
                Debug.Log("DEFENSE !");
                return TreeStatus.SUCCESS;
            }
            Debug.Log("Yikes ! Can't Defend Myself wtf am I Doing ??");
            return TreeStatus.FAILURE;
        }

        protected override void OnReset()
        {
            throw new System.NotImplementedException();
        }
    }
}