using System.Collections.Generic;
using Games.Global.Spells.SpellsController;
using UnityEngine;

namespace Games.Global.Spells
{
    public class GenericSummonSpell : EntityPrefab
    {
        public Entity summoner;

        public Entity summon;
        
        public bool isTargeatable;
        public int nbUseSpells;
        public bool canMove;
        
        public SpellComponent linkedSpellOnEnable;
        public SpellComponent linkedSpellOnDisapear;

        public List<SpellComponent> spells;
        public SpellComponent basicAttack;
        public SpellComponent spellWhenPlayerCall;

        public SummonBehaviorType summonBehaviorType;

        public GameObject selfGameObject;

        public void SummonEntity(Entity newSummoner, SummonSpell summonSpell, GameObject gameObject)
        {
            summoner = newSummoner;
            canMove = summonSpell.canMove;
            
            spells = summonSpell.spells;
            basicAttack = summonSpell.basicAttack;
            spellWhenPlayerCall = summonSpell.spellWhenPlayerCall;
            linkedSpellOnEnable = summonSpell.linkedSpellOnEnable;
            linkedSpellOnDisapear = summonSpell.linkedSpellOnDisapear;

            summonBehaviorType = summonSpell.summonBehaviorType;
            selfGameObject = gameObject;
            
            summon = new Entity();
            summon.def = 0;
            summon.magicalDef = 0;
            summon.physicalDef = 0;
            summon.hp = summonSpell.hp;
            summon.attSpeed = summonSpell.attackSpeed;
            summon.att = summonSpell.attackDamage;
            summon.speed = summonSpell.moveSpeed;

            if (linkedSpellOnEnable != null)
            {
                SpellController.CastSpellComponent(summon, linkedSpellOnEnable, selfGameObject.transform.position, summon);
            }
        }

        public void DestroySummon()
        {
            if (linkedSpellOnDisapear != null)
            {
                SpellController.CastSpellComponent(summoner, linkedSpellOnDisapear, selfGameObject.transform.position, summoner);
            }

            selfGameObject.SetActive(false);
        }
    }
}
