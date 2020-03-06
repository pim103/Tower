using System;
using Games.Global.Patterns;
using Games.Players;
using PA_INST = Games.Global.Patterns.PatternInstructions;

namespace Games.Global.Weapons
{
    [Serializable]
    public class TwoHandedAxe : Weapon
    {
        public TwoHandedAxe()
        {
            pattern = new Pattern[4];
            
            pattern[0] = new Pattern(PA_INST.ROTATE_DOWN, 90, 0.1f, 0.01f);
            pattern[1] = new Pattern(PA_INST.ROTATE_LEFT, 90, 0.1f, 0.01f);
            pattern[2] = new Pattern(PA_INST.ROTATE_RIGHT, 90, 0.1f, 0.01f);
            pattern[3] = new Pattern(PA_INST.ROTATE_UP, 90, 0.1f, 0.01f);
        }

        public override void InitPlayerSkill(Classes classe)
        {
            switch (classe)
            {
                case Classes.MAGE:
                    break;
                case Classes.ROGUE:
                    break;
                case Classes.RANGER:
                    break;
                case Classes.WARRIOR:
                    break;
            }
        }
    }
}