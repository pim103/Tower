using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Games.Global.Entities;
using Games.Global.Spells;
using Games.Players;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace Games.Global
{
    public class EffectController : MonoBehaviour
    {
        [SerializeField] private List<Sprite> spritesEffect;

        public static EffectController EffectControllerInstance;

        public static readonly TypeEffect[] MovementControlEffect =
        {
            TypeEffect.Freezing, TypeEffect.Immobilization, TypeEffect.Slow
        };
        
        public static readonly TypeEffect[] IncapacitateEffect =
        {
            TypeEffect.Charm, TypeEffect.Confusion, TypeEffect.Expulsion,
            TypeEffect.Fear, TypeEffect.Sleep, TypeEffect.Stun
        };

        public static readonly TypeEffect[] BuffEffect =
        {
            TypeEffect.Heal, TypeEffect.Regen, TypeEffect.Resurrection, TypeEffect.AttackUp, TypeEffect.AttackSpeedUp,
            TypeEffect.DefenseUp, TypeEffect.DivineShield, TypeEffect.AntiSpell, TypeEffect.Will, TypeEffect.Thorn,
            TypeEffect.Intangible, TypeEffect.Mirror, TypeEffect.SpeedUp, TypeEffect.MagicalDefUp, TypeEffect.PhysicalDefUp,
            TypeEffect.Purification, TypeEffect.ResourceFill, TypeEffect.Link
        };

        public static readonly TypeEffect[] DebuffEffect =
        {
             TypeEffect.Bleed, TypeEffect.Blind, TypeEffect.BrokenDef, TypeEffect.Burn,
             TypeEffect.Poison, TypeEffect.Silence
        };

        private void Start()
        {
            EffectControllerInstance = this;
        }

        private static void AddEffectSprite(Effect effect)
        {
            Sprite buffSprite = EffectControllerInstance.spritesEffect.Find(sprite => sprite.name == effect.typeEffect.ToString());
            PlayerPrefab playerPrefab = DataObject.playerInScene[GameController.PlayerIndex];
            int indexBuff = playerPrefab.entity.GetNbUnderEffect();

            if (buffSprite != null)
            {
                // 20 images of sprite
                if (indexBuff < 20)
                {
                    playerPrefab.buffCases[indexBuff].sprite = buffSprite;
                    Color color = playerPrefab.buffCases[indexBuff].color;
                    color.a = 1;
                    playerPrefab.buffCases[indexBuff].color = color;
                }
            }
        }

        private static void RemoveEffectSprite(Effect effect)
        {
            PlayerPrefab playerPrefab = DataObject.playerInScene[GameController.PlayerIndex];
            Image buffImage = playerPrefab.buffCases.Find(image => image.sprite != null && image.sprite.name == effect.typeEffect.ToString());

            if (buffImage != null)
            {
                buffImage.sprite = null;
                Color color = buffImage.color;
                color.a = 0;
                buffImage.color = color;
            }
        }

        public static void DeleteEffectFromTargetsFound(Effect effect, TargetsFound targetsFound)
        {
            if (targetsFound.targets.Count > 0)
            {
                targetsFound.targets.ForEach(target =>
                {
                    if (target.EntityIsUnderEffect(effect.typeEffect))
                    {
                        StopCurrentEffect(target, target.TryGetEffectInUnderEffect(effect.typeEffect));
                    }
                });
            } 
            else if (targetsFound.target != null)
            {
                if (targetsFound.target.EntityIsUnderEffect(effect.typeEffect))
                {
                    StopCurrentEffect(targetsFound.target, targetsFound.target.TryGetEffectInUnderEffect(effect.typeEffect));
                }
            }
        }

        public static void ApplyEffectFromTargetsFound(Entity origin, Effect effect, TargetsFound targetsFound)
        {
// TODO : IMPLEMENT CORRECT SRC OF DAMAGES
            if (targetsFound.targets.Count > 0)
            {
                targetsFound.targets.ForEach(target => ApplyEffect(target, effect, origin, origin.entityPrefab.transform.position));
            } 
            else if (targetsFound.target != null)
            {
                ApplyEffect(targetsFound.target, effect, origin, origin.entityPrefab.transform.position);
            }
        }

        public static void ApplyEffect(Entity entityAffected, Effect effect, Entity origin, Vector3 srcDamage, List<Entity> affectedEntity = null)
        {   
            if (effect == null || entityAffected.hasWill && MovementControlEffect.Contains(effect.typeEffect))
            {
                return;
            }

            if (entityAffected.isLinked && !BuffEffect.Contains(effect.typeEffect))
            {
                if (affectedEntity == null)
                {
                    affectedEntity = new List<Entity>();
                }

                affectedEntity.Add(entityAffected);
                foreach (Entity entityInRange in entityAffected.entityInRange)
                {
                    if (!affectedEntity.Contains(entityInRange) && entityInRange.isLinked)
                    {
                        ApplyEffect(entityInRange, effect, origin, srcDamage, affectedEntity);
                    }
                }
            }

            if (entityAffected.EntityIsUnderEffect(effect.typeEffect))
            {
                Effect effectInList = entityAffected.TryGetEffectInUnderEffect(effect.typeEffect);
                effectInList.UpdateEffect(entityAffected, effect);

                return;
            }

            StartCoroutineEffect(entityAffected, effect, origin, srcDamage);
        }

        public static void StartCoroutineEffect(Entity entity, Effect effect, Entity origin, Vector3 srcDamage)
        {
            Effect cloneEffect = Tools.Clone(effect);
            cloneEffect.launcher = origin;
            cloneEffect.positionSrcDamage = srcDamage;
            entity.AddEffectInUnderEffect(cloneEffect);
            Coroutine currentCoroutine = EffectControllerInstance.StartCoroutine(PlayEffectOnTime(entity, cloneEffect));

            cloneEffect.currentCoroutine = currentCoroutine;

            if (entity.isPlayer)
            {
                AddEffectSprite(effect);
            }
        }

        public static IEnumerator PlayEffectOnTime(Entity entity, Effect effect)
        {
// Théoriquement inutile depuis que effect est une class et plus une struct
//            Effect effectInList = entity.underEffects[effect.typeEffect];
            Debug.Log("Start couroutine for " + entity.entityPrefab.name + " with type effect : " + effect.typeEffect);

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
                    effect.launcher.ressource -= effect.ressourceCost;

                    if (effect.launcher.ressource <= 0)
                    {
                        StopCurrentEffect(entity, effect);
                        yield break;
                    }
                }

                effect.TriggerEffectAtTime(entity);

                effect.durationInSeconds -= 0.1f;
            }

            StopCurrentEffect(entity, effect);
        }

        public static void StopCurrentEffect(Entity entity, Effect effect)
        {
            Debug.Log("Stop couroutine for " + entity.entityPrefab.name + " for type effect : " + effect.typeEffect);
            if (effect.currentCoroutine != null)
            {
                EffectControllerInstance.StopCoroutine(effect.currentCoroutine);
            }

            effect.EndEffect(entity);

            if (entity.isPlayer)
            {
                RemoveEffectSprite(effect);
            }
            entity.RemoveUnderEffect(effect.typeEffect);
        }
    }
}