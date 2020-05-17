using System;

namespace Games.Global.Spells
{
    [Serializable]
    public class PassiveSpell : SpellComponent
    {
        public PassiveSpell()
        {
            typeSpell = TypeSpell.Passive;
        }

        public float interval;
        public SpellComponent linkedEffectOnInterval;

        public SpellComponent permanentLinkedEffect;
        public Spell newDefensiveSpell;
    }
}
