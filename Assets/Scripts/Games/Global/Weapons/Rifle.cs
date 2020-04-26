//using Games.Global.Patterns;
using Games.Players;

namespace Games.Global.Weapons
{
    public class Rifle: Weapon
    {
        public Rifle()
        {
           //pattern = //pattern[2];
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
