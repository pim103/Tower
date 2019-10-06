using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Games.Global
{
    public enum TypeEquipement
    {
        SHORT_SWORD,
        LONG_SWORD,
        SPEAR,
        AXE,
        TWO_HAND_AXE,
        HAMMER,
        HALBERD,
        MACE,
        BOW,
        STAFF,
        DAGGER,
        TRIDENT,
        RIFLE,
        CROSSBOW,
        SLING,
        HANDGUN
    };

    //Class for equipements
    public abstract class Equipement : MonoBehaviour
    {
        public string equipementName;
        public TypeEquipement type;
        public int damage;
        public bool isCac;

        public int lootRate;
        public int cost;

        // DEFINE METHOD TO USE GENERIC EQUIPEMENTS
    }
}