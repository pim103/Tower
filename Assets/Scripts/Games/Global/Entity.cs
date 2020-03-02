using System.Collections.Generic;
using Games.Global.Patterns;
using Games.Global.Weapons;
using UnityEngine;
using UnityEngine.Serialization;

namespace Games.Global
{
    public enum TypeEntity
    {
        PLAYER,
        MOB,
        BOSS
    }

    // Class for mobs and players
    public abstract class Entity : ItemModel
    {
        private const int DEFAULT_HP = 100;
        private const int DEFAULT_DEF = 10;
        private const int DEFAULT_ATT = 10;
        private const int DEFAULT_SPEED = 10;
        private const int DEFAULT_NB_WEAPONS = 1;

        public int hp = DEFAULT_HP;
        public int def = DEFAULT_DEF;
        public int att = DEFAULT_ATT;
        public int speed = DEFAULT_SPEED;

        // If needed, create WeaponExposer to get all scripts of a weapon
        public Weapon[] weapons;

        public TypeEntity typeEntity;

        public List<TypeEffect> underEffects;

        [FormerlySerializedAs("movementPattern")] public MovementPatternController movementPatternController;

        public void InitEquipementArray(int nbWeapons = DEFAULT_NB_WEAPONS)
        {
            weapons = new Weapon[nbWeapons];
        }

        public void HiHere()
        {
            Debug.Log("Hey");
        }
    }
}