using System;
using System.Collections.Generic;
using UnityEngine;
using Utils;
using Object = UnityEngine.Object;

namespace Games.Global.Spells
{
    [Serializable]
    public class SummonSpell : SpellComponent
    {
        public SummonSpell()
        {
            TypeSpellComponent = TypeSpellComponent.Summon;
        }

        public override void AtTheStart()
        {
            if (isUnique)
            {
                List<Entity> invocs = DataObject.invocationsInScene.FindAll(invoc => ((GenericSummonSpell) invoc.entityPrefab).summoner == caster);

                foreach (Entity summon in invocs)
                {
                    GenericSummonSpell genericSummonSpell = (GenericSummonSpell) summon.entityPrefab;
                    if (genericSummonSpell.summoner == caster)
                    {
                        genericSummonSpell.DestroySummon();
                    }
                }
            }

            prefabsSummon = new List<GenericSummonSpell>();

            for (int nbSummon = 0; nbSummon < summonNumber; nbSummon++)
            {
                GameObject summon = Object.Instantiate((GameObject) Resources.Load(pathObjectToInstantiate));
                summon.transform.position = startAtPosition + GroupsPosition.position[nbSummon];

                GenericSummonSpell genericSummonSpell = summon.GetComponent<GenericSummonSpell>();
                genericSummonSpell.SummonEntity(caster, this, summon);

                summon.SetActive(true);
                prefabsSummon.Add(genericSummonSpell);
            }
        }

        public override void AtTheEnd()
        {
            foreach (GenericSummonSpell summon in prefabsSummon)
            {
                summon.DestroySummon();
            }
        }

        public string pathObjectToInstantiate { get; set; }

        public float hp { get; set; }
        public bool isTargetable { get; set; }
        public List<Vector3> positionPresets { get; set; }
        
        public float attackDamage { get; set; }
        public float moveSpeed { get; set; }
        public int summonNumber { get; set; }
        public float attackSpeed { get; set; }
        public int nbUseSpells { get; set; }

        public BehaviorType BehaviorType { get; set; }
        public AttackBehaviorType AttackBehaviorType { get; set; }

        // Destroy other invocation of same type
        public bool isUnique { get; set; }
        public bool canMove { get; set; }

        public List<Spell> spells { get; set; }
        public Spell basicAttack { get; set; }

        /* useless for initialisation of spell */
        public List<GenericSummonSpell> prefabsSummon;
    }
}