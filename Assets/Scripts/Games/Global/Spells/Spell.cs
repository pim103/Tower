using System;
using System.Collections.Generic;
using Games.Global.Spells.SpellBehavior;
using Games.Global.Spells.SpellParameter;
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

    public enum OriginArea
    {
        Center,
        From
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
    
    public enum Trigger
    {
        START,
        END,
        ON_TRIGGER_ENTER,
        ON_TRIGGER_END,
        ON_TRIGGER_STAY,
//        ON_HIT_AT_START,
//        ON_HIT_DURING_INTERVAL,
//        ON_DAMAGE_RECEIVE_AT_START,
//        ON_DAMAGE_RECEIVE_DURING_INTERVAL,
        INTERVAL
    }
    
    public enum TargetType
    {
        ENNEMIES,
        CASTER,
        ALLIES
    }

    public class ActionTriggered
    {
        public Effect effect;
        public SpellComponent spellComponent;
        public int damageDeal;
        public int percentageToTrigger;
    }

    [Serializable]
    public class SpellComponent
    {
        public string nameSpellComponent { get; set; }
        public TypeSpell typeSpell { get; set; }
        public DamageType damageType { get; set; }

        public Coroutine currentCoroutine;

        public StartFrom startFrom { get; set; }
//        public OriginalDirection OriginalDirection { get; set; }
//
//        public Vector3 startPosition { get; set; }
//        public Vector3 initialRotation { get; set; }
//        public Vector3 trajectoryNormalized { get; set; }
//
//        public bool isBasicAttack { get; set; }
//        public bool needPositionToMidToEntity { get; set; }
//        public bool castByPassive { get; set; }

        public Dictionary<Trigger, Dictionary<TargetType, List<ActionTriggered>>> actions;
        
        /* New var */
        public float spellDuration;
        public float spellInterval;

        public Trajectory trajectory;
        public SpellToInstantiate spellToInstantiate;

        public SpellPrefabController spellPrefabController;
        public Entity caster;

        public Entity targetAtCast;
    }

    [Serializable]
    public class Spell
    {
        public string nameSpell { get; set; }
        public float initialCooldown { get; set; }
        public float cooldown { get; set; }
        public float cost { get; set; }
        public float castTime { get; set; }

        public bool deactivatePassiveWhenActive { get; set; }
        public bool isOnCooldown { get; set; }

        public int nbUse { get; set; } = -1;

        public bool canCastDuringCast { get; set; } = false;
        public bool wantToCastDuringCast { get; set; } = false;

        // Active:
        public SpellComponent activeSpellComponent { get; set; }

        // Passive:
        public SpellComponent passiveSpellComponent { get; set; }

        // DuringCast:
        public SpellComponent duringCastSpellComponent { get; set; }
        public bool interruptCurrentCast { get; set; }

        //Recast
        public SpellComponent recastSpellComponent { get; set; }
        public bool canRecast { get; set; }
        public bool alreadyRecast { get; set; }
    }
}