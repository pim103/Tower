using System;
using System.Collections;
using System.Linq.Expressions;
using Games.Global.Entities;
using Games.Global.Spells;
using Games.Global.Spells.SpellsController;
//using Games.Global.Patterns;
using Games.Global.Weapons;
using UnityEngine;
using UnityEngine.Serialization;

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
//        [SerializeField] public GameObject virtualHand;
        
        // hand play animation of weapon
//        [SerializeField] public GameObject hand;

        [SerializeField] public GameObject rightHand;
        [SerializeField] public GameObject leftHand;
        
//        [SerializeField] private MovementPatternController movementPatternController;
        [SerializeField] private Rigidbody rigidbodyEntity;
        
        [SerializeField] protected MeshRenderer meshRenderer;

        [SerializeField] public Animator animator;

        [SerializeField] public GameObject characterMesh;

        [SerializeField] public Transform positionInRightHand;
        [SerializeField] public Transform angleWithOneRight;
        
        [SerializeField] public Transform positionInLeftHand;
        [SerializeField] public Transform angleWithOneLeft;

        public Entity entity;

        public bool cameraBlocked = false;
        public bool intentBlocked = false;

        public bool isCharging = false;

        public bool canDoSomething = true;
        public bool canMove = true;

        public Vector3 forcedDirection = Vector3.zero;

        public Vector3 positionPointed;

        public void WantToApplyForce(Vector3 direction, int level)
        {
            StartCoroutine(ApplyForce(direction, level));
        }

        public IEnumerator ApplyForce(Vector3 direction, int level)
        {
            rigidbodyEntity.isKinematic = false;
            
            rigidbodyEntity.AddForce((direction * level * 5), ForceMode.Impulse);
            yield return new WaitForSeconds(1);
            rigidbodyEntity.isKinematic = true;
        }

        public void PlayBasicAttack(WeaponPrefab weaponPrefab)
        {
            BuffController.EntityAttack(entity, positionPointed);

            weaponPrefab.BasicAttack();
        }

        public void AddItemInHand(Weapon weapon)
        {
            GameObject hand = rightHand;
            Transform position = positionInRightHand;
            Transform angle = angleWithOneRight;

            if (weapon.category == CategoryWeapon.BOW)
            {
                hand = leftHand;
                position = positionInLeftHand;
                angle = angleWithOneLeft;
            }

            GameObject weaponGameObject = Instantiate(weapon.model, hand.transform, true);

            WeaponPrefab weaponPrefab = weaponGameObject.GetComponent<WeaponPrefab>();
            weapon.weaponPrefab = weaponPrefab;
            weaponPrefab.SetWielder(entity);
            weaponPrefab.SetWeapon(weapon);
            weaponPrefab.SetPositionToParent(position, angle);
        }

        public void SetMaterial(Material material)
        {
            meshRenderer.material = material;
        }

        public virtual void SetInvisibility()
        {
        }
    }
}