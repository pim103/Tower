namespace Games.Global.Spells
{
    public class PassiveSpell : SpellComponent
    {
        private void Start()
        {
            typeSpell = TypeSpell.Passive;
        }
        
        public float interval;
        public SpellComponent linkedEffectOnInterval;

        public SpellComponent permanentLinkedEffect;
        public SpellComponent newDefensiveSpell;
    }
}
