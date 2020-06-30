﻿using System;
using System.Collections.Generic;
using Games.Global.Spells.SpellsController;
using UnityEngine;
using UnityEngine.AI;

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
                    spellsClone.Add(SpellController.Clone(spellOrigin, summonSpell.nbUseSpells));
                }
            }

            summon = new Entity
            {
                def = 0,
                magicalDef = 0,
                physicalDef = 0,
                hp = summonSpell.hp,
                attSpeed = summonSpell.attackSpeed,
                att = summonSpell.attackDamage,
                speed = summonSpell.moveSpeed,
                typeEntity = summoner.typeEntity,
                isUntargeatable = !summonSpell.isTargetable,
                spells = spellsClone,
                basicAttack = (summonSpell.basicAttack != null ? SpellController.Clone(summonSpell.basicAttack) : null),
                BehaviorType = summonSpell.BehaviorType,
                AttackBehaviorType = summonSpell.AttackBehaviorType,
                isSummon = true
            };

            entity = summon;
            entity.InitEntityList();
            entity.entityPrefab = this;

            if (linkedSpellOnEnable != null)
            {
                SpellController.CastSpellComponent(summon, linkedSpellOnEnable, selfGameObject.transform.position, summon);
            }

            SpellController.CastPassiveSpell(entity);

            DataObject.invocationsInScene.Add(summon);
        }

        public void DestroySummon()
        {
            if (linkedSpellOnDisapear != null)
            {
                SpellController.CastSpellComponent(summoner, linkedSpellOnDisapear, selfGameObject.transform.position, summoner);
            }

            DataObject.invocationsInScene.Remove(summon);
            selfGameObject.SetActive(false);
        }

        public void TriggerSpellWhenPlayerCall()
        {
            if (spellWhenPlayerCall != null)
            {
                SpellController.CastSpellComponent(entity, spellWhenPlayerCall, transform.position, target);
            }
        }
    }
}