using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.Expressions;
using Games.Global.Entities;
using Games.Global.Spells;
using Games.Global.Spells.SpellsController;
//using Games.Global.Patterns;
using Games.Global.Weapons;
using Games.Players;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Assertions.Comparers;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Games.Global
{
    public class EntityPrefab : MonoBehaviour
    {
        [SerializeField] public GameObject rightHand;
        [SerializeField] public GameObject leftHand;

        [SerializeField] private Rigidbody rigidbodyEntity;

        [SerializeField] protected MeshRenderer meshRenderer;

        [SerializeField] public Animator animator;

        [SerializeField] public GameObject characterMesh;

        [SerializeField] public Transform positionInRightHand;
        [SerializeField] public Transform angleWithOneRight;

        [SerializeField] public Transform positionInLeftHand;
        [SerializeField] public Transform angleWithOneLeft;
        [SerializeField] public NavMeshAgent navMeshAgent;
        
        public Entity entity;

        public bool cameraBlocked = false;
        public bool intentBlocked = false;

        public bool isCharging = false;

        public bool canDoSomething = true;
        public bool canMove = true;

        public Vector3 forcedDirection = Vector3.zero;

        public Vector3 positionPointed;

        public Entity target;

        private void FixedUpdate()
        {
            if (entity.BehaviorType == BehaviorType.Player)
            {
                return;
            }

            if (!canDoSomething)
            {
                navMeshAgent.SetDestination(transform.position);
                return;
            }

            FindTarget();

            if (canMove)
            {
                if (entity.BehaviorType == BehaviorType.Melee ||
                    entity.BehaviorType == BehaviorType.MoveOnTargetAndDie)
                {
                    MoveToTarget(1);
                }
                else if (entity.BehaviorType == BehaviorType.Distance)
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
            }
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
            BuffController.EntityAttack(entity, positionPointed);

            if (entity.weapons.Count > 0)
            {
                WeaponPrefab weaponPrefab = entity.weapons[0].weaponPrefab;
                weaponPrefab.BasicAttack();    
            }

            if (entity.basicAttack != null)
            {
                SpellController.CastSpell(entity, entity.basicAttack, transform.position + (Vector3.up * 1.5f),  entity);
            }
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

            entity.basicAttack = weapon.basicAttack;
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
            
            switch (entity.AttackBehaviorType)
            {
                case AttackBehaviorType.AllSpellsIFirst:
                    foreach (Spell spell in entity.spells)
                    {
                        if (!spell.isOnCooldown)
                        {
                            SpellController.CastSpell(entity, spell, transform.position, target);
                            return true;
                        }
                    }
                    break;
                case AttackBehaviorType.Random:
                    int rand = Random.Range(0, entity.spells.Count);
                    SpellController.CastSpell(entity, entity.spells[rand], transform.position, target);

                    return !entity.spells[rand].isOnCooldown;
            }

            return false;
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
                switch (entity.BehaviorType)
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

            switch (entity.typeEntity)
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
            if (entity.typeEntity == TypeEntity.MOB)
            {
                if (!entity.isSummon)
                {
                    DataObject.monsterInScene.Remove((Monster) entity);
                }
                else
                {
                    DataObject.invocationsInScene.Remove(entity);
                }
            }
            else if (entity.typeEntity == TypeEntity.ALLIES)
            {
                if (entity.isPlayer)
                {
                    DataObject.playerInScene.Remove(GameController.PlayerIndex);
                }
                else if (entity.isSummon)
                {
                    DataObject.invocationsInScene.Remove(entity);
                }
            }

            gameObject.SetActive(false);
        }
    }
}