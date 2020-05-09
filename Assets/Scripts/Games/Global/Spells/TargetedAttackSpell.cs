﻿using UnityEngine;

namespace Games.Global.Spells
{
    public enum TargetingType
    {
        Raycast,
        ClosestEnemy
    }
    
    public class TargetedAttackSpell : SpellComponent
    {
        private void Start()
        {
            typeSpell = TypeSpell.TargetedAttack;
        }
        
        public TargetingType targetingType;
        public Entity target;

        public float distanceMax;

        public SpellComponent linkedSpell;
    }
}