using UnityEngine;

namespace Games.Global.Spells
{
    public enum Geometry
    {
        Square,
        Sphere,
        Cone,
    }
    
    public enum TypeSpell
    {
        
    }

    public class SpellComponent: MonoBehaviour
    {
        public TypeSpell typeSpell;
    }

    public class Spell
    {
        public float cooldown;
        public float cost;
        public float castTime;

        public bool deactivatePassiveWhenActive;

        // Active:
        public SpellComponent activeSpellComponent;

        // Passive:
        public SpellComponent passiveSpellComponent;

        // DuringCast:
        public SpellComponent duringCastSpellComponent;

        //Recast - InterruptCast
        public SpellComponent recastSpellComponent;
    }
}