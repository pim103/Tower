using System.Collections.Generic;
using Games.Global.Weapons;
using UnityEngine;

namespace Games.Global.Entities
{
    public class Monster: Entity
    {
        public int id;
        public string mobName;
        public int nbWeapon;

        public Family family;
        
        public List<Skill> skills;

        public override void InitWeapon(int idWeapon)
        {
            if (weapons.Count < nbWeapon)
            {
                if (instantiateModel != null)
                {
                    Transform mobHand = instantiateModel.transform.GetChild(0);
                    Weapon weapon = DataObject.WeaponList.GetWeaponWithId(idWeapon);

                    InstantiateParameters param = new InstantiateParameters();
                    param.item = weapon;
                    param.type = TypeItem.Weapon;
                    param.wielder = this;

                    weapon.InstantiateModel(param, Vector3.zero, mobHand);

                    weapons.Add(weapon);
                }
            }
        }
    }
}
