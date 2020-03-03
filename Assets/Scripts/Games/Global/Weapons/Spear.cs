using System;
using Games.Global.Patterns;
using PA_INST = Games.Global.Patterns.PatternInstructions;

namespace Games.Global.Weapons
{
    [Serializable]
    public class Spear : Weapon
    {
        public Spear()
        {
            pattern = new Pattern[2];
            pattern[0] = new Pattern(PA_INST.FRONT, 1, 0.1f, 0.01f);
            pattern[1] = new Pattern(PA_INST.BACK, 1, 0.1f, 0.01f);
        }
    }
}