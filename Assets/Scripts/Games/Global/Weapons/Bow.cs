using System.Collections.Generic;
using Games.Global.Abilities;
using Games.Global.Patterns;
using Games.Players;
using UnityEngine;
using PA_INST = Games.Global.Patterns.PatternInstructions;

namespace Games.Global.Weapons
{
    public class Bow: Weapon
    {
        public Bow()
        {
            pattern = new Pattern[2];
            pattern[0] = new Pattern(PA_INST.BACK, 0.2f);
            pattern[1] = new Pattern(PA_INST.FRONT, 0.2f);
        }

        public override void InitPlayerSkill(Classes classe)
        {
            base.InitPlayerSkill(classe);

            switch (classe)
            {
                case Classes.Mage:
                    break;
                case Classes.Rogue:
                    InitRegenWhenHit();
                    break;
                case Classes.Ranger:
                    break;
                case Classes.Warrior:
                    break;
            }
        }

        public void InitRegenWhenHit()
        {
            Effect effect = new Effect();
            effect.typeEffect = TypeEffect.Regen;
            effect.durationInSeconds = 5;

            SpellInstruction spellInstruction = new SpellInstruction();
            spellInstruction.typeSpell = TypeSpell.EffectOnDamageReceive;
            spellInstruction.effect = effect;
            spellInstruction.durationInstruction = 20;

            Spell spell = new Spell();
            spell.isPassive = false;
            spell.cooldown = 20;
            spell.cost = 10;
            spell.castTime = 0;
            spell.spellInstructions.Add(spellInstruction);

            skill1 = spell;
        }
    }
}
