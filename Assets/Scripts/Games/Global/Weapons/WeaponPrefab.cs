using System;
using Games.Global.Abilities;
using Games.Global.Entities;
using UnityEngine;

namespace Games.Global.Weapons
{
    public class WeaponPrefab : MonoBehaviour
    {
        private Weapon weapon;

        private void OnTriggerEnter(Collider other)
        {
            MobPrefab mobPrefab = other.GetComponent<MobPrefab>();
            Monster monster = mobPrefab.GetMonster();

            int damageReceived = (weapon.damage - monster.def) > 0 ? (weapon.damage - monster.def) : 0;
            monster.hp -= damageReceived;

            AbilityParameters abilityParameters = new AbilityParameters();
            monster.OnDamageDealt(abilityParameters);

            if (damageReceived > 0)
            {
                weapon.OnDamageDealt(abilityParameters);   
            }
        }

        public void SetWeapon(Weapon weapon)
        {
            this.weapon = weapon;
        }

        public Weapon GetWeapon()
        {
            return weapon;
        }
    }
}
