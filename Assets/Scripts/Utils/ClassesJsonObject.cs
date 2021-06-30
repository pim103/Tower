using System;
using System.Collections.Generic;
using Games.Global;
using Games.Players;

namespace Utils
{
    public class ClassesJsonObject {
        public string id { get; set; } = "";

        public string name { get; set; }
        public string def { get; set; } = "0";
        public string hp { get; set; } = "0";
        public string att { get; set; } = "0";
        public string speed { get; set; } = "0";
        public string attSpeed { get; set; } = "0";
        public string ressource { get; set; } = "0";
        public int spellDefense { get; set; }

        public Classes ConvertToClasses()
        {
            return new Classes
            {
                id = Int32.Parse(id),
                name = name,
                hp = Int32.Parse(hp),
                att = Int32.Parse(att),
                def = Int32.Parse(def),
                speed = Int32.Parse(speed),
                attSpeed = Int32.Parse(attSpeed),
                ressource = Int32.Parse(ressource),
                defenseSpell = DataObject.SpellList.GetSpellById(spellDefense)
            };
        }
    }
    
    public class ClassesListJsonObject
    {
        public List<ClassesJsonObject> classes { get; set; }
    }
    
    public class ClassesWeaponSpellJson
    {
        public int id { get; set; }
        public int classes { get; set; }
        public int categoryWeapon { get; set; }
        public int spell1 { get; set; }
        public int spell2 { get; set; }
        public int spell3 { get; set; }

        public ClassesWeaponSpell Convert()
        {
            return new ClassesWeaponSpell
            {
                id = id,
                classes = DataObject.ClassesList.GetClassesFromId(classes),
                categoryWeapon = DataObject.CategoryWeaponList.GetCategoryFromId(categoryWeapon),
                spell1 = DataObject.SpellList.GetSpellById(spell1),
                spell2 = DataObject.SpellList.GetSpellById(spell2),
                spell3 = DataObject.SpellList.GetSpellById(spell3)
            };
        }
    }

    public class ClassesWeaponSpellJsonList
    {
        public List<ClassesWeaponSpellJson> classesCategory { get; set; }
    }
}