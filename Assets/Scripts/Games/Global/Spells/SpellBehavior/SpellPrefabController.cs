using System.Collections.Generic;
using Games.Global.Spells.SpellParameter;
using Games.Global.Spells.SpellsController;
using PathCreation;
using UnityEngine;

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

            if (traj.objectToFollow != null)
            {
                transform.position = traj.objectToFollow.position;
            } 
            else if (spellComponent.pathCreator != null)
            {
                distanceTravelled += speed * Time.deltaTime;
                transform.position = spellComponent.pathCreator.path.GetPointAtDistance(distanceTravelled, EndOfPathInstruction.Stop);
                transform.rotation = spellComponent.pathCreator.path.GetRotationAtDistance(distanceTravelled, EndOfPathInstruction.Stop);
            }

            if (spellToInstantiate.incrementAmplitudeByTime != Vector3.zero)
            {
                transform.localScale += (spellToInstantiate.incrementAmplitudeByTime * Time.deltaTime);
            }
        }

        public void SetValues(Entity originEntity, SpellComponent originSpellComponent, GameObject children)
        {
            childrenGameObject = children;
            
            casterOfSpell = originEntity;
            spellComponent = originSpellComponent;
            speed = originSpellComponent.trajectory.speed;

            if (enemiesTouchedBySpell == null)
            {
                enemiesTouchedBySpell = new List<Entity>();
            }

            if (alliesTouchedBySpell == null)
            {
                alliesTouchedBySpell = new List<Entity>();
            }
        }

        public void ClearValues()
        {
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
                    cone.SetActive(true);
                    break;
                case Geometry.Sphere:
                    sphereCollider.enabled = true;
                    sphere.SetActive(true);
                    break;
                case Geometry.Square:
                    boxCollider.enabled = true;
                    square.SetActive(true);
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
                        SpellInterpreter.EndSpellComponent(spellComponent);

                    SpellInterpreter.EndSpellComponent(otherSpellComponent);
                }

                return false;
            }
            
            Entity entityEnter = other.GetComponent<EntityPrefab>().entity;

            if ( (casterOfSpell.typeEntity == TypeEntity.MOB && entityEnter.typeEntity == TypeEntity.ALLIES ) ||
                 (casterOfSpell.typeEntity == TypeEntity.ALLIES && entityEnter.typeEntity == TypeEntity.MOB ))
            {
                if (isEnter)
                {
                    enemiesTouchedBySpell.Add(entityEnter);
                }
                else
                {
                    enemiesTouchedBySpell.Remove(entityEnter);
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
                    enemiesTouchedBySpell.Remove(entityEnter);
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

            spellComponent.OnTriggerEnter(other.GetComponent<EntityPrefab>().entity);
            SpellInterpreter.PlaySpellActions(spellComponent, Trigger.ON_TRIGGER_ENTER);

            if (!spellComponent.spellToInstantiate.passingThroughEntity)
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

            spellComponent.OnTriggerExit(other.GetComponent<EntityPrefab>().entity);
            SpellInterpreter.PlaySpellActions(spellComponent, Trigger.ON_TRIGGER_END);
        }
    }
}
