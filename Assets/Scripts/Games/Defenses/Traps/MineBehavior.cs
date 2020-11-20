using Games.Global;
using Games.Global.Spells;
using Games.Players;
using UnityEngine;

namespace Games.Defenses.Traps
{
    public class MineBehavior : MonoBehaviour
    {
        private Entity playerEntity;
        private Entity entity;
        private void Start()
        {
            entity = new Entity
            {
                entityPrefab = gameObject.AddComponent<EntityPrefab>()
            };
            entity.SetBehaviorType(BehaviorType.Player);
            entity.SetTypeEntity(TypeEntity.MOB);

            entity.entityPrefab.entity = entity;
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                playerEntity = other.gameObject.GetComponent<PlayerPrefab>().entity;
                playerEntity.TakeDamage(20.0f, null, DamageType.Physical);
                gameObject.SetActive(false);
            }
        }
    }
}
