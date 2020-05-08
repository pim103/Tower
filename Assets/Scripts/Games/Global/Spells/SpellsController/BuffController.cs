using System.Collections;
using UnityEngine;

namespace Games.Global.Spells.SpellsController
{
    public class BuffController : MonoBehaviour, ISpellController
    {
        public void LaunchSpell(Entity entity, SpellComponent spellComponent)
        {
            Coroutine currentCoroutine = StartCoroutine(PlayBuffSpell(entity, spellComponent));
            spellComponent.currentCoroutine = currentCoroutine;
        }

        private IEnumerator PlayBuffSpell(Entity entity, SpellComponent spellComponent)
        {
            BuffSpell buffSpell = (BuffSpell) spellComponent;

            InitialBuff(entity, buffSpell);
            float duration = buffSpell.duration;

            while (duration > 0)
            {
                yield return new WaitForSeconds(buffSpell.interval);
                duration -= buffSpell.interval;

                if (buffSpell.linkedSpellOnInterval != null)
                {
                    SpellController.CastSpellComponent(entity, buffSpell.linkedSpellOnInterval);
                }
            }

            while (buffSpell.stack > 0)
            {
                yield return null;
            }

            if (buffSpell.stack == 0)
            {
                EffectController.ApplyEffect(entity, buffSpell.effectOnSelfWhenNoCharge);
            }

            EndBuff(entity, buffSpell);
        }

        private void InitialBuff(Entity entity, BuffSpell buffSpell)
        {
            entity.currentBuff.Add(buffSpell);

            foreach (Effect effect in buffSpell.effectOnSelf)
            {
                EffectController.ApplyEffect(entity, effect);
            }

            foreach (Effect effect in buffSpell.effectOnSelfOnDamageReceived)
            {
                entity.damageReceiveExtraEffect.Add(effect);
            }

            foreach (Effect effect in buffSpell.effectOnTargetOnHit)
            {
                entity.damageDealExtraEffect.Add(effect);
            }

            entity.ApplyDamage(buffSpell.damageOnSelf);
        }

        public static void EndBuff(Entity entity, BuffSpell buffSpell)
        {
            entity.currentBuff.Remove(buffSpell);

            foreach (Effect effect in buffSpell.effectOnSelf)
            {
                EffectController.StopCurrentEffect(entity, effect);
            }

            foreach (Effect effect in buffSpell.effectOnSelfOnDamageReceived)
            {
                entity.damageReceiveExtraEffect.Remove(effect);
            }

            foreach (Effect effect in buffSpell.effectOnTargetOnHit)
            {
                entity.damageDealExtraEffect.Remove(effect);
            }

            if (buffSpell.currentCoroutine != null)
            {
                buffSpell.StopCoroutine(buffSpell.currentCoroutine);
            }
        }

        public static void EntityDealDamage(Entity entity)
        {
            foreach (BuffSpell buffSpell in entity.currentBuff)
            {
                if (buffSpell.linkedSpellOnHit)
                {
                    SpellController.CastSpellComponent(entity, buffSpell.linkedSpellOnHit);
                }
            }
        }

        public static void EntityReceivedDamage(Entity entity)
        {
            foreach (BuffSpell buffSpell in entity.currentBuff)
            {
                if (buffSpell.linkedSpellOnDamageReceived)
                {
                    SpellController.CastSpellComponent(entity, buffSpell.linkedSpellOnDamageReceived);
                }

                if (buffSpell.conditionReduceCharge == ConditionReduceCharge.OnDamageReceived)
                {
                    buffSpell.stack--;
                }

                if (buffSpell.disapearOnDamageReceived)
                {
                    EndBuff(entity, buffSpell);
                }
            }
        }

        public static void EntityAttack(Entity entity)
        {
            foreach (BuffSpell buffSpell in entity.currentBuff)
            {
                if (buffSpell.linkedSpellOnAttack)
                {
                    SpellController.CastSpellComponent(entity, buffSpell.linkedSpellOnAttack);
                }

                if (buffSpell.conditionReduceCharge == ConditionReduceCharge.OnAttack)
                {
                    buffSpell.stack--;
                }
            }
        }
    }
}