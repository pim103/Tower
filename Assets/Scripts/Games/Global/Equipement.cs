using System;
using Games.Global.Abilities;

namespace Games.Global
{
    //Class for equipements
    public abstract class Equipement : Item
    {
        public int cost { get; set; }

        // DEFINE METHOD TO USE GENERIC EQUIPEMENTS
        // Method called when damage received
        public Func<AbilityParameters, bool> OnDamageReceive { get; set; }

        // Method called when damage dealt
        public Func<AbilityParameters, bool> OnDamageDealt { get; set; }
    }
}