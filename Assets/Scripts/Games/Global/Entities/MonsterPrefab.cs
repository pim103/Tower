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

        private PlayerExposer playerExposer;

        private PlayerPrefab target;

        private void Start()
        {
            playerExposer = DataObject.playerInScene[GameController.PlayerIndex].playerExposer;
        }

        private void Update()
        {
            float diff = (float) entity.hp / (float) entity.initialHp;
            hpBar.value = diff;
            hpBar.transform.LookAt(playerExposer.playerCamera.transform);
            hpBar.transform.Rotate(Vector3.up * 180);
   
            gameObject.transform.LookAt(playerExposer.playerTransform);
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
                target = newTarget;
                hand.transform.LookAt(newTarget.transform);
            }
        }

        public void SetMonster(Monster monster)
        {
            entity = monster;
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
