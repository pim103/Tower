using System.Collections;
using System.Collections.Generic;
using Games.Global.Spells.SpellBehavior;
using Games.Global.Spells.SpellParameter;
using UnityEngine;

namespace Games.Global.Spells.SpellsController
{
    public class SpellInterpreter : MonoBehaviour
    {
        public void StartSpellTreatment(SpellComponent spellComponent, Vector3 startPosition)
        {
            InstantiateSpell.InstantiateNewSpell(spellComponent, startPosition);
            
            StartCoroutine(IntervallSpell(spellComponent));
        }
        
        private void AtTheStartSpellBehavior(SpellComponent spellComponent)
        {
            PlaySpellActions(spellComponent, Trigger.START);
        }

        private void AtTheEndSpellBehavior(SpellComponent spellComponent)
        {
            PlaySpellActions(spellComponent, Trigger.END);
        }

        private void DuringIntervalSpellBehavior(SpellComponent spellComponent)
        {
            PlaySpellActions(spellComponent, Trigger.INTERVAL);
        }

        public static void PlaySpellActions(SpellComponent spellComponent, Trigger trigger)
        {
            Dictionary<TargetType, List<ActionTriggered>> actionsToPlay = spellComponent.actions[trigger];

            foreach (KeyValuePair<TargetType, List<ActionTriggered>> actionToTarget in actionsToPlay)
            {
                TargetType targetType = actionToTarget.Key;
                List<ActionTriggered> actionsTriggereds = actionToTarget.Value;

                foreach (ActionTriggered action in actionsTriggereds)
                {
                    switch (targetType)
                    {
                        case TargetType.CASTER:
                            PlayActionOnEntity(action, spellComponent.caster, spellComponent);
                            break;
                        case TargetType.ALLIES:
                            foreach (Entity ally in spellComponent.spellPrefabController.alliesTouchedBySpell)
                            {
                                PlayActionOnEntity(action, ally, spellComponent);
                            }
                            break;
                        case TargetType.ENNEMIES:
                            foreach (Entity enemy in spellComponent.spellPrefabController.alliesTouchedBySpell)
                            {
                                PlayActionOnEntity(action, enemy, spellComponent);
                            }
                            break;
                    }
                }
            }
        }

        private static void PlayActionOnEntity(ActionTriggered actionTriggered, Entity entityAffected, SpellComponent initialSpellComponent)
        {
            if (actionTriggered.percentageToTrigger != 100)
            {
                int triggerAction = Random.Range(0, 100);
                if (triggerAction < actionTriggered.percentageToTrigger)
                {
                    return;
                }
            }

            SpellComponent spellComponent = actionTriggered.spellComponent;
            Effect effect = actionTriggered.effect;

            if (spellComponent != null)
            {
                SpellController.CastSpellComponent(initialSpellComponent.caster, spellComponent, spellComponent.targetAtCast, initialSpellComponent);
            }

            if (effect != null)
            {
                EffectController.ApplyEffect(entityAffected, effect, spellComponent.caster, spellComponent.spellPrefabController.transform.position);
            }
            
            
            entityAffected.TakeDamage(actionTriggered.damageDeal, initialSpellComponent.caster, initialSpellComponent.damageType);
        }

        private IEnumerator IntervallSpell(SpellComponent spellComponent)
        {
            // Wait for instantiation if needed
            yield return new WaitForSeconds(0.05f);
            
            AtTheStartSpellBehavior(spellComponent);

            float spellDuration = spellComponent.spellDuration;
            float spellInterval = spellComponent.spellInterval;

            while (spellDuration > 0)
            {
                DuringIntervalSpellBehavior(spellComponent);
                yield return new WaitForSeconds(spellInterval);
                spellDuration -= spellInterval;
            }

            AtTheEndSpellBehavior(spellComponent);
        }
    }
}