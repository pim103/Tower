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

        public Spell skill1;
        public Spell skill2;
        public Spell skill3;

        public int idPoolProjectile;

        public virtual void InitPlayerSkill(Classes classe)
        {
            skill1 = new Spell();
            skill2 = new Spell();
            skill3 = new Spell();
        }
    }
}