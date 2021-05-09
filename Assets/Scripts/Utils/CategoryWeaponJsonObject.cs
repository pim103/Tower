using System;
using System.Collections.Generic;
using Games.Global;
using Games.Global.Weapons;

namespace Utils
{
    public class CategoryWeaponJsonObject
    {
        public int id { get; set; }
        public string name { get; set; }
        public int spellAttack { get; set; }

        public CategoryWeapon ConvertToCategoryWeapon()
        {
            return new CategoryWeapon
            {
                id = id,
                name = name,
                spellAttack = DataObject.SpellList.GetSpellById(spellAttack)
            };
        }
    }

    public class CategoryWeaponListJsonObject
    {
        public List<CategoryWeaponJsonObject> categories;
    }
}