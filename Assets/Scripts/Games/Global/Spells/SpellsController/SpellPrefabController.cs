using System;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;

namespace Games.Global.Spells.SpellsController
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

        public SpellComponent spellComponent;
        public Entity originOfSpell;

        public void SetValues(Entity originEntity, SpellComponent originSpellComponent)
        {
            originOfSpell = originEntity;
            spellComponent = originSpellComponent;
        }

        public void ActiveCollider(Geometry geometry)
        {
            square.SetActive(false);
            sphere.SetActive(false);
            cone.SetActive(false);
            
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

        private void OnTriggerEnter(Collider other)
        {
            int playerLayer = LayerMask.NameToLayer("Player");
            int monsterLayer = LayerMask.NameToLayer("Monster");
            int spellLayer = LayerMask.NameToLayer("Spell");

//            Debug.Log("List layer : player => " + playerLayer + " monster => " + monsterLayer + " spell => " + spellLayer);
//            Debug.Log("Other layer : " + other.gameObject.layer);
            if (other.gameObject.layer != playerLayer && other.gameObject.layer != monsterLayer &&
                other.gameObject.layer != spellLayer)
            {
                return;
            }

            if (spellComponent == null)
            {
                return;
            }

            switch (spellComponent.typeSpell)
            {
                case TypeSpell.Buff:
                    break;
                case TypeSpell.Projectile:
                    ProjectileController.EntityTriggerEnter(originOfSpell, other, (ProjectileSpell) spellComponent, other.gameObject.layer == spellLayer);
                    break;
                case TypeSpell.Summon:
                    break;
                case TypeSpell.Transformation:
                    break;
                case TypeSpell.Wave:
                    WaveController.EntityTriggerEnter(originOfSpell, other, (WaveSpell) spellComponent, other.gameObject.layer == spellLayer);
                    break;
                case TypeSpell.AreaOfEffect:
                    AreaOfEffectController.EntityTriggerEnter(originOfSpell, other, (AreaOfEffectSpell)spellComponent, other.gameObject.layer == spellLayer);
                    break;
                case TypeSpell.Movement:
                    break;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            int playerLayer = LayerMask.NameToLayer("Player");
            int monsterLayer = LayerMask.NameToLayer("Monster");

            if (other.gameObject.layer != playerLayer && other.gameObject.layer != monsterLayer)
            {
                return;
            }

            Entity entityEnter = other.GetComponent<EntityPrefab>().entity;

            if (spellComponent == null)
            {
                return;
            }

            switch (spellComponent.typeSpell)
            {
                case TypeSpell.Buff:
                    break;
                case TypeSpell.Projectile:
                    break;
                case TypeSpell.Summon:
                    break;
                case TypeSpell.Transformation:
                    break;
                case TypeSpell.Wave:
                    break;
                case TypeSpell.AreaOfEffect:
                    AreaOfEffectController.EntityTriggerExit(originOfSpell, entityEnter, (AreaOfEffectSpell)spellComponent);
                    break;
                case TypeSpell.Movement:
                    break;
            }
        }
    }
}
