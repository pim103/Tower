﻿using System;
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

        private bool isActive = false;

        private int radius;

        [SerializeField] public GameObject previewSpell;
        [SerializeField] public SphereCollider sphereCollider;

        private void Start()
        {
            previewSpell.transform.localScale = Vector3.right * (sphereCollider.radius * 2) + Vector3.forward * (sphereCollider.radius * 2) + Vector3.up * 0.01f;
        }

        public void SetPreviewLocation(Vector3 position)
        {
            if (!previewSpell.activeSelf)
            {
                previewSpell.transform.parent = null;
                previewSpell.SetActive(true);
            }

            previewSpell.transform.position = position;
        }

        private void OnEnable()
        {
            isActive = false;
        }

        public void EnableAreaEffect()
        {
            entityInZone = new List<Entity>();

            isActive = true;
            
            ActiveArea();

            Debug.Log("Allo ici ahah");
            previewSpell.transform.parent = transform;
            previewSpell.SetActive(false);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!isActive)
            {
                return;
            }
            
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
                    PlayerPrefab playerPrefab = other.GetComponent<PlayerPrefab>();
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
            if (!isActive)
            {
                return;
            }

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
                    PlayerPrefab playerPrefab = other.GetComponent<PlayerPrefab>();
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