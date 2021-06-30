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

        private void FixedUpdate()
        {
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

        public void PlayBasicAttack()
        {
            SpellInterpreter.TriggerWhenEntityAttack(entity.activeSpellComponents);
            
            if (entity.basicAttack != null)
            {
                if (entity.weapon != null)
                {
                    WeaponPrefab weaponPrefab = entity.weapon.weaponPrefab;
                    bool initAttack = weaponPrefab.BasicAttack();

                    if (initAttack)
                    {
                        SpellController.CastSpell(entity, entity.basicAttack);
                    }
                }
            }
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

        public bool CastSpell()
        {
            if (entity.spells == null || entity.spells.Count == 0)
            {
                return false;
            }
            
            switch (entity.GetAttackBehaviorType())
            {
                case AttackBehaviorType.AllSpellsIFirst:
                    foreach (Spell spell in entity.spells)
                    {
                        if (SpellController.CastSpell(entity, spell))
                        {
                            return true;
                        }
                    }
                    break;
                case AttackBehaviorType.Random:
                    int rand = Random.Range(0, entity.spells.Count);
                    SpellController.CastSpell(entity, entity.spells[rand]);

                    return !entity.spells[rand].isOnCooldown;
            }

            return false;
        }

        public void MoveOutFromAOE(SpellPrefabController aoe)
        {
            if (aoe.boxCollider == enabled)
            {
                Bounds boundsBox = aoe.boxCollider.bounds;
                
                Vector3 heading = -((boundsBox.center - transform.position) + boundsBox.size);
                heading *= 1.5f;
                navMeshAgent.SetDestination(heading);

            }
            else if (aoe.meshCollider == enabled)
            {
                Bounds boundsBox = aoe.meshCollider.bounds;
                
                Vector3 heading = -((boundsBox.center - transform.position) + boundsBox.size);
                heading *= 1.5f;
                navMeshAgent.SetDestination(heading);
            }
            else
            {
                Bounds boundsBox = aoe.sphereCollider.bounds;
                
                Vector3 heading = -((boundsBox.center - transform.position) + boundsBox.size);
                heading *= 1.5f;
                navMeshAgent.SetDestination(heading);
            }
        }

        public void AttackTarget()
        {
            transform.LookAt(target.entityPrefab.transform);
            Vector3 localEulerAngles = transform.localEulerAngles;
            localEulerAngles.x = 0;
            localEulerAngles.z = 0;
            transform.localEulerAngles = localEulerAngles;

            if (!CastSpell())
            {
                switch (entity.GetBehaviorType())
                {
                    // TODO : adapt range of weapon for attack
                    case BehaviorType.Distance:
                        PlayBasicAttack();
                        break;
                    case BehaviorType.Melee:
                    case BehaviorType.MoveOnTargetAndDie:
                        if (navMeshAgent.remainingDistance <= 3)
                        {
                            PlayBasicAttack();
                        }
                        break;
                }
            }
        }

        public void MoveToTarget(float range)
        {
            if (!navMeshAgent.enabled)
            {
                return;
            }

            if (entity.isFeared || entity.isCharmed)
            {
                navMeshAgent.SetDestination(transform.position + forcedDirection);
                return;
            }

            if (target != null)
            {
                navMeshAgent.SetDestination(target.entityPrefab.transform.position);

                if (navMeshAgent.remainingDistance <= range && navMeshAgent.hasPath)
                {
                    navMeshAgent.SetDestination(transform.position);
                }
            }
        }

        public void FindTarget()
        {
            Entity newTarget = null;
            bool findEnemyWithTaunt = false;
            float dist = 10000;
            float minDistAggro = 100;

            switch (entity.GetTypeEntity())
            {
                case TypeEntity.MOB:
                    foreach (KeyValuePair<int, PlayerPrefab> value in DataObject.playerInScene)
                    {
                        PlayerPrefab player = value.Value;
                        float newDist = Vector3.Distance(transform.position, player.transform.position);

                        if (newDist > minDistAggro)
                        {
                            continue;
                        }

                        if (!player.entity.isInvisible && !player.entity.isUntargeatable && !player.entity.hasNoAggro)
                        {
                            if ((dist > newDist))
                            {
                                dist = newDist;
                                newTarget = player.entity;
                            }
                        }

                        if (player.entity.hasTaunt)
                        {
                            newTarget = player.entity;
                            findEnemyWithTaunt = true;
                            break;
                        }
                    }

                    foreach (Entity summon in DataObject.invocationsInScene)
                    {
                        float newDist = Vector3.Distance(transform.position, summon.entityPrefab.transform.position);
                        if (newDist > minDistAggro)
                        {
                            continue;
                        }

                        if (!summon.isInvisible && !summon.isUntargeatable && !summon.hasNoAggro && !findEnemyWithTaunt)
                        {
                            if ((dist > newDist))
                            {
                                dist = newDist;
                                newTarget = summon;
                            }
                        }

                        if (summon.hasTaunt)
                        {
                            newTarget = summon;
                            break;
                        }
                    }

                    break;
                case TypeEntity.ALLIES:
                    foreach (Monster monster in DataObject.monsterInScene)
                    {
                        if (!monster.entityPrefab)
                        {
                            continue;
                        }

                        float newDist = Vector3.Distance(transform.position, monster.entityPrefab.transform.position);
                        if (newDist > minDistAggro)
                        {
                            continue;
                        }

                        if (!monster.isInvisible && !monster.isUntargeatable && !monster.hasNoAggro)
                        {
                            if ((dist > newDist))
                            {
                                dist = newDist;
                                newTarget = monster;
                            }
                        }

                        if (monster.hasTaunt)
                        {
                            newTarget = monster;
                            break;
                        }
                    }

                    break;
            }

            target = newTarget;
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
    }
}