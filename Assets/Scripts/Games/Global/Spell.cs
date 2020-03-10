using System;
using System.Collections.Generic;
using Games.Global.Abilities;
using UnityEngine;

namespace Games.Global
{
    public enum TypeSpell
    {
        SelfEffect,
        EffectOnDamageDeal,
        EffectOnDamageReceive,
        InstantiateSomething
    }

    public struct SpellInstruction
    {
        public TypeSpell typeSpell;
        public Effect effect;
        public GameObject gameObject;
        
        // Time wait before next instructions
        public float timeWait;

        public float durationInstruction;
    }

    public class Spell
    {
        public bool isPassive;
        public int cost;
        public int startCooldownTimer;
        public float castTime;
        public int cooldown;

        public bool canLaunch;

        public List<SpellInstruction> spellInstructions;

        public Spell()
        {
            isPassive = false;
            cost = 0;
            startCooldownTimer = 0;
            castTime = 0;
            cooldown = 0;
            canLaunch = true;
            spellInstructions = new List<SpellInstruction>();
        }
    }
}