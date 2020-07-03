namespace Games.Global.Spells.SpellsController
{
    public interface ISpellController
    {
        void LaunchSpell(Entity entity, SpellComponent spellComponent, SpellComponent origin = null);
    }
}