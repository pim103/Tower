using System;
using System.Collections.Generic;
using Games.Global.Abilities;
using Games.Global.Weapons;
using UnityEngine;

namespace Games.Global
{
    public enum OldTypeSpell
    {
        Active,
        Passive,
        Toggle,
        ActiveWithPassive,
        ToggleWithPassive
    }
    
    public enum OldTypeSpellInstruction
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

    public enum OldTypeSpellObject
    {
        Projectile,
        GroundArea,
        Weapon,
        OnHimself
    }

    public struct OldWeaponNewStats
    {
        public float attSpeedModifier;
        public int damageModifier;
        public TypeWeapon typeWeapon;
    }

    public struct OldSpellInstruction
    {
        public OldTypeSpellInstruction OldTypeSpellInstruction;
        
        // If Type == effect
        public Effect effect;
        
        // If type == effectOnSomething
        public float durationInstruction;
        
        // If type == instantiate or type == changeBasickAttack /* ID poolerSpell */
        public int idPoolObject;
        public OldTypeSpellObject OldTypeSpellObject;
        
        // If type == changeBasicAttack
        public OldWeaponNewStats OldWeaponNewStats;
        
        // Time wait before next instructions
        public float timeWait;

        // Passive or Active only when Spell.typeSpell == ActiveWithPassive || ToggleWithPassive
        public OldTypeSpell SpecificOldTypeSpell;

        // If Type == specialMovement
        public SpecialMovement specialMovement;
    }

    public class OldSpell
    {
        public OldTypeSpell OldTypeSpell;
        public int cost;
        public float castTime;
        public int cooldown;

        public bool canLaunch;

        // TODO : delete unused origin
        public Entity origin;

        public List<OldSpellInstruction> spellInstructions;

        // Specific for GroundArea with preview
        public GameObject spellInstantiate;

        public OldSpell()
        {
            OldTypeSpell = OldTypeSpell.Active;
            cost = 0;
            castTime = 0;
            cooldown = 0;
            canLaunch = true;
            spellInstructions = new List<OldSpellInstruction>();
        }
    }
}