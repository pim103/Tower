using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using UnityEngine;
using UnityEngine.UI;
using Utils;
using Vector3 = UnityEngine.Vector3;

namespace Games.Global
{
    public enum TypeEffect
    {
        Pierce,
        PierceOnBack,
        Burn,
        Poison,
        Freezing,
        Bleed,
        Weak,
        Stun,
        Sleep,
        Immobilization,
        Thorn,
        Mirror,
        Slow,
        Expulsion,
        DefenseUp,
        Intangible,
        AntiSpell,
        DivineShield,
        PhysicalDefUp,
        MagicalDefUp,
        Resurrection,
        Purification,
        Silence,
        BrokenDef,
        Confusion,
        Will,
        Fear,
        Charm,
        Regen,
        Blind,

        Invisibility,
        AttackSpeedUp,
        Link,
        AttackUp,
        SpeedUp,
        Untargetable,

        MadeADash,
        DotDamageIncrease
    }

    public enum OriginExpulsion
    {
        Entity,
        SrcDamage
    }
    
    public enum DirectionExpulsion
    {
        Out,
        In
    }

    public struct Effect
    {
        public TypeEffect typeEffect;
        public int level;
        public float durationInSeconds;

        public Entity launcher;
        public float ressourceCost;

        public Coroutine currentCoroutine;

        // direction use for effect expulsion
//        public Vector3 direction;
        public DirectionExpulsion directionExpul;
        public OriginExpulsion originExpulsion;
        public Vector3 positionSrcDamage;

        public void InitialTrigger(Entity entity)
        {
            switch (typeEffect)
            {
                case TypeEffect.Pierce:
                    entity.canPierce = true;
                    break;
                case TypeEffect.PierceOnBack:
                    entity.canPierceOnBack = true;
                    break;
                case TypeEffect.Stun:
                    entity.entityPrefab.canDoSomething = false;
                    break;
                case TypeEffect.Sleep:
                    entity.isSleep = true;
                    entity.entityPrefab.canDoSomething = false;
                    break;
                case TypeEffect.Invisibility:
                    entity.entityPrefab.SetMaterial(StaticMaterials.invisibleMaterial);
                    break;
                case TypeEffect.AttackSpeedUp:
                    entity.attSpeed = entity.initialAttSpeed + (0.5f * level);
                    break;
                case TypeEffect.AttackUp:
                    entity.att = entity.initialAtt + (1 * level);
                    break;
                case TypeEffect.SpeedUp:
                    entity.speed = entity.initialSpeed + (1 * level);
                    break;
                case TypeEffect.Immobilization:
                    entity.entityPrefab.canMove = false;
                    break;
                case TypeEffect.Thorn:
                    entity.hasThorn = true;
                    break;
                case TypeEffect.Mirror:
                    entity.hasMirror = true;
                    break;
                case TypeEffect.Expulsion:
                    Vector3 direction = CreateDirection(entity);
                    entity.entityPrefab.WantToApplyForce(direction, level);
                    break;
                case TypeEffect.Intangible:
                    entity.isIntangible = true;
                    break;
                case TypeEffect.AntiSpell:
                    entity.hasAntiSpell = true;
                    break;
                case TypeEffect.DivineShield:
                    entity.hasDivineShield = true;
                    break;
                case TypeEffect.Resurrection:
                    entity.shooldResurrect = true;
                    break;
                case TypeEffect.Purification:
                    List<Effect> effects = entity.underEffects.Values.ToList();

                    foreach (Effect effect in effects)
                    {
                        EffectController.StopCurrentEffect(entity, effect);
                    }

                    break;
                case TypeEffect.Silence:
                    entity.isSilence = true;
                    break;
                case TypeEffect.BrokenDef:
                    entity.def = entity.def - entity.initialDef >= 0 ? entity.def - entity.initialDef : 0;
                    entity.physicalDef = entity.physicalDef - entity.initialPhysicalDef >= 0
                        ? entity.physicalDef - entity.initialPhysicalDef
                        : 0;
                    entity.magicalDef = entity.magicalDef - entity.initialMagicalDef >= 0
                        ? entity.magicalDef - entity.initialMagicalDef
                        : 0;
                    break;
                case TypeEffect.Confusion:
                    entity.isConfuse = true;
                    break;
                case TypeEffect.Will:
                    entity.hasWill = true;

                    foreach (TypeEffect type in EffectController.ControlEffect)
                    {
                        if (entity.underEffects.ContainsKey(type))
                        {
                            EffectController.StopCurrentEffect(entity, entity.underEffects[type]);
                        }
                    }
                    
                    break;
                case TypeEffect.Fear:
                    entity.isFeared = true;
                    break;
                case TypeEffect.Charm:
                    entity.isCharmed = true;
                    break;
                case TypeEffect.Blind:
                    entity.isBlind = true;
                    break;
            }
        }

        public void TriggerEffectAtTime(Entity entity)
        {
            float extraDamage = launcher.underEffects.ContainsKey(TypeEffect.DotDamageIncrease) ? 0.2f : 0;
            Vector3 dir = Vector3.zero;
            
            switch (typeEffect)
            {
                case TypeEffect.Slow:
                    entity.speed = entity.initialSpeed / 2;
                    break;
                case TypeEffect.Burn:
                    if (entity.underEffects.ContainsKey(TypeEffect.Sleep))
                    {
                        Effect sleep = entity.underEffects[TypeEffect.Sleep];
                        EffectController.StopCurrentEffect(entity, sleep);
                    }

                    entity.ApplyDamage(0.2f + extraDamage);
                    break;
                case TypeEffect.Freezing:
                    if (entity.underEffects.ContainsKey(TypeEffect.Sleep))
                    {
                        Effect sleep = entity.underEffects[TypeEffect.Sleep];
                        EffectController.StopCurrentEffect(entity, sleep);
                    }

                    entity.speed = entity.initialSpeed - level;
                    entity.ApplyDamage(0.1f + extraDamage);
                    break;
                case TypeEffect.Bleed:
                    if (entity.underEffects.ContainsKey(TypeEffect.Sleep))
                    {
                        Effect sleep = entity.underEffects[TypeEffect.Sleep];
                        EffectController.StopCurrentEffect(entity, sleep);
                    }
                    entity.ApplyDamage(0.1f * level);
                    break;
                case TypeEffect.Poison:
                    entity.ApplyDamage(0.1f + extraDamage);
                    break;
                case TypeEffect.DefenseUp:
                    entity.def = entity.underEffects.ContainsKey(TypeEffect.BrokenDef) ? (1 * level) : entity.initialDef + (1 * level);
                    break;
                case TypeEffect.PhysicalDefUp:
                    entity.physicalDef = entity.underEffects.ContainsKey(TypeEffect.BrokenDef) ? (1 * level) : entity.initialPhysicalDef + (1 * level);
                    break;
                case TypeEffect.MagicalDefUp:
                    entity.magicalDef = entity.underEffects.ContainsKey(TypeEffect.BrokenDef) ? (1 * level) : entity.initialMagicalDef + (1 * level);
                    break;
                case TypeEffect.Regen:
                    entity.hp += 0.2f;
                    break;
                case TypeEffect.Fear:
                    originExpulsion = OriginExpulsion.Entity;
                    directionExpul = DirectionExpulsion.Out;
                    dir = CreateDirection(entity);

                    entity.entityPrefab.forcedDirection = dir;
                    break;
                case TypeEffect.Charm:
                    originExpulsion = OriginExpulsion.Entity;
                    directionExpul = DirectionExpulsion.In;
                    dir = CreateDirection(entity);

                    entity.entityPrefab.forcedDirection = dir;
                    break;
            }
        }

