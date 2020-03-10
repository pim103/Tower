﻿using Games.Global.Patterns;
using Games.Players;

namespace Games.Global.Weapons
{
    public class Crossbow : Weapon
    {
        public Crossbow()
        {
            pattern = new Pattern[2];
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
