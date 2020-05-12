using Games.Global.Spells.SpellsController;
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

    public enum PositionToStartSpell
    {
        Himself,
        DynamicPosition,
        AlreadySet
    }

    public abstract class SpellComponent
    {
        public TypeSpell typeSpell;
        public DamageType damageType;

        public Coroutine currentCoroutine;

        public OriginalPosition OriginalPosition;
        public OriginalDirection OriginalDirection;

        public Vector3 startPosition;
        public Vector3 initialRotation;
        public Vector3 trajectoryNormalized;

        public bool isBasicAttack;
    }

    public class Spell
    {
        public float cooldown;
        public float cost;
        public float castTime;

        public bool deactivatePassiveWhenActive;
        public bool isOnCooldown;

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