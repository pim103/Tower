using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using Games.Global.Abilities;
using Games.Global.Armors;
using Games.Global.Spells.SpellParameter;
using Games.Global.Weapons;
using UnityEngine;
using Utils;
using Debug = UnityEngine.Debug;
using Vector3 = UnityEngine.Vector3;

namespace Games.Global.Spells.SpellsController
{
    public class AreaOfEffectController : MonoBehaviour, ISpellController
    {
        private static Transform parent;

        public AreaOfEffectSpell Clone(SpellComponent spellComponent)
        {
            AreaOfEffectSpell origin = (AreaOfEffectSpell) spellComponent;

            AreaOfEffectSpell cloneAreaOfEffectSpell = new AreaOfEffectSpell
            {
                duration = origin.duration,
                damageType = origin.damageType,
                damagesOnAlliesOnInterval = origin.damagesOnAlliesOnInterval,
                geometry = origin.geometry,
                interval = origin.interval,
                scale = origin.scale,
                currentCoroutine = origin.currentCoroutine,
                objectPooled = origin.objectPooled,
                onePlay = origin.onePlay,
                randomPosition = origin.randomPosition,
                startPosition = origin.startPosition,
                typeSpell = origin.typeSpell,
                alliesInZone = origin.alliesInZone,
                canStopProjectile = origin.canStopProjectile,
                enemiesInZone = origin.enemiesInZone,
                isBasicAttack = origin.isBasicAttack,
                randomTargetHit = origin.randomTargetHit,
                spellWithConditions = origin.spellWithConditions,
                transformToFollow = origin.transformToFollow,
                wantToFollow = origin.wantToFollow,
                linkedSpellOnEnd = origin.linkedSpellOnEnd,
                linkedSpellOnInterval = origin.linkedSpellOnInterval,
                positionToStartSpell = origin.positionToStartSpell,
                appliesPlayerOnHitEffect = origin.appliesPlayerOnHitEffect,
                damagesOnEnemiesOnInterval = origin.damagesOnAlliesOnInterval,
                effectOnHitOnStart = origin.effectOnHitOnStart,
                effectsOnAlliesOnInterval = origin.effectsOnAlliesOnInterval,
                effectsOnEnemiesOnInterval = origin.effectsOnEnemiesOnInterval,
                effectsOnPlayerOnInterval = origin.effectsOnPlayerOnInterval,
                deleteEffectsOnEnemiesOnInterval = origin.deleteEffectsOnEnemiesOnInterval,
                deleteEffectsOnPlayerOnInterval = origin.deleteEffectsOnPlayerOnInterval
            };

            return cloneAreaOfEffectSpell;
        }
        
        public void LaunchSpell(Entity entity, SpellComponent spellComponent)
        {
            AreaOfEffectSpell areaOfEffectSpell = Clone(spellComponent);
            Coroutine currentCoroutine = SpellController.instance.StartCoroutine(PlayAreaSpell(entity, areaOfEffectSpell));
            areaOfEffectSpell.currentCoroutine = currentCoroutine;
        }

        private IEnumerator PlayAreaSpell(Entity entity, AreaOfEffectSpell areaOfEffectSpell)
        {
            PoolAreaWithParameter(entity, areaOfEffectSpell);

            float duration = areaOfEffectSpell.duration;            
            yield return new WaitForSeconds(0.05f);
            duration -= 0.05f;

            InitialArea(entity, areaOfEffectSpell);

            if (areaOfEffectSpell.onePlay)
            {
                duration = 0.1f;
                areaOfEffectSpell.interval = 0.1f;
            }

            while (duration > 0)
            {
                yield return new WaitForSeconds(areaOfEffectSpell.interval);
                duration -= areaOfEffectSpell.interval;

                IntervalArea(entity, areaOfEffectSpell);
            }

            EndArea(entity, areaOfEffectSpell);
        }

        private void PoolAreaWithParameter(Entity entity, AreaOfEffectSpell areaOfEffectSpell)
        {
            areaOfEffectSpell.enemiesInZone = new List<Entity>();
            areaOfEffectSpell.alliesInZone = new List<Entity>();

            GameObject genericSpellPrefab = ObjectPooler.SharedInstance.GetPooledObject(1);

            parent = genericSpellPrefab.transform.parent;
            
            genericSpellPrefab.transform.localScale = areaOfEffectSpell.scale;
            genericSpellPrefab.transform.position = areaOfEffectSpell.startPosition;

            SpellPrefabController spellPrefabController = genericSpellPrefab.GetComponent<SpellPrefabController>();
            spellPrefabController.ActiveCollider(areaOfEffectSpell.geometry);
            spellPrefabController.SetValues(entity, areaOfEffectSpell);

            areaOfEffectSpell.objectPooled = genericSpellPrefab;

            if (areaOfEffectSpell.wantToFollow && areaOfEffectSpell.transformToFollow)
            {
                genericSpellPrefab.transform.parent = areaOfEffectSpell.transformToFollow;
            }
            
            genericSpellPrefab.SetActive(true);
        }
        
        private void InitialArea(Entity entity, AreaOfEffectSpell areaOfEffectSpell)
        {
            foreach (Entity enemy in areaOfEffectSpell.enemiesInZone)
            {
                EffectController.ApplyEffect(enemy, areaOfEffectSpell.effectOnHitOnStart);
            }
        }

        private void PlaySpecialCondition(Entity entity, Entity enemy, AreaOfEffectSpell areaOfEffectSpell)
        {
            if (areaOfEffectSpell.spellWithConditions == null)
            {
                return;
            }

            foreach (SpellWithCondition spell in areaOfEffectSpell.spellWithConditions)
            {
                switch (spell.conditionType)
                {
                    case ConditionType.IfPlayerHasEffect:
                        if (entity.underEffects.ContainsKey(spell.conditionEffect.typeEffect))
                        {
                            if (spell.instructionTargeting == InstructionTargeting.ApplyOnTarget)
                            {
                                EffectController.ApplyEffect(enemy, spell.effect);
                            }
                        }
                        break;
                    case ConditionType.IfTargetHasEffectWhenHit:
                        if (enemy.underEffects.ContainsKey(spell.conditionEffect.typeEffect))
                        {
                            switch (spell.instructionTargeting)
                            {
                                case InstructionTargeting.ApplyOnTarget:
                                    EffectController.ApplyEffect(enemy, spell.effect);
                                    break;
                                case InstructionTargeting.DeleteOnTarget:
                                    if (enemy.underEffects.ContainsKey(spell.effect.typeEffect))
                                    {
                                        EffectController.StopCurrentEffect(enemy, enemy.underEffects[spell.effect.typeEffect]);
                                    }
                                    break;
                            }
                        }
                        break;
                }
            }
        }

        private void BasicAttack(Entity entity, Entity enemy, float extraDamage, AreaOfEffectSpell areaOfEffectSpell)
        {
            AbilityParameters paramaters = new AbilityParameters { origin = entity };

            if ((enemy.isIntangible && areaOfEffectSpell.damageType == DamageType.Physical) ||
                (enemy.hasAntiSpell && areaOfEffectSpell.damageType == DamageType.Magical) ||
                entity.isBlind ||
                enemy.isUntargeatable)
            {
                return;
            }

            BuffController.EntityReceivedDamage(enemy, entity);

            if (entity.hasDivineShield)
            {
                return;
            }

            Weapon weapon = entity.weapons[0];

            if (weapon != null)
            {
                weapon.OnDamageDealt(paramaters);
                extraDamage += weapon.damage;
            }

            foreach (Armor armor in entity.armors)
            {
                armor.OnDamageDealt(paramaters);
            }

            List<Effect> effects = entity.damageDealExtraEffect.DistinctBy(currentEffect => currentEffect.typeEffect)
                .ToList();
            foreach (Effect effect in effects)
            {
                Effect copy = effect;
                copy.positionSrcDamage = entity.entityPrefab.transform.position;
                EffectController.ApplyEffect(enemy, copy);
            }

            BuffController.EntityDealDamage(entity, enemy);

            float damage = entity.att + extraDamage;
            if (entity.isWeak)
            {
                damage /= 2;
            }

            enemy.TakeDamage(damage, paramaters, entity.canPierce);
        }

        private void IntervalHitEnemies(Entity entity, AreaOfEffectSpell areaOfEffectSpell)
        {
            AbilityParameters paramaters = new AbilityParameters { origin = entity };

            List<Entity> enemies = new List<Entity>();

            if (areaOfEffectSpell.randomTargetHit && areaOfEffectSpell.enemiesInZone.Count > 0)
            {
                int rand = Random.Range(0, areaOfEffectSpell.enemiesInZone.Count);
                Entity enemy = areaOfEffectSpell.enemiesInZone[rand];
                enemies.Clear();
                enemies.Add(enemy);
            }
            else
            {
                enemies = areaOfEffectSpell.enemiesInZone;
            }

            foreach (Entity enemy in enemies)
            {
                SpellWithCondition conditionSpell;
                int extraDamage = 0;

                if (areaOfEffectSpell.spellWithConditions != null)
                {
                    if ((conditionSpell = areaOfEffectSpell.spellWithConditions.Find(spell =>
                            spell.conditionType == ConditionType.DamageIfTargetHasEffect)) != null)
                    {
                        if (enemy.underEffects.ContainsKey(conditionSpell.conditionEffect.typeEffect))
                        {
                            extraDamage = conditionSpell.level;
                        }
                    }

                    if ((conditionSpell = areaOfEffectSpell.spellWithConditions.Find(spell =>
                            spell.conditionType == ConditionType.IfTargetDies)) != null)
                    {
                        if (enemy.hp - areaOfEffectSpell.damagesOnEnemiesOnInterval + extraDamage < 0)
                        {
                            EffectController.ApplyEffect(entity, conditionSpell.effect);
                        }
                    }
                }

                if (areaOfEffectSpell.isBasicAttack)
                {
                    BasicAttack(entity, enemy, extraDamage, areaOfEffectSpell);
                }
                else
                {
                    enemy.TakeDamage(areaOfEffectSpell.damagesOnEnemiesOnInterval + extraDamage, paramaters);

                    if (areaOfEffectSpell.appliesPlayerOnHitEffect && !areaOfEffectSpell.isBasicAttack)
                    {
                        List<Effect> effects = entity.damageDealExtraEffect.DistinctBy(currentEffect => currentEffect.typeEffect)
                            .ToList();
                        foreach (Effect effect in effects)
                        {
                            Effect copy = effect;
                            copy.positionSrcDamage = areaOfEffectSpell.objectPooled.transform.position;
                            EffectController.ApplyEffect(enemy, copy);
                        }
                    }
                }

                if (areaOfEffectSpell.effectsOnEnemiesOnInterval != null)
                {
                    foreach (Effect effect in areaOfEffectSpell.effectsOnEnemiesOnInterval)
                    {
                        Effect copy = effect;
                        copy.positionSrcDamage = areaOfEffectSpell.objectPooled.transform.position;
                        EffectController.ApplyEffect(enemy, effect);
                    }
                }

                if (areaOfEffectSpell.deleteEffectsOnEnemiesOnInterval != null)
                {
                    foreach (TypeEffect typeEffect in areaOfEffectSpell.deleteEffectsOnEnemiesOnInterval)
                    {
                        if (entity.underEffects.ContainsKey(typeEffect))
                        {
                            EffectController.StopCurrentEffect(entity, entity.underEffects[typeEffect]);
                        }
                    }
                }

                PlaySpecialCondition(entity, enemy, areaOfEffectSpell);
            }
        }

        private void IntervalHitAllies(Entity entity, AreaOfEffectSpell areaOfEffectSpell)
        {
            AbilityParameters paramaters = new AbilityParameters { origin = entity };
            
            foreach (Entity ally in areaOfEffectSpell.alliesInZone)
            {
                if (ally == entity)
                {
                    if (areaOfEffectSpell.effectsOnPlayerOnInterval != null)
                    {
                        foreach (Effect effect in areaOfEffectSpell.effectsOnPlayerOnInterval)
                        {
                            EffectController.ApplyEffect(ally, effect);
                        }
                    }

                    if (areaOfEffectSpell.deleteEffectsOnPlayerOnInterval != null)
                    {
                        foreach (TypeEffect typeEffect in areaOfEffectSpell.deleteEffectsOnPlayerOnInterval)
                        {
                            if (entity.underEffects.ContainsKey(typeEffect))
                            {
                                EffectController.StopCurrentEffect(ally, entity.underEffects[typeEffect]);
                            }
                        }
                    }

                    continue;
                }

                ally.TakeDamage(areaOfEffectSpell.damagesOnAlliesOnInterval, paramaters);

                if (areaOfEffectSpell.effectsOnAlliesOnInterval != null)
                {
                    foreach (Effect effect in areaOfEffectSpell.effectsOnAlliesOnInterval)
                    {
                        EffectController.ApplyEffect(ally, effect);
                    }
                }
            }
        }

        private void IntervalArea(Entity entity, AreaOfEffectSpell areaOfEffectSpell)
        {
            if (areaOfEffectSpell.linkedSpellOnInterval != null)
            {
                Vector3 position = areaOfEffectSpell.startPosition;
                if (areaOfEffectSpell.randomPosition)
                {
                    if (areaOfEffectSpell.geometry == Geometry.Sphere)
                    {
                        float t = 2 * Mathf.PI * Random.Range(0.0f, 1.0f);
                        float rx = Random.Range(0.0f, areaOfEffectSpell.scale.x / 2);
                        float rz = Random.Range(0.0f, areaOfEffectSpell.scale.z / 2);
                        position = new Vector3
                        {
                            x = areaOfEffectSpell.startPosition.x + rx * Mathf.Cos(t), 
                            y = areaOfEffectSpell.startPosition.y, 
                            z = areaOfEffectSpell.startPosition.z + rz * Mathf.Sin(t)
                        };
                    }
                    else
                    {
                        position = new Vector3
                        {
                            x = areaOfEffectSpell.startPosition.x + Random.Range(-areaOfEffectSpell.scale.x/2, areaOfEffectSpell.scale.x/2), 
                            y = areaOfEffectSpell.startPosition.y, 
                            z = areaOfEffectSpell.startPosition.z + Random.Range(-areaOfEffectSpell.scale.z/2, areaOfEffectSpell.scale.z/2)
                        };
                    }
                }

                Entity enemy = null;
                if (areaOfEffectSpell.randomTargetHit)
                {
                    int rand = Random.Range(0, areaOfEffectSpell.enemiesInZone.Count);
                    if (areaOfEffectSpell.enemiesInZone.Count > 0)
                    {
                        enemy = areaOfEffectSpell.enemiesInZone[rand];
                        position = enemy.entityPrefab.transform.position;
                    }
                }

                SpellController.CastSpellComponent(entity, areaOfEffectSpell.linkedSpellOnInterval, position, enemy);
            }

            IntervalHitEnemies(entity, areaOfEffectSpell);
            IntervalHitAllies(entity, areaOfEffectSpell);
        }

        private static void EndArea(Entity entity, AreaOfEffectSpell areaOfEffectSpell)
        {
            if (areaOfEffectSpell.linkedSpellOnEnd != null)
            {
                SpellController.CastSpellComponent(entity, areaOfEffectSpell.linkedSpellOnEnd, areaOfEffectSpell.startPosition, entity);
            }

            areaOfEffectSpell.objectPooled.transform.parent = parent;
            areaOfEffectSpell.objectPooled.SetActive(false);
            
            if (areaOfEffectSpell.currentCoroutine != null)
            {
                SpellController.instance.StopCoroutine(areaOfEffectSpell.currentCoroutine);
            }
        }

        public static void EntityTriggerEnter(Entity origin, Collider other, AreaOfEffectSpell areaOfEffectSpell, bool isSpell)
        {
            if (isSpell)
            {
                SpellPrefabController spellPrefab = other.transform.GetComponent<SpellPrefabController>();
                if (spellPrefab == null)
                {
                    spellPrefab = other.transform.parent.GetComponent<SpellPrefabController>();
                }

                SpellComponent spell = spellPrefab.spellComponent;
                if (spell.typeSpell == TypeSpell.Projectile && areaOfEffectSpell.canStopProjectile)
                {
                    ProjectileSpell projSpell = (ProjectileSpell) spell;
                    ProjectileController.EndArea(origin, projSpell);
                }

                return;
            }
            
            Entity entityEnter = other.GetComponent<EntityPrefab>().entity;
            
            if ( (origin.typeEntity == TypeEntity.MOB && entityEnter.typeEntity == TypeEntity.PLAYER ) ||
                 (origin.typeEntity == TypeEntity.PLAYER && entityEnter.typeEntity == TypeEntity.MOB ))
            {
                areaOfEffectSpell.enemiesInZone.Add(entityEnter);
            }
            else
            {
                areaOfEffectSpell.alliesInZone.Add(entityEnter);
            }
        }

        public static void EntityTriggerExit(Entity origin, Entity entityExit, AreaOfEffectSpell areaOfEffectSpell)
        {
            if ( (origin.typeEntity == TypeEntity.MOB && entityExit.typeEntity == TypeEntity.PLAYER ) ||
                 (origin.typeEntity == TypeEntity.PLAYER && entityExit.typeEntity == TypeEntity.MOB ))
            {
                if (areaOfEffectSpell.enemiesInZone.Contains(entityExit))
                {
                    areaOfEffectSpell.enemiesInZone.Remove(entityExit);
                }
            }
            else
            {
                if (areaOfEffectSpell.alliesInZone.Contains(entityExit))
                {
                    areaOfEffectSpell.alliesInZone.Remove(entityExit);
                }
            }
        }
    }
}
