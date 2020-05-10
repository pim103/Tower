using System.Collections;
using UnityEngine;

namespace Games.Global.Spells.SpellsController
{
    public class MovementController : MonoBehaviour, ISpellController
    {
        public void LaunchSpell(Entity entity, SpellComponent spellComponent)
        {
            Coroutine currentCoroutine = SpellController.instance.StartCoroutine(PlayMovementSpell(entity, spellComponent));
            spellComponent.currentCoroutine = currentCoroutine;
        }

        private IEnumerator PlayMovementSpell(Entity entity, SpellComponent spellComponent)
        {
            MovementSpell movementSpell = (MovementSpell) spellComponent;

            if (movementSpell.linkedSpellAtTheStart != null)
            {
                SpellController.CastSpellComponent(entity, movementSpell.linkedSpellAtTheStart, entity.entityPrefab.transform.position, entity);
            }

            if (movementSpell.movementSpellType == MovementSpellType.Tp || movementSpell.movementSpellType == MovementSpellType.TpWithTarget)
            {
                DoTp(entity, movementSpell);
                EndMovement(entity, movementSpell);
                yield break;
            }
            
            entity.entityPrefab.canMove = false;

            if (!movementSpell.isFollowingMouse)
            {
                entity.entityPrefab.cameraBlocked = true;
            }

            if (movementSpell.movementSpellType == MovementSpellType.Charge)
            {
                entity.entityPrefab.isCharging = true;
            }

            float duration = movementSpell.duration;
            while (duration > 0)
            {
                yield return new WaitForSeconds(0.05f);
                duration -= 0.05f;

                UpdatePosition(entity, movementSpell);
            }

            EndMovement(entity, movementSpell);
        }

        private void DoTp(Entity entity, MovementSpell movementSpell)
        {
            Vector3 newPosition = movementSpell.tpPosition;
            if (movementSpell.movementSpellType == MovementSpellType.TpWithTarget && movementSpell.target != null)
            {        
//                entity.entityPrefab.transform.LookAt(movementSpell.target.entityPrefab.transform);
                entity.entityPrefab.transform.forward = movementSpell.target.entityPrefab.transform.forward;
                newPosition -= movementSpell.target.entityPrefab.transform.forward;
            }

            entity.entityPrefab.transform.position = newPosition;
        }

        private void UpdatePosition(Entity entity, MovementSpell movementSpell)
        {
            EntityPrefab entityPrefab = entity.entityPrefab;

            float speed = movementSpell.speed * 0.05f;
            if (movementSpell.movementSpellType == MovementSpellType.Charge)
            {
                entityPrefab.transform.position += (entity.entityPrefab.transform.forward * speed);
            }
            else
            {
                entityPrefab.transform.position += (movementSpell.trajectory * speed);
            }
        }

        public static void EndMovement(Entity entity, MovementSpell movementSpell)
        {
            entity.entityPrefab.canMove = true;
            entity.entityPrefab.cameraBlocked = false;
            entity.entityPrefab.isCharging = false;

            if (movementSpell.linkedSpellAtTheEnd != null)
            {
                SpellController.CastSpellComponent(entity, movementSpell.linkedSpellAtTheEnd, entity.entityPrefab.transform.position);
            }

            if (movementSpell.currentCoroutine != null)
            {
                SpellController.instance.StopCoroutine(movementSpell.currentCoroutine);
            }
        }
    }
}
