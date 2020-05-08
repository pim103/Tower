using System;
using System.Collections.Generic;
using Games.Global.Spells.SpellParameter;

namespace Games.Global.Spells
{
    public enum ConditionReduceCharge
    {
        OnAttack,
        OnDamageReceived
    }
    
    public class BuffSpell : SpellComponent
    {
        private void Start()
        {
            typeSpell = TypeSpell.Buff;
        }

        public float interval;
        public float duration;

        // instead of duration
        public int stack;
        public bool disapearOnDamageReceived;

        public float damageOnSelf;
        public ProjectileSpell replaceProjectile;

        public List<Effect> effectOnSelf;
        public List<Effect> effectOnSelfOnDamageReceived;
        public List<Effect> effectOnTargetOnHit;

        public SpellComponent linkedSpellOnDamageReceived;
        public SpellComponent linkedSpellOnHit;
        public SpellComponent linkedSpellOnAttack;
        public SpellComponent linkedSpellOnInterval;

        public ConditionReduceCharge conditionReduceCharge;

        public Effect effectOnSelfWhenNoCharge;

        public List<SpellWithCondition> spellWithCondition;

        public SummonBehaviorType newPlayerBehaviour;
    }
}
