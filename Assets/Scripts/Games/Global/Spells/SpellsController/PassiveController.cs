using System.Collections;
using UnityEngine;

namespace Games.Global.Spells.SpellsController
{
    public class PassiveController : MonoBehaviour, ISpellController
    {
        private PassiveSpell Clone(SpellComponent spellComponent)
        {
            PassiveSpell passiveSpell = (PassiveSpell) spellComponent;
            PassiveSpell clone = new PassiveSpell
            {
                interval = passiveSpell.interval,
                damageType = passiveSpell.damageType,
                initialRotation = passiveSpell.initialRotation,
                startPosition = passiveSpell.startPosition,
                trajectoryNormalized = passiveSpell.trajectoryNormalized,
                typeSpell = passiveSpell.typeSpell,
                isBasicAttack = passiveSpell.isBasicAttack,
                newDefensiveSpell = passiveSpell.newDefensiveSpell,
                OriginalDirection = passiveSpell.OriginalDirection,
                OriginalPosition = passiveSpell.OriginalPosition,
                permanentLinkedEffect = passiveSpell.permanentLinkedEffect,
                linkedEffectOnInterval = passiveSpell.linkedEffectOnInterval,
                needPositionToMidToEntity = passiveSpell.needPositionToMidToEntity
            };

            return clone;
        }
        
        public void LaunchSpell(Entity entity, SpellComponent spellComponent)
        {
            PassiveSpell passiveSpell = Clone(spellComponent);
            Coroutine currentCoroutine = SpellController.instance.StartCoroutine(PlayPassiveSpell(entity, passiveSpell));
            passiveSpell.currentCoroutine = currentCoroutine;
        }

        private IEnumerator PlayPassiveSpell(Entity entity, PassiveSpell passiveSpell)
        {
            Spell originBasicDefSpell = entity.basicDefense;
            
            if (passiveSpell.permanentLinkedEffect != null)
            {
                SpellController.CastSpellComponent(entity, passiveSpell.permanentLinkedEffect, entity.entityPrefab.positionPointed, entity.entityPrefab.target);
            }

            while (true)
            {
                if (entity.hasPassiveDeactivate)
                {
                    if (passiveSpell.newDefensiveSpell != null)
                    {
                        entity.basicDefense = originBasicDefSpell;
                    }
                    
                    continue;
                }

                if (passiveSpell.newDefensiveSpell != null)
                {
                    entity.basicDefense = passiveSpell.newDefensiveSpell;
                }

                yield return new WaitForSeconds(passiveSpell.interval);
                if (passiveSpell.linkedEffectOnInterval != null)
                {
                    SpellController.CastSpellComponent(entity, passiveSpell.linkedEffectOnInterval, entity.entityPrefab.positionPointed, entity.entityPrefab.target);
                }
            }
        }
    }
}
