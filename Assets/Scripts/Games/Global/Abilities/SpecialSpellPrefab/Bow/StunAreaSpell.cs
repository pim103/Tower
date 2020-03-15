namespace Games.Global.Abilities.SpecialSpellPrefab.Bow
{
    public class StunAreaSpell : AreaSpell
    {
        private Effect effect;
        
        public void Start()
        {
            effect = new Effect {typeEffect = TypeEffect.Stun, durationInSeconds = 3, level = 1};
        }

        public override void TriggerAreaEffect(Entity entity)
        {
            entity.ApplyEffect(effect);
        }
    }
}
