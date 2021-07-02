using System.Collections.Generic;
using Games.Global.TreeBehavior.TestTreeBehavior;
using Games.Players;
using UnityEngine;
using Slider = UnityEngine.UI.Slider;

namespace Games.Global.Entities
{
    public class MonsterPrefab : EntityPrefab
    {
        [SerializeField] private Slider hpBar;

        [SerializeField] 
        public List<GameObject> objectsToLoot;

        [SerializeField] private GameObject root;

        private MonsterBasicBehavior behavior;

        private PlayerPrefab playerPrefab;

        private Monster monster;
        private Vector3 initialRootPosition;

        private void Start()
        {
            if (entity == null)
            {
                monster = new Monster();
                monster.def = 1;
                monster.initialDef = 1;
                
                monster.InitMonster(this);
                monster.InitEntityList();
                monster.weaponOriginalId = 1;
                //TODO : change to monster
                monster.SetBehaviorType(BehaviorType.Player);
                monster.SetAttackBehaviorType(AttackBehaviorType.AllSpellsIFirst);
                
                entity = monster;
                entity.entityPrefab = this;

                monster.InitOriginalWeapon();

                DataObject.monsterInScene.Add(monster);
            }

            entity.SetTypeEntity(TypeEntity.MOB);
            entity.playerInBack = new List<int>();
            initialRootPosition = root.transform.localPosition;

            behavior = new MonsterBasicBehavior();
            behavior?.InitBehaviorTree(monster);
        }

        private void Update()
        {
            if (DataObject.playerInScene == null || DataObject.playerInScene.Count == 0)
            {
                return;
            }

            if (playerPrefab == null)
            {
                playerPrefab = DataObject.playerInScene[GameController.PlayerIndex];
            }

            float diff = (float) entity.hp / (float) entity.initialHp;

            hpBar.value = diff;
            hpBar.transform.LookAt(playerPrefab.camera.transform);
            hpBar.transform.Rotate(Vector3.up * 180);
            if (Vector3.Distance(root.transform.localPosition, initialRootPosition) > 0.1)
            {
                hpBar.transform.position = root.transform.position + Vector3.up * 2;
            }

            behavior?.UpdateBehaviorTree();
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