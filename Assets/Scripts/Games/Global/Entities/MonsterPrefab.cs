using System;
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

        public List<PlayerPrefab> playerInBack;

        private void Start()
        {
            playerPrefab = DataObject.playerInScene[GameController.PlayerIndex];
            playerInBack = new List<PlayerPrefab>();
        }


        public void OnTriggerEnter(Collider other)
        {
            int layerPlayer = LayerMask.NameToLayer("Player");

            if (other.gameObject.layer == layerPlayer)
            {
                Debug.Log("PLAYER ENTER");
                playerInBack.Add(other.GetComponent<PlayerPrefab>());
            }
        }

        public void OnTriggerExit(Collider other)
        {
            int layerPlayer = LayerMask.NameToLayer("Player");

            if (other.gameObject.layer == layerPlayer)
            {
                Debug.Log("PLAYER LEAVE");
                playerInBack.Remove(other.GetComponent<PlayerPrefab>());
            }
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
            else
            {
                target = null;
            }
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
                gameObject.transform.LookAt(newTarget.playerTransform);
                virtualHand.transform.LookAt(newTarget.playerTransform);
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
