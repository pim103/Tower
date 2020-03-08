using System.Collections;
using System.Diagnostics;
using Games.Global.Patterns;
using Games.Global.Weapons;
using UnityEngine;

namespace Games.Global
{
    public enum SpecialMovement
    {
        Dash,
        BackDash
    }
    
    public class EntityPrefab: MonoBehaviour
    {
        [SerializeField] public GameObject hand;
        [SerializeField] private MovementPatternController movementPatternController;
        [SerializeField] private Rigidbody rigidbodyEntity;

        public Entity entity;
        
        public void PlaySpecialMovement(SpecialMovement specialMovement)
        {
            switch (specialMovement)
            {
                case SpecialMovement.Dash:
                    rigidbodyEntity.AddRelativeForce(Vector3.forward * 15f, ForceMode.Impulse);
                    break;
                case SpecialMovement.BackDash:
                    rigidbodyEntity.AddRelativeForce(Vector3.back * 15f, ForceMode.Impulse);
                    break;
            }
        }

        public void PlayBasicAttack(WeaponPrefab weaponPrefab)
        {
            weaponPrefab.BasicAttack(movementPatternController, hand);
        }

        public void AddItemInHand(Weapon weapon)
        {
            GameObject weaponGameObject = Instantiate(weapon.model, hand.transform);
            WeaponPrefab weaponPrefab = weaponGameObject.GetComponent<WeaponPrefab>();
            weapon.weaponPrefab = weaponPrefab;
            weaponPrefab.SetWielder(entity);
            weaponPrefab.SetWeapon(weapon);
        }
        
        public void StartCoroutineEffect(Effect effect)
        {
            StartCoroutine(PlayEffectOnTime(effect));
        }

        public IEnumerator PlayEffectOnTime(Effect effect)
        {
            entity.underEffects.Add(effect.typeEffect, effect);

            Effect effectInList = entity.underEffects[effect.typeEffect];
            while (effectInList.durationInSeconds > 0)
            {
                yield return new WaitForSeconds(0.1f);
                if (effect.launcher != null && effect.ressourceCost > 0)
                {
                    effect.launcher.ressource1 -= effect.ressourceCost;

                    if (effect.launcher.ressource1 <= 0)
                    {
                        break;
                    }
                }

                entity.TriggerEffect(effectInList);

                effectInList = entity.underEffects[effect.typeEffect];
                effectInList.durationInSeconds -= 0.1f;
                entity.underEffects[effect.typeEffect] = effectInList;
            }

            entity.underEffects.Remove(effect.typeEffect);
        }
    }
}