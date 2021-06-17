using System;
using System.Collections.Generic;
using UnityEngine;

namespace Games.Global.Spells
{
    [Serializable]
    public class ReplaceSpell
    {
        public int slotSpell { get; set; }
        public Spell newSpell { get; set; }
    }
    
    [Serializable]
    public class TransformationSpell : SpellComponent
    {
        public TransformationSpell()
        {
            TypeSpellComponent = TypeSpellComponent.Transformation;
        }

        // int : spellSlot - Spell : newSpell
        public List<ReplaceSpell> newSpells { get; set; } = new List<ReplaceSpell>();
        
        // Save original spell
        private List<Spell> originalSpell = new List<Spell>();
        
        public Spell newBasicAttack { get; set; }
        private Spell originalBasicAttack;
        
        public Spell newBasicDefense { get; set; }
        private Spell originalBasicDefense;

        public override void AtTheStart()
        {
            if (newSpells.Count > 0)
            {
                caster.spells.ForEach(spell => originalSpell.Add(spell));

                foreach (ReplaceSpell replaceSpell in newSpells)
                {
                    if (caster.spells.Count > replaceSpell.slotSpell)
                    {
                        caster.spells[replaceSpell.slotSpell] = replaceSpell.newSpell;
                    }
                    else
                    {
                        caster.spells.Add(replaceSpell.newSpell);
                    }
                }
            }

            if (newBasicAttack != null)
            {
                originalBasicAttack = caster.basicAttack;
                caster.basicAttack = newBasicAttack;
            }

            if (newBasicDefense != null)
            {
                originalBasicDefense = caster.basicDefense;
                caster.basicDefense = newBasicDefense;
            }
        }

        public override void AtTheEnd()
        {
            if (newSpells.Count > 0)
            {
                caster.spells.Clear();
                foreach (Spell spell in originalSpell)
                {
                    caster.spells.Add(spell);
                }
                originalSpell.Clear();
            }

            if (newBasicAttack != null)
            {
                caster.basicAttack = originalBasicAttack;
                originalBasicAttack = null;
            }
            
            if (newBasicDefense != null)
            {
                caster.basicDefense = originalBasicDefense;
                originalBasicDefense = null;
            }
        }
    }
}