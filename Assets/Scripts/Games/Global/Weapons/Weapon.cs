using Games.Players;
using UnityEngine;

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

        public string animationToPlay;

        public int idPoolProjectile;

        public int oneHitDamageUp = 0;

        public virtual void InitPlayerSkill(Classes classe)
        {
        }

        public virtual void FixAngleAttack(bool isFirstIteration, Entity wielder)
        {
            
        }
    }
}