using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Games.Global.Armors;
using Games.Global.Spells.SpellBehavior;
using Games.Global.Spells.SpellParameter;
using Games.Global.Weapons;
using UnityEngine;
using Utils;
using Vector3 = UnityEngine.Vector3;

namespace Games.Global.Spells.SpellsController
{
    public class AreaOfEffectController : MonoBehaviour, ISpellController
    {
        private static Transform parent;

        public void LaunchSpell(Entity entity, SpellComponent spellComponent, SpellComponent origin = null)
        {
            AreaOfEffectSpell areaOfEffectSpell = Tools.Clone((AreaOfEffectSpell) spellComponent);
            Coroutine currentCoroutine = SpellController.instance.StartCoroutine(PlayAreaSpell(entity, areaOfEffectSpell, origin));
            areaOfEffectSpell.currentCoroutine = currentCoroutine;
        }

        private IEnumerator PlayAreaSpell(Entity entity, AreaOfEffectSpell areaOfEffectSpell, SpellComponent origin = null)
        {
            PoolAreaWithParameter(entity, areaOfEffectSpell, origin);

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

        private void PoolAreaWithParameter(Entity entity, AreaOfEffectSpell areaOfEffectSpell, SpellComponent origin = null)
        {
//            areaOfEffectSpell.enemiesInZone = new List<Entity>();
//            areaOfEffectSpell.alliesInZone = new List<Entity>();
//
//            GameObject genericSpellPrefab = ObjectPooler.SharedInstance.GetPooledObject(1);
//
//            parent = genericSpellPrefab.transform.parent;
//
//            genericSpellPrefab.transform.localScale = areaOfEffectSpell.scale;
//            genericSpellPrefab.transform.position = areaOfEffectSpell.startPosition;
//            genericSpellPrefab.transform.localEulerAngles = entity.entityPrefab.transform.localEulerAngles;
//
//            if (areaOfEffectSpell.originArea == OriginArea.From)
//            {
//                genericSpellPrefab.transform.position += genericSpellPrefab.transform.forward *(areaOfEffectSpell.scale.z / 2);
//            }
//
//            SpellPrefabController spellPrefabController = genericSpellPrefab.GetComponent<SpellPrefabController>();
//            spellPrefabController.ActiveCollider(areaOfEffectSpell.geometry);
//            spellPrefabController.SetValues(entity, areaOfEffectSpell);
//
//            areaOfEffectSpell.objectPooled = genericSpellPrefab;
//
//            if (areaOfEffectSpell.wantToFollow)
//            {
//                if (areaOfEffectSpell.OriginalPosition == StartFrom.Caster)
//                {
//                    genericSpellPrefab.transform.parent = entity.entityPrefab.transform;
//                } else if (areaOfEffectSpell.OriginalPosition == StartFrom.PositionInParameter && origin != null && origin.GetType() == typeof(ProjectileSpell))
//                {
//                    ProjectileSpell proj = (ProjectileSpell) origin;
//                    genericSpellPrefab.transform.parent = proj.objectPooled.transform;
//                }
//            }
//
//            genericSpellPrefab.SetActive(true);
        }
        
        private void InitialArea(Entity entity, AreaOfEffectSpell areaOfEffectSpell)
        {
            if (areaOfEffectSpell.effectOnHitOnStart == null)
            {
                return;
            }

            foreach (Entity enemy in areaOfEffectSpell.enemiesInZone)
            {
//                EffectController.ApplyEffect(enemy, areaOfEffectSpell.effectOnHitOnStart, entity, areaOfEffectSpell.startPosition);
            }
        }

        private void PlaySpecialCondition(Entity entity, Entity enemy, AreaOfEffectSpell areaOfEffectSpell)
        {
//            if (areaOfEffectSpell.spellWithConditions == null)
//            {
//                return;
//            }
//
//            foreach (SpellWithCondition spell in areaOfEffectSpell.spellWithConditions)
//            {
//                switch (spell.conditionType)
//                {
//                    case ConditionType.IfPlayerHasEffect:
//                        if (entity.underEffects.ContainsKey(spell.conditionEffect.typeEffect))
//                        {
//                            if (spell.instructionTargeting == InstructionTargeting.ApplyOnTarget)
//                            {
//                                if (spell.effect == null)
//                                {
//                                    continue;
//                                }
//                                EffectController.ApplyEffect(enemy, spell.effect, entity, areaOfEffectSpell.startPosition);
//                            }
//                        }
//                        break;
//                    case ConditionType.IfTargetHasEffectWhenHit:
//                        if (enemy.underEffects.ContainsKey(spell.conditionEffect.typeEffect))
//                        {
//                            switch (spell.instructionTargeting)
//                            {
//                                case InstructionTargeting.ApplyOnTarget:
//                                    if (spell.effect == null)
//                                    {
//                                        continue;
//                                    }
//                                    EffectController.ApplyEffect(enemy, spell.effect, entity, areaOfEffectSpell.startPosition);
//                                    break;
//                                case InstructionTargeting.DeleteOnTarget:
//                                    if (enemy.underEffects.ContainsKey(spell.effect.typeEffect))
//                                    {
//                                        EffectController.StopCurrentEffect(enemy, enemy.underEffects[spell.effect.typeEffect]);
//                                    }
//                                    break;
//                            }
//                        }
//                        break;
//                }
//            }
        }

        private void IntervalHitEnemies(Entity entity, AreaOfEffectSpell areaOfEffectSpell)
        {
//            AbilityParameters paramaters = new AbilityParameters { origin = entity };
//
//            List<Entity> enemies = new List<Entity>();
//
//            if (areaOfEffectSpell.randomTargetHit && areaOfEffectSpell.enemiesInZone.Count > 0)
//            {
//                int rand = Random.Range(0, areaOfEffectSpell.enemiesInZone.Count);
//                Entity enemy = areaOfEffectSpell.enemiesInZone[rand];
//                enemies.Clear();
//                enemies.Add(enemy);
//            }
//            else
//            {
//                enemies = areaOfEffectSpell.enemiesInZone;
//            }
//
//            foreach (Entity enemy in enemies)
//            {
//                SpellWithCondition conditionSpell;
//                int extraDamage = 0;
//
//                if (areaOfEffectSpell.spellWithConditions != null)
//                {
//                    if ((conditionSpell = areaOfEffectSpell.spellWithConditions.Find(spell =>
//                            spell.conditionType == ConditionType.DamageIfTargetHasEffect)) != null)
//                    {
//                        if (enemy.underEffects.ContainsKey(conditionSpell.conditionEffect.typeEffect))
//                        {
//                            extraDamage = conditionSpell.level;
//                        }
//                    }
//
//                    if ((conditionSpell = areaOfEffectSpell.spellWithConditions.Find(spell =>
//                            spell.conditionType == ConditionType.IfTargetDies)) != null)
//                    {
//                        if (enemy.hp - areaOfEffectSpell.damagesOnEnemiesOnInterval + extraDamage < 0)
//                        {
//                            EffectController.ApplyEffect(entity, conditionSpell.effect, entity, areaOfEffectSpell.objectPooled.transform.position);
//                        }
//                    }
//                }
//
//                if (areaOfEffectSpell.isBasicAttack)
//                {
//                    BasicAttack(entity, enemy, extraDamage, areaOfEffectSpell);
//                }
//                else
//                {
//                    enemy.TakeDamage(areaOfEffectSpell.damagesOnEnemiesOnInterval + extraDamage, paramaters, areaOfEffectSpell.damageType);
//
//                    if (areaOfEffectSpell.appliesPlayerOnHitEffect && !areaOfEffectSpell.isBasicAttack)
//                    {
//                        List<Effect> effects = entity.damageDealExtraEffect.DistinctBy(currentEffect => currentEffect.typeEffect)
//                            .ToList();
//                        foreach (Effect effect in effects)
//                        {
//                            EffectController.ApplyEffect(enemy, effect, entity, areaOfEffectSpell.objectPooled.transform.position);
//                        }
//                    }
//                }
//
//                if (areaOfEffectSpell.effectsOnEnemiesOnInterval != null)
//                {
//                    foreach (Effect effect in areaOfEffectSpell.effectsOnEnemiesOnInterval)
//                    {
//                        EffectController.ApplyEffect(enemy, effect, entity, areaOfEffectSpell.objectPooled.transform.position);
//                    }
//                }
//
//                if (areaOfEffectSpell.deleteEffectsOnEnemiesOnInterval != null)
//                {
//                    foreach (TypeEffect typeEffect in areaOfEffectSpell.deleteEffectsOnEnemiesOnInterval)
//                    {
//                        if (entity.underEffects.ContainsKey(typeEffect))
//                        {
//                            EffectController.StopCurrentEffect(entity, entity.underEffects[typeEffect]);
//                        }
//                    }
//                }
//
//                PlaySpecialCondition(entity, enemy, areaOfEffectSpell);
//            }
        }

        private void IntervalHitAllies(Entity entity, AreaOfEffectSpell areaOfEffectSpell)
        {
//            AbilityParameters paramaters = new AbilityParameters { origin = entity };
//            
//            foreach (Entity ally in areaOfEffectSpell.alliesInZone)
//            {
//                if (ally == entity)
//                {
//                    if (areaOfEffectSpell.effectsOnPlayerOnInterval != null)
//                    {
//                        foreach (Effect effect in areaOfEffectSpell.effectsOnPlayerOnInterval)
//                        {
//                            if (effect == null)
//                            {
//                                continue;
//                            }
//                            EffectController.ApplyEffect(ally, effect, entity, areaOfEffectSpell.startPosition);
//                        }
//                    }
//
//                    if (areaOfEffectSpell.deleteEffectsOnPlayerOnInterval != null)
//                    {
//                        foreach (TypeEffect typeEffect in areaOfEffectSpell.deleteEffectsOnPlayerOnInterval)
//                        {
//                            if (entity.underEffects.ContainsKey(typeEffect))
//                            {
//                                EffectController.StopCurrentEffect(ally, entity.underEffects[typeEffect]);
//                            }
//                        }
//                    }
//
//                    continue;
//                }
//
//                ally.TakeDamage(areaOfEffectSpell.damagesOnAlliesOnInterval, paramaters, areaOfEffectSpell.damageType);
//
//                if (areaOfEffectSpell.effectsOnAlliesOnInterval != null)
//                {
//                    foreach (Effect effect in areaOfEffectSpell.effectsOnAlliesOnInterval)
//                    {
//                        if (effect == null)
//                        {
//                            continue;
//                        }
//                        EffectController.ApplyEffect(ally, effect, entity, areaOfEffectSpell.startPosition);
//                    }
//                }
//            }
        }

        private void IntervalArea(Entity entity, AreaOfEffectSpell areaOfEffectSpell)
        {
//            if (areaOfEffectSpell.linkedSpellOnInterval != null)
//            {
//                Vector3 position = areaOfEffectSpell.startPosition;
//                if (areaOfEffectSpell.randomPosition)
//                {
//                    if (areaOfEffectSpell.geometry == Geometry.Sphere)
//                    {
//                        float t = 2 * Mathf.PI * Random.Range(0.0f, 1.0f);
//                        float rx = Random.Range(0.0f, areaOfEffectSpell.scale.x / 2);
//                        float rz = Random.Range(0.0f, areaOfEffectSpell.scale.z / 2);
//                        position = new Vector3
//                        {
//                            x = areaOfEffectSpell.startPosition.x + rx * Mathf.Cos(t), 
//                            y = areaOfEffectSpell.startPosition.y, 
//                            z = areaOfEffectSpell.startPosition.z + rz * Mathf.Sin(t)
//                        };
//                    }
//                    else
//                    {
//                        position = new Vector3
//                        {
//                            x = areaOfEffectSpell.startPosition.x + Random.Range(-areaOfEffectSpell.scale.x/2, areaOfEffectSpell.scale.x/2), 
//                            y = areaOfEffectSpell.startPosition.y, 
//                            z = areaOfEffectSpell.startPosition.z + Random.Range(-areaOfEffectSpell.scale.z/2, areaOfEffectSpell.scale.z/2)
//                        };
//                    }
//                }
//
//                Entity enemy = null;
//                if (areaOfEffectSpell.randomTargetHit)
//                {
//                    int rand = Random.Range(0, areaOfEffectSpell.enemiesInZone.Count);
//                    if (areaOfEffectSpell.enemiesInZone.Count > 0)
//                    {
//                        enemy = areaOfEffectSpell.enemiesInZone[rand];
//                        position = enemy.entityPrefab.transform.position;
//                    }
//                }
//
//                SpellController.CastSpellComponent(entity, areaOfEffectSpell.linkedSpellOnInterval, position, enemy, areaOfEffectSpell);
//            }
//
//            IntervalHitEnemies(entity, areaOfEffectSpell);
//            IntervalHitAllies(entity, areaOfEffectSpell);
        }

        private static void EndArea(Entity entity, AreaOfEffectSpell areaOfEffectSpell)
        {
            if (areaOfEffectSpell.linkedSpellOnEnd != null)
            {
//                SpellController.CastSpellComponent(entity, areaOfEffectSpell.linkedSpellOnEnd, areaOfEffectSpell.startPosition, entity, areaOfEffectSpell);
            }

            areaOfEffectSpell.objectPooled.transform.localScale = Vector3.one;
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

//                SpellComponent spell = spellPrefab.spellComponent;
//                if (spell.typeSpell == TypeSpell.Projectile && areaOfEffectSpell.canStopProjectile)
//                {
//                    ProjectileSpell projSpell = (ProjectileSpell) spell;
//                    ProjectileController.EndArea(origin, projSpell);
//                }

                return;
            }

            Entity entityEnter = other.GetComponent<EntityPrefab>().entity;
            
            if ( (origin.typeEntity == TypeEntity.MOB && entityEnter.typeEntity == TypeEntity.ALLIES ) ||
                 (origin.typeEntity == TypeEntity.ALLIES && entityEnter.typeEntity == TypeEntity.MOB ))
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
            if ( (origin.typeEntity == TypeEntity.MOB && entityExit.typeEntity == TypeEntity.ALLIES ) ||
                 (origin.typeEntity == TypeEntity.ALLIES && entityExit.typeEntity == TypeEntity.MOB ))
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
