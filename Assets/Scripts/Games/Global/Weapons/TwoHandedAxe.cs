using System;
//using Games.Global.Patterns;
using Games.Players;
//using PA_INST = Games.Global.Patterns.PatternInstructions;

namespace Games.Global.Weapons
{
    [Serializable]
    public class TwoHandedAxe : Weapon
    {
        public TwoHandedAxe()
        {
           //pattern = //pattern[4];
            
           //pattern[0] = //pattern(PA_INST.ROTATE_DOWN, 90, 0.1f, 0.01f);
           //pattern[1] = //pattern(PA_INST.ROTATE_LEFT, 90, 0.1f, 0.01f);
           //pattern[2] = //pattern(PA_INST.ROTATE_RIGHT, 90, 0.1f, 0.01f);
           //pattern[3] = //pattern(PA_INST.ROTATE_UP, 90, 0.1f, 0.01f);
        }

        public override void InitPlayerSkill(Classes classe)
        {
            switch (classe)
            {
                case Classes.Mage:
                    break;
                case Classes.Rogue:
                    break;
                case Classes.Ranger:
                    break;
                case Classes.Warrior:
                    break;
            }
        }
    }
}