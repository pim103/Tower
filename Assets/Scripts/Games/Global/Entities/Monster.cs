using System;
using System.Collections.Generic;
using Games.Global.Abilities;
using Games.Global.Weapons;
using UnityEngine;

namespace Games.Global.Entities
{
    public class Monster: Entity
    {
        public int id;
        public string mobName;
        public int nbWeapon;
        public string weaponOriginalName;

        public Family family;
        
        public List<Skill> skills;

        public TypeWeapon constraint;

        private MonsterPrefab monsterPrefab;

        public override void BasicAttack()
        {
            if (monsterPrefab.target)
            {
                monsterPrefab.PlayBasicAttack(weapons[0].weaponPrefab);
            }
        }
        
        public override void BasicDefense()
        {
            throw new System.NotImplementedException();
        }

        public override void DesactiveBasicDefense()
        {
            throw new NotImplementedException();
        }

        public override void ApplyDamage(float directDamage)
        {
            base.ApplyDamage(directDamage);

            if (hp <= 0)
            {
                monsterPrefab.EntityDie();
            }
        }

        public void SetMonsterPrefab(MonsterPrefab newMonsterPrefab)
        {
            monsterPrefab = newMonsterPrefab;
        }

        public bool InitWeapon(int idWeapon)
        {
            if (weapons.Count < nbWeapon)
            {
                Weapon weapon = DataObject.WeaponList.GetWeaponWithId(idWeapon);

                if (constraint != weapon.type)
                {
                    return false;
                }

                monsterPrefab.AddItemInHand(weapon);
                weapons.Add(weapon);

                return true;
            }

            return false;
        }
        
        public void InitOriginalWeapon()
        {
            Weapon weapon = DataObject.WeaponList.GetWeaponWithName(weaponOriginalName);

            monsterPrefab.AddItemInHand(weapon);

            weapons.Add(weapon);
        }
    }
}
