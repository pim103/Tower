using System.Collections;
using Games.Global.Spells;
using UnityEngine;

namespace Games.Global.Weapons
{
    public class WeaponPrefab : MonoBehaviour
    {
        private Weapon weapon;
        private Entity wielder;

        public bool isAttacking;

        [SerializeField] private Transform positionInHand;

        public IEnumerator PlayAnimationAttack(Spell basicAttack)
        {
            Animator animator = wielder.entityPrefab.animator;
            float initialSpeed = animator.speed;

            //animator.speed *= (wielder.attSpeed + weapon.attSpeed);
            animator.Play(weapon.animationToPlay);

            //weapon.FixAngleAttack(true, wielder);

            do
            {
                yield return new WaitForSeconds(0.1f);
            } while (animator.GetCurrentAnimatorStateInfo(0).IsName("ok"));

            //animator.SetBool(weapon.animationToPlay, false);
            //weapon.FixAngleAttack(false, wielder);

            wielder.entityPrefab.characterMesh.transform.localPosition = Vector3.zero;
            wielder.entityPrefab.characterMesh.transform.localEulerAngles = Vector3.zero;

            animator.speed = initialSpeed;
            isAttacking = false;
        }

        public bool BasicAttack()
        {
            bool initAttack = !isAttacking;

            if (!isAttacking)
            {
                Animator animator = wielder.entityPrefab.animator;
                //animator.SetBool(weapon.animationToPlay, true);
                isAttacking = true;
            }

            return initAttack;
        }

        public void DeactivateBoolAttack()
        {
            Animator animator = wielder.entityPrefab.animator;
            //animator.SetBool(weapon.animationToPlay, false);
            isAttacking = false;
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