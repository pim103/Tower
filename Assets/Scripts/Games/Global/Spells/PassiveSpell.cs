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

        public float interval { get; set; }
        public SpellComponent linkedEffectOnInterval { get; set; }

        public SpellComponent permanentLinkedEffect { get; set; }
        public Spell newDefensiveSpell { get; set; }
    }
}