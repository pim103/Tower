using Games.Global.Patterns;
using Games.Players;
using PA_INST = Games.Global.Patterns.PatternInstructions;

namespace Games.Global.Weapons
{
    public class LongSword: Weapon
    {
        public LongSword()
        {
            pattern = new Pattern[2];
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
