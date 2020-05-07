using System.Collections.Generic;
using Games.Global.Weapons;
using Games.Players;
using UnityEngine;
using UnityEngine.AI;
using Slider = UnityEngine.UI.Slider;

namespace Games.Global.Entities
{
    public class MonsterPrefab : EntityPrefab
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

            if (!canDoSomething)
            {
                navMeshAgent.SetDestination(transform.position);
                return;
            }

            if (!aggroForced)
            {
                FindTarget();
            }

            if (canMove)
            {
                MoveToTarget();
            }
            else
            {
                navMeshAgent.SetDestination(transform.position);
            }
        }

        private void FindTarget()
        {
            PlayerPrefab newTarget = null;

            if (!aggroForced)
            {
                foreach (KeyValuePair<int, PlayerPrefab> value in DataObject.playerInScene)
                {
                    if (!value.Value.entity.isInvisible && !value.Value.entity.isUntargeatable)
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
            }

            target = newTarget;
        }

        public void SetMonster(Monster monster)
        {
            entity = monster;
            entity.entityPrefab = this;
            this.monster = monster;
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
            if (entity.isFeared || entity.isCharmed)
            {
                navMeshAgent.SetDestination(transform.position + forcedDirection);
                return;
            }
            
//            if (target != null && monster.constraint == TypeWeapon.Cac)
//            {
//                navMeshAgent.SetDestination(target.transform.position);
//                
//                if (navMeshAgent.remainingDistance <= 1 && navMeshAgent.hasPath)
//                {
//                    navMeshAgent.SetDestination(transform.position);
//                }
//            }
        }
    }
}
