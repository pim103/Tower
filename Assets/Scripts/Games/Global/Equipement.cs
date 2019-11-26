using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Games.Global
{
    //Class for equipements
    public abstract class Equipement : MonoBehaviour
    {
        public int lootRate;
        public int cost;

        // DEFINE METHOD TO USE GENERIC EQUIPEMENTS
        // Method called when damage received
        public bool OnDamageReceive()
        {
            return false;
        }

        // Method called when damage dealt
        public bool OnDamageDealt()
        {
            return false;
        }
    }
}