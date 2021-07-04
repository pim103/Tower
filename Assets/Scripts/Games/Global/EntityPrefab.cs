using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using Games.Global.Entities;
using Games.Global.Spells;
using Games.Global.Spells.SpellBehavior;
using Games.Global.Spells.SpellsController;
using Games.Global.Weapons;
using Games.Players;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace Games.Global
{
    public class EntityPrefab : MonoBehaviour
    {
        [SerializeField] public GameObject rightHand;
        [SerializeField] public GameObject leftHand;

        [SerializeField] private Rigidbody rigidbodyEntity;

        [SerializeField] protected MeshRenderer meshRenderer;
        [SerializeField] public MeshRenderer[] mrShaderTargets;
        [SerializeField] public SkinnedMeshRenderer[] smrShaderTargets;

        [SerializeField] public Animator animator;

        [SerializeField] public GameObject characterMesh;

        [SerializeField] public Transform positionInRightHand;
        [SerializeField] public Transform angleWithOneRight;

        [SerializeField] public Transform positionInLeftHand;
        [SerializeField] public Transform angleWithOneLeft;
        [SerializeField] public NavMeshAgent navMeshAgent;
        
        [SerializeField] public GameObject headCurve;
        [SerializeField] public GameObject thornSphere;
        [SerializeField] public GameObject distortionSphere;

        [SerializeField] public Material burningMaterial;
        
        public Entity entity;

        public bool cameraBlocked = false;
        public bool intentBlocked = false;

        public bool isCharging = false;

        public bool canDoSomething = true;
        public bool canMove = true;

        public Vector3 forcedDirection = Vector3.zero;

        public Vector3 positionPointed;

        public Entity target;

        public Coroutine ragdollCoroutine;

        [SerializeField] private Rigidbody[] entityRigidbodies;
        [SerializeField] public AudioSource audioSource;
        [SerializeField] public AudioClip hitClip;
        [SerializeField] public AudioClip swordAttackClip;
        [SerializeField] public AudioClip bowAttackClip;
        [SerializeField] public AudioClip footstepsClip;

        public Coroutine footstepsRoutine;

        private void FixedUpdate()
        {
            if (navMeshAgent && entity != null)
            {
                navMeshAgent.speed = entity.speed / 2;
            }

            if (entity.GetBehaviorType() == BehaviorType.Player)
            {
                return;
            }

            if (!canDoSomething)
            {
                navMeshAgent.SetDestination(transform.position);
                return;
            }

            //FindTarget();

            /*if (canMove)
            {
                if (entity.GetBehaviorType() == BehaviorType.Melee ||
                    entity.GetBehaviorType() == BehaviorType.MoveOnTargetAndDie)
                {
                    MoveToTarget(1);
                }
                else if (entity.GetBehaviorType() == BehaviorType.Distance)
                {
                    MoveToTarget(10);
                }
            }
            else
            {
                navMeshAgent.SetDestination(transform.position);
            }

            if (target != null)
            {
                AttackTarget();   
            }*/
        }

        public void WantToApplyForce(Vector3 direction, int level)
        {
            StartCoroutine(ApplyForce(direction, level));
        }

        public IEnumerator ApplyForce(Vector3 direction, int level)
        {
            if (navMeshAgent != null)
            {
                navMeshAgent.enabled = false;
            }

            rigidbodyEntity.isKinematic = false;

            rigidbodyEntity.AddForce((direction * level * 5), ForceMode.Impulse);
            yield return new WaitForSeconds(1);
            rigidbodyEntity.isKinematic = true;
            
            if (navMeshAgent != null)
            {
                navMeshAgent.enabled = true;
            }
        }

        public bool PlayBasicAttack()
        {
            SpellInterpreter.TriggerWhenEntityAttack(entity.activeSpellComponents);

            if (entity.basicAttack != null)
            {
                entity.basicAttack.cooldown = 1 / entity.attSpeed;
                SpellController.CastSpell(entity, entity.basicAttack);
                return true;
            }

            return false;
        }

        public void CancelBasicAttack()
        {
            if (entity.weapon != null)
            {
                WeaponPrefab weaponPrefab = entity.weapon.weaponPrefab;
                weaponPrefab.DeactivateBoolAttack();    
            }
        }

        public void AddItemInHand(Weapon weapon)
        {
            GameObject hand = rightHand;
            Transform position = positionInRightHand;
            Transform angle = angleWithOneRight;

            if (weapon.category != null && weapon.category.name == "BOW")
            {
                hand = leftHand;
                position = positionInLeftHand;
                angle = angleWithOneLeft;
            }

            if (entity.weapon != null && entity.weapon.weaponPrefab)
            {
                Destroy(entity.weapon.weaponPrefab.gameObject);
            }

            GameObject weaponGameObject = Instantiate(weapon.model, hand.transform, true);

            WeaponPrefab weaponPrefab = weaponGameObject.GetComponent<WeaponPrefab>();
            weapon.weaponPrefab = weaponPrefab;

            weaponPrefab.SetWielder(entity);
            weaponPrefab.SetWeapon(weapon);
            weaponPrefab.SetPositionToParent(position, angle);

            weapon.InitWeapon();
        }

        public virtual void SetInvisibility()
        {
        }

        public void MoveOutFromAOE(SpellPrefabController aoe)
        {
            if (aoe.boxCollider == enabled)
            {
                Bounds boundsBox = aoe.boxCollider.bounds;
                
                Vector3 heading = -((boundsBox.center - transform.position) + boundsBox.size);
                heading *= 1.5f;

                if (navMeshAgent.enabled)
                {
                    navMeshAgent.SetDestination(heading);
                }

                animator.SetFloat("Locomotion",1.0f);

            }
            else if (aoe.meshCollider == enabled)
            {
                Bounds boundsBox = aoe.meshCollider.bounds;
                
                Vector3 heading = -((boundsBox.center - transform.position) + boundsBox.size);
                heading *= 1.5f;

                if (navMeshAgent.enabled)
                {
                    navMeshAgent.SetDestination(heading);
                }

                animator.SetFloat("Locomotion",1.0f);
            }
            else
            {
                Bounds boundsBox = aoe.sphereCollider.bounds;
                
                Vector3 heading = -((boundsBox.center - transform.position) + boundsBox.size);
                heading *= 1.5f;

                if (navMeshAgent.enabled)
                {
                    navMeshAgent.SetDestination(heading);
                }

                animator.SetFloat("Locomotion",1.0f);
            }
        }

        public void EntityDie()
        {
            if (entity.GetTypeEntity() == TypeEntity.MOB)
            {
                if (!entity.isSummon)
                {
                    DataObject.monsterInScene.Remove((Monster) entity);
                    MonsterPrefab monsterPrefab = (MonsterPrefab) this;
                    foreach (GameObject objectToLoot in monsterPrefab.objectsToLoot)
                    {
                        objectToLoot.SetActive(true);
                        objectToLoot.transform.position = transform.position;
                    }
                }
                else
                {
                    DataObject.invocationsInScene.Remove(entity);
                }
            }
            else if (entity.GetTypeEntity() == TypeEntity.ALLIES)
            {
                if (entity.isPlayer)
                {
                    DataObject.playerInScene.Remove(GameController.PlayerIndex);
                    GameController.mainCamera.SetActive(true);
                }
                else if (entity.isSummon)
                {
                    DataObject.invocationsInScene.Remove(entity);
                }
            }

            gameObject.SetActive(false);
        }

        public void LaunchEnableRagdoll(int time)
        {
            if (!gameObject.activeSelf)
            {
                return;
            }

            ragdollCoroutine = StartCoroutine(EnableRagdoll(time));
        }

        private IEnumerator EnableRagdoll(int time)
        {
            animator.enabled = false;
            navMeshAgent.enabled = false;
            int cpt = 0;

            foreach (Rigidbody rigidbody in entityRigidbodies)
            {
                rigidbody.useGravity = true;
            }

            while (cpt < time)
            {
                cpt++;
                yield return new WaitForSeconds(0.1f);
            }

            foreach (Rigidbody rigidbody in entityRigidbodies)
            {
                rigidbody.useGravity = false;
            }

            animator.enabled = true;
            navMeshAgent.enabled = true;
            ragdollCoroutine = null;
        }

        public IEnumerator PlayFootsteps()
        {
            while (true)
            {
                audioSource.PlayOneShot(footstepsClip);
                yield return new WaitForSeconds(0.4f);
            }
        }
    }
}