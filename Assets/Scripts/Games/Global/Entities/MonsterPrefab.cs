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

        private Monster monster;

        private void Start()
        {
            if (entity == null)
            {
                monster = new Monster();

                monster.SetMonsterPrefab(this);
                monster.InitEntityList();

                entity = monster;
                entity.entityPrefab = this;
                entity.typeEntity = TypeEntity.MOB;

                DataObject.monsterInScene.Add(monster);
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
                if (monster.constraint == TypeWeapon.Cac)
                {
                    MoveToTarget(navMeshAgent, 1);
                }
                else
                {
                    MoveToTarget(navMeshAgent, 10);
                }
            }
            else
            {
                navMeshAgent.SetDestination(transform.position);
            }
        }

        public void SetMonster(Monster monster)
        {
            entity = monster;
            entity.entityPrefab = this;
            this.monster = monster;
        }

        public Monster GetMonster()
        {
            return (Monster) entity;
        }

        public void EntityDie()
        {
            DataObject.monsterInScene.Remove((Monster) entity);

            gameObject.SetActive(false);
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