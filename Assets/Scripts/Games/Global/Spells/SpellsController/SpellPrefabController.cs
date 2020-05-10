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

        public SpellComponent spellComponent;
        public Entity originOfSpell;

//        public void Update()
//        {
//            if (spellComponent != null && spellComponent.typeSpell == TypeSpell.AreaOfEffect)
//            {
//                AreaOfEffectSpell area = (AreaOfEffectSpell) spellComponent;
//
//                if (area.wantToFollow && area.transformToFollow)
//                {
//                    transform.position = area.transformToFollow.position;
//                }
//            }
//        }

        public void SetValues(Entity originEntity, SpellComponent originSpellComponent)
        {
            originOfSpell = originEntity;
            spellComponent = originSpellComponent;

//            if (originSpellComponent.typeSpell == TypeSpell.AreaOfEffect)
//            {
//                rigidbody.isKinematic = true;
//            }
//            else
//            {
//                rigidbody.isKinematic = false;
//            }
        }

        public void ActiveCollider(Geometry geometry)
        {
            switch (geometry)
            {
                case Geometry.Cone:
                    meshCollider.enabled = true;
                    break;
                case Geometry.Sphere:
                    sphereCollider.enabled = true;
                    break;
                case Geometry.Square:
                    boxCollider.enabled = true;
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
                case TypeSpell.SpecialAttack:
                    break;
                case TypeSpell.AreaOfEffect:
                    AreaOfEffectController.EntityTriggerEnter(originOfSpell, other, (AreaOfEffectSpell)spellComponent, other.gameObject.layer == spellLayer);
                    break;
                case TypeSpell.TargetedAttack:
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
                case TypeSpell.SpecialAttack:
                    break;
                case TypeSpell.AreaOfEffect:
                    AreaOfEffectController.EntityTriggerExit(originOfSpell, entityEnter, (AreaOfEffectSpell)spellComponent);
                    break;
                case TypeSpell.TargetedAttack:
                    break;
                case TypeSpell.Movement:
                    break;
            }
        }
    }
}
