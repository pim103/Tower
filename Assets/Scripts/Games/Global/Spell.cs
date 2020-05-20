﻿using System;
using System.Collections.Generic;
using Games.Global.Abilities;
using Games.Global.Weapons;
using UnityEngine;

namespace Games.Global
{
    public enum TypeSpell
    {
        Active,
        Passive,
        Toggle,
        ActiveWithPassive,
        ToggleWithPassive
    }
    
    public enum TypeSpellInstruction
    {
        SelfEffect,
        EffectOnTargetWhenDamageDeal,
        EffectOnTargetWhenDamageReceive,
        SelfEffectOnDamageReceive,
        SelfEffectOnDamageDeal,
        InstantiateSomething,
        ChangeBasicAttack,
        SpecialMovement
    }

    public enum TypeSpellObject
    {
        Projectile,
        GroundArea,
        Weapon,
        OnHimself
    }

    public struct WeaponNewStats
    {
        public float attSpeedModifier;
        public int damageModifier;
        public TypeWeapon typeWeapon;
    }

    public struct SpellInstruction
    {
        public TypeSpellInstruction TypeSpellInstruction;
        
        // If Type == effect
        public Effect effect;
        
        // If type == effectOnSomething
        public float durationInstruction;
        
        // If type == instantiate or type == changeBasickAttack /* ID poolerSpell */
        public int idPoolObject;
        public TypeSpellObject typeSpellObject;
        
        // If type == changeBasicAttack
        public WeaponNewStats weaponNewStats;
        
        // Time wait before next instructions
        public float timeWait;

        // Passive or Active only when Spell.typeSpell == ActiveWithPassive || ToggleWithPassive
        public TypeSpell specificTypeSpell;

        // If Type == specialMovement
        public SpecialMovement specialMovement;
    }

    public class Spell
    {
        public TypeSpell typeSpell;
        public int cost;
        public float castTime;
        public int cooldown;

        public bool canLaunch;

        // TODO : delete unused origin
        public Entity origin;

        public List<SpellInstruction> spellInstructions;

        // Specific for GroundArea with preview
        public GameObject spellInstantiate;

        public Spell()
        {
            typeSpell = TypeSpell.Active;
            cost = 0;
            castTime = 0;
            cooldown = 0;
            canLaunch = true;
            spellInstructions = new List<SpellInstruction>();
        }
    }
}