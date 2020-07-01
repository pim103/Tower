using System.Collections;
using System.Collections.Generic;
using Games.Global.Spells.SpellParameter;
using Games.Global.Weapons;
using UnityEngine;
using Utils;

namespace Games.Global.Spells.SpellsController
{
    public class SummonController : MonoBehaviour, ISpellController
    {
        public void LaunchSpell(Entity entity, SpellComponent spellComponent)
        {
            SummonSpell summonSpell = Tools.Clone((SummonSpell) spellComponent);
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
                List<Entity> invocs = DataObject.invocationsInScene.FindAll(invoc => ((GenericSummonSpell) invoc.entityPrefab).summoner == entity);

                foreach (Entity summon in invocs)
                {
                    GenericSummonSpell genericSummonSpell = (GenericSummonSpell) summon.entityPrefab;
                    if (genericSummonSpell.summoner == entity)
                    {
                        genericSummonSpell.DestroySummon();
                    }
                }
            }

            summonSpell.prefabsSummon = new List<GenericSummonSpell>();

            for (int nbSummon = 0; nbSummon < summonSpell.summonNumber; nbSummon++)
            {
                GameObject summon = ObjectPooler.SharedInstance.GetPooledObject(summonSpell.idPoolObject);
                summon.transform.position = summonSpell.startPosition + GroupsPosition.position[nbSummon];

                GenericSummonSpell genericSummonSpell = summon.GetComponent<GenericSummonSpell>();
                genericSummonSpell.SummonEntity(entity, summonSpell, summon);

                summon.SetActive(true);
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
