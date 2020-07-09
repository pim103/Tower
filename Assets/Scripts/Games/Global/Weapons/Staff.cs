using System;
using System.Collections.Generic;
using Games.Players;

namespace Games.Global.Weapons
{
    [Serializable]
    public class Staff : Weapon
    {
        public Staff()
        {
           animationToPlay = "ShortSwordAttack";
           spellOfBasicAttack = "StaffBasicAttack";
            
           warriorSpells = new List<string>();
           warriorSpells.Add("WarriorStaff1");
           warriorSpells.Add("WarriorStaff2");
           warriorSpells.Add("WarriorStaff3");

           mageSpells = new List<string>();
           mageSpells.Add("");
           mageSpells.Add("MageStaff2");
           mageSpells.Add("");

           rangerSpells = new List<string>();
           rangerSpells.Add("RangerStaff1");
           rangerSpells.Add("RangerStaff2");
           rangerSpells.Add("RangerStaff3");

           rogueSpells = new List<string>();
           rogueSpells.Add("RogueStaff1");
           rogueSpells.Add("RogueStaff2");
           rogueSpells.Add("RogueStaff3");
        }
    }
}