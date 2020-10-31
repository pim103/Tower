using Games.Global;
using Games.Global.Spells;
using Games.Players;
using UnityEngine;

namespace Games.Defenses
{
    public class BearTrapBehavior : MonoBehaviour
    {
        [SerializeField] 
        private GameObject inactiveState;
        [SerializeField] 
        private GameObject activeState;
        private bool hasBeenTriggered;
        private Entity playerEntity;
        private Entity entity;
        
        private void Start()
        {
            entity = new Entity
            {
                entityPrefab = gameObject.AddComponent<EntityPrefab>(), 
                BehaviorType = BehaviorType.Player,
                typeEntity = TypeEntity.MOB
            };
            entity.entityPrefab.entity = entity;
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Player") &&!hasBeenTriggered){
                hasBeenTriggered = true;
                inactiveState.SetActive(false);
                activeState.SetActive(true);
                playerEntity = other.gameObject.GetComponent<PlayerPrefab>().entity;
                playerEntity.TakeDamage(20.0f, entity, DamageType.Physical);
            }
        }
    }
}
