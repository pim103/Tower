using System;
using System.Collections;
using System.Collections.Generic;
using Games.Global.Entities;
using Games.Players;
using UnityEngine;

namespace Games.Global.Abilities.SpecialSpellPrefab.Bow
{
    public class WarriorBigSpear : SpellScript
    {
        public Entity origin;
        
        private List<Entity> entityInSpear;

        private Effect effect;

        public IEnumerator StunEntity(Entity entity)
        {
            while (true)
            {
                entity.ApplyEffect(effect);

                yield return new WaitForSeconds(0.2f);
            }
        }

        public void Awake()
        {
            effect = new Effect { typeEffect = TypeEffect.Stun, durationInSeconds = 0.2f, level = 1};
            entityInSpear = new List<Entity>();
        }

        public override void PlaySpecialEffect(Entity origin, Entity target)
        {
            entityInSpear.Add(target);
            StartCoroutine(StunEntity(target));
        }

        public void Update()
        {
            foreach (Entity entity in entityInSpear)
            {
                entity.entityPrefab.transform.position = transform.position;
            }
        }
    }
}
