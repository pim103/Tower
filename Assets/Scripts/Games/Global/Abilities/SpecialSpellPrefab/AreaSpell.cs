using System;
using System.Collections.Generic;
using Games.Global.Entities;
using Games.Players;
using UnityEngine;

namespace Games.Global.Abilities.SpecialSpellPrefab
{
    public abstract class AreaSpell : MonoBehaviour
    {
        public Entity origin;
        public Transform parent;

        public List<Entity> entityInZone;
        
        private void OnEnable()
        {
            entityInZone = new List<Entity>();
            
            ActiveArea();
        }

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
                
                entityInZone.Add(entity);
                TriggerAreaEffect(entity);
            }
        }

        private void OnTriggerExit(Collider other)
        {
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

                if (entityInZone.Contains(entity))
                {
                    entityInZone.Remove(entity);
                    QuitAreaEffect(entity);
                }
            }
        }

        public virtual void ActiveArea()
        {
            
        }

        public virtual void TriggerAreaEffect(Entity entity)
        {
            
        }

        public virtual void QuitAreaEffect(Entity entity)
        {
            
        }
    }
}
