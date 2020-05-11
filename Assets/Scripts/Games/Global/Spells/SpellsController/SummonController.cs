using System.Collections;
using Games.Global.Spells.SpellParameter;
using Games.Global.Weapons;
using UnityEngine;

namespace Games.Global.Spells.SpellsController
{
    public class SummonController : MonoBehaviour, ISpellController
    {
        public SummonSpell Clone(SpellComponent spellComponent)
        {
            SummonSpell origin = (SummonSpell) spellComponent;

            SummonSpell cloneSummonSpell = new SummonSpell
            {
                duration = origin.duration,
                hp = origin.hp,
                spells = origin.spells,
                attackDamage = origin.attackDamage,
                attackSpeed = origin.attackSpeed,
                basicAttack = origin.basicAttack,
                canMove = origin.canMove,
                currentCoroutine = origin.currentCoroutine,
                damageType = origin.damageType,
                isTargetable = origin.isTargetable,
                isUnique = origin.isUnique,
                moveSpeed = origin.moveSpeed,
                positionPresets = origin.positionPresets,
                prefabsSummon = origin.prefabsSummon,
                startPosition = origin.startPosition,
                summonNumber = origin.summonNumber,
                typeSpell = origin.typeSpell,
                idPoolObject = origin.idPoolObject,
                isBasicAttack = origin.isBasicAttack,
                nbUseSpells = origin.nbUseSpells,
                BehaviorType = origin.BehaviorType,
                linkedSpellOnDisapear = origin.linkedSpellOnDisapear,
                linkedSpellOnEnable = origin.linkedSpellOnEnable,
                positionToStartSpell = origin.positionToStartSpell,
                spellWhenPlayerCall = origin.spellWhenPlayerCall
            };
            return cloneSummonSpell;
        }
        public void LaunchSpell(Entity entity, SpellComponent spellComponent)
        {
            SummonSpell summonSpell = Clone(spellComponent);
            Coroutine currentCoroutine = SpellController.instance.StartCoroutine(PlaySummonSpell(entity, summonSpell));
            summonSpell.currentCoroutine = currentCoroutine;
        }

        public IEnumerator PlaySummonSpell(Entity entity, SummonSpell summonSpell)
        {
            PoolSummon(entity, summonSpell);
            
            float duration = summonSpell.duration;
            while (duration > 0)
            {
                yield return new WaitForSeconds(0.05f);
                duration -= 0.05f;
            }

            EndSummon(entity, summonSpell);
        }

        private void PoolSummon(Entity entity, SummonSpell summonSpell)
        {
            if (summonSpell.isUnique)
            {
                foreach (GenericSummonSpell summon in summonSpell.prefabsSummon)
                {
                    summon.DestroySummon();
                }
            }
            
            for (int nbSummon = 0; nbSummon < summonSpell.summonNumber; nbSummon++)
            {
                GameObject summon = ObjectPooler.SharedInstance.GetPooledObject(summonSpell.idPoolObject);
                summon.transform.position = summonSpell.startPosition + summonSpell.positionPresets[nbSummon];

                GenericSummonSpell genericSummonSpell = summon.GetComponent<GenericSummonSpell>();
                genericSummonSpell.SummonEntity(entity, summonSpell, summon);

                summonSpell.prefabsSummon.Add(genericSummonSpell);
            }
        }

        public static void EndSummon(Entity entity, SummonSpell summonSpell)
        {
            foreach (GenericSummonSpell summon in summonSpell.prefabsSummon)
            {
                summon.DestroySummon();
            }

            if (summonSpell.currentCoroutine != null)
            {
                SpellController.instance.StopCoroutine(summonSpell.currentCoroutine);
            }
        }
    }
}
