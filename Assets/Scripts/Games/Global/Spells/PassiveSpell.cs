namespace Games.Global.Spells
{
    public class PassiveSpell : SpellComponent
    {
        public PassiveSpell()
        {
            typeSpell = TypeSpell.Passive;
        }
        
        public float interval;
        public SpellComponent linkedEffectOnInterval;

        public SpellComponent permanentLinkedEffect;
        public SpellComponent newDefensiveSpell;
    }
}
