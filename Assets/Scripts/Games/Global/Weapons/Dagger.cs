using System;
using Games.Global.Patterns;
using Games.Players;
using PA_INST = Games.Global.Patterns.PatternInstructions;

namespace Games.Global.Weapons
{
    [Serializable]
    public class Dagger : Weapon
    {
        public Dagger()
        {
            pattern = new Pattern[2];
            pattern[0] = new Pattern(PA_INST.FRONT, 1, 0.2f, 0.01f);
            pattern[1] = new Pattern(PA_INST.BACK, 1, 0.2f, 0.01f);
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