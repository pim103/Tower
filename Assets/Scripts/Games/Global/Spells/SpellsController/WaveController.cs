﻿using System.Collections;
using Games.Global.Weapons;
using UnityEngine;

namespace Games.Global.Spells.SpellsController
{
    public class WaveController : MonoBehaviour, ISpellController
    {
        private WaveSpell Clone(SpellComponent spellComponent)
        {
            WaveSpell origin = (WaveSpell) spellComponent;
            WaveSpell clone = new WaveSpell
            {
                damages = origin.damages,
                duration = origin.duration,
                damageType = origin.damageType,
                geometryPropagation = origin.geometryPropagation,
                initialRotation = origin.initialRotation,
                initialWidth = origin.initialWidth,
                speedPropagation = origin.speedPropagation,
                startPosition = origin.startPosition,
                typeSpell = origin.typeSpell,
                effectsOnHit = origin.effectsOnHit,
                isBasicAttack = origin.isBasicAttack,
                incrementAmplitudeByTime = origin.incrementAmplitudeByTime,
                OriginalDirection = origin.OriginalDirection,
                OriginalPosition = origin.OriginalPosition,
                trajectoryNormalized = origin.trajectoryNormalized,
            };

            return clone;
        }
        
        public void LaunchSpell(Entity entity, SpellComponent spellComponent)
        {
            WaveSpell waveSpell = Clone(spellComponent);
            Coroutine currentCoroutine = SpellController.instance.StartCoroutine(PlayWaveSpell(entity, waveSpell));
            waveSpell.currentCoroutine = currentCoroutine;
        }

        private IEnumerator PlayWaveSpell(Entity entity, WaveSpell waveSpell)
        {
            PoolAreaWithParameter(entity, waveSpell);

            float duration = waveSpell.duration;
            while (duration > 0)
            {
                yield return new WaitForSeconds(0.05f);
                duration -= 0.05f;

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
            float amplitudePropagation = waveSpell.incrementAmplitudeByTime * 0.05f;

            if (waveSpell.geometryPropagation == Geometry.Square)
            {
                float speed = waveSpell.speedPropagation * 0.05f;
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
            
            if ( (origin.typeEntity == TypeEntity.MOB && entityEnter.typeEntity == TypeEntity.ALLIES ) ||
                 (origin.typeEntity == TypeEntity.ALLIES && entityEnter.typeEntity == TypeEntity.MOB ))
            {
                entityEnter.ApplyDamage(waveSpell.damages);

                if (waveSpell.effectsOnHit != null)
                {
                    foreach (Effect effect in waveSpell.effectsOnHit)
                    {
                        EffectController.ApplyEffect(entityEnter, effect, origin, waveSpell.objectPooled.transform.position);
                    }
                }
            }
        }
        
        private static void EndArea(Entity entity, WaveSpell waveSpell)
        {
            waveSpell.objectPooled.transform.localScale = Vector3.one;
            waveSpell.objectPooled.SetActive(false);
            
            if (waveSpell.currentCoroutine != null)
            {
                SpellController.instance.StopCoroutine(waveSpell.currentCoroutine);
            }
        }
    }
}