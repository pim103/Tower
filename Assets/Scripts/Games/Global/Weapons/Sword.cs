using System;
using System.Collections.Generic;
using System.Diagnostics;
using Games.Global.Spells;
using Games.Global.Spells.SpellsController;
//using Games.Global.Patterns;
using Games.Players;
using UnityEngine;

//using PA_INST = Games.Global.Patterns.PatternInstructions;

namespace Games.Global.Weapons
{
    [Serializable]
    public class Sword : Weapon
    {
        public Sword()
        {
            animationToPlay = "ShortSwordAttack";
            spellOfBasicAttack = "ShortSwordBasicAttack";
            
            warriorSpells = new List<string>();
            warriorSpells.Add("WarriorShortSword1");
            warriorSpells.Add("WarriorShortSword2");
            warriorSpells.Add("WarriorShortSword3");

            mageSpells = new List<string>();
            mageSpells.Add("MageShortSword1");
            mageSpells.Add("MageShortSword2");
            mageSpells.Add("MageShortSword3");

            rangerSpells = new List<string>();
            rangerSpells.Add("");
            rangerSpells.Add("");
            rangerSpells.Add("");

            rogueSpells = new List<string>();
            rogueSpells.Add("");
            rogueSpells.Add("");
            rogueSpells.Add("");
        }
    }
}