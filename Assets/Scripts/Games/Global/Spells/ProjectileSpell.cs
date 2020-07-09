using System;
using System.Collections.Generic;
using UnityEngine;

namespace Games.Global.Spells
{
    [Serializable]
    public class ProjectileSpell : SpellComponent
    {
        public ProjectileSpell()
        {
            typeSpell = TypeSpell.Projectile;
        }

        public int idPoolObject { get; set; }

        public float speed { get; set; }

        public float damages { get; set; }
        public List<Effect> effectsOnHit { get; set; }

        public float duration { get; set; }

        public bool passingThroughEntity { get; set; }

        public float damageMultiplierOnDistance { get; set; }

        public SpellComponent linkedSpellOnEnable { get; set; }
        public SpellComponent linkedSpellOnDisable { get; set; }

        /* Useless for instantiation of spell */
        public GameObject objectPooled;
        public GameObject prefabPooled;
    }
}