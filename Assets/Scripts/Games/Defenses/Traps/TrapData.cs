using System;
using System.Collections.Generic;

namespace Games.Defenses.Traps
{
    public enum TrapType
    {
        Arrows,
        Spikes,
        EjectionPlate,
        RammingRail,
        BearTrap,
        Fan,
        SpikyPole,
        Mine
    }

    public enum AdditionalEffects
    {
        Poison,
        Burn,
        Freeze,
        Weakness,
        Stun
    }
    
    [Serializable]
    public class TrapData
    {
        public TrapType mainType { get; set; }
        public List<AdditionalEffects> trapEffects { get; set; }
    }
}