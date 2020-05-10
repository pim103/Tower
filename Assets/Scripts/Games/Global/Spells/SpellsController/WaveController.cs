using System.Collections;
using Games.Global.Weapons;
using UnityEngine;

namespace Games.Global.Spells.SpellsController
{
    public class WaveController : MonoBehaviour, ISpellController
    {
        public void LaunchSpell(Entity entity, SpellComponent spellComponent)
        {
            Coroutine currentCoroutine = SpellController.instance.StartCoroutine(PlayWaveSpell(entity, spellComponent));
            spellComponent.currentCoroutine = currentCoroutine;
        }

        private IEnumerator PlayWaveSpell(Entity entity, SpellComponent spellComponent)
        {
            WaveSpell waveSpell = (WaveSpell) spellComponent;

            PoolAreaWithParameter(entity, waveSpell);

            float duration = waveSpell.duration;
            while (duration > 0)
            {
                yield return new WaitForSeconds(0.1f);
                duration -= 0.1f;

                WavePropagation(entity, waveSpell);
            }

            EndArea(entity, waveSpell);
        }
        
        private void PoolAreaWithParameter(Entity entity, WaveSpell waveSpell)
        {
            GameObject genericSpellPrefab = ObjectPooler.SharedInstance.GetPooledObject(1);

            genericSpellPrefab.transform.position = waveSpell.startPosition;
            Vector3 scale = genericSpellPrefab.transform.localScale;
            scale.x = waveSpell.initialWidth;
            scale.y = waveSpell.initialWidth;

            if (waveSpell.geometryPropagation == Geometry.Sphere)
            {
                scale.z = waveSpell.initialWidth;
            }

            genericSpellPrefab.transform.localScale = scale;

            if (waveSpell.geometryPropagation == Geometry.Square)
            {
                genericSpellPrefab.transform.localEulerAngles = waveSpell.initialRotation;
            }

            SpellPrefabController spellPrefabController = genericSpellPrefab.GetComponent<SpellPrefabController>();
            spellPrefabController.ActiveCollider(waveSpell.geometryPropagation);
            spellPrefabController.SetValues(entity, waveSpell);

            waveSpell.objectPooled = genericSpellPrefab;

            genericSpellPrefab.SetActive(true);
        }

        private void WavePropagation(Entity entity, WaveSpell waveSpell)
        {
            float amplitudePropagation = waveSpell.incrementAmplitudeByTime * 0.1f;

            if (waveSpell.geometryPropagation == Geometry.Square)
            {
                float speed = waveSpell.speedPropagation * 0.1f;
                waveSpell.objectPooled.transform.position += (waveSpell.objectPooled.transform.forward * speed);

                waveSpell.objectPooled.transform.localScale += (Vector3.right * amplitudePropagation);
            }
            else
            {
                waveSpell.objectPooled.transform.localScale += (Vector3.one * amplitudePropagation);
            }
        }

        public static void EntityTriggerEnter(Entity origin, Collider other, WaveSpell waveSpell, bool isSpell)
        {
            if (isSpell)
            {
                return;
            }

            Entity entityEnter = other.GetComponent<EntityPrefab>().entity;
            
            if ( (origin.typeEntity == TypeEntity.MOB && entityEnter.typeEntity == TypeEntity.PLAYER ) ||
                 (origin.typeEntity == TypeEntity.PLAYER && entityEnter.typeEntity == TypeEntity.MOB ))
            {
                entityEnter.ApplyDamage(waveSpell.damages);

                if (waveSpell.effectsOnHit != null)
                {
                    foreach (Effect effect in waveSpell.effectsOnHit)
                    {
                        EffectController.ApplyEffect(entityEnter, effect);
                    }
                }
            }
        }
        
        private static void EndArea(Entity entity, WaveSpell waveSpell)
        {
            waveSpell.objectPooled.SetActive(false);
            
            if (waveSpell.currentCoroutine != null)
            {
                SpellController.instance.StopCoroutine(waveSpell.currentCoroutine);
            }
        }
    }
}
