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
            
            Vector3 rot = proj.transform.localEulerAngles;
            rot.y = transform.parent.eulerAngles.y;
            proj.transform.localEulerAngles = rot;
            proj.SetActive(true);

            ProjectilesPrefab projectilesPrefab = proj.GetComponent<ProjectilesPrefab>();
            projectilesPrefab.rigidbody.AddRelativeForce(Vector3.up * 1000, ForceMode.Acceleration);

            projectilesPrefab.weaponOrigin = this;
        }
        
        private void OnTriggerEnter(Collider other)
        {
            TouchEntity(other);
        }

        public void TouchEntity(Collider other)
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
                entity = playerPrefab.player;
            }
            else
            {
                return;
            }

            AbilityParameters abilityParameters = new AbilityParameters();
            abilityParameters.originDamage = wielder;
            abilityParameters.directTarget = entity;

            weapon.OnDamageDealt(abilityParameters); 
            entity.TakeDamage(weapon.damage, abilityParameters);  
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
