using System.Collections.Generic;
using UnityEngine;

namespace Games.Global.Spells
{
    public enum SummonBehaviorType
    {
        MoveOnTargetAndDie,
        Distance,
        Melee,
        WithoutTarget
    }
    
    public class SummonSpell : SpellComponent
    {
        private void Start()
        {
            typeSpell = TypeSpell.Summon;
        }

        public GameObject prefab;
        public float hp;
        public bool isTargetable;
        public List<Vector3> startPosition;
        public float duration;
        public float attackDamage;
        public int summonNumber;
        public float attackSpeed;
        public int nbUseSpells;

        public SummonBehaviorType summonBehaviorType;
        
        // Destroy other invocation of same type
        public bool isUnique;
        public bool canMove;

        public SpellComponent linkedSpellOnEnable;
        public SpellComponent linkedSpellOnDisapear;

        public List<SpellComponent> spells;
        public SpellComponent basicAttack;
        public SpellComponent spellWhenPlayerCall;
    }
}
