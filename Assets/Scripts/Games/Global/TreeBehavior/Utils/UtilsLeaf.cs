using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Games.Global.Entities;
using Games.Global.Spells;
using Games.Global.TreeBehavior.TestTreeBehavior;

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
