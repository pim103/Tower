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

        public float interval { get; set; }
        public float duration { get; set; }

        // instead of duration
        public int stack;
        public bool disapearOnDamageReceived { get; set; }

        public float damageOnSelf;
        public ProjectileSpell replaceProjectile { get; set; }

        public List<Effect> effectOnSelf { get; set; }
        public List<Effect> effectOnSelfOnDamageReceived { get; set; }
        public List<Effect> effectOnTargetOnHit { get; set; }

        public bool needNewPositionOnDamageReceived { get; set; }
        public SpellComponent linkedSpellOnDamageReceived { get; set; }
        public bool needNewPositionOnHit { get; set; }
        public SpellComponent linkedSpellOnHit { get; set; }
        public bool needNewPositionOnAttack { get; set; }
        public SpellComponent linkedSpellOnAttack { get; set; }
        public bool needNewPositionOnInterval { get; set; }
        public SpellComponent linkedSpellOnInterval { get; set; }

        public ConditionReduceCharge conditionReduceCharge { get; set; }

        public Effect effectOnSelfWhenNoCharge { get; set; }

        public List<SpellWithCondition> spellWithCondition { get; set; }

        public BehaviorType newPlayerBehaviour { get; set; }

        public bool triggerInvocationCallOneTime { get; set; }
    }
}