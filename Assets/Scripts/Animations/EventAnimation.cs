using System;
using Games.Global;
using Games.Global.Spells.SpellsController;
using UnityEngine;

namespace Animations
{
    public class EventAnimation : MonoBehaviour
    {
        [SerializeField] private EntityPrefab entityPrefab;
        
        [SerializeField] private GameObject shortSwordSlash;

        public void StartAttackAnimation()
        {
        }

        public void TriggerShortSwordSlash()
        {
            shortSwordSlash.SetActive(true);
            Entity entity = entityPrefab.entity;
            
            SpellController.CastSpell(entity, entity.basicAttack, transform.position + (Vector3.up * 1.5f),  entity);
            //shortSwordSlash.GetComponent<ParticleSystem>().Play();
        }

        public void EndAttackAnimation()
        {
            /*
            if (entityPrefab.entity.weapons.Count > 0)
            {
                entityPrefab.entity.weapons[0].weaponPrefab.DeactivateBoolAttack();
            }
            */
            shortSwordSlash.SetActive(false);
        }
    }
}
