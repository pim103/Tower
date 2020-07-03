﻿using System;
using System.Collections;
using System.Collections.Generic;
using Games.Global.Spells.SpellParameter;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace Games.Global.Spells.SpellsController
{
    public class BuffController : MonoBehaviour, ISpellController
    {
        private SpellComponent originalBasicAttackComponent;
        
        public void LaunchSpell(Entity entity, SpellComponent spellComponent, SpellComponent origin = null)
        {
            BuffSpell buffSpell = Tools.Clone((BuffSpell) spellComponent);
            Coroutine currentCoroutine = SpellController.instance.StartCoroutine(PlayBuffSpell(entity, buffSpell));
            buffSpell.currentCoroutine = currentCoroutine;
        }

        private IEnumerator PlayBuffSpell(Entity entity, BuffSpell buffSpell)
        {
            InitialBuff(entity, buffSpell);
            float duration = buffSpell.duration;

            while (duration > 0)
            {
                if (!entity.hasPassiveDeactivate && buffSpell.castByPassive)
                {
                    yield return new WaitForSeconds(0.1f);
                    continue;
                }

                yield return new WaitForSeconds(buffSpell.interval);
                duration -= buffSpell.interval;

                if (buffSpell.linkedSpellOnInterval != null)
                {
                    SpellController.CastSpellComponent(entity, buffSpell.linkedSpellOnInterval, entity.entityPrefab.transform.position, entity, buffSpell);
                }

                CheckSpellWithCondition(entity, buffSpell.spellWithCondition);
            }

            while (buffSpell.stack > 0)
            {
                yield return null;
            }

            if (buffSpell.stack == 0)
            {
                EffectController.ApplyEffect(entity, buffSpell.effectOnSelfWhenNoCharge, entity, entity.entityPrefab.transform.position);
            }

            EndBuff(entity, buffSpell, originalBasicAttackComponent);
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
                                EffectController.ApplyEffect(entity, spellWithCondition.effect, entity, entity.entityPrefab.transform.position);
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

            if (buffSpell.replaceProjectile != null)
            {
                originalBasicAttackComponent = entity.basicAttack.activeSpellComponent;
                entity.basicAttack.activeSpellComponent = buffSpell.replaceProjectile;
                
                Debug.Log("Replace");
                Debug.Log(buffSpell.replaceProjectile.nameSpellComponent);
            }

            if (buffSpell.effectOnSelf != null)
            {
                foreach (Effect effect in buffSpell.effectOnSelf)
                {
                    EffectController.ApplyEffect(entity, effect, entity, entity.entityPrefab.transform.position);
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
                    EffectController.ApplyEffect(entity, spellCondition.effect, entity, entity.entityPrefab.transform.position);
                    SpellController.CastSpellComponent(entity, spellCondition.spellComponent, entity.entityPrefab.transform.position, entity);
                }
            }
            else if (ifPlayerDoesntDie != null)
            {
                foreach (SpellWithCondition spellCondition in ifPlayerDoesntDie)
                {
                    EffectController.ApplyEffect(entity, spellCondition.effect, entity, entity.entityPrefab.transform.position);
                    SpellController.CastSpellComponent(entity, spellCondition.spellComponent, entity.entityPrefab.transform.position, entity);
                }
            }

            entity.ApplyDamage(buffSpell.damageOnSelf);

            if (buffSpell.triggerInvocationCallOneTime)
            {
                foreach (Entity summon in DataObject.invocationsInScene)
                {
                    GenericSummonSpell genericSummonSpell = (GenericSummonSpell) summon.entityPrefab;
                    if (genericSummonSpell.summoner == entity)
                    {
                        genericSummonSpell.TriggerSpellWhenPlayerCall();
                    }
                }
            }
        }

        public static void EndBuff(Entity entity, BuffSpell buffSpell, SpellComponent originalBasicAttackComponent = null)
        {
            entity.currentBuff.Remove(buffSpell);

            if (buffSpell.replaceProjectile != null)
            {
                entity.basicAttack.activeSpellComponent = originalBasicAttackComponent;
            }

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
                    
                    SpellController.CastSpellComponent(entity, buffSpell.linkedSpellOnHit, position, entity, buffSpell);
                }
            }
        }

        public static void EntityReceivedDamage(Entity entity, Entity entityOriginOfDamage)
        {
            List<BuffSpell> entityCurrentBuff = new List<BuffSpell>(entity.currentBuff);
            foreach (BuffSpell buffSpell in entityCurrentBuff)
            {
                if (buffSpell.linkedSpellOnDamageReceived != null)
                {
                    Vector3 position = Vector3.positiveInfinity;
                    if (buffSpell.needNewPositionOnDamageReceived)
                    {
                        position = entityOriginOfDamage.entityPrefab.transform.position;
                    }

                    SpellController.CastSpellComponent(entity, buffSpell.linkedSpellOnDamageReceived, position, entity, buffSpell);
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

                    SpellController.CastSpellComponent(entity, buffSpell.linkedSpellOnAttack, position, entity, buffSpell);
                }

                if (buffSpell.conditionReduceCharge == ConditionReduceCharge.OnAttack)
                {
                    buffSpell.stack--;
                }
            }
        }
    }
}