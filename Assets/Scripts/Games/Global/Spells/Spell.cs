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

    public enum TypeSpellComponent
    {
        Classic,
        Movement,
        Summon,
        Passive,
        Transformation,
        BasicAttack
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

        public Vector3 position = Vector3.zero;
    }

    [Serializable]
    public class ConditionToTrigger
    {
        public string conditionName;
        public ConditionType conditionType { get; set; }
        public TypeEffect typeEffectNeeded { get; set; }
        public int valueNeeded { get; set; }

        public bool TestCondition(Entity caster, Entity target)
        {
            bool conditionIsValid;
            
            switch (conditionType)
            {
                case ConditionType.IfCasterHasEffect:
                    conditionIsValid = caster.EntityIsUnderEffect(typeEffectNeeded);
                    break;
                case ConditionType.IfTargetHasEffect:
                    conditionIsValid = target.EntityIsUnderEffect(typeEffectNeeded);
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

    public enum TypeSpell
    {
        UniqueActivation,
        MultipleActivation,
        Passive,
        Holding,
        HoldThenRelease,
        ActivePassive
    }

    [Serializable]
    public class SpellComponentWithTimeCast
    {
        public SpellComponent spellComponent { get; set; }
        public float timeBeforeReset { get; set; }
    }

    [Serializable]
    public class ActionTriggered
    {
        public StartFrom startFrom { get; set; }
        public ActionOnEffectType actionOnEffectType { get; set; }
        public Effect effect { get; set; }
        public SpellComponent spellComponent { get; set; }
        public int damageDeal { get; set; }
        public int percentageToTrigger { get; set; } = 100;
        public ConditionToTrigger conditionToTrigger { get; set; }
    }

    [Serializable]
    public class SpellComponent
    {
        public string nameSpellComponent { get; set; }
        public DamageType damageType { get; set; }

        /* New var */
        public Dictionary<Trigger, List<ActionTriggered>> actions { get; set; } = new Dictionary<Trigger, List<ActionTriggered>>();
        public float spellDuration { get; set; }
        public float spellInterval { get; set; }

        public ConditionReduceCharge conditionReduceCharge { get; set; }
        public int spellCharges { get; set; }

        public Trajectory trajectory { get; set; }
        public SpellToInstantiate spellToInstantiate { get; set; }

        public float damageMultiplierOnDistance { get; set; }

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
        public Spell originSpell;

        public Coroutine currentCoroutine;
        public Entity caster;
        public Entity targetAtCast;

        public Vector3 startAtPosition;

        public PathCreator pathCreator;
        public TypeSpellComponent TypeSpellComponent { get; set; }
        public SpellPrefabController spellPrefabController;
        
        public override string ToString()
        {
            return nameSpellComponent;
        }
    }

    public enum SpellTag
    {
        MeleeDamage,
        DistanceDamage,
        HealHimself,
        HealOther,
        HealArea,
        MeleeControl,
        DistanceControl,
        DefensiveBuff,
        OffensiveBuff,
        Movement,
        DispelHimself,
        DispelOther,
        DispelEnemy
    }

    [Serializable]
    public class Spell
    {
        public int id { get; set; } = -1;
        public SpellTag spellTag { get; set; }
        public TypeSpell TypeSpell { get; set; }
        public StartFrom startFrom { get; set; }
        public string nameSpell { get; set; }
        public float initialCooldown { get; set; }
        public float cooldown { get; set; }
        public float cost { get; set; }
        public float castTime { get; set; }
        public bool isOnCooldown { get; set; }

        public int nbUse { get; set; } = -1;

        public Texture2D sprite { get; set; }

        public SpellComponent spellComponentFirstActivation { get; set; }

        // Passive
        public SpellComponent passiveSpellComponent { get; set; }
        public bool deactivatePassiveWhenActive { get; set; }
        
        // Holding
        public float holdingCost { get; set; }
        public float maximumTimeHolding { get; set; }
        
        public bool isHolding;

        public float timeHolding { get; set; }

        public SpellComponent spellComponentAfterHolding { get; set; }

        // Multiple Activation
        public List<SpellComponentWithTimeCast> spellComponents { get; set; }
        public int nbSpellActivation = 0;
        
        // In game parameters
        public float currentCooldown;

        public override string ToString()
        {
            return nameSpell;
        }
    }
}