using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Games.Global.Abilities;
using Games.Global.Armors;
using Games.Global.Weapons;
using UnityEngine;
using Utils;

namespace Games.Global.Spells.SpellsController
{
    public class ProjectileController : MonoBehaviour, ISpellController
    {
        public ProjectileSpell Clone(SpellComponent spellComponent)
        {
            ProjectileSpell origin = (ProjectileSpell) spellComponent;

            ProjectileSpell cloneProjectileSpell = new ProjectileSpell
            {
                damages = origin.damages,
                duration = origin.duration,
                speed = origin.speed,
                trajectory = origin.trajectory,
                damageType = origin.damageType,
                initialRotation = origin.initialRotation,
                startPosition = origin.startPosition,
                trajectoryType = origin.trajectoryType,
                typeSpell = origin.typeSpell,
                effectsOnHit = origin.effectsOnHit,
                idPoolObject = origin.idPoolObject,
                isBasicAttack = origin.isBasicAttack,
                passingThroughEntity = origin.passingThroughEntity,
                damageMultiplierOnDistance = origin.damageMultiplierOnDistance,
                linkedSpellOnDisable = origin.linkedSpellOnDisable,
                linkedSpellOnEnable = origin.linkedSpellOnEnable,
                positionToStartSpell = origin.positionToStartSpell
            };

            return cloneProjectileSpell;
        }
        
        public void LaunchSpell(Entity entity, SpellComponent spellComponent)
        {
            ProjectileSpell newProjectileSpell = Clone(spellComponent);

            Coroutine currentCoroutine =
                SpellController.instance.StartCoroutine(PlayProjectileSpell(entity, newProjectileSpell));
            newProjectileSpell.currentCoroutine = currentCoroutine;
        }

        private IEnumerator PlayProjectileSpell(Entity entity, ProjectileSpell projectileSpell)
        {
            PoolAreaWithParameter(entity, projectileSpell);

            if (projectileSpell.linkedSpellOnEnable != null)
            {
                SpellController.CastSpellComponent(entity, projectileSpell.linkedSpellOnEnable,
                    projectileSpell.startPosition, entity);
            }

            float duration = projectileSpell.duration;
            while (duration > 0)
            {
                yield return new WaitForSeconds(0.05f);
                duration -= 0.05f;

                UpdatePosition(entity, projectileSpell);
            }

            EndArea(entity, projectileSpell);
        }

        private void PoolAreaWithParameter(Entity entity, ProjectileSpell projectileSpell)
        {
            GameObject genericSpellPrefab = ObjectPooler.SharedInstance.GetPooledObject(1);

            Vector3 initialRotation = projectileSpell.initialRotation;
            
            if (projectileSpell.trajectoryType == Trajectory.OriginForward)
            {
                initialRotation = entity.entityPrefab.transform.localEulerAngles;
                projectileSpell.trajectory = entity.entityPrefab.transform.forward;
            }

            genericSpellPrefab.transform.position = projectileSpell.startPosition;
            genericSpellPrefab.transform.localEulerAngles = initialRotation;
            genericSpellPrefab.transform.localScale = Vector3.one;

            GameObject prefabWanted = ObjectPooler.SharedInstance.GetPooledObject(projectileSpell.idPoolObject);
            prefabWanted.transform.parent = genericSpellPrefab.transform;
            prefabWanted.transform.localPosition = Vector3.zero;
            prefabWanted.transform.localEulerAngles = Vector3.zero;

            SpellPrefabController spellPrefabController = genericSpellPrefab.GetComponent<SpellPrefabController>();
            spellPrefabController.SetValues(entity, projectileSpell);

            projectileSpell.objectPooled = genericSpellPrefab;
            projectileSpell.prefabPooled = prefabWanted;

            genericSpellPrefab.SetActive(true);
            prefabWanted.SetActive(true);
        }

        private void UpdatePosition(Entity entity, ProjectileSpell projectileSpell)
        {
            float speed = projectileSpell.speed * 0.05f;
            projectileSpell.objectPooled.transform.position += (projectileSpell.trajectory * speed);
        }

        private static void BasicAttack(Entity entity, Entity enemy, float extraDamage, ProjectileSpell projectileSpell)
        {
            AbilityParameters paramaters = new AbilityParameters {origin = entity};

            if ((enemy.isIntangible && projectileSpell.damageType == DamageType.Physical) ||
                (enemy.hasAntiSpell && projectileSpell.damageType == DamageType.Magical) ||
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

        public static void EntityTriggerEnter(Entity origin, Collider other, ProjectileSpell projectileSpell,
            bool isSpell)
        {
            if (isSpell)
            {
                return;
            }

            Entity entityEnter = other.GetComponent<EntityPrefab>().entity;

            if ((origin.typeEntity == TypeEntity.MOB && entityEnter.typeEntity == TypeEntity.ALLIES) ||
                (origin.typeEntity == TypeEntity.ALLIES && entityEnter.typeEntity == TypeEntity.MOB))
            {
                float extraDamage = 0;
                if (projectileSpell.damageMultiplierOnDistance != 0)
                {
                    extraDamage = Vector3.Distance(projectileSpell.startPosition,
                                      projectileSpell.objectPooled.transform.position) *
                                  projectileSpell.damageMultiplierOnDistance;
                }

                if (projectileSpell.isBasicAttack)
                {
                    BasicAttack(origin, entityEnter, extraDamage, projectileSpell);
                }
                else
                {
                    AbilityParameters paramaters = new AbilityParameters {origin = origin};
                    entityEnter.TakeDamage(projectileSpell.damages + extraDamage, paramaters);
                }

                if (projectileSpell.effectsOnHit != null)
                {
                    foreach (Effect effect in projectileSpell.effectsOnHit)
                    {
                        EffectController.ApplyEffect(entityEnter, effect);
                    }
                }

                if (!projectileSpell.passingThroughEntity)
                {
                    EndArea(origin, projectileSpell);
                }
            }
        }

        public static void EndArea(Entity entity, ProjectileSpell projectileSpell)
        {
            if (projectileSpell.linkedSpellOnDisable != null)
            {
                SpellController.CastSpellComponent(entity, projectileSpell.linkedSpellOnDisable,
                    projectileSpell.objectPooled.transform.position, entity);
            }

            projectileSpell.objectPooled.SetActive(false);
            projectileSpell.prefabPooled.SetActive(false);

            if (projectileSpell.currentCoroutine != null)
            {
                SpellController.instance.StopCoroutine(projectileSpell.currentCoroutine);
            }
        }
    }
}