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
            warriorSpells.Add("");
            warriorSpells.Add("");
            warriorSpells.Add("");

            mageSpells = new List<string>();
            mageSpells.Add("");
            mageSpells.Add("");
            mageSpells.Add("");

            rangerSpells = new List<string>();
            rangerSpells.Add("");
            rangerSpells.Add("");
            rangerSpells.Add("");

            rogueSpells = new List<string>();
            rogueSpells.Add("");
            rogueSpells.Add("");
            rogueSpells.Add("");

//            AreaOfEffectSpell area = new AreaOfEffectSpell
//            {
//                damageType = DamageType.Physical,
//                geometry = Geometry.Cone,
//                scale = Vector3.one + Vector3.forward,
//                onePlay = true,
//                isBasicAttack = true,
//                OriginalPosition = OriginalPosition.Caster,
//                OriginalDirection = OriginalDirection.Forward,
//                needPositionToMidToEntity = true,
//                damagesOnEnemiesOnInterval = 50
//            };
//
//            basicAttack = new Spell
//            {
//                cost = 0,
//                cooldown = 1f,
//                castTime = 0,
//                activeSpellComponent = area
//            };
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