using System;

namespace Games.Global
{
    //Class for equipements
    public abstract class Equipement : Item
    {
        public int cost;

        // DEFINE METHOD TO USE GENERIC EQUIPEMENTS
        // Method called when damage received
        public Func<bool> OnDamageReceive;

        // Method called when damage dealt
        public Func<bool> OnDamageDealt;
    }
}