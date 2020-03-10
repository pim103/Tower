using Games.Global.Patterns;
using Games.Players;
using PA_INST = Games.Global.Patterns.PatternInstructions;

namespace Games.Global.Weapons
{
    public class Bow: Weapon
    {
        public Bow()
        {
            pattern = new Pattern[2];
            pattern[0] = new Pattern(PA_INST.BACK, 0.2f);
            pattern[1] = new Pattern(PA_INST.FRONT, 0.2f);
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
