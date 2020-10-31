using System;
using System.Collections.Generic;
using Games.Global.Spells.SpellParameter;
using UnityEngine;

namespace Games.Global.Spells
{
    [Serializable]
    public class AreaOfEffectSpell : SpellComponent
    {
        public AreaOfEffectSpell()
        {
            typeSpell = TypeSpell.AreaOfEffect;
        }

        public float interval { get; set; }
        public float duration { get; set; }

        public Geometry geometry { get; set; }
        public Vector3 scale { get; set; }

        public float damagesOnEnemiesOnInterval { get; set; }
        public float damagesOnAlliesOnInterval { get; set; }

        public List<Effect> effectsOnEnemiesOnInterval { get; set; }
        public List<Effect> effectsOnPlayerOnInterval { get; set; }
        public List<Effect> effectsOnAlliesOnInterval { get; set; }

        public List<TypeEffect> deleteEffectsOnPlayerOnInterval { get; set; }
        public List<TypeEffect> deleteEffectsOnEnemiesOnInterval { get; set; }

        public bool wantToFollow { get; set; }
        public bool canStopProjectile { get; set; }
        public bool randomTargetHit { get; set; }
        public bool randomPosition { get; set; }
        public bool onePlay { get; set; }

        /* Area applies player extraEffectOnDamageDeal */
        public bool appliesPlayerOnHitEffect { get; set; }

        public SpellComponent linkedSpellOnInterval { get; set; }

        public Effect effectOnHitOnStart { get; set; }
        public SpellComponent linkedSpellOnEnd { get; set; }

        public List<SpellWithCondition> spellWithConditions { get; set; }

        /* Useless for initialisation of spell */
        public List<Entity> enemiesInZone { get; set; }
        public List<Entity> alliesInZone { get; set; }
        public GameObject objectPooled;
    }
}