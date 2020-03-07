using System.Collections;
using Games.Global.Patterns;
using Games.Global.Weapons;
using Games.Players;
using UnityEngine;
using Slider = UnityEngine.UI.Slider;

namespace Games.Global.Entities
{
    public class MonsterPrefab : MonoBehaviour, EffectInterface
    {
        [SerializeField] private GameObject hand;

        [SerializeField] private Slider hpBar;

        [SerializeField] private MovementPatternController movementPatternController;

        private PlayerExposer playerExposer;
 
        private Monster monster;

        private void Start()
        {
            // TODO : Replace find with another method ?
            ObjectsInScene ois = GameObject.Find("Controller").GetComponent<ObjectsInScene>();
            playerExposer = ois.playerExposer[GameController.PlayerIndex];
        }

        private void Update()
        {
            float diff = (float) monster.hp / (float) monster.initialHp;
            hpBar.value = diff;
            hpBar.transform.LookAt(playerExposer.playerCamera.transform);
            hpBar.transform.Rotate(Vector3.up * 180);
   
            gameObject.transform.LookAt(playerExposer.playerTransform);
        }

        public void SetMonster(Monster monster)
        {
            this.monster = monster;
            monster.effectInterface = this;
        }

        public Monster GetMonster()
        {
            return monster;
        }

        public void AddItemInHand(Weapon weapon)
        {
            GameObject weaponGameObject = Instantiate(weapon.model, hand.transform);
            WeaponPrefab weaponPrefab = weaponGameObject.GetComponent<WeaponPrefab>();
            weapon.weaponPrefab = weaponPrefab;
            weaponPrefab.SetWielder(monster);
            weaponPrefab.SetWeapon(weapon);
        }

        public void PlayBasicAttack(WeaponPrefab weaponPrefab)
        {
            weaponPrefab.BasicAttack(movementPatternController, hand);
        }

        public void EntityDie()
        {
            DataObject.monsterInScene.Remove(monster);

            Destroy(gameObject);
        }

        public void StartCoroutineEffect(Effect effect)
        {
            StartCoroutine(PlayEffectOnTime(effect));
        }
        
        public IEnumerator PlayEffectOnTime(Effect effect)
        {
            monster.underEffects.Add(effect.typeEffect, effect);

            Effect effectInList = monster.underEffects[effect.typeEffect];
            while (effectInList.durationInSeconds > 0)
            {
                yield return new WaitForSeconds(0.5f);

                monster.TriggerEffect(effectInList);

                effectInList = monster.underEffects[effect.typeEffect];
                effectInList.durationInSeconds -= 0.5f;
                monster.underEffects[effect.typeEffect] = effectInList;
            }

            monster.underEffects.Remove(effect.typeEffect);
        }
    }
}
