using System;
using System.Collections;
using Games.Global.Weapons;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Games.Global.Abilities.SpecialSpellPrefab.Bow
{
    public class WarriorAreaSpell : AreaSpell
    {
        private int idPoolObject = 7;

        [SerializeField]
        private GameObject origPoint;

        private float duration = 5;

        private Effect effect;

        private IEnumerator AppearSpear()
        {
            while (duration > 0)
            {
                GameObject spear = ObjectPooler.SharedInstance.GetPooledObject(idPoolObject);

                int randX = Random.Range(-3, 3);
                int randZ = Random.Range(-3, 3);
                Vector3 pos = origPoint.transform.position;

                pos.x += randX;
                pos.z += randZ;

                spear.transform.position = pos;
                spear.SetActive(true);
                
                yield return new WaitForSeconds(0.1f);
                duration -= 0.1f;
            }

//            transform.parent = parent;
            gameObject.SetActive(false);
        }

        private IEnumerator DealDamage()
        {
            while (duration > 0)
            {
                foreach (Entity entity in entityInZone)
                {
                    entity.ApplyDamage(2);
                    entity.ApplyEffect(effect);
                }

                yield return new WaitForSeconds(0.5f);
            }
        }
        
        public override void ActiveArea()
        {
            base.ActiveArea();

            duration = 5;
            StartCoroutine(AppearSpear());
            StartCoroutine(DealDamage());

            effect = new Effect { typeEffect = TypeEffect.Slow, durationInSeconds = 0.5f, level = 1 };
        }

        public override void TriggerAreaEffect(Entity entity)
        {
            
        }
    }
}
