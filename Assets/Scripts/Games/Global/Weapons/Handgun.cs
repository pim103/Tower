using Games.Global.Patterns;
using Games.Players;

namespace Games.Global.Weapons
{
    public class Handgun : Weapon
    {
        public Handgun()
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
