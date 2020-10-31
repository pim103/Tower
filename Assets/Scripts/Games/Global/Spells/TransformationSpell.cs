using System;
using System.Collections.Generic;
using UnityEngine;

namespace Games.Global.Spells
{
    [Serializable]
    public class TransformationSpell : SpellComponent
    {
        public TransformationSpell()
        {
            typeSpell = TypeSpell.Transformation;
        }

        // int : spellSlot - Spell : newSpell
        public Dictionary<int, Spell> newSpells { get; set; }
        
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

                foreach (KeyValuePair<int, Spell> newSpell in newSpells)
                {
                    if (caster.spells.Count > newSpell.Key)
                    {
                        caster.spells[newSpell.Key] = newSpell.Value;
                    }
                    else
                    {
                        caster.spells.Add(newSpell.Value);
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