using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Games.Global.Spells.SpellsController
{
    public class TransformationController : MonoBehaviour, ISpellController
    {
        private TransformationSpell Clone(SpellComponent spellComponent)
        {
            TransformationSpell origin = (TransformationSpell) spellComponent;
            TransformationSpell clone = new TransformationSpell
            {
                damageType = origin.damageType,
                initialRotation = origin.initialRotation,
                startPosition = origin.startPosition,
                trajectoryNormalized = origin.trajectoryNormalized,
                typeSpell = origin.typeSpell,
                isBasicAttack = origin.isBasicAttack,
                OriginalDirection = origin.OriginalDirection,
                OriginalPosition = origin.OriginalPosition,
                needPositionToMidToEntity = origin.needPositionToMidToEntity,
                duration = origin.duration,
                prefab = origin.prefab,
                newSpells = origin.newSpells,
                castByPassive = origin.castByPassive
            };

            return clone;
        }
        
        public void LaunchSpell(Entity entity, SpellComponent spellComponent)
        {
            TransformationSpell passiveSpell = Clone(spellComponent);
            Coroutine currentCoroutine = SpellController.instance.StartCoroutine(PlayTransformationSpell(entity, passiveSpell));
            passiveSpell.currentCoroutine = currentCoroutine;
        }

        private IEnumerator PlayTransformationSpell(Entity entity, TransformationSpell transformationSpell)
        {
            List<Spell> originSpells = entity.spells;

            if (transformationSpell.newSpells != null)
            {
                for (int i = 0; i < transformationSpell.newSpells.Count; i++)
                {
                    originSpells.Add(entity.spells[i]);
                    entity.spells[i] = transformationSpell.newSpells[i];
                }
            }

            float duration = transformationSpell.duration;
            if (duration <= -0.99999)
            {
                yield break;
            }

            while (duration > 0)
            {
                yield return new WaitForSeconds(0.1f);
                duration -= 0.1f;
            }

            if (transformationSpell.newSpells != null)
            {
                for (int i = 0; i < transformationSpell.newSpells.Count; i++)
                {
                    entity.spells[i] = originSpells[i];
                }
            }
        }
    }
}
