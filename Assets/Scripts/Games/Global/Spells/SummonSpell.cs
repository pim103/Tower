using System.Collections.Generic;
using UnityEngine;

namespace Games.Global.Spells
{   
    public class SummonSpell : SpellComponent
    {
        public SummonSpell()
        {
            typeSpell = TypeSpell.Summon;
        }

        public int idPoolObject;

        public float hp;
        public bool isTargetable;
        public Vector3 startPosition;
        public List<Vector3> positionPresets;
        public float duration;
        public float attackDamage;
        public float moveSpeed;
        public int summonNumber;
        public float attackSpeed;
        public int nbUseSpells;

        public BehaviorType BehaviorType;

        // Destroy other invocation of same type
        public bool isUnique;
        public bool canMove;

        public SpellComponent linkedSpellOnEnable;
        public SpellComponent linkedSpellOnDisapear;

        public List<Spell> spells;
        public Spell basicAttack;
        public SpellComponent spellWhenPlayerCall;

        /* useless for initialisation of spell */
        public List<GenericSummonSpell> prefabsSummon;
    }
}
