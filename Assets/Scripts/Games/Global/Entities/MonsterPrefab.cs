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

        public EntityPrefab target;

        private Monster monster;
        
        private void Start()
        {
            if (entity == null)
            {
                entity = new Monster();
                entity.InitEntityList();
                entity.entityPrefab = this;
                entity.typeEntity = TypeEntity.MOB;
            }

            playerPrefab = DataObject.playerInScene[GameController.PlayerIndex];
            entity.playerInBack = new List<int>();
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

            FindTarget();

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

            foreach (KeyValuePair<int, PlayerPrefab> value in DataObject.playerInScene)
            {
                PlayerPrefab player = value.Value;
                if (!player.entity.isInvisible && !player.entity.isUntargeatable && !player.entity.hasNoAggro)
                {
                    newTarget = player;
                }

                if (player.entity.hasTaunt)
                {
                    newTarget = player;
                    break;
                }
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

        public override void SetInvisibility()
        {
            if (entity.isInvisible)
            {
                meshRenderer.gameObject.SetActive(false);
            }
            else
            {
                meshRenderer.gameObject.SetActive(true);
            }
        }
    }
}
