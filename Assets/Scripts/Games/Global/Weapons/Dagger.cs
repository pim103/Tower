using System;
using Games.Global.Patterns;
using PA_INST = Games.Global.Patterns.PatternInstructions;

namespace Games.Global.Weapons
{
    [Serializable]
    public class Dagger : Weapon
    {
        public Dagger()
        {
            pattern = new Pattern[4];
        }
    }
}