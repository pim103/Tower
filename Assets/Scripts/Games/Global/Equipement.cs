using System;
using Games.Global.Abilities;

namespace Games.Global
{
    //Class for equipements
    public abstract class Equipement : Item
    {
        public int cost;

        // DEFINE METHOD TO USE GENERIC EQUIPEMENTS
        // Method called when damage received
        public Func<AbilityParameters, bool> OnDamageReceive;

        // Method called when damage dealt
        public Func<AbilityParameters, bool> OnDamageDealt;
    }
}