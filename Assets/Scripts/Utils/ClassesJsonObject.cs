using System;
using System.Collections.Generic;
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
        public string defenseSpell { get; set; } = "";

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
                defenseSpell = defenseSpell
            };
        }
    }
    
    public class ClassesListJsonObject
    {
        public List<ClassesJsonObject> classes;
    }
}