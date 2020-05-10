namespace Games.Global.Spells
{
    public class SpecialAttackSpell : SpellComponent
    {
        public SpecialAttackSpell()
        {
            typeSpell = TypeSpell.SpecialAttack;
        }
        
        public float damage;
        public Effect effectOnHit;

        public SpellComponent linkedSpellIfTargetDies;
    }
}
