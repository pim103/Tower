using System.Collections.Generic;
using Games.Global.Abilities;
using Games.Global.Weapons;
using UnityEngine;

namespace Games.Global.Entities
{
    public class Monster: Entity
    {
        public int idInitialisation;
        public int id;
        public string mobName;
        public int nbWeapon;
        public string weaponOriginalName;

        public Family family;
        
        public List<Skill> skills;

        public TypeWeapon constraint;

        public override bool InitWeapon(int idWeapon)
        {
            if (weapons.Count < nbWeapon)
            {
                if (instantiateModel != null)
                {
                    Weapon weapon = DataObject.WeaponList.GetWeaponWithId(idWeapon);

                    if (constraint != weapon.type)
                    {
                        return false;
                    }
                    
                    Transform mobHand = instantiateModel.transform.GetChild(0);

                    InstantiateParameters param = new InstantiateParameters();
                    param.item = weapon;
                    param.type = TypeItem.Weapon;
                    param.wielder = this;

                    weapon.InstantiateModel(param, Vector3.zero, mobHand);

                    weapons.Add(weapon);

                    return true;
                }
            }

            return false;
        }

        public void InitOriginalWeapon()
        {
            if (instantiateModel != null)
            {
                Transform mobHand = instantiateModel.transform.GetChild(0);
                Weapon weapon = DataObject.WeaponList.GetWeaponWithName(weaponOriginalName);

                InstantiateParameters param = new InstantiateParameters();
                param.item = weapon;
                param.type = TypeItem.Weapon;
                param.wielder = this;

                weapon.InstantiateModel(param, Vector3.zero, mobHand);

                weapons.Add(weapon);
            }
        }

        public override void BasicAttack()
        {
            if (weapons.Count > 0)
            {
                Transform mobHand = instantiateModel.transform.GetChild(0);
                weapons[0].BasicAttack(movementPatternController, mobHand.gameObject);
            }
            else
            {
                Debug.Log("Basic attack of monster ?");
            }
        }

        public override void TakeDamage(int initialDamage, AbilityParameters abilityParameters)
        {
            base.TakeDamage(initialDamage, abilityParameters);
            
            if (hp <= 0)
            {
                EntityDie();
            }
        }

        private void EntityDie()
        {
            int index = DataObject.monsterInScene.FindIndex(monster => monster.idInitialisation == idInitialisation);
            DataObject.monsterInScene.RemoveAt(index);
            
            Destroy(instantiateModel);
        }
    }
}
