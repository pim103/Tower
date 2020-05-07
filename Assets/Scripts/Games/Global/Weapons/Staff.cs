using System;
//using Games.Global.Patterns;
using Games.Players;
//using PA_INST = Games.Global.Patterns.PatternInstructions;

namespace Games.Global.Weapons
{
    [Serializable]
    public class Staff : Weapon
    {
        public Staff()
        {
           //pattern = //pattern[2];
           //pattern[0] = //pattern(PA_INST.ROTATE_DOWN, 70, 0.2f, 0.02f);
           //pattern[1] = //pattern(PA_INST.ROTATE_UP, 70, 0.2f, 0.02f);
           animationToPlay = "ShortSwordAttack";
           idPoolProjectile = 2;
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