using System.Collections.Generic;
using Games.Global.Spells;
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

        private PlayerPrefab playerPrefab;

        private Monster monster;

        private void Start()
        {
            if (entity == null)
            {
                monster = new Monster();

                monster.SetMonsterPrefab(this);
                monster.InitEntityList();
                monster.weaponOriginalName = "Basic_Bow";
                monster.BehaviorType = BehaviorType.Distance;

                entity = monster;
                entity.entityPrefab = this;
                entity.typeEntity = TypeEntity.MOB;

                monster.InitOriginalWeapon();

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