using Games.Global;
using UnityEngine;

namespace Games.Defenses.Traps
{
    public class SpinningPoleBehavior : MonoBehaviour
    {
        public Entity playerEntity;
        public Entity entity;
        
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
        
        private void Update()
        {
            transform.Rotate (0,0,60*Time.deltaTime);
        }
    }
}
