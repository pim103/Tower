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
        public bool needPositionToMidToEntity;
        public bool castByPassive;
    }

    public class Spell
    {
        public float initialCooldown;
        public float cooldown;
        public float cost;
        public float castTime;

        public bool deactivatePassiveWhenActive;
        public bool isOnCooldown;

        public int nbUse = -1;

        public bool canCastDuringCast = false;
        public bool wantToCastDuringCast = false;

        // Active:
        public SpellComponent activeSpellComponent;

        // Passive:
        public SpellComponent passiveSpellComponent;

        // DuringCast:
        public SpellComponent duringCastSpellComponent;
        public bool interruptCurrentCast;

        //Recast
        public SpellComponent recastSpellComponent;
        public bool canRecast;
        public bool alreadyRecast;
    }
}