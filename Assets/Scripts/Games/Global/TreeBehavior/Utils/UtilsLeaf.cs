using Games.Global.Entities;
using Games.Global.Spells;

namespace Games.Global.TreeBehavior.Utils
{
    public class UtilsLeaf
    {
        public static Spell HasSpellFromTag(SpellTag tag, Monster monster)
        {
            Spell wantedSpell = null;
            foreach (Spell spell in monster.spells)
            {
                if (spell.spellTag == tag)
                {
                    wantedSpell = spell;
                    break;
                }
            }

            return wantedSpell;
        }

        public static bool IsInAOE(Monster monster)
        {
            if (monster.inNefastSpells != null && monster.inNefastSpells.Count > 0)
            {
                return true;
            }
            return false;
        }

        public static bool CheckCanLaunchSpell(Spell spell, Monster monster)
        {
            if (monster.ressource >= spell.cost && !spell.isOnCooldown)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
