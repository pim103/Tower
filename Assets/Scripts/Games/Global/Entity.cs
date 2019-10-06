using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Games.Global
{
    public enum TypeEntity
    {
        PLAYER,
        MOB,
        BOSS
    }

    // Class for mobs and players
    public abstract class Entity : MonoBehaviour
    {
        private const int DEFAULT_HP = 100;
        private const int DEFAULT_DEF = 10;
        private const int DEFAULT_ATT = 10;
        private const int DEFAULT_NB_EQUIPEMENTS = 1;

        public int hp = DEFAULT_HP;
        public int def = DEFAULT_DEF;
        public int att = DEFAULT_ATT;

        public int nbEquipements = DEFAULT_NB_EQUIPEMENTS;
        public Equipement[] equipements;

        public TypeEntity typeEntity;

        public void HiHere()
        {
            Debug.Log("Hey");
        }
    }
}