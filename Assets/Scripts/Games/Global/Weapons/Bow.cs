using System.Collections.Generic;
using Games.Global.Abilities;
//using Games.Global.Patterns;
using Games.Players;
using UnityEngine;
//using PA_INST = Games.Global.Patterns.PatternInstructions;

namespace Games.Global.Weapons
{
    public class Bow: Weapon
    {
        public Bow()
        {
           //pattern = //pattern[2];
           //pattern[0] = //pattern(PA_INST.BACK, 0.2f);
           //pattern[1] = //pattern(PA_INST.FRONT, 0.2f);
           animationToPlay = "BowAttack";
           idPoolProjectile = 0;
        }

        public override void FixAngleAttack(bool isFirstIteration, Entity wielder)
        {
            if (isFirstIteration)
            {
                wielder.entityPrefab.characterMesh.transform.Rotate(Vector3.up * 90);
            }
            else
            {
                wielder.entityPrefab.characterMesh.transform.Rotate(Vector3.down * 90);
            }
        }

        private void InitRangerSpell()
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

        private void InitMageSpell()
        {
            /* Spell 1 */
            Effect linkEffect = new Effect {typeEffect = TypeEffect.Link, durationInSeconds = 5, level = 1 };
            Effect fireEffect = new Effect {typeEffect = TypeEffect.Bleed, durationInSeconds = 5, level = 1 };
            
            SpellInstruction spellInstruction = new SpellInstruction
            {
                TypeSpellInstruction = TypeSpellInstruction.EffectOnTargetWhenDamageDeal,
                effect = linkEffect,
                timeWait = 0,
                durationInstruction = -1
            };
            
            SpellInstruction spellInstruction2 = new SpellInstruction
            {
                TypeSpellInstruction = TypeSpellInstruction.EffectOnTargetWhenDamageDeal,
                effect = fireEffect,
                timeWait = 0,
                durationInstruction = -1
            };

            Spell spell1 = new Spell {typeSpell = TypeSpell.Passive, cooldown = 10, castTime = 0};
            spell1.spellInstructions.Add(spellInstruction);
            spell1.spellInstructions.Add(spellInstruction2);

            /* Spell 2 */

            spellInstruction = new SpellInstruction
            {
                TypeSpellInstruction = TypeSpellInstruction.InstantiateSomething,
                idPoolObject = 3,
                timeWait = 0,
                typeSpellObject = TypeSpellObject.Projectile
            };

            Spell spell2 = new Spell { typeSpell = TypeSpell.Active, cooldown = 4, cost = 10};
            spell2.spellInstructions.Add(spellInstruction);

            /* Spell 3 */

            spellInstruction = new SpellInstruction
            {
                TypeSpellInstruction = TypeSpellInstruction.InstantiateSomething,
                idPoolObject = 4,
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
        
        private void InitWarriorSpell()
        {
            WeaponNewStats weaponNewStats = new WeaponNewStats {attSpeedModifier = 0.5f, damageModifier = 1, typeWeapon = TypeWeapon.Distance};
            
            SpellInstruction spellInstruction = new SpellInstruction
            {
                TypeSpellInstruction = TypeSpellInstruction.ChangeBasicAttack,
                idPoolObject = 5,
                weaponNewStats = weaponNewStats
            };

            Spell spell1 = new Spell {typeSpell = TypeSpell.Passive, cooldown = 10, castTime = 0};
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

        private void InitRogueSpell()
        {
            
        }
    }
}
