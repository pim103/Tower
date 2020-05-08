namespace Games.Global.Spells
{
    public class PassiveSpell : SpellComponent
    {
        public float interval;
        public SpellComponent linkedEffectOnInterval;

        public SpellComponent permanentLinkedEffect;
        public SpellComponent newDefensiveSpell;
    }
}
