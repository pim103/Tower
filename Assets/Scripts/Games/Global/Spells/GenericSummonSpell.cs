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

        public GameObject selfGameObject;

        public void SummonEntity(Entity newSummoner, SummonSpell summonSpell, GameObject newGameObject)
        {
            summoner = newSummoner;
            selfGameObject = newGameObject;
            canMove = summonSpell.canMove;

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
                isUntargeatable = !summonSpell.isTargetable,
                basicAttack = (summonSpell.basicAttack != null ? Tools.Clone(summonSpell.basicAttack) : null),
                isSummon = true,
                IdEntity = DataObject.nbEntityInScene
            };

            entity.SetBehaviorType(summonSpell.BehaviorType);
            entity.SetTypeEntity(summoner.GetTypeEntity());
            entity.SetAttackBehaviorType(summonSpell.AttackBehaviorType);
            entity = summon;
            entity.InitEntityList();
            entity.entityPrefab = this;
            entity.spells = spellsClone;

            SpellController.CastPassiveSpell(entity);

            DataObject.invocationsInScene.Add(summon);
            DataObject.nbEntityInScene++;
        }

        public void DestroySummon()
        {
            DataObject.invocationsInScene.Remove(summon);
            selfGameObject.SetActive(false);
        }
    }
}
