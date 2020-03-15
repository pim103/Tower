using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Games.Global.Abilities.SpecialSpellPrefab.Bow
{
    public class AreaSpellArrow3 : AreaSpell
    {
        private Effect effect1;
        private Effect effect2;

        private Transform originParent;

        private Coroutine currentCoroutine;

        public void Start()
        {
            effect1 = new Effect {typeEffect = TypeEffect.Burn, durationInSeconds = 1};
            effect2 = new Effect {typeEffect = TypeEffect.Weak, durationInSeconds = 1};

            originParent = transform.parent;
        }

        public IEnumerator TimerBetweenDisapear()
        {
            transform.parent = null;
            int duration = 5;

            while (duration > 0)
            {
                yield return new WaitForSeconds(1);

                foreach (Entity entity in entityInZone)
                {
                    entity.ApplyEffect(effect1);
                    entity.ApplyEffect(effect2);
                }

                Debug.Log(entityInZone.Count);
                
                duration--;
            }

            transform.parent = originParent;
        }

        public override void TriggerAreaEffect(Entity entity)
        {
            if (currentCoroutine == null)
            {
                currentCoroutine = StartCoroutine(TimerBetweenDisapear());
            }
        }
        
        public override void QuitAreaEffect(Entity entity)
        {
        }
    }
}
