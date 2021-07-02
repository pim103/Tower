using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Games.Global.Entities;
using Games.Global.Spells;
using Games.Global.TreeBehavior.TestTreeBehavior;

public class UtilsLeaf
{
    public static List<Spell> HasSpellFromTag(Monster monster, List<SpellTag> tags)
    {
        List<Spell> wantedSpell = new List<Spell>();

        foreach (Spell spell in monster.spells)
        {
            if (tags.Contains(spell.spellTag))
            {
                wantedSpell.Add(spell);
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

    public static Spell CheckCanLaunchSpell(List<Spell> spells, Monster monster)
    {
        List<Spell> castableSpells = new List<Spell>();
        foreach (Spell spell in spells)
        {
            if (monster.ressource >= spell.cost && !spell.isOnCooldown)
            {
                castableSpells.Add(spell);
            }
        }

        if (castableSpells.Count > 0)
        {
            return castableSpells[Random.Range(0, castableSpells.Count)];
        }
        else
        {
            return null;
        }
        
    }
}

        