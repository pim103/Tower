using System.Collections;
using System.Linq.Expressions;
using Games.Global.Entities;
using Games.Global.Patterns;
using Games.Global.Weapons;
using UnityEngine;

namespace Games.Global
{
    public enum SpecialMovement
    {
        Dash,
        BackDash,
        Charge,
        HeavyBasicAttack
    }
    
    public class EntityPrefab: MonoBehaviour
    {
        // virtual hand look at cursor or target
        [SerializeField] public GameObject virtualHand;
        
        // hand play animation of weapon
        [SerializeField] public GameObject hand;

        [SerializeField] private MovementPatternController movementPatternController;
        [SerializeField] private Rigidbody rigidbodyEntity;
        
        [SerializeField] protected MeshRenderer meshRenderer;

        public Entity entity;

        public bool movementBlocked = false;
        public bool cameraBlocked = false;
        public bool intentBlocked = false;

        public bool isCharging = false;
        
        public IEnumerator PlayCharge(float duration)
        {
            movementBlocked = true;
            isCharging = true;

            Vector3 initialForward = transform.forward;

            while (duration > 0)
            {
                rigidbodyEntity.velocity = (initialForward * 20);
                
                yield return new WaitForSeconds(0.1f);
                duration -= 0.1f;
            }
            movementBlocked = false;
            isCharging = false;
        }

        private IEnumerator ApplyForce()
        {
            rigidbodyEntity.isKinematic = false;
            
            rigidbodyEntity.AddRelativeForce(Vector3.back * 10, ForceMode.Impulse);
            yield return new WaitForSeconds(2);
            rigidbodyEntity.isKinematic = true;
        }

        public void OnCollisionEnter(Collision other)
        {
            int monsterLayer = LayerMask.NameToLayer("Monster");
            
            if (isCharging)
            {
                if (monsterLayer == other.gameObject.layer)
                {
                    MonsterPrefab monsterPrefab = other.gameObject.GetComponent<MonsterPrefab>();
                    StartCoroutine(monsterPrefab.ApplyForce());
                }
            }
        }

        public void PlaySpecialMovement(SpecialMovement specialMovement, float durationSpecialMovement = 0)
        {
            switch (specialMovement)
            {
                case SpecialMovement.Dash:
                    rigidbodyEntity.AddRelativeForce(Vector3.forward * 30f, ForceMode.Impulse);
                    break;
                case SpecialMovement.BackDash:
                    rigidbodyEntity.AddRelativeForce(Vector3.back * 30f, ForceMode.Impulse);
                    break;
                case SpecialMovement.Charge:
                    StartCoroutine(PlayCharge(durationSpecialMovement));
                    break;
                case SpecialMovement.HeavyBasicAttack:
                    entity.weapons[0].oneHitDamageUp = 20;
                    entity.BasicAttack();
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
            entity.underEffects.Add(effect.typeEffect, effect);
            Coroutine currentCoroutine = StartCoroutine(PlayEffectOnTime(effect));

            effect.currentCoroutine = currentCoroutine;
            entity.underEffects[effect.typeEffect] = effect;
        }

        public IEnumerator PlayEffectOnTime(Effect effect)
        {
            Effect effectInList = entity.underEffects[effect.typeEffect];

            entity.InitialTrigger(effectInList);
            
            while (effectInList.durationInSeconds > 0)
            {
                yield return new WaitForSeconds(0.1f);
                if (effect.launcher != null && effect.ressourceCost > 0)
                {
                    effect.launcher.ressource1 -= effect.ressourceCost;

                    if (effect.launcher.ressource1 <= 0)
                    {
                        StopCurrentEffect(effect);
                        yield break;
                    }
                }

                entity.TriggerEffect(effectInList);

                effectInList = entity.underEffects[effect.typeEffect];
                effectInList.durationInSeconds -= 0.1f;
                entity.underEffects[effect.typeEffect] = effectInList;
            }

            StopCurrentEffect(effect);
        }

        public void StopCurrentEffect(Effect effect)
        {
            if (effect.currentCoroutine != null)
            {
                StopCoroutine(effect.currentCoroutine);
            }

            entity.EndEffect(effect);
            entity.underEffects.Remove(effect.typeEffect);
        }

        public void SetMaterial(Material material)
        {
            meshRenderer.material = material;
        }
        
        public IEnumerator AddDamageDealExtraEffect(Effect effect, float duration)
        {
            if (entity.damageDealExtraEffect.ContainsKey(effect.typeEffect))
            {
                entity.damageDealExtraEffect[effect.typeEffect] = effect;
            }
            else
            {
                entity.damageDealExtraEffect.Add(effect.typeEffect, effect);
            }
            
            yield return new WaitForSeconds(duration);

            if (entity.damageDealExtraEffect.ContainsKey(effect.typeEffect))
            {
                entity.damageDealExtraEffect.Remove(effect.typeEffect);
            }
        }
        
        public IEnumerator AddDamageReceiveExtraEffect(Effect effect, float duration)
        {
            if (entity.damageReceiveExtraEffect.ContainsKey(effect.typeEffect))
            {
                entity.damageReceiveExtraEffect[effect.typeEffect] = effect;
            }
            else
            {
                entity.damageReceiveExtraEffect.Add(effect.typeEffect, effect);
            }

            yield return new WaitForSeconds(duration);

            if (entity.damageReceiveExtraEffect.ContainsKey(effect.typeEffect))
            {
                entity.damageReceiveExtraEffect.Remove(effect.typeEffect);
            }
        }
    }
}