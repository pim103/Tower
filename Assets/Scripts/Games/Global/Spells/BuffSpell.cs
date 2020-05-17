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
    
    [Serializable]
    public class BuffSpell : SpellComponent
    {
        public BuffSpell()
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

        public bool needNewPositionOnDamageReceived;
        public SpellComponent linkedSpellOnDamageReceived;
        public bool needNewPositionOnHit;
        public SpellComponent linkedSpellOnHit;
        public bool needNewPositionOnAttack;
        public SpellComponent linkedSpellOnAttack;
        public bool needNewPositionOnInterval;
        public SpellComponent linkedSpellOnInterval;

        public ConditionReduceCharge conditionReduceCharge;

        public Effect effectOnSelfWhenNoCharge;

        public List<SpellWithCondition> spellWithCondition;

        public BehaviorType newPlayerBehaviour;

        public bool triggerInvocationCallOneTime;
    }
}
