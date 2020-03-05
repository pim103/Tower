using System;
using Games.Global.Patterns;
using PA_INST = Games.Global.Patterns.PatternInstructions;

namespace Games.Global.Weapons
{
    [Serializable]
    public class Trident : Weapon
    {
        public Trident()
        {
            pattern = new Pattern[4];
        }
    }
}