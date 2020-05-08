using System;
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

                entity.underEffects[effect.typeEffect] = effectInList;
                return;
            }

            if (effect.durationInSeconds <= -1.00001)
            {
                entity.underEffects.Add(effect.typeEffect, effect);
                return;
            }

            StartCoroutineEffect(entity, effect);
        }

        public static void RemoveEffect(Entity entity, TypeEffect typeEffect)
        {
            if (entity.underEffects.ContainsKey(typeEffect))
            {
                StopCurrentEffect(entity, entity.underEffects[typeEffect]);
            }
        }

        public static void StartCoroutineEffect(Entity entity, Effect effect)
        {
            entity.underEffects.Add(effect.typeEffect, effect);
            Coroutine currentCoroutine = EffectControllerInstance.StartCoroutine(PlayEffectOnTime(entity, effect));

            effect.currentCoroutine = currentCoroutine;
        }

        public static IEnumerator PlayEffectOnTime(Entity entity, Effect effect)
        {
            Effect effectInList = entity.underEffects[effect.typeEffect];

            effectInList.InitialTrigger(entity);

            if (effectInList.durationInSeconds == 0)
            {
                StopCurrentEffect(entity, effectInList);
                yield break;
            }
            
            while (effectInList.durationInSeconds > 0)
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

                effectInList.TriggerEffectAtTime(entity);

                effectInList = entity.underEffects[effect.typeEffect];
                effectInList.durationInSeconds -= 0.1f;
                entity.underEffects[effect.typeEffect] = effectInList;
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

        public IEnumerator AddDamageDealExtraEffect(Entity entity, Effect effect, float duration)
        {
            if (entity.damageDealExtraEffect.ContainsKey(effect.typeEffect))
            {
                entity.damageDealExtraEffect[effect.typeEffect] = effect;
            }
            else
            {
                entity.damageDealExtraEffect.Add(effect.typeEffect, effect);
            }
            
            yield return new WaitForSeconds(duration);

            if (entity.damageDealExtraEffect.ContainsKey(effect.typeEffect))
            {
                entity.damageDealExtraEffect.Remove(effect.typeEffect);
            }
        }
        
        public IEnumerator AddDamageReceiveExtraEffect(Entity entity, Effect effect, float duration)
        {
            if (entity.damageReceiveExtraEffect.ContainsKey(effect.typeEffect))
            {
                entity.damageReceiveExtraEffect[effect.typeEffect] = effect;
            }
            else
            {
                entity.damageReceiveExtraEffect.Add(effect.typeEffect, effect);
            }

            yield return new WaitForSeconds(duration);

            if (entity.damageReceiveExtraEffect.ContainsKey(effect.typeEffect))
            {
                entity.damageReceiveExtraEffect.Remove(effect.typeEffect);
            }
        }
    }
}
