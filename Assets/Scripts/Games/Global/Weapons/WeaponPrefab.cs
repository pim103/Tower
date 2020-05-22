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

            weapon.FixAngleAttack(true, wielder);

            do
            {
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
            wielder = entity;
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