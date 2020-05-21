﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Games.Global.Entities;
using UnityEngine;
using Utils;

namespace Games.Global
{
    public class EffectController : MonoBehaviour
    {
        public static EffectController EffectControllerInstance;
        public static readonly TypeEffect[] ControlEffect = {TypeEffect.Freezing, TypeEffect.Stun, TypeEffect.Sleep, TypeEffect.Immobilization, TypeEffect.Slow, TypeEffect.Expulsion, 
            TypeEffect.Confusion, TypeEffect.Fear, TypeEffect.Charm };

        public static readonly TypeEffect[] BuffEffect =
        {
            TypeEffect.Heal, TypeEffect.Regen, TypeEffect.Resurrection, TypeEffect.AttackUp, TypeEffect.AttackSpeedUp, TypeEffect.DefenseUp, TypeEffect.DivineShield, TypeEffect.AntiSpell, TypeEffect.Will, TypeEffect.Thorn, TypeEffect.Intangible,
            TypeEffect.Mirror, TypeEffect.SpeedUp, TypeEffect.MagicalDefUp, TypeEffect.PhysicalDefUp, TypeEffect.Purification, TypeEffect.ResourceFill, TypeEffect.Link
        };

        private void Start()
        {
            EffectControllerInstance = this;
        }

        private static Effect CloneEffect(Effect origin)
        {
            Effect clone = new Effect
            {
                launcher = origin.launcher,
                level = origin.level,
                directionExpul = origin.directionExpul,
                originExpulsion = origin.originExpulsion,
                ressourceCost = origin.ressourceCost,
                typeEffect = origin.typeEffect,
                durationInSeconds = origin.durationInSeconds,
                positionSrcDamage = origin.positionSrcDamage
            };

            return clone;
        }
        
        public static void ApplyEffect(Entity entity, Effect effect, List<Entity> affectedEntity = null)
        {
            if (entity.hasWill && ControlEffect.Contains(effect.typeEffect))
            {
                return;
            }

            if (entity.isLinked && !BuffEffect.Contains(effect.typeEffect))
            {
                if (affectedEntity == null)
                {
                    affectedEntity = new List<Entity>();
                }

                affectedEntity.Add(entity);
                foreach (Entity entityInRange in entity.entityInRange)
                {
                    if (!affectedEntity.Contains(entityInRange) && entityInRange.isLinked)
                    {
                        ApplyEffect(entityInRange, effect, affectedEntity);
                    }
                }
            }

            if (entity.underEffects.ContainsKey(effect.typeEffect))
            {
                Effect effectInList = entity.underEffects[effect.typeEffect];
                effectInList.UpdateEffect(entity, effect);

                // Théoriquement inutile depuis que effect est une class et plus une struct
//                entity.underEffects[effect.typeEffect] = effectInList;
                return;
            }

//            if (effect.durationInSeconds <= -0.9999)
//            {
//                entity.underEffects.Add(effect.typeEffect, effect);
//                return;
//            }

            StartCoroutineEffect(entity, effect);
        }

        public static void StartCoroutineEffect(Entity entity, Effect effect)
        {
            Effect cloneEffect = CloneEffect(effect);
            entity.underEffects.Add(effect.typeEffect, cloneEffect);
            Coroutine currentCoroutine = EffectControllerInstance.StartCoroutine(PlayEffectOnTime(entity, cloneEffect));

            effect.currentCoroutine = currentCoroutine;
            // Théoriquement inutile depuis que effect est une class et plus une struct
//            entity.underEffects[effect.typeEffect] = effect;
        }

        public static IEnumerator PlayEffectOnTime(Entity entity, Effect effect)
        {
            // Théoriquement inutile depuis que effect est une class et plus une struct
//            Effect effectInList = entity.underEffects[effect.typeEffect];

            effect.InitialTrigger(entity);

            yield return new WaitForSeconds(0.05f);

            if (effect.durationInSeconds == 0)
            {
                StopCurrentEffect(entity, effect);
                yield break;
            }
            
            while (effect.durationInSeconds > 0)
            {
                yield return new WaitForSeconds(0.1f);
                if (effect.launcher != null && effect.ressourceCost > 0)
                {
                    effect.launcher.ressource1 -= effect.ressourceCost;

                    if (effect.launcher.ressource1 <= 0)
                    {
                        StopCurrentEffect(entity, effect);
                        yield break;
                    }
                }

                effect.TriggerEffectAtTime(entity);

//                effectInList = entity.underEffects[effect.typeEffect];
                effect.durationInSeconds -= 0.1f;
//                entity.underEffects[effect.typeEffect] = effectInList;
            }

            StopCurrentEffect(entity, effect);
        }

        public static void StopCurrentEffect(Entity entity, Effect effect)
        {
            if (effect.currentCoroutine != null)
            {
                EffectControllerInstance.StopCoroutine(effect.currentCoroutine);
            }

            effect.EndEffect(entity);

            entity.underEffects.Remove(effect.typeEffect);
        }
    }
}
