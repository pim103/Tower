namespace Games.Global.Spells
{
    public class SpecialAttackSpell : SpellComponent
    {
        private void Start()
        {
            typeSpell = TypeSpell.SpecialAttack;
        }
        
        public float damage;
        public Effect effectOnHit;

        public SpellComponent linkedSpellIfTargetDies;
    }
}
