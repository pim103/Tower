using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Games.Global.Armors
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

        public Effect[] effects;
    } 
}