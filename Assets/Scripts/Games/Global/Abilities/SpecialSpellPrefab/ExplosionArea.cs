using Games.Global.Entities;
using Games.Players;
using UnityEngine;

namespace Games.Global.Abilities.SpecialSpellPrefab
{
    public abstract class ExplosionArea : MonoBehaviour
    {
        public Entity origin;
        
        private void OnTriggerEnter(Collider other)
        {
            Debug.Log("In explosion : " + other.name);
            if (other.gameObject.layer != LayerMask.NameToLayer("Wall") || other.gameObject.layer != LayerMask.NameToLayer("Ground"))
            {
                int monsterLayer = LayerMask.NameToLayer("Monster");
                int playerLayer = LayerMask.NameToLayer("Player");

                Entity entity;
                
                if (other.gameObject.layer == monsterLayer && origin.typeEntity != TypeEntity.MOB)
                {
                    MonsterPrefab monsterPrefab = other.GetComponent<MonsterPrefab>();
                    entity = monsterPrefab.GetMonster();
                } else if (other.gameObject.layer == playerLayer && origin.typeEntity != TypeEntity.PLAYER)
                {
                    PlayerPrefab playerPrefab = other.transform.parent.GetComponent<PlayerPrefab>();
                    entity = playerPrefab.entity;
                }
                else
                {
                    return;
                }
                
                TriggerExplosion(entity);
            }
        }

        public abstract void TriggerExplosion(Entity entity);
    }
}
