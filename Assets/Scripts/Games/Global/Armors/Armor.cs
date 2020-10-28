using System.Collections.Generic;

namespace Games.Global.Armors
{
    public enum CategoryArmor
    {
        HELMET,
        CHESTPLATE,
        LEGGINGS
    }
    
    public class Armor : Equipement
    {
        public int def;
        public CategoryArmor armorCategory;

        public List<TypeEffect> effects;
    } 
}