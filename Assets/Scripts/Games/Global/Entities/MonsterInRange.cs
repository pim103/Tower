using System;
using UnityEngine;

namespace Games.Global.Entities
{
    public class MonsterInRange : MonoBehaviour
    {
        [SerializeField] private EntityPrefab entityPrefab;

        private void OnTriggerEnter(Collider other)
        {
            int layerMask = LayerMask.NameToLayer("Monster");

            if (layerMask == other.gameObject.layer)
            {
                EntityPrefab otherPrefab = other.GetComponent<ColliderEntityExposer>().entityPrefab;
                if (!entityPrefab.entity.entityInRange.Contains(otherPrefab.entity))
                {
                    entityPrefab.entity.entityInRange.Add(otherPrefab.entity);
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            int layerMask = LayerMask.NameToLayer("Monster");

            if (layerMask == other.gameObject.layer)
            {
                EntityPrefab otherPrefab = other.GetComponent<ColliderEntityExposer>().entityPrefab;

                if (entityPrefab.entity.entityInRange.Contains(otherPrefab.entity))
                {
                    entityPrefab.entity.entityInRange.Remove(otherPrefab.entity);
                }
            }
        }
    }
}
