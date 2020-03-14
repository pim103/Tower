using System;
using UnityEngine;

namespace Games.Global.Abilities.SpecialSpellPrefab.Bow
{
    public class MageArrow2 : SpellScript
    {
        private Effect effect1;
        private Effect effect2;
        private Effect effect3;
        
        private void Start()
        {
            effect1 = new Effect { typeEffect = TypeEffect.Burn, durationInSeconds = 1.5f };
            effect2 = new Effect { typeEffect = TypeEffect.Weak, durationInSeconds = 1.5f };
            effect3 = new Effect { typeEffect = TypeEffect.Slow, durationInSeconds = 1.5f };
        }

        public override void PlaySpecialEffect(Entity origin, Entity target)
        {
            target.ApplyEffect(effect1);
            target.ApplyEffect(effect2);
            target.ApplyEffect(effect3);
            
            target.ApplyDamage(initalDamage);
        }
    }
}