        public void EndEffect(Entity entity)
        {
            switch (typeEffect)
            {
                case TypeEffect.Pierce:
                    entity.canPierce = false;
                    break;
                case TypeEffect.PierceOnBack:
                    entity.canPierceOnBack = false;
                    break;
                case TypeEffect.Stun:
                    if (!entity.underEffects.ContainsKey(TypeEffect.Sleep))
                    {
                        entity.entityPrefab.canDoSomething = true;
                    }
                    break;
                case TypeEffect.Sleep:
                    entity.isSleep = false;
                    if (!entity.underEffects.ContainsKey(TypeEffect.Stun))
                    {
                        entity.entityPrefab.canDoSomething = true;
                    }
                    break;
                case TypeEffect.Slow:
                case TypeEffect.Freezing:
                    entity.speed = entity.initialSpeed;
                    break;
                case TypeEffect.DefenseUp:
                    entity.def = entity.initialDef;
                    break;
                case TypeEffect.Invisibility:
                    entity.entityPrefab.SetMaterial(StaticMaterials.defaultMaterial);
                    break;
                case TypeEffect.AttackSpeedUp:
                    entity.attSpeed = entity.initialAttSpeed;
                    break;
                case TypeEffect.AttackUp:
                    entity.speed = entity.initialSpeed;
                    break;
                case TypeEffect.SpeedUp:
                    entity.att = entity.initialAtt;
                    break;
                case TypeEffect.Immobilization:
                    entity.entityPrefab.canMove = true;
                    break;
                case TypeEffect.Thorn:
                    entity.hasThorn = false;
                    break;
                case TypeEffect.Mirror:
                    entity.hasMirror = false;
                    break;
                case TypeEffect.Intangible:
                    entity.isIntangible = false;
                    break;
                case TypeEffect.AntiSpell:
                    entity.hasAntiSpell = false;
                    break;
                case TypeEffect.DivineShield:
                    entity.hasDivineShield = false;
                    break;
                case TypeEffect.PhysicalDefUp:
                    entity.physicalDef = entity.initialPhysicalDef;
                    break;
                case TypeEffect.MagicalDefUp:
                    entity.magicalDef = entity.initialMagicalDef;
                    break;
                case TypeEffect.Resurrection:
                    entity.shooldResurrect = false;
                    break;
                case TypeEffect.Silence:
                    entity.isSilence = false;
                    break;
                case TypeEffect.BrokenDef:
                    if (entity.underEffects.ContainsKey(TypeEffect.DefenseUp))
                    {
                        entity.def += entity.initialDef;
                    }
                    else
                    {
                        entity.def = entity.initialDef;
                    }
                    
                    if (entity.underEffects.ContainsKey(TypeEffect.MagicalDefUp))
                    {
                        entity.magicalDef += entity.initialMagicalDef;
                    }
                    else
                    {
                        entity.magicalDef = entity.initialMagicalDef;
                    }

                    if (entity.underEffects.ContainsKey(TypeEffect.PhysicalDefUp))
                    {
                        entity.physicalDef += entity.initialPhysicalDef;
                    }
                    else
                    {
                        entity.physicalDef = entity.initialPhysicalDef;
                    }

                    break;
                case TypeEffect.Confusion:
                    entity.isConfuse = false;
                    break;
                case TypeEffect.Will:
                    entity.hasWill = false;
                    break;
                case TypeEffect.Fear:
                    entity.isFeared = false;
                    break;
                case TypeEffect.Charm:
                    entity.isCharmed = false;
                    break;
                case TypeEffect.Blind:
                    entity.isBlind = false;
                    break;
            }
        }

        public void UpdateEffect(Entity entity, Effect newEffect)
        {
            switch (typeEffect)
            {
                case TypeEffect.Poison:
                case TypeEffect.Burn:
                    durationInSeconds += newEffect.durationInSeconds;

                    if (durationInSeconds > 20)
                    {
                        durationInSeconds = 20;
                    }
                    break;
                case TypeEffect.Freezing:
                    if (durationInSeconds < newEffect.durationInSeconds)
                    {
                        durationInSeconds = newEffect.durationInSeconds;
                    }

                    if (level < 4)
                    {
                        level += newEffect.level;

                        if (level == 4)
                        {
                            Effect stunEffect = new Effect { typeEffect = TypeEffect.Stun, level = 1, durationInSeconds = 1, launcher = entity};
                            EffectController.ApplyEffect(entity, stunEffect);
                            EffectController.StopCurrentEffect(entity, this);
                        }
                    }
                    break;
                case TypeEffect.Bleed:
                    if (durationInSeconds < newEffect.durationInSeconds)
                    {
                        durationInSeconds = newEffect.durationInSeconds;
                    }

                    if (level < 5)
                    {
                        level += newEffect.level;
                    }
                    break;
                case TypeEffect.Weak:
                case TypeEffect.AttackUp:
                case TypeEffect.SpeedUp:
                case TypeEffect.Regen:
                case TypeEffect.Blind:
                    if (durationInSeconds < newEffect.durationInSeconds)
                    {
                        durationInSeconds = newEffect.durationInSeconds;
                    }
                    break;
                case TypeEffect.MagicalDefUp:
                case TypeEffect.PhysicalDefUp:
                case TypeEffect.DefenseUp:
                    durationInSeconds = newEffect.durationInSeconds;
                    level = newEffect.level;
                    break;
            }
        }

        private Vector3 CreateDirection(Entity entity)
        {
            Vector3 originExpulsionPosition = entity.entityPrefab.transform.position;
            Vector3 entityPosition = entity.entityPrefab.transform.position;
            
            if (originExpulsion == OriginExpulsion.Entity)
            {
                originExpulsionPosition = launcher.entityPrefab.transform.position;
            }

            Vector3 dir = Vector3.zero;
            if (directionExpul == DirectionExpulsion.Out)
            {
                if (originExpulsion == OriginExpulsion.SrcDamage)
                {
                    dir = positionSrcDamage;
                }
                else
                {
                    dir = (entityPosition - originExpulsionPosition).normalized;
                }
            }
            else if (directionExpul == DirectionExpulsion.In)
            {
                if (originExpulsion == OriginExpulsion.SrcDamage)
                {
                    dir = -positionSrcDamage;
                }
                else
                {
                    dir = (originExpulsionPosition - entityPosition).normalized;
                }
            }

            return dir;
        }
    }
}