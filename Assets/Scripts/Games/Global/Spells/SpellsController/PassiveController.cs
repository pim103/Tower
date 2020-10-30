using System.Collections;
using UnityEngine;
using Utils;

namespace Games.Global.Spells.SpellsController
{
    public class PassiveController : MonoBehaviour, ISpellController
    {
        public void LaunchSpell(Entity entity, SpellComponent spellComponent, SpellComponent origin = null)
        {
            PassiveSpell passiveSpell = Tools.Clone((PassiveSpell) spellComponent);
            Coroutine currentCoroutine = SpellController.instance.StartCoroutine(PlayPassiveSpell(entity, passiveSpell));
            passiveSpell.currentCoroutine = currentCoroutine;
        }

        private IEnumerator PlayPassiveSpell(Entity entity, PassiveSpell passiveSpell)
        {
            Spell originBasicDefSpell = entity.basicDefense;
            
            if (passiveSpell.permanentLinkedEffect != null)
            {
//                SpellController.CastSpellComponent(entity, passiveSpell.permanentLinkedEffect, entity.entityPrefab.positionPointed, entity.entityPrefab.target, passiveSpell);
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
//                    SpellController.CastSpellComponent(entity, passiveSpell.linkedEffectOnInterval, entity.entityPrefab.positionPointed, entity.entityPrefab.target, passiveSpell);
                }
            }
        }
    }
}
