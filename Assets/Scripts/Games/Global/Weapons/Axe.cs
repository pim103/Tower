using System;
using Games.Global.Patterns;
using PA_INST = Games.Global.Patterns.PatternInstructions;

namespace Games.Global.Weapons
{
    [Serializable]
    public class Axe : Weapon
    {
        public Axe()
        {
            pattern = new Pattern[2];
        }
    }
}