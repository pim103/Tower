using System;
using Games.Global.Patterns;
using Games.Players;
using PA_INST = Games.Global.Patterns.PatternInstructions;

namespace Games.Global.Weapons
{
    [Serializable]
    public class Dagger : Weapon
    {
        public Dagger()
        {
            pattern = new Pattern[2];
            pattern[0] = new Pattern(PA_INST.FRONT, 1, 0.2f, 0.01f);
            pattern[1] = new Pattern(PA_INST.BACK, 1, 0.2f, 0.01f);
        }

        public override void InitPlayerSkill(Classes classe)
        {
            switch (classe)
            {
                case Classes.Mage:
                    break;
                case Classes.Rogue:
                    InitRogueSkill();
                    break;
                case Classes.Ranger:
                    break;
                case Classes.Warrior:
                    break;
            }
        }

        private void InitRogueSkill()
        {
            Effect effect = new Effect { typeEffect = TypeEffect.PierceOnBack, level = 1 };
            
            SpellInstruction spellInstruction = new SpellInstruction
            {
                TypeSpellInstruction = TypeSpellInstruction.SelfEffect,
                effect = effect,
                specificTypeSpell = TypeSpell.Passive
            };

            Spell spell1 = new Spell {typeSpell = TypeSpell.ActiveWithPassive, cooldown = 10, castTime = 0};
            spell1.spellInstructions.Add(spellInstruction);

            /* Spell 2 */

            spellInstruction = new SpellInstruction
            {
                TypeSpellInstruction = TypeSpellInstruction.InstantiateSomething,
                idPoolObject = 6,
                timeWait = 0,
                typeSpellObject = TypeSpellObject.Projectile
            };

            Spell spell2 = new Spell { typeSpell = TypeSpell.Active, cooldown = 10, cost = 10};
            spell2.spellInstructions.Add(spellInstruction);

            /* Spell 3 */

            spellInstruction = new SpellInstruction
            {
                TypeSpellInstruction = TypeSpellInstruction.InstantiateSomething,
                idPoolObject = 8,
                timeWait = 0,
                typeSpellObject = TypeSpellObject.GroundArea
            };

            Spell spell3 = new Spell { typeSpell = TypeSpell.Active, cooldown = 10, cost = 10 };
            spell3.spellInstructions.Add(spellInstruction);

            /* Setting Spell */

            skill1 = spell1;
            skill2 = spell2;
            skill3 = spell3;
        }
    }
}