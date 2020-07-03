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
            rangerSpells.Add("RangerShortSword1");
            rangerSpells.Add("RangerShortSword2");
            rangerSpells.Add("RangerShortSword3");

            rogueSpells = new List<string>();
            rogueSpells.Add("RogueShortSword1");
            rogueSpells.Add("RogueShortSword2");
            rogueSpells.Add("RogueShortSword3");
        }
    }
}