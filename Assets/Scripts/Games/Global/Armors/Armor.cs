using System.Collections.Generic;

namespace Games.Global.Armors
{
    public enum TypeArmor
    {
        HELMET,
        CHESTPLATE,
        LEGGINGS
    }

    public abstract class Armor : Equipement
    {
        public int def;
        public TypeArmor typeArmor;

        public List<TypeEffect> effects;
    } 
}