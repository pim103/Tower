using System;
using System.Collections.Generic;
using Games.Global.Spells.SpellsController;
using UnityEngine;
using UnityEngine.AI;
using Utils;

namespace Games.Global.Spells
{
    public class GenericSummonSpell : EntityPrefab
    {
        public Entity summoner;
        public Entity summon;

        public SpellComponent linkedSpellOnEnable;
        public SpellComponent linkedSpellOnDisapear;

        public SpellComponent spellWhenPlayerCall;

        public GameObject selfGameObject;

        public void SummonEntity(Entity newSummoner, SummonSpell summonSpell, GameObject newGameObject)
        {
            summoner = newSummoner;

            spellWhenPlayerCall = summonSpell.spellWhenPlayerCall;
            linkedSpellOnEnable = summonSpell.linkedSpellOnEnable;
            linkedSpellOnDisapear = summonSpell.linkedSpellOnDisapear;

            selfGameObject = newGameObject;

            List<Spell> spellsClone = new List<Spell>(); 

            if (summonSpell.spells != null)
            {
                foreach (Spell spellOrigin in summonSpell.spells)
                {
                    spellsClone.Add(Tools.Clone(spellOrigin));
                }
            }

            summon = new Entity
            {
                initialDef = 0,
                initialMagicalDef = 0,
                initialPhysicalDef = 0,
                def = 0,
                magicalDef = 0,
                physicalDef = 0,
                initialHp = summonSpell.hp,
                hp = summonSpell.hp,
                initialAttSpeed = summonSpell.attackSpeed,
                attSpeed = summonSpell.attackSpeed,
                initialAtt = summonSpell.attackDamage,
                att = summonSpell.attackDamage,
                initialSpeed = summonSpell.moveSpeed,
                speed = summonSpell.moveSpeed,
                typeEntity = summoner.typeEntity,
                isUntargeatable = !summonSpell.isTargetable,
                basicAttack = (summonSpell.basicAttack != null ? Tools.Clone(summonSpell.basicAttack) : null),
                BehaviorType = summonSpell.BehaviorType,
                AttackBehaviorType = summonSpell.AttackBehaviorType,
                isSummon = true,
                IdEntity = DataObject.nbEntityInScene
            };

            entity = summon;
            entity.InitEntityList();
            entity.entityPrefab = this;
            entity.spells = spellsClone;

            if (linkedSpellOnEnable != null)
            {
                SpellController.CastSpellComponent(summon, linkedSpellOnEnable, target);
            }

            SpellController.CastPassiveSpell(entity);

            DataObject.invocationsInScene.Add(summon);
            DataObject.nbEntityInScene++;
        }

        public void DestroySummon()
        {
            if (linkedSpellOnDisapear != null)
            {
                SpellController.CastSpellComponent(summoner, linkedSpellOnDisapear, target);
            }

            DataObject.invocationsInScene.Remove(summon);
            selfGameObject.SetActive(false);
        }

        public void TriggerSpellWhenPlayerCall()
        {
            if (spellWhenPlayerCall != null)
            {
                SpellController.CastSpellComponent(entity, spellWhenPlayerCall, target);
            }
        }
    }
}
