using System.Collections;
using Games.Global.Spells.SpellParameter;
using Games.Global.Weapons;
using UnityEngine;

namespace Games.Global.Spells.SpellsController
{
    public class SummonController : MonoBehaviour, ISpellController
    {
        public void LaunchSpell(Entity entity, SpellComponent spellComponent)
        {
            Coroutine currentCoroutine = SpellController.instance.StartCoroutine(PlaySummonSpell(entity, spellComponent));
            spellComponent.currentCoroutine = currentCoroutine;
        }

        public IEnumerator PlaySummonSpell(Entity entity, SpellComponent spellComponent)
        {
            SummonSpell summonSpell = (SummonSpell) spellComponent;

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
