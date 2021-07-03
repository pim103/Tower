using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Games.Global.Spells.SpellBehavior;
using UnityEngine;
using Utils;
using Random = UnityEngine.Random;

namespace Games.Global.Spells.SpellsController
{
    public class SpellInterpreter : MonoBehaviour
    {
        public static SpellInterpreter instance;
        
        public void StartSpellTreatment(SpellComponent spellComponent, Vector3 startPosition)
        {
            spellComponent.startAtPosition = startPosition;
            InstantiateSpell.InstantiateNewSpell(spellComponent, startPosition);
            
            spellComponent.currentCoroutine = StartCoroutine(IntervallSpell(spellComponent));
        }
        
        private void AtTheStartSpellBehavior(SpellComponent spellComponent)
        {
            spellComponent.AtTheStart();
            PlaySpellActions(spellComponent, Trigger.START);
        }

        private static void AtTheEndSpellBehavior(SpellComponent spellComponent)
        {
            spellComponent.AtTheEnd();
            PlaySpellActions(spellComponent, Trigger.END);
        }

        private void DuringIntervalSpellBehavior(SpellComponent spellComponent)
        {
            spellComponent.DuringInterval();
            PlaySpellActions(spellComponent, Trigger.INTERVAL);
        }

        public static void TriggerWhenEntityAttack(List<SpellComponent> spellComponents)
        {
            spellComponents.ForEach(spellComponent =>
            {
                PlaySpellActions(spellComponent, Trigger.ON_ATTACK);
                spellComponent.OnAttack();

                if (spellComponent.conditionReduceCharge == ConditionReduceCharge.OnAttack)
                {
                    spellComponent.spellCharges--;
                }
            });
        }

        public static void TriggerWhenEntityReceivedDamage(List<SpellComponent> spellComponents)
        {
            spellComponents.ForEach(spellComponent =>
            {
                PlaySpellActions(spellComponent, Trigger.ON_DAMAGE_RECEIVED);
                spellComponent.OnDamageReceive();

                if (spellComponent.stopSpellComponentAtDamageReceived)
                {
                    EndSpellComponent(spellComponent);
                }

                if (spellComponent.conditionReduceCharge == ConditionReduceCharge.OnDamageReceived)
                {
                    spellComponent.spellCharges--;
                }
            });
        }

        public static void TriggerWhenEntityDie(SpellComponent spellComponent)
        {
            PlaySpellActions(spellComponent, Trigger.ON_ENTITY_DIE);
        }

        private static bool CheckActionConditionsAndValidTarget(Entity caster, ActionTriggered action, TargetsFound targetsFound)
        {
            if (action.percentageToTrigger != 100)
            {
                int triggerAction = Random.Range(0, 100);
                if (triggerAction < action.percentageToTrigger)
                {
                    return false;
                }
            }

            if (action.conditionToTrigger != null)
            {
                if (targetsFound.target != null)
                {
                    return action.conditionToTrigger.TestCondition(caster, targetsFound.target);
                }

                if (targetsFound.targets != null)
                {
                    List<Entity> entities = targetsFound.targets;
                    entities.ForEach(target =>
                    {
                        if (!action.conditionToTrigger.TestCondition(caster, target))
                        {
                            targetsFound.targets.Remove(target);
                        }
                    });

                    if (targetsFound.targets.Count == 0)
                    {
                        return false;
                    }
                }
            }
            else
            {
                if (targetsFound.target == null && (targetsFound.targets == null || targetsFound.targets.Count == 0) &&
                    targetsFound.position == Vector3.zero)
                {
                    return false;
                }
            }
            
            return true;
        }
        
        public static bool PlaySpellActions(SpellComponent spellComponent, Trigger trigger)
        {
            bool findAction = false;
            
            if (!spellComponent.actions.ContainsKey(trigger))
            {
                return findAction;
            }

            List<ActionTriggered> actionsToPlay = spellComponent.actions[trigger];

            foreach (ActionTriggered action in actionsToPlay)
            {   
                TargetsFound targetsFound =
                    SpellController.GetTargetGetWithStartForm(spellComponent.caster, action.startFrom,
                        spellComponent);

                if (!CheckActionConditionsAndValidTarget(spellComponent.caster, action, targetsFound))
                {
                    continue;
                }

                findAction = true;

                if (action.spellComponent != null)
                {
                    SpellController.CastSpellComponentFromTargetsFound(spellComponent.caster, spellComponent.originSpell, action.spellComponent, targetsFound, spellComponent);
                }

                if (action.effect != null)
                {
                    switch (action.actionOnEffectType)
                    {
                        case ActionOnEffectType.ADD:
                            EffectController.ApplyEffectFromTargetsFound(spellComponent.caster, action.effect, targetsFound);
                            break;
                        case ActionOnEffectType.DELETE:
                            EffectController.DeleteEffectFromTargetsFound(action.effect, targetsFound);
                            break;
                        case ActionOnEffectType.BUFF_ATTACK:
                            EffectController.AddEffectBuff(spellComponent.caster, action.effect, true);
                            break;
                        case ActionOnEffectType.BUFF_DEFENSE:
                            EffectController.AddEffectBuff(spellComponent.caster, action.effect, false);
                            break;
                    }
                }

                if (action.damageDeal > 0)
                {
                    float damageDeal = action.damageDeal + spellComponent.caster.att + spellComponent.caster.weapon.damage;

                    if (spellComponent.damageMultiplierOnDistance != 0)
                    {
                        damageDeal += (int)(spellComponent.spellPrefabController.distanceTravelled *
                                      spellComponent.damageMultiplierOnDistance);
                    }
                    
                    if (targetsFound.targets.Count > 0)
                    {
                        targetsFound.targets.ForEach(entity => entity.TakeDamage(damageDeal, spellComponent.caster, spellComponent.damageType));
                    }
                    else if (targetsFound.target != null)
                    {
                        targetsFound.target.TakeDamage(damageDeal, spellComponent.caster, spellComponent.damageType);
                    }

                    if (spellComponent.appliesPlayerOnHitEffect)
                    {
                        List<Effect> effects = spellComponent.caster.damageDealExtraEffect.DistinctBy(currentEffect => currentEffect.typeEffect)
                            .ToList();
                        foreach (Effect effect in effects)
                        {
                            EffectController.ApplyEffectFromTargetsFound(spellComponent.caster, effect, targetsFound);
                        }
                    }
                }
            }

            return findAction;
        }

        private IEnumerator IntervallSpell(SpellComponent spellComponent)
        {
            // Wait for instantiation if needed
            yield return new WaitForSeconds(0.05f);
            
            AtTheStartSpellBehavior(spellComponent);

            float spellDuration = spellComponent.spellDuration;
            float spellInterval = spellComponent.spellInterval <= 0.00001 ? 1 : spellComponent.spellInterval;
            int originalSpellCharges = spellComponent.spellCharges;
            
            while (
                spellDuration > 0 || 
                (originalSpellCharges != 0 && spellComponent.spellCharges > 0) || 
                (spellComponent.trajectory != null && spellComponent.trajectory.disapearAtTheEndOfTrajectory) ||
                (spellComponent.originSpell != null && spellComponent.originSpell.isHolding))
            {   
                DuringIntervalSpellBehavior(spellComponent);
                yield return new WaitForSeconds(spellInterval);
                spellDuration -= spellInterval;
            }

            EndSpellComponent(spellComponent);
        }

        public static void EndSpellComponent(SpellComponent spellComponent)
        {
            AtTheEndSpellBehavior(spellComponent);

            if (spellComponent.currentCoroutine != null)
            {
                instance.StopCoroutine(spellComponent.currentCoroutine);
            }

            spellComponent.caster.activeSpellComponents.Remove(spellComponent);

            InstantiateSpell.DeactivateSpell(spellComponent);
        }
    }
}