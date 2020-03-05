using System;
using Games.Global.Patterns;
using PA_INST = Games.Global.Patterns.PatternInstructions;

namespace Games.Global.Weapons
{
    [Serializable]
    public class Hammer : Weapon
    {
        public Hammer()
        {
            pattern = new Pattern[2];
        }
    }
}