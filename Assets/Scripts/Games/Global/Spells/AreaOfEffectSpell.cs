﻿using System.Collections.Generic;
using Games.Global.Spells.SpellParameter;
using UnityEngine;

namespace Games.Global.Spells
{
    public class AreaOfEffectSpell : SpellComponent
    {
        private void Start()
        {
            typeSpell = TypeSpell.AreaOfEffect;
        }
        
        public float interval;
        public float duration;
        public Vector3 startPosition;
        public Transform transformToFollow;

        public Geometry geometry;
        public Vector3 scale;

        public float damagesOnEnemiesOnInterval;
        public float damagesOnAlliesOnInterval;

        public List<Effect> effectsOnEnemiesOnInterval;
        public List<Effect> effectsOnPlayerOnInterval;
        public List<Effect> effectsOnAlliesOnInterval;

        public List<TypeEffect> deleteEffectsOnPlayerOnInterval;
        public List<TypeEffect> deleteEffectsOnEnemiesOnInterval;

        public bool canStopProjectile;
        public bool randomTargetHit;
        public bool randomPosition;
        public bool onePlay;

        /* Area applies player extraEffectOnDamageDeal */
        public bool appliesPlayerOnHitEffect;
        
        public SpellComponent linkedSpellOnInterval;

        public Effect effectOnHitOnStart;
        public SpellComponent linkedSpellOnEnd;

        public List<SpellWithCondition> spellWithConditions;
        
        /* Useless for initialisation of spell */
        public List<Entity> enemiesInZone;
        public List<Entity> alliesInZone;
        public GameObject objectPooled;
    }
}
