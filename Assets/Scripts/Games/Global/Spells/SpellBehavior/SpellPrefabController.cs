using System;
using System.Collections.Generic;
using Games.Global.Spells.SpellParameter;
using Games.Global.Spells.SpellsController;
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

        private SpellComponent spellComponent;
        private Entity casterOfSpell;

        public List<Entity> enemiesTouchedBySpell;
        public List<Entity> alliesTouchedBySpell;

        public void Update()
        {
            Trajectory traj = spellComponent.trajectory;
        }

        public void SetValues(Entity originEntity, SpellComponent originSpellComponent)
        {
            casterOfSpell = originEntity;
            spellComponent = originSpellComponent;
            
            enemiesTouchedBySpell = new List<Entity>();
            alliesTouchedBySpell = new List<Entity>();
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
            
            // TODO : Implément le cas lors de la collision avec des spells et des murs
            if (other.gameObject.layer == spellLayer && other.gameObject.layer == wallLayer)
            {
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
            if (OnTriggerCheckOtherType(other, true))
            {
                return;
            }

            SpellInterpreter.PlaySpellActions(spellComponent, Trigger.ON_TRIGGER_ENTER);
        }

        private void OnTriggerExit(Collider other)
        {
            if (OnTriggerCheckOtherType(other, false))
            {
                return;
            }

            SpellInterpreter.PlaySpellActions(spellComponent, Trigger.ON_TRIGGER_END);
        }
    }
}
