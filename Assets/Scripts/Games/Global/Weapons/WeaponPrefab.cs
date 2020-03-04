using Games.Global.Abilities;
using Games.Global.Entities;
using UnityEngine;

namespace Games.Global.Weapons
{
    public class WeaponPrefab : MonoBehaviour
    {
        private Weapon weapon;
        private Entity wielder;

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log(other);
            MobPrefab mobPrefab = other.GetComponent<MobPrefab>();
            Monster monster = mobPrefab.GetMonster();

            AbilityParameters abilityParameters = new AbilityParameters();
            abilityParameters.originDamage = wielder;
            abilityParameters.directTarget = monster;

            weapon.OnDamageDealt(abilityParameters); 
            monster.TakeDamage(weapon.damage, abilityParameters);  
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
