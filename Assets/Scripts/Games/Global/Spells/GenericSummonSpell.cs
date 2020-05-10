using System;
using System.Collections.Generic;
using Games.Global.Spells.SpellsController;
using UnityEngine;
using UnityEngine.AI;

namespace Games.Global.Spells
{
    public class GenericSummonSpell : EntityPrefab
    {
        [SerializeField] private NavMeshAgent navMeshAgent;
        
        public Entity summoner;
        public Entity summon;

        public int nbUseSpells;
        
        public SpellComponent linkedSpellOnEnable;
        public SpellComponent linkedSpellOnDisapear;

        public SpellComponent spellWhenPlayerCall;

        public SummonBehaviorType summonBehaviorType;

        public GameObject selfGameObject;

        public void SummonEntity(Entity newSummoner, SummonSpell summonSpell, GameObject newGameObject)
        {
            summoner = newSummoner;

            spellWhenPlayerCall = summonSpell.spellWhenPlayerCall;
            linkedSpellOnEnable = summonSpell.linkedSpellOnEnable;
            linkedSpellOnDisapear = summonSpell.linkedSpellOnDisapear;

            summonBehaviorType = summonSpell.summonBehaviorType;
            selfGameObject = newGameObject;

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
                spells = summonSpell.spells,
                basicAttack = summonSpell.basicAttack
            };

            if (linkedSpellOnEnable != null)
            {
                SpellController.CastSpellComponent(summon, linkedSpellOnEnable, selfGameObject.transform.position, summon);
            }
            
            DataObject.invocationsInScene.Add(summon);
        }

        private void FixedUpdate()
        {
            FindTarget();

            if (summonBehaviorType == SummonBehaviorType.Melee ||
                summonBehaviorType == SummonBehaviorType.MoveOnTargetAndDie)
            {
                MoveToTarget(navMeshAgent, 1);
            }
            else if (summonBehaviorType == SummonBehaviorType.Distance)
            {
                MoveToTarget(navMeshAgent, 10);
            }
            
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
    }
}
