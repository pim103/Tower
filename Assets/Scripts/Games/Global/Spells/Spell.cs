using System;
using System.Collections.Generic;
using Games.Global.Spells.SpellBehavior;
using Games.Global.Spells.SpellParameter;
using PathCreation;
using UnityEngine;

namespace Games.Global.Spells
{
    // StartFrom is used by spell and by ActionTriggered
    public enum StartFrom
    {
        Caster,
        TargetEntity,
        CursorTarget,
        AllEnemiesInArea,
        AllAlliesInArea,
        RandomPositionInArea,
        RandomEnemyInArea,
        LastSpellComponent,
        ClosestEnemyFromCaster
    }

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
    
    public enum Trigger
    {
        START,
        END,
        ON_TRIGGER_ENTER,
        ON_TRIGGER_END,
        INTERVAL,
        ON_ATTACK,
        ON_DAMAGE_RECEIVED,
        ON_ENTITY_DIE
    }
    
    public enum ConditionType
    {
        IfTargetHasEffect,
        IfCasterHasEffect,
        MinEnemiesInArea
    }

    public class TargetsFound
    {
        public List<Entity> targets = new List<Entity>();
        public Entity target = null;

        public Vector3 position = Vector3.negativeInfinity;
    }

    [Serializable]
    public class ConditionToTrigger
    {
        public ConditionType conditionType { get; set; }
        public TypeEffect typeEffectNeeded { get; set; }
        public int valueNeeded { get; set; }

        public bool TestCondition(Entity caster, Entity target)
        {
            bool conditionIsValid;
            
            switch (conditionType)
            {
                case ConditionType.IfCasterHasEffect:
                    conditionIsValid = caster.underEffects.ContainsKey(typeEffectNeeded);
                    break;
                case ConditionType.IfTargetHasEffect:
                    conditionIsValid = target.underEffects.ContainsKey(typeEffectNeeded);
                    break;
                case ConditionType.MinEnemiesInArea:
                    conditionIsValid = caster.entityInRange.Count >= valueNeeded;
                    break;
                default:
                    conditionIsValid = true;
                    break;
            }

            return conditionIsValid;
        }
    }

    public enum ActionOnEffectType
    {
        ADD,
        DELETE
    }

    public enum ConditionReduceCharge
    {
        None,
        OnAttack,
        OnDamageReceived
    }

    [Serializable]
    public class ActionTriggered
    {
        public StartFrom startFrom { get; set; }
        public ActionOnEffectType actionOnEffectType { get; set; }
        public Effect effect { get; set; }
        public SpellComponent spellComponent { get; set; }
        public int damageDeal { get; set; }
        public int percentageToTrigger { get; set; }
        public ConditionToTrigger conditionToTrigger { get; set; }
    }

    [Serializable]
    public class SpellComponent
    {
        public string nameSpellComponent { get; set; }
        public TypeSpell typeSpell { get; set; }
        public DamageType damageType { get; set; }

        /* New var */
        public Dictionary<Trigger, List<ActionTriggered>> actions { get; set; } = new Dictionary<Trigger, List<ActionTriggered>>();
        public float spellDuration { get; set; }
        public float spellInterval { get; set; }

        public ConditionReduceCharge conditionReduceCharge { get; set; }
        public int spellCharges { get; set; }

        public Trajectory trajectory { get; set; }
        public SpellToInstantiate spellToInstantiate { get; set; }
        public SpellPrefabController spellPrefabController { get; set; }

        public int damageMultiplierOnDistance { get; set; }

        public bool appliesPlayerOnHitEffect { get; set; }
        public bool canStopProjectile { get; set; }
        public bool stopSpellComponentAtDamageReceived { get; set; }

        public virtual void AtTheStart() {}
        public virtual void AtTheEnd() {}
        public virtual void DuringInterval() {}
        public virtual void OnAttack() {}
        public virtual void OnDamageReceive() {}
        public virtual void OnTriggerEnter(Entity enemy) {}
        public virtual void OnTriggerExit(Entity enemy) {}

        /* Parameters used in game */
        public Coroutine currentCoroutine;
        public Entity caster;
        public Entity targetAtCast;

        public Vector3 startAtPosition;

        public PathCreator pathCreator;
    }

    [Serializable]
    public class Spell
    {
        public StartFrom startFrom { get; set; }
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