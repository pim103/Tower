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
            
            Effect repulse = new Effect { typeEffect = TypeEffect.Expulsion, level = 10, directionExpul = DirectionExpulsion.Out, originExpulsion = OriginExpulsion.SrcDamage};
            List<Effect> effects = new List<Effect>();
            effects.Add(repulse);

            AreaOfEffectSpell area = new AreaOfEffectSpell
            {
                startPosition = Vector3.zero,
                scale = Vector3.one * 4,
                duration = 3,
                interval = 0.05f,
                typeSpell = TypeSpell.AreaOfEffect,
                geometry = Geometry.Square,
                damagesOnEnemiesOnInterval = 11.0f,
                effectsOnEnemiesOnInterval = effects,
                wantToFollow = true,
                OriginalPosition = OriginalPosition.Caster,
                OriginalDirection = OriginalDirection.Forward
            };
            
            MovementSpell movementSpell = new MovementSpell
            {
                duration = 3f,
                speed = 20,
                isFollowingMouse = true,
                movementSpellType = MovementSpellType.Charge,
                linkedSpellAtTheStart = area,
                OriginalPosition = OriginalPosition.Caster,
                OriginalDirection = OriginalDirection.Forward
            };
            
//            AreaOfEffectSpell area = new AreaOfEffectSpell
//            {
//                damageType = DamageType.Physical,
//                geometry = Geometry.Cone,
//                scale = Vector3.one + Vector3.forward,
//                onePlay = true,
//                isBasicAttack = true,
//                OriginalPosition = OriginalPosition.Caster,
//                OriginalDirection = OriginalDirection.Forward,
//                needPositionToMidToEntity = true
//            };

            basicAttack = new Spell
            {
                cost = 0,
                cooldown = 1,
                castTime = 0,
                activeSpellComponent = movementSpell
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