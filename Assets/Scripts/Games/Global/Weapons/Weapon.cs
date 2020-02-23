using System;
using System.Collections.Generic;
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

        public List<TypeEffect> effects;

        public Pattern[] pattern;

        // Basic attack active trigger and play movement
        public void BasicAttack(MovementPatternController movementPatternController, GameObject objectToMove)
        {
            PlayMovement(movementPatternController, objectToMove);
        }

        public void PlayMovement(MovementPatternController movementPatternController, GameObject objectToMove)
        {
            movementPatternController.PlayMovement(pattern, objectToMove);
        }
    }
}