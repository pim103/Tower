using Games.Global.Patterns;
using Scripts.Games.Global;
using UnityEngine;

namespace Games.Global.Weapons
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

    public abstract class Weapon : Equipement
    {
        public string equipementName;
        public TypeEquipement type;
        public int damage;
        public int attSpeed;

        public Effect[] effects;

        public Pattern[] pattern;

        public abstract void BasicAttack();

        public void PlayMovement(MovementPattern movementPattern, GameObject objectToMove)
        {
            movementPattern.PlayMovement(pattern, objectToMove);
        }
    }
}