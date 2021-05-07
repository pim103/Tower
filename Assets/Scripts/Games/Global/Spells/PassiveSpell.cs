using System;
using Games.Global.Spells.SpellsController;

namespace Games.Global.Spells
{
    [Serializable]
    public class PassiveSpell : SpellComponent
    {
        public PassiveSpell()
        {
            TypeSpellComponent = TypeSpellComponent.Passive;
        }

        public SpellComponent permanentSpellComponent { get; set; }
        
        // use in game
        private SpellComponent permanentSpellComponentInstantiate;

        public override void DuringInterval()
        {
            spellDuration = 99999;

            if (caster.hasPassiveDeactivate && permanentSpellComponentInstantiate != null)
            {
                SpellInterpreter.EndSpellComponent(permanentSpellComponentInstantiate);
                permanentSpellComponentInstantiate = null;
            }
            else if (permanentSpellComponentInstantiate == null)
            {
                permanentSpellComponentInstantiate = SpellController.CastSpellComponent(caster, permanentSpellComponent, caster, caster.entityPrefab.transform.position, null, this);
            }
        }
    }
}