using System;
using System.Collections.Generic;
using UnityEngine;

namespace Games.Global.Spells
{
    [Serializable]
    public class SummonSpell : SpellComponent
    {
        public SummonSpell()
        {
            typeSpell = TypeSpell.Summon;
        }

        public int idPoolObject { get; set; }

        public float hp { get; set; }
        public bool isTargetable { get; set; }
        public List<Vector3> positionPresets { get; set; }
        public float duration { get; set; }
        public float attackDamage { get; set; }
        public float moveSpeed { get; set; }
        public int summonNumber { get; set; }
        public float attackSpeed { get; set; }
        public int nbUseSpells { get; set; }

        public BehaviorType BehaviorType { get; set; }
        public AttackBehaviorType AttackBehaviorType { get; set; }

        // Destroy other invocation of same type
        public bool isUnique { get; set; }
        public bool canMove { get; set; }

        public SpellComponent linkedSpellOnEnable { get; set; }
        public SpellComponent linkedSpellOnDisapear { get; set; }

        public List<Spell> spells { get; set; }
        public Spell basicAttack { get; set; }
        public SpellComponent spellWhenPlayerCall { get; set; }

        /* useless for initialisation of spell */
        public List<GenericSummonSpell> prefabsSummon;
    }
}