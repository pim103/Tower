using System;
using Games.Players;

namespace Games.Global.Weapons
{
    [Serializable]
    public class Staff : Weapon
    {
        public Staff()
        {
           animationToPlay = "ShortSwordAttack";
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