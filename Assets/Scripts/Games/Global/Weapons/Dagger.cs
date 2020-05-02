using System;
using Games.Players;
using UnityEngine;

namespace Games.Global.Weapons
{
    [Serializable]
    public class Dagger : Weapon
    {
        public Dagger()
        {
            animationToPlay = "DaggerAttack";
        }
        
        public override void FixAngleAttack(bool isFirstIteration, Entity wielder)
        {
            if (isFirstIteration)
            {
                wielder.entityPrefab.characterMesh.transform.Rotate(Vector3.up * 70);
            }
            else
            {
                wielder.entityPrefab.characterMesh.transform.Rotate(Vector3.down * 70);
            }
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
                timeWait = 0,
                specificTypeSpell = TypeSpell.Passive,
                durationInstruction = -1
            };
            
            effect = new Effect { typeEffect = TypeEffect.AttackUp, level = 1, durationInSeconds = 4 };
            
            SpellInstruction spellInstruction2 = new SpellInstruction
            {
                TypeSpellInstruction = TypeSpellInstruction.SelfEffect,
                effect = effect,
                timeWait = 0,
                specificTypeSpell = TypeSpell.Active
            };
            
            effect = new Effect { typeEffect = TypeEffect.SpeedUp, level = 1, durationInSeconds = 4 };
            
            SpellInstruction spellInstruction3 = new SpellInstruction
            {
                TypeSpellInstruction = TypeSpellInstruction.SelfEffect,
                effect = effect,
                timeWait = 0,
                specificTypeSpell = TypeSpell.Active
            };

            Spell spell1 = new Spell {typeSpell = TypeSpell.ActiveWithPassive, cooldown = 9, castTime = 0, cost = 10};
            spell1.spellInstructions.Add(spellInstruction);
            spell1.spellInstructions.Add(spellInstruction2);
            spell1.spellInstructions.Add(spellInstruction3);

            /* Spell 2 */

            spellInstruction = new SpellInstruction
            {
                TypeSpellInstruction = TypeSpellInstruction.InstantiateSomething,
                idPoolObject = 9,
                timeWait = 0,
                typeSpellObject = TypeSpellObject.OnHimself
            };

            Spell spell2 = new Spell { typeSpell = TypeSpell.Active, cooldown = 10, cost = 10 };
            spell2.spellInstructions.Add(spellInstruction);

            /* Spell 3 */

            spellInstruction = new SpellInstruction
            {
                TypeSpellInstruction = TypeSpellInstruction.InstantiateSomething,
                idPoolObject = 10,
                timeWait = 0,
                typeSpellObject = TypeSpellObject.GroundArea
            };

            Spell spell3 = new Spell { typeSpell = TypeSpell.Active, cooldown = 1, cost = 1 };
            spell3.spellInstructions.Add(spellInstruction);

            /* Setting Spell */

            skill1 = spell1;
            skill2 = spell2;
            skill3 = spell3;
        }
    }
}