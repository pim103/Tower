﻿using System;
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

            AreaOfEffectSpell area = new AreaOfEffectSpell
            {
                damageType = DamageType.Physical,
                geometry = Geometry.Cone,
                scale = Vector3.one + Vector3.forward,
                onePlay = true,
                isBasicAttack = true,
                OriginalPosition = OriginalPosition.Caster,
                OriginalDirection = OriginalDirection.Forward,
                needPositionToMidToEntity = true,
                damagesOnEnemiesOnInterval = 50
            };

            basicAttack = new Spell
            {
                cost = 0,
                cooldown = 1f,
                castTime = 0,
                activeSpellComponent = area
            };
        }

        public override void InitPlayerSkill(Classes classe)
        {
            switch (classe)
            {
                case Classes.Warrior:
                    
                    break;
            }
        }
    }
}