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
        Movement,
        Buff,
        SpecialAttack,
        TargetedAttack,
        AreaOfEffect,
        Wave,
        Projectile,
        Summon,
        Passive,
        Transformation
    }
    
    public enum DamageType
    {
        Magical,
        Physical
    }

    public class SpellComponent: MonoBehaviour
    {
        public TypeSpell typeSpell;
        public DamageType damageType;
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