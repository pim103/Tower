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
            warriorSpells.Add("bouledefeu");
            warriorSpells.Add("ChargeSpell");
            warriorSpells.Add("CoupChargeSpell");

            mageSpells = new List<string>();
            mageSpells.Add("MageWave");
            mageSpells.Add("mageSpellShortSword2");
            mageSpells.Add("MageSummonShortSword");

            rangerSpells = new List<string>();
            rangerSpells.Add("");
            rangerSpells.Add("");
            rangerSpells.Add("");

            rogueSpells = new List<string>();
            rogueSpells.Add("");
            rogueSpells.Add("");
            rogueSpells.Add("");
        }

        public override void InitPlayerSkill(Classes classe)
        {
            switch (classe)
            {
                case Classes.Warrior:
                    InitWeaponSpellWithJson(warriorSpells);
                    break;
                case Classes.Mage:
                    InitWeaponSpellWithJson(mageSpells);
                    break;
                case Classes.Rogue:
                    InitWeaponSpellWithJson(rogueSpells);
                    break;
                case Classes.Ranger:
                    InitWeaponSpellWithJson(rangerSpells);
                    break;
            }
        }
    }
}