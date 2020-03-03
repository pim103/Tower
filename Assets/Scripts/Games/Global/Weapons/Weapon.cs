using System.Collections.Generic;
using Games.Global.Patterns;
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
        public float attSpeed;

        public List<TypeEffect> effects;

        public Pattern[] pattern;

        // Basic attack active trigger and play movement
        public void BasicAttack(MovementPatternController movementPatternController, GameObject objectToMove)
        {
            BoxCollider bc = instantiateModel.GetComponent<BoxCollider>();

            if (!bc.enabled)
            {
                bc.enabled = true;
            
                PlayMovement(movementPatternController, attSpeed, objectToMove, bc);
            }
            
        }

        private void PlayMovement(MovementPatternController movementPatternController, float attSpeed, GameObject objectToMove, BoxCollider bc)
        {
            movementPatternController.PlayMovement(pattern, attSpeed, objectToMove, bc);
        }
    }
}