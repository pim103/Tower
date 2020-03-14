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
                    InitRangerSpell();
                    break;
                case Classes.Warrior:
                    break;
            }
        }

        public void InitRegenWhenHit()
        {
            Effect effect = new Effect {typeEffect = TypeEffect.Regen, durationInSeconds = 5};

            SpellInstruction spellInstruction = new SpellInstruction
            {
                TypeSpellInstruction = TypeSpellInstruction.SelfEffectOnDamageReceive,
                effect = effect,
                durationInstruction = 20
            };

            Spell spell = new Spell {typeSpell = TypeSpell.Active, cooldown = 20, cost = 10, castTime = 0};
            spell.spellInstructions.Add(spellInstruction);

            skill1 = spell;
        }

        public void InitRangerSpell()
        {
            /* Spell 1 */
            Effect effect = new Effect { typeEffect = TypeEffect.AttackSpeedUp, durationInSeconds = 3, level = 1 };
            Effect bleed = new Effect {typeEffect = TypeEffect.Bleed, durationInSeconds = 5, level = 1 };

            SpellInstruction spellInstruction = new SpellInstruction
            {
                TypeSpellInstruction = TypeSpellInstruction.SelfEffect,
                effect = effect,
                timeWait = 0
            };
            
            SpellInstruction spellInstruction2 = new SpellInstruction
            {
                TypeSpellInstruction = TypeSpellInstruction.EffectOnTargetWhenDamageDeal,
                effect = bleed,
                timeWait = 0,
                durationInstruction = 5
            };

            Spell spell1 = new Spell {typeSpell = TypeSpell.Active, cooldown = 10, cost = 10, castTime = 0};
            spell1.spellInstructions.Add(spellInstruction);
            spell1.spellInstructions.Add(spellInstruction2);

            /* Spell 2 */

            spellInstruction = new SpellInstruction
            {
                TypeSpellInstruction = TypeSpellInstruction.InstantiateSomething,
                idPoolObject = 1,
                timeWait = 0,
                typeSpellObject = TypeSpellObject.Projectile
            };

            Spell spell2 = new Spell { typeSpell = TypeSpell.Active, cooldown = 4, cost = 10};
            spell2.spellInstructions.Add(spellInstruction);

            /* Spell 3 */

            spellInstruction = new SpellInstruction
            {
                TypeSpellInstruction = TypeSpellInstruction.InstantiateSomething,
                idPoolObject = 2,
                timeWait = 0,
                typeSpellObject = TypeSpellObject.Projectile
            };

            Spell spell3 = new Spell { typeSpell = TypeSpell.Active, cooldown = 15, cost = 10};
            spell3.spellInstructions.Add(spellInstruction);

            /* Setting Spell */

            skill1 = spell1;
            skill2 = spell2;
            skill3 = spell3;
        }
    }
}
