using System.Diagnostics;
using Games.Global.Abilities;
using Games.Global.Entities;
using Games.Players;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Games.Global.Weapons
{
    public class WeaponPrefab : MonoBehaviour
    {
        private Weapon weapon;
        private Entity wielder;

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
                MobPrefab mobPrefab = other.GetComponent<MobPrefab>();
                entity = mobPrefab.GetMonster();
            } else if (other.gameObject.layer == playerLayer && wielder.typeEntity != TypeEntity.PLAYER)
            {
                entity = other.transform.parent.GetComponent<Player>();
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
