﻿using Games.Global.Patterns;
using Games.Players;
using PA_INST = Games.Global.Patterns.PatternInstructions;

namespace Games.Global.Weapons
{
    public class Mace: Weapon
    {
        public Mace()
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
