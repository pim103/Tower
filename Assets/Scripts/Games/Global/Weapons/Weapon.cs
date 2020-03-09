using System.Collections.Generic;
using Games.Global.Patterns;
using Games.Players;
using UnityEngine;
using UnityEngine.Serialization;

namespace Games.Global.Weapons
{
    public enum CategoryWeapon
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

    public enum TypeWeapon
    {
        Distance,
        Cac
    }

    public abstract class Weapon : Equipement
    {
        public WeaponPrefab weaponPrefab;
        
        public string equipementName;
        public CategoryWeapon category;
        public TypeWeapon type;
        public int damage;
        public float attSpeed;

        public Pattern[] pattern;

        public Skill skill1;
        public Skill skill2;
        public Skill skill3;

        public abstract void InitPlayerSkill(Classes classe);
    }
}