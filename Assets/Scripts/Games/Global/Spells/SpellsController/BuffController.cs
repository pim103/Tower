using System;
using System.Collections;
using System.Collections.Generic;
using Games.Global.Spells.SpellParameter;
using UnityEngine;
using UnityEngine.UI;

namespace Games.Global.Spells.SpellsController
{
    public class BuffController : MonoBehaviour, ISpellController
    {
        private BuffSpell Clone(SpellComponent spellComponent)
        {
            BuffSpell origin = (BuffSpell) spellComponent;
            BuffSpell clone = new BuffSpell
            {
                duration = origin.duration,
                interval = origin.interval,
                stack = origin.stack,
                damageType = origin.damageType,
                replaceProjectile = origin.replaceProjectile,
                typeSpell = origin.typeSpell,
                conditionReduceCharge = origin.conditionReduceCharge,
                damageOnSelf = origin.damageOnSelf,
                effectOnSelf = origin.effectOnSelf,
                isBasicAttack = origin.isBasicAttack,
                newPlayerBehaviour = origin.newPlayerBehaviour,
                spellWithCondition = origin.spellWithCondition,
                disapearOnDamageReceived = origin.disapearOnDamageReceived,
                linkedSpellOnAttack = origin.linkedSpellOnAttack,
                linkedSpellOnHit = origin.linkedSpellOnHit,
                linkedSpellOnInterval = origin.linkedSpellOnInterval,
                effectOnTargetOnHit = origin.effectOnTargetOnHit,
                linkedSpellOnDamageReceived = origin.linkedSpellOnDamageReceived,
                needNewPositionOnAttack = origin.needNewPositionOnAttack,
                needNewPositionOnHit = origin.needNewPositionOnHit,
                needNewPositionOnInterval = origin.needNewPositionOnInterval,
                effectOnSelfOnDamageReceived = origin.effectOnSelfOnDamageReceived,
                effectOnSelfWhenNoCharge = origin.effectOnSelfWhenNoCharge,
                needNewPositionOnDamageReceived = origin.needNewPositionOnDamageReceived,
                OriginalDirection = origin.OriginalDirection,
                OriginalPosition = origin.OriginalPosition,
                startPosition = origin.startPosition,
                initialRotation = origin.initialRotation,
                trajectoryNormalized = origin.trajectoryNormalized,
            };

            return clone;
        }

        public void LaunchSpell(Entity entity, SpellComponent spellComponent)
        {
            BuffSpell buffSpell = Clone(spellComponent);
            Coroutine currentCoroutine = SpellController.instance.StartCoroutine(PlayBuffSpell(entity, buffSpell));
            buffSpell.currentCoroutine = currentCoroutine;
        }

        private IEnumerator PlayBuffSpell(Entity entity, BuffSpell buffSpell)
        {
            InitialBuff(entity, buffSpell);
            float duration = buffSpell.duration;

            while (duration > 0)
            {
                yield return new WaitForSeconds(buffSpell.interval);
                duration -= buffSpell.interval;

                if (buffSpell.linkedSpellOnInterval != null)
                {
                    SpellController.CastSpellComponent(entity, buffSpell.linkedSpellOnInterval, entity.entityPrefab.transform.position, entity);
                }

                CheckSpellWithCondition(entity, buffSpell.spellWithCondition);
            }

            while (buffSpell.stack > 0)
            {
                yield return null;
            }

            if (buffSpell.stack == 0)
            {
                EffectController.ApplyEffect(entity, buffSpell.effectOnSelfWhenNoCharge);
            }

            EndBuff(entity, buffSpell);
        }

        private void CheckSpellWithCondition(Entity entity, List<SpellWithCondition> spellWithConditions)
        {
            if (spellWithConditions == null)
            {
                return;
            }

            List<SpellWithCondition> minEnemyInArea = spellWithConditions.FindAll(spell => spell.conditionType == ConditionType.MinEnemiesInArea);

            foreach (SpellWithCondition spellWithCondition in minEnemyInArea)
            {
                if (spellWithCondition.conditionType == ConditionType.MinEnemiesInArea)
                {
                    if (entity.entityInRange.Count >= spellWithCondition.level)
                    {
                        switch (spellWithCondition.instructionTargeting)
                        {
                            case InstructionTargeting.ApplyOnSelf:
                                EffectController.ApplyEffect(entity, spellWithCondition.effect);
                                SpellController.CastSpellComponent(entity, spellWithCondition.spellComponent, entity.entityPrefab.transform.position, entity);
                                break;
                            case InstructionTargeting.ApplyOnTarget:
                                break;
                            case InstructionTargeting.DeleteOnSelf:
                                break;
                            case InstructionTargeting.DeleteOnTarget:
                                break;
                        }
                    }
                }
            }
        }

        private void InitialBuff(Entity entity, BuffSpell buffSpell)
        {
            entity.currentBuff.Add(buffSpell);

            if (buffSpell.effectOnSelf != null)
            {
                foreach (Effect effect in buffSpell.effectOnSelf)
                {
                    EffectController.ApplyEffect(entity, effect);
                }
            }

            if (buffSpell.effectOnSelfOnDamageReceived != null)
            {
                foreach (Effect effect in buffSpell.effectOnSelfOnDamageReceived)
                {
                    entity.damageReceiveExtraEffect.Add(effect);
                }
            }

            if (buffSpell.effectOnTargetOnHit != null)
            {
                foreach (Effect effect in buffSpell.effectOnTargetOnHit)
                {
                    entity.damageDealExtraEffect.Add(effect);
                }
            }

            List<SpellWithCondition> ifPlayerDies = null;
            List<SpellWithCondition> ifPlayerDoesntDie = null;

            if (buffSpell.spellWithCondition != null)
            {
                ifPlayerDies = buffSpell.spellWithCondition.FindAll(condition => condition.conditionType == ConditionType.PlayerDies);
                
                ifPlayerDoesntDie = buffSpell.spellWithCondition.FindAll(condition => condition.conditionType == ConditionType.PlayerDoesntDie);
            }

            if ((entity.hp - buffSpell.damageOnSelf) <= 0 && ifPlayerDies != null)
            {
                foreach (SpellWithCondition spellCondition in ifPlayerDies)
                {
                    EffectController.ApplyEffect(entity, spellCondition.effect);
                    SpellController.CastSpellComponent(entity, spellCondition.spellComponent, entity.entityPrefab.transform.position, entity);
                }
            }
            else if (ifPlayerDoesntDie != null)
            {
                foreach (SpellWithCondition spellCondition in ifPlayerDoesntDie)
                {
                    EffectController.ApplyEffect(entity, spellCondition.effect);
                    SpellController.CastSpellComponent(entity, spellCondition.spellComponent, entity.entityPrefab.transform.position, entity);
                }
            }

            entity.ApplyDamage(buffSpell.damageOnSelf);
        }

        public static void EndBuff(Entity entity, BuffSpell buffSpell)
        {
            entity.currentBuff.Remove(buffSpell);

            if (buffSpell.disapearOnDamageReceived && buffSpell.effectOnSelf != null)
            {
                foreach (Effect effect in buffSpell.effectOnSelf)
                {
                    EffectController.StopCurrentEffect(entity, effect);
                }
            }

            if (buffSpell.effectOnSelfOnDamageReceived != null)
            {
                foreach (Effect effect in buffSpell.effectOnSelfOnDamageReceived)
                {
                    entity.damageReceiveExtraEffect.Remove(effect);
                }
            }

            if (buffSpell.effectOnTargetOnHit != null)
            {
                foreach (Effect effect in buffSpell.effectOnTargetOnHit)
                {
                    entity.damageDealExtraEffect.Remove(effect);
                }
            }

            if (buffSpell.currentCoroutine != null)
            {
                SpellController.instance.StopCoroutine(buffSpell.currentCoroutine);
            }
        }

        public static void EntityDealDamage(Entity entity, Entity entityTouch)
        {
            foreach (BuffSpell buffSpell in entity.currentBuff)
            {
                if (buffSpell.linkedSpellOnHit != null)
                {
                    Vector3 position = Vector3.positiveInfinity;
                    if (buffSpell.needNewPositionOnHit)
                    {
                        position = entityTouch.entityPrefab.transform.position;
                    }
                    
                    SpellController.CastSpellComponent(entity, buffSpell.linkedSpellOnHit, position, entity);
                }
            }
        }

        public static void EntityReceivedDamage(Entity entity, Entity entityOriginOfDamage)
        {
            foreach (BuffSpell buffSpell in entity.currentBuff)
            {
                if (buffSpell.linkedSpellOnDamageReceived != null)
                {
                    Vector3 position = Vector3.positiveInfinity;
                    if (buffSpell.needNewPositionOnDamageReceived)
                    {
                        position = entityOriginOfDamage.entityPrefab.transform.position;
                    }
                    
                    SpellController.CastSpellComponent(entity, buffSpell.linkedSpellOnDamageReceived, position, entity);
                }

                if (buffSpell.conditionReduceCharge == ConditionReduceCharge.OnDamageReceived)
                {
                    buffSpell.stack--;
                }

                if (buffSpell.disapearOnDamageReceived)
                {
                    EndBuff(entity, buffSpell);
                }
            }
        }

        public static void EntityAttack(Entity entity, Vector3 positionPointed)
        {
            foreach (BuffSpell buffSpell in entity.currentBuff)
            {
                if (buffSpell.linkedSpellOnAttack != null)
                {
                    Vector3 position = entity.entityPrefab.transform.position;
                    if (buffSpell.needNewPositionOnAttack)
                    {
                        position = positionPointed;
                    }

                    SpellController.CastSpellComponent(entity, buffSpell.linkedSpellOnAttack, position, entity);
                }

                if (buffSpell.conditionReduceCharge == ConditionReduceCharge.OnAttack)
                {
                    buffSpell.stack--;
                }
            }
        }
    }
}