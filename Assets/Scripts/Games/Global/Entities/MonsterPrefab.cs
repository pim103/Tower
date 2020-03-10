using System.Collections;
using System.Collections.Generic;
using Games.Global.Patterns;
using Games.Global.Weapons;
using Games.Players;
using UnityEngine;
using Slider = UnityEngine.UI.Slider;

namespace Games.Global.Entities
{
    public class MonsterPrefab : EntityPrefab, EffectInterface
    {
        [SerializeField] private Slider hpBar;

        private PlayerPrefab playerPrefab;

        public PlayerPrefab target;

        private void Start()
        {
            playerPrefab = DataObject.playerInScene[GameController.PlayerIndex];
        }

        private void Update()
        {
            float diff = (float) entity.hp / (float) entity.initialHp;
            hpBar.value = diff;
            hpBar.transform.LookAt(playerPrefab.camera.transform);
            hpBar.transform.Rotate(Vector3.up * 180);
   
            gameObject.transform.LookAt(playerPrefab.playerTransform);
            FindTarget();
        }

        private void FindTarget()
        {
            PlayerPrefab newTarget = null;
            
            foreach (KeyValuePair<int, PlayerPrefab> value in DataObject.playerInScene)
            {
                if (!value.Value.entity.underEffects.ContainsKey(TypeEffect.Invisibility))
                {
                    newTarget = value.Value;
                }
            }

            if (newTarget != null)
            {
                virtualHand.transform.LookAt(newTarget.transform);
            }

            target = newTarget;
        }

        public void SetMonster(Monster monster)
        {
            entity = monster;
            entity.entityPrefab = this;
            monster.effectInterface = this;
        }

        public Monster GetMonster()
        {
            return (Monster)entity;
        }

        public void EntityDie()
        {
            DataObject.monsterInScene.Remove((Monster)entity);

            Destroy(gameObject);
        }
    }
}
