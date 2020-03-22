using System;
using System.Diagnostics;
using Games.Global.Patterns;
using Games.Players;
using PA_INST = Games.Global.Patterns.PatternInstructions;

namespace Games.Global.Weapons
{
    [Serializable]
    public class Sword : Weapon
    {
        public Sword()
        {
            pattern = new Pattern[4];
            
            pattern[0] = new Pattern(PA_INST.ROTATE_DOWN, 90, 0.1f, 0.01f);
            pattern[1] = new Pattern(PA_INST.ROTATE_LEFT, 90, 0.1f, 0.01f);
            pattern[2] = new Pattern(PA_INST.ROTATE_RIGHT, 90, 0.1f, 0.01f);
            pattern[3] = new Pattern(PA_INST.ROTATE_UP, 90, 0.1f, 0.01f);
        }

        public override void InitPlayerSkill(Classes classe)
        {
            base.InitPlayerSkill(classe);

            switch (classe)
            {
                case Classes.Mage:
                    break;
                case Classes.Rogue:
                    break;
                case Classes.Ranger:
                    break;
                case Classes.Warrior:
                    InitWarriorSpell();
                    break;
            }
        }

        private void InitWarriorSpell()
        {   
            SpellInstruction spellInstruction = new SpellInstruction
            {
                TypeSpellInstruction = TypeSpellInstruction.SpecialMovement,
                specialMovement = SpecialMovement.Charge,
                timeWait = 0,
                durationInstruction = 3
            };

            Spell spell1 = new Spell {typeSpell = TypeSpell.Active, cooldown = 1, castTime = 0, cost = 1};
            spell1.spellInstructions.Add(spellInstruction);

            /* Spell 2 */

            Effect effect = new Effect { typeEffect = TypeEffect.DefeneseUp, level = 2 };
            
            spellInstruction = new SpellInstruction
            {
                TypeSpellInstruction = TypeSpellInstruction.SelfEffect,
                effect = effect,
                timeWait = 0,
                durationInstruction = 5
            };

            Spell spell2 = new Spell { typeSpell = TypeSpell.Active, cooldown = 10, cost = 10 };
            spell2.spellInstructions.Add(spellInstruction);

            /* Spell 3 */

            spellInstruction = new SpellInstruction
            {
                TypeSpellInstruction = TypeSpellInstruction.SpecialMovement,
                timeWait = 0,
                specialMovement = SpecialMovement.HeavyBasicAttack
            };

            Spell spell3 = new Spell { typeSpell = TypeSpell.Active, cooldown = 1, cost = 1, castTime = 2};
            spell3.spellInstructions.Add(spellInstruction);

            /* Setting Spell */

            skill1 = spell1;
            skill2 = spell2;
            skill3 = spell3;
        }
    }
}