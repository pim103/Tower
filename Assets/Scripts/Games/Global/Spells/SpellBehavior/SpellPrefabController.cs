using System;
using System.Collections.Generic;
using Games.Global.Entities;
using Games.Global.Spells.SpellParameter;
using Games.Global.Spells.SpellsController;
using PathCreation;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Games.Global.Spells.SpellBehavior
{
    public class SpellPrefabController : MonoBehaviour
    {
        [SerializeField] public SphereCollider sphereCollider;
        [SerializeField] public BoxCollider boxCollider;
        [SerializeField] public MeshCollider meshCollider;
        [SerializeField] public Rigidbody rigidbody;

        [SerializeField] public GameObject sphere;
        [SerializeField] public GameObject cone;
        [SerializeField] public GameObject square;

        // Use when idPoolObject of SpellToInstantiate is set
        public GameObject childrenGameObject;
        
        private SpellComponent spellComponent;
        private Entity casterOfSpell;

        public List<Entity> enemiesTouchedBySpell;
        public List<Entity> alliesTouchedBySpell;

        public float distanceTravelled;
        private float speed;

        public void Update()
        {
            Trajectory traj = spellComponent.trajectory;
            SpellToInstantiate spellToInstantiate = spellComponent.spellToInstantiate;

            // Si il y a un path creator, on privilégie le follow de trajectoire - sinon, on cherche l'object to follow
            if (spellComponent.pathCreator != null)
            {
                Vector3 lastPosition = transform.position;
                
                distanceTravelled += speed * Time.deltaTime;
                transform.position = spellComponent.pathCreator.path.GetPointAtDistance(distanceTravelled, traj.endOfPathInstruction);
                transform.rotation = spellComponent.pathCreator.path.GetRotationAtDistance(distanceTravelled, traj.endOfPathInstruction);

                if (lastPosition == transform.position && traj.disapearAtTheEndOfTrajectory)
                {
                    SpellInterpreter.EndSpellComponent(spellComponent);
                }
            }
            else if (traj != null && traj.objectToFollow != null)
            {
                SetPosition(traj.objectToFollow.position, traj.objectToFollow);
            }

            if (spellToInstantiate.incrementAmplitudeByTime != Vector3.zero)
            {
                transform.localScale += (spellToInstantiate.incrementAmplitudeByTime * Time.deltaTime);
                if (childrenGameObject) childrenGameObject.transform.localScale += (spellToInstantiate.incrementAmplitudeByTime * Time.deltaTime);
            }
        }

        public void SetPosition(Vector3 startPosition, Transform transformMarker)
        {
            Vector3 offset = spellComponent.spellToInstantiate.offsetStartPosition;
            Vector3 forward = transformMarker != null ? transformMarker.forward : Vector3.forward;
            Vector3 position = startPosition;

            position += forward * offset.z + forward * offset.x +
                        Vector3.up * offset.y;
            
            transform.position = position;
            transform.forward = forward;
        }

        public void SetSpellParameter(SpellComponent originSpellComponent, Vector3 startPosition, bool initChild = true)
        {
            casterOfSpell = originSpellComponent.caster;
            spellComponent = originSpellComponent;

            transform.localScale = originSpellComponent.spellToInstantiate.scale;
            SetPosition(startPosition, casterOfSpell?.entityPrefab.transform);

            if (originSpellComponent.trajectory != null)
            {
                speed = originSpellComponent.trajectory.speed;
            }

            if (enemiesTouchedBySpell == null)
            {
                enemiesTouchedBySpell = new List<Entity>();
            }

            if (alliesTouchedBySpell == null)
            {
                alliesTouchedBySpell = new List<Entity>();
            }

            ActiveCollider(originSpellComponent.spellToInstantiate.geometry);

            if (initChild)
            {
                InitChildObject();
            }
        }

        private void InitChildObject()
        {
            if (!String.IsNullOrEmpty(spellComponent.spellToInstantiate.pathGameObjectToInstantiate) && !childrenGameObject)
            {
                GameObject wantedGo =
                    Resources.Load<GameObject>(spellComponent.spellToInstantiate.pathGameObjectToInstantiate);

                childrenGameObject = Object.Instantiate(wantedGo, transform, true);

                Vector3 parentScale = transform.localScale;
                Vector3 offset = spellComponent.spellToInstantiate.offsetObjectToInstantiate;
                offset.x /= parentScale.x;
                offset.y /= parentScale.y;
                offset.z /= parentScale.z;
                childrenGameObject.transform.localPosition = offset;
                childrenGameObject.transform.localEulerAngles = Vector3.zero;
                childrenGameObject.transform.localScale = wantedGo.transform.localScale;
                childrenGameObject.SetActive(true);   
            }
        }

        public void ClearValues()
        {
            foreach (Entity entity in enemiesTouchedBySpell)
            {
                entity.inNefastSpells.Remove(this);
            }

            square.SetActive(false);
            sphere.SetActive(false);
            cone.SetActive(false);
            meshCollider.enabled = false;
            sphereCollider.enabled = false;
            boxCollider.enabled = false;

            spellComponent = null;
            casterOfSpell = null;
            distanceTravelled = 0;
            speed = 0;
            childrenGameObject = null;
            enemiesTouchedBySpell.Clear();
            alliesTouchedBySpell.Clear();
        }

        public void ActiveCollider(Geometry geometry, bool isProj = false)
        {
            square.SetActive(false);
            sphere.SetActive(false);
            cone.SetActive(false);
            meshCollider.enabled = false;
            sphereCollider.enabled = false;
            boxCollider.enabled = false;

            if (isProj)
            {
                return;
            }

            switch (geometry)
            {
                case Geometry.Cone:
                    meshCollider.enabled = true;
                    // cone.SetActive(true);
                    break;
                case Geometry.Sphere:
                    sphereCollider.enabled = true;
                    // sphere.SetActive(true);
                    break;
                case Geometry.Square:
                    boxCollider.enabled = true;
                    // square.SetActive(true);
                    break;
            }
        }

        private bool OnTriggerCheckOtherType(Collider other, bool isEnter)
        {
            int playerLayer = LayerMask.NameToLayer("Player");
            int monsterLayer = LayerMask.NameToLayer("Monster");
            int spellLayer = LayerMask.NameToLayer("Spell");
            int wallLayer = LayerMask.NameToLayer("Wall");

            if ((other.gameObject.layer != playerLayer && other.gameObject.layer != monsterLayer &&
                 other.gameObject.layer != spellLayer && other.gameObject.layer != wallLayer) ||
                spellComponent == null)
            {
                return false;
            }

            if (other.gameObject.layer == wallLayer)
            {
                if (!spellComponent.spellToInstantiate.passingThroughEntity)
                {
                    SpellInterpreter.EndSpellComponent(spellComponent);
                }

                return false;
            }

            if (other.gameObject.layer == spellLayer)
            {
                if (spellComponent.canStopProjectile)
                {
                    SpellComponent otherSpellComponent = other.GetComponent<SpellPrefabController>().spellComponent;
                    SpellInterpreter.EndSpellComponent(otherSpellComponent);
                }

                return false;
            }
            
            Entity entityEnter = other.GetComponent<ColliderEntityExposer>().entityPrefab.entity;

            if ( (casterOfSpell.GetTypeEntity() == TypeEntity.MOB && entityEnter.GetTypeEntity() == TypeEntity.ALLIES ) ||
                 (casterOfSpell.GetTypeEntity() == TypeEntity.ALLIES && entityEnter.GetTypeEntity() == TypeEntity.MOB ))
            {
                if (enemiesTouchedBySpell.Contains(entityEnter))
                {
                    return false;
                }

                if (isEnter)
                {
                    enemiesTouchedBySpell.Add(entityEnter);
                    entityEnter.inNefastSpells.Add(this);
                }
                else
                {
                    enemiesTouchedBySpell.Remove(entityEnter);
                    entityEnter.inNefastSpells.Remove(this);
                }
            }
            else
            {
                if (isEnter)
                {
                    alliesTouchedBySpell.Add(entityEnter);
                }
                else
                {
                    alliesTouchedBySpell.Remove(entityEnter);
                }
            }

            return true;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!OnTriggerCheckOtherType(other, true))
            {
                return;
            }

            EntityPrefab entityPrefab = other.GetComponent<ColliderEntityExposer>().entityPrefab;
            bool doSomething = spellComponent.OnTriggerEnter(entityPrefab.entity);
            bool hasFindingAction = SpellInterpreter.PlaySpellActions(spellComponent, Trigger.ON_TRIGGER_ENTER);

            /*if (other.gameObject.layer == LayerMask.NameToLayer("Monster") && entityPrefab.ragdollCoroutine == null)
            {
                //entityPrefab.LaunchEnableRagdoll(20);
                entityPrefab.animator.SetTrigger("Got Hit");
                entityPrefab.audioSource.PlayOneShot(entityPrefab.hitClip);
            }*/

            if ((hasFindingAction || doSomething) && !spellComponent.spellToInstantiate.passingThroughEntity)
            {
                SpellInterpreter.EndSpellComponent(spellComponent);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (!OnTriggerCheckOtherType(other, false))
            {
                return;
            }

            spellComponent.OnTriggerExit(other.GetComponent<ColliderEntityExposer>().entityPrefab.entity);
            SpellInterpreter.PlaySpellActions(spellComponent, Trigger.ON_TRIGGER_END);
        }
    }
}
