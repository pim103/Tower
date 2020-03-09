using System.Collections;
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

        private void Start()
        {
            // TODO : Replace find with another method ?
            ObjectsInScene ois = GameObject.Find("Controller").GetComponent<ObjectsInScene>();
            playerExposer = ois.playerExposer[GameController.PlayerIndex];
        }

        private void Update()
        {
            float diff = (float) entity.hp / (float) entity.initialHp;
            hpBar.value = diff;
            hpBar.transform.LookAt(playerExposer.playerCamera.transform);
            hpBar.transform.Rotate(Vector3.up * 180);
   
            gameObject.transform.LookAt(playerExposer.playerTransform);
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
