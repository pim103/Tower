using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Games.Global.Abilities;
using Games.Global.Armors;
using Games.Global.Entities;
using Games.Global.Spells;
using Games.Global.Spells.SpellsController;
//using Games.Global.Patterns;
using Games.Players;
using UnityEngine;
using Utils;

namespace Games.Global.Weapons
{
    public class WeaponPrefab : MonoBehaviour
    {
        private Weapon weapon;
        private Entity wielder;

        private bool isAttacking;

        [SerializeField] private BoxCollider boxCollider;
        [SerializeField] private Transform positionInHand;

        public IEnumerator PlayAnimationAttack()
        {
            boxCollider.enabled = true;

            Animator animator = wielder.entityPrefab.animator;
            float initialSpeed = animator.speed;

            animator.speed = weapon.attSpeed + wielder.attSpeed;
            animator.Play(weapon.animationToPlay);

            if (weapon.type == TypeWeapon.Distance)
            {
                PoolProjectiles();
            }

            weapon.FixAngleAttack(true, wielder);

            do {
                yield return new WaitForSeconds(0.1f);
            } while (animator.GetCurrentAnimatorStateInfo(0).IsName(weapon.animationToPlay));

            weapon.FixAngleAttack(false, wielder);

            wielder.entityPrefab.characterMesh.transform.localPosition = Vector3.zero;
            wielder.entityPrefab.characterMesh.transform.localEulerAngles = Vector3.zero;

            animator.speed = initialSpeed;
            isAttacking = false;
            boxCollider.enabled = false;
        }

        public void BasicAttack()
        {
            if (!isAttacking)
            {
                isAttacking = true;
                StartCoroutine(PlayAnimationAttack());
            }
        }

        private void PoolProjectiles()
        {
            GameObject proj = ObjectPooler.SharedInstance.GetPooledObject(weapon.idPoolProjectile);

            proj.transform.position = transform.position;
            float rotX = proj.transform.localEulerAngles.x;

            proj.transform.localEulerAngles = wielder.entityPrefab.transform.eulerAngles + (Vector3.right * rotX);
            proj.SetActive(true);

            ProjectilesPrefab projectilesPrefab = proj.GetComponent<ProjectilesPrefab>();
            projectilesPrefab.rigidbody.AddForce(wielder.entityPrefab.transform.forward * 1000, ForceMode.Acceleration);

            projectilesPrefab.weaponOrigin = this;
        }

        private void OnTriggerEnter(Collider other)
        {
            int monsterLayer = LayerMask.NameToLayer("Monster");
            int playerLayer = LayerMask.NameToLayer("Player");

            Entity entity;

            if (other.gameObject.layer == monsterLayer && wielder.typeEntity != TypeEntity.MOB)
            {
                MonsterPrefab monsterPrefab = other.GetComponent<MonsterPrefab>();
                entity = monsterPrefab.GetMonster();
            }
            else if (other.gameObject.layer == playerLayer && wielder.typeEntity != TypeEntity.PLAYER)
            {
                PlayerPrefab playerPrefab = other.GetComponent<PlayerPrefab>();
                entity = playerPrefab.entity;
            }
            else
            {
                return;
            }

            if (entity.IdEntity == wielder.IdEntity &&
                ((other.gameObject.layer == monsterLayer && wielder.typeEntity == TypeEntity.MOB) ||
                 (other.gameObject.layer == playerLayer && wielder.typeEntity == TypeEntity.PLAYER))
            )
            {
                return;
            }

            TouchEntity(entity, transform.position);
        }

        public bool TouchEntity(Entity entity, Vector3 originDamage)
        {
            AbilityParameters abilityParameters = new AbilityParameters {origin = wielder, directTarget = entity};

            bool isPhysic = false;
            bool isMagic = false;

            if ((entity.isIntangible && isPhysic) ||
                (entity.hasAntiSpell && isMagic) ||
                wielder.isBlind ||
                entity.isUntargeatable)
            {
                return true;
            }

            BuffController.EntityReceivedDamage(entity);

            if ( entity.hasDivineShield)
            {
                return true;
            }

            weapon.OnDamageDealt(abilityParameters);
            foreach (Armor armor in wielder.armors)
            {
                armor.OnDamageDealt(abilityParameters);
            }

            List<Effect> effects = wielder.damageDealExtraEffect.DistinctBy(currentEffect => currentEffect.typeEffect).ToList();
            foreach (Effect effect in effects)
            {
                Effect copy = effect;
                copy.positionSrcDamage = originDamage;
                EffectController.ApplyEffect(entity, copy);
            }

            BuffController.EntityDealDamage(wielder);

            int damage = weapon.damage + wielder.att + weapon.oneHitDamageUp;
            if (wielder.isWeak)
            {
                damage /= 2;
            }

            entity.TakeDamage(damage, abilityParameters, wielder.canPierce);

            return true;
        }

        public void SetWeapon(Weapon weapon)
        {
            this.weapon = weapon;
        }

        public Weapon GetWeapon()
        {
            return weapon;
        }

        public void SetWielder(Entity entity)
        {
            this.wielder = entity;
        }

        public Entity GetWielder()
        {
            return wielder;
        }

        public void SetPositionToParent(Transform initialPosition, Transform angle)
        {
            positionInHand.position = initialPosition.position;
            positionInHand.LookAt(angle);
            positionInHand.Rotate(Vector3.up * 180);
        }
    }
}