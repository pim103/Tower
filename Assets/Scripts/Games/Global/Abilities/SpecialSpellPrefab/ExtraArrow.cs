using System;
using UnityEngine;

namespace Games.Global.Abilities.SpecialSpellPrefab
{
    public class ExtraArrow : SpellScript
    {
        private Effect effect;

        private void Start()
        {
            effect = new Effect { typeEffect = TypeEffect.BrokenDef, durationInSeconds = 5 };
        }

        public override void PlaySpecialEffect(Entity origin, Entity target)
        {
            target.ApplyEffect(effect);
            target.ApplyDamage(10);
        }
    }
}
