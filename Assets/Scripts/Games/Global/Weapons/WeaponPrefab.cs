using System.Collections;
using System.Collections.Generic;
using Games.Global.Abilities;
using Games.Global.Armors;
using Games.Global.Entities;
//using Games.Global.Patterns;
using Games.Players;
using UnityEngine;

namespace Games.Global.Weapons
{
    public class WeaponPrefab : MonoBehaviour
    {
        private Weapon weapon;
        private Entity wielder;

        private bool isAttacking;

        [SerializeField] private BoxCollider boxCollider;

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
            } while (animator.GetCurrentAnimatorStateInfo(0).IsName(weapon.animationToPlay)) ;

            wielder.entityPrefab.characterMesh.transform.localPosition = Vector3.zero;

            weapon.FixAngleAttack(false, wielder);

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

            TouchEntity(entity);
        }

        public bool TouchEntity(Entity entity)
        {
            AbilityParameters abilityParameters = new AbilityParameters {origin = wielder, directTarget = entity};

            weapon.OnDamageDealt(abilityParameters);
            foreach (Armor armor in wielder.armors)
            {
                armor.OnDamageDealt(abilityParameters);
            }

            foreach (KeyValuePair<TypeEffect, Effect> effects in wielder.damageDealExtraEffect)
            {
                entity.ApplyEffect(effects.Value);
            }

            int damage = weapon.damage + wielder.att + weapon.oneHitDamageUp;
            if (wielder.underEffects.ContainsKey(TypeEffect.Weak))
            {
                damage /= 2;
            }

            entity.TakeDamage(damage, abilityParameters);

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
    }
}