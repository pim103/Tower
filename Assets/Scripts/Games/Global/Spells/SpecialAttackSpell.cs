namespace Games.Global.Spells
{
    public class SpecialAttackSpell : SpellComponent
    {
        public float damage;
        public Effect effectOnHit;

        public SpellComponent linkedSpellIfTargetDies;
    }
}
