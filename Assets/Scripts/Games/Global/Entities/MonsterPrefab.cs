using System;
using System.Collections;
using System.Collections.Generic;
using Games.Global.Patterns;
using Games.Global.Weapons;
using Games.Players;
using UnityEngine;
using UnityEngine.AI;
using Slider = UnityEngine.UI.Slider;

namespace Games.Global.Entities
{
    public class MonsterPrefab : EntityPrefab, EffectInterface
    {
        [SerializeField] private Slider hpBar;
        [SerializeField] private NavMeshAgent navMeshAgent;

        private PlayerPrefab playerPrefab;

        public bool aggroForced;
        public PlayerPrefab target;

        private Monster monster;

        private void Start()
        {
            playerPrefab = DataObject.playerInScene[GameController.PlayerIndex];
            entity.playerInBack = new List<int>();
            aggroForced = false;
        }

        private void Update()
        {
            float diff = (float) entity.hp / (float) entity.initialHp;
            hpBar.value = diff;
            hpBar.transform.LookAt(playerPrefab.camera.transform);
            hpBar.transform.Rotate(Vector3.up * 180);

            if (!entity.underEffects.ContainsKey(TypeEffect.Stun) && !entity.underEffects.ContainsKey(TypeEffect.Sleep))
            {
                FindTarget();
            }
            else if(!aggroForced)
            {
                target = null;
            }

            MoveToTarget();
        }

        private void FindTarget()
        {
            PlayerPrefab newTarget = null;

            if (!aggroForced)
            {
                foreach (KeyValuePair<int, PlayerPrefab> value in DataObject.playerInScene)
                {
                    if (!value.Value.entity.underEffects.ContainsKey(TypeEffect.Invisibility) && !value.Value.entity.underEffects.ContainsKey(TypeEffect.Untargetable))
                    {
                        newTarget = value.Value;
                    }
                }
            }
            else
            {
                newTarget = target;
            }

            if (newTarget != null)
            {
                gameObject.transform.LookAt(newTarget.playerTransform);
                virtualHand.transform.LookAt(newTarget.playerTransform);
            }

            target = newTarget;
        }

        public void SetMonster(Monster monster)
        {
            entity = monster;
            entity.entityPrefab = this;
            this.monster = monster;
            monster.effectInterface = this;
        }

        public Monster GetMonster()
        {
            return (Monster)entity;
        }

        public void EntityDie()
        {
            DataObject.monsterInScene.Remove((Monster)entity);

            gameObject.SetActive(false);
        }

        private void MoveToTarget()
        {
            if (target != null && monster.constraint == TypeWeapon.Cac)
            {
                navMeshAgent.SetDestination(target.transform.position);
                
                if (navMeshAgent.remainingDistance <= 1 && navMeshAgent.hasPath)
                {
                    navMeshAgent.SetDestination(transform.position);
                }
            }
        }
    }
}
