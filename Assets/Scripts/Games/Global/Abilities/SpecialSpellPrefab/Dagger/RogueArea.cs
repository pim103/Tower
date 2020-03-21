using System.Collections;
using UnityEngine;

namespace Games.Global.Abilities.SpecialSpellPrefab.Dagger
{
    public class RogueArea : AreaSpell
    {
        private float duration;

        private Effect effect;

        private Effect buffOnKill;
        
        private IEnumerator TpAndHitOnTarget()
        {
            origin.ApplyEffect(effect);
            
            while (duration > 0)
            {
                ChooseRandomTarget();
                yield return new WaitForSeconds(0.2f);
                duration -= 0.2f;
            }
            
            gameObject.SetActive(false);
        }

        private void ChooseRandomTarget()
        {
            int monsterListSize = entityInZone.Count;

            if (monsterListSize == 0)
            {
                return;
            }
            
            int idTarget = Random.Range(0, monsterListSize);
            EntityPrefab target = entityInZone[idTarget].entityPrefab;

            origin.entityPrefab.transform.position = target.transform.position - (target.transform.forward);
            origin.entityPrefab.transform.localEulerAngles = target.transform.localEulerAngles;
            origin.entityPrefab.hand.transform.LookAt(target.transform);
            
            origin.BasicAttack();

            if (entityInZone[idTarget].hp <= 0)
            {
                entityInZone.RemoveAt(idTarget);
                origin.ApplyEffect(buffOnKill);
            }
        }

        public override void ActiveArea()
        {
            duration = 5;
            effect = new Effect { typeEffect = TypeEffect.Untargetable, durationInSeconds = duration };
            buffOnKill = new Effect { typeEffect = TypeEffect.AttackUp, durationInSeconds = 2, level = 1} ;
            StartCoroutine(TpAndHitOnTarget());
        }
    }
}
