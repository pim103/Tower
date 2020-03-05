using System;
using Games.Global.Patterns;
using PA_INST = Games.Global.Patterns.PatternInstructions;

namespace Games.Global.Weapons
{
    [Serializable]
    public class Staff : Weapon
    {
        public Staff()
        {
            pattern = new Pattern[2];
        }
    }
}