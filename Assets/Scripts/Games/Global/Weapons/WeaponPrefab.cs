using System.Diagnostics;
using Games.Global.Abilities;
using Games.Global.Entities;
using Games.Global.Patterns;
using Games.Players;
using UnityEngine;
using UnityEngine.Serialization;
using Debug = UnityEngine.Debug;

namespace Games.Global.Weapons
{
    public class WeaponPrefab : MonoBehaviour
    {
        private Weapon weapon;
        private Entity wielder;

        [SerializeField] private BoxCollider boxCollider;

        public void BasicAttack(MovementPatternController movementPatternController, GameObject objectToMove)
        {
            if (!boxCollider.enabled)
            {
                boxCollider.enabled = true;
            
                PlayMovement(movementPatternController, weapon.attSpeed, objectToMove, boxCollider);

                if (weapon.type == TypeWeapon.Distance)
                {
                    PoolProjectiles();
                }
            }
        }
        
        private void PlayMovement(MovementPatternController movementPatternController, float attSpeed, GameObject objectToMove, BoxCollider bc)
        {
            movementPatternController.PlayMovement(weapon.pattern, attSpeed, objectToMove, bc);
        }

        private void PoolProjectiles()
        {
            GameObject proj = ObjectPooler.SharedInstance.GetPooledObject(0);

            proj.transform.position = transform.position;
            float rotX = proj.transform.localEulerAngles.x;

            proj.transform.localEulerAngles = transform.parent.eulerAngles + (Vector3.right * rotX);
            proj.SetActive(true);

            ProjectilesPrefab projectilesPrefab = proj.GetComponent<ProjectilesPrefab>();
            projectilesPrefab.rigidbody.AddForce(transform.right * -1000, ForceMode.Acceleration);

            projectilesPrefab.weaponOrigin = this;
        }

        private void OnTriggerEnter(Collider other)
        {
            TouchEntity(other);
        }

        public bool TouchEntity(Collider other)
        {
            int monsterLayer = LayerMask.NameToLayer("Monster");
            int playerLayer = LayerMask.NameToLayer("Player");

            Entity entity;

            if (other.gameObject.layer == monsterLayer && wielder.typeEntity != TypeEntity.MOB)
            {
                MonsterPrefab monsterPrefab = other.GetComponent<MonsterPrefab>();
                entity = monsterPrefab.GetMonster();
            } else if (other.gameObject.layer == playerLayer && wielder.typeEntity != TypeEntity.PLAYER)
            {
                PlayerPrefab playerPrefab = other.transform.parent.GetComponent<PlayerPrefab>();
                entity = playerPrefab.entity;
            }
            else
            {
                return false;
            }

            if (entity.IdEntity == wielder.IdEntity &&
                ((other.gameObject.layer == monsterLayer && wielder.typeEntity == TypeEntity.MOB) ||
                 (other.gameObject.layer == playerLayer && wielder.typeEntity == TypeEntity.PLAYER))
            )
            {
                return false;
            }

            AbilityParameters abilityParameters = new AbilityParameters();
            abilityParameters.origin = wielder;
            abilityParameters.directTarget = entity;

            weapon.OnDamageDealt(abilityParameters); 
            entity.TakeDamage(weapon.damage, abilityParameters);

            return true;
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
            this.wielder = entity;
        }

        public Entity GetWielder()
        {
            return wielder;
        }
    }
}
