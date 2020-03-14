namespace Games.Global.Abilities.SpecialSpellPrefab.Bow
{
    public class StunArea : ExplosionArea
    {
        private Effect effect;
        
        public void Start()
        {
            effect = new Effect {typeEffect = TypeEffect.Stun, durationInSeconds = 3, level = 1};
        }

        public override void TriggerExplosion(Entity entity)
        {
            entity.ApplyEffect(effect);
        }
    }
}
