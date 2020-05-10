using System.Collections;
using Games.Global.Weapons;
using UnityEngine;

namespace Games.Global.Spells.SpellsController
{
    public class ProjectileController : MonoBehaviour, ISpellController
    {
        public void LaunchSpell(Entity entity, SpellComponent spellComponent)
        {
            Coroutine currentCoroutine = SpellController.instance.StartCoroutine(PlayProjectileSpell(entity, spellComponent));
            spellComponent.currentCoroutine = currentCoroutine;
        }

        private IEnumerator PlayProjectileSpell(Entity entity, SpellComponent spellComponent)
        {
            ProjectileSpell projectileSpell = (ProjectileSpell) spellComponent;
            
            PoolAreaWithParameter(entity, projectileSpell);

            if (projectileSpell.linkedSpellOnEnable != null)
            {
                SpellController.CastSpellComponent(entity, projectileSpell.linkedSpellOnEnable, projectileSpell.startPosition);
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

            genericSpellPrefab.transform.position = projectileSpell.startPosition;
            genericSpellPrefab.transform.localEulerAngles = projectileSpell.initialRotation;

            genericSpellPrefab.transform.localScale = Vector3.one;
            GameObject prefabWanted = ObjectPooler.SharedInstance.GetPooledObject(projectileSpell.idPoolObject);
            prefabWanted.transform.parent = genericSpellPrefab.transform;

            SpellPrefabController spellPrefabController = genericSpellPrefab.GetComponent<SpellPrefabController>();
            spellPrefabController.SetValues(entity, projectileSpell);

            projectileSpell.objectPooled = genericSpellPrefab;

            genericSpellPrefab.SetActive(true);
        }

        private void UpdatePosition(Entity entity, ProjectileSpell projectileSpell)
        {
            float speed = projectileSpell.speed * 0.05f;
            projectileSpell.objectPooled.transform.position += (projectileSpell.trajectory * speed);
        }
        
        public static void EntityTriggerEnter(Entity origin, Collider other, ProjectileSpell projectileSpell, bool isSpell)
        {
            if (isSpell)
            {
                return;
            }

            Entity entityEnter = other.GetComponent<EntityPrefab>().entity;
            
            if ( (origin.typeEntity == TypeEntity.MOB && entityEnter.typeEntity == TypeEntity.PLAYER ) ||
                 (origin.typeEntity == TypeEntity.PLAYER && entityEnter.typeEntity == TypeEntity.MOB ))
            {
                float extraDamage = 0;
                if (projectileSpell.damageMultiplierOnDistance != 0)
                {
                    extraDamage = Vector3.Distance(projectileSpell.startPosition,
                        projectileSpell.objectPooled.transform.position) * projectileSpell.damageMultiplierOnDistance;
                }
                
                entityEnter.ApplyDamage(projectileSpell.damages + extraDamage);

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
                SpellController.CastSpellComponent(entity, projectileSpell.linkedSpellOnDisable, projectileSpell.objectPooled.transform.position);
            }

            projectileSpell.objectPooled.SetActive(false);

            if (projectileSpell.currentCoroutine != null)
            {
                SpellController.instance.StopCoroutine(projectileSpell.currentCoroutine);
            }
        }
    }
}
