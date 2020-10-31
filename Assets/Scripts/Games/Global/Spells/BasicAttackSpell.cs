using System.Collections.Generic;
using System.Linq;
using Utils;

namespace Games.Global.Spells
{
    public class BasicAttackSpell : SpellComponent
    {
        public override void OnTriggerEnter(Entity enemy)
        {
            bool damageIsNull = (enemy.isIntangible && damageType == DamageType.Physical) ||
                                (enemy.hasAntiSpell && damageType == DamageType.Magical) ||
                                caster.isBlind ||
                                enemy.isUntargeatable ||
                                caster.hasDivineShield;

            List<Effect> effects = caster.damageDealExtraEffect.DistinctBy(currentEffect => currentEffect.typeEffect)
                .ToList();
            foreach (Effect effect in effects)
            {
                if (effect == null)
                {
                    continue;
                }
                EffectController.ApplyEffect(enemy, effect, caster, startAtPosition);
            }

            float damage = caster.att;
            if (caster.isWeak)
            {
                damage /= 2;
            }
        
            if (enemy.hasMirror && damageType == DamageType.Magical)
            {
                caster.TakeDamage(damage * 0.4f, caster, DamageType.Magical, this);
            }

            if (enemy.hasThorn && damageType == DamageType.Physical)
            {
                caster.TakeDamage(damage * 0.4f, caster, DamageType.Physical, this);
            }

            damage = damageIsNull ? 0 : damage;

            enemy.TakeDamage(damage, caster, damageType, this);
        }
    }
}