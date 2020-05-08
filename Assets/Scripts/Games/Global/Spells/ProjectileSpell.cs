using System.Collections.Generic;
using UnityEngine;

namespace Games.Global.Spells
{
    public class ProjectileSpell : SpellComponent
    {
        public GameObject prefab;
        public Vector3 trajectory;
        public float damages;
        public List<Effect> effectsOnHit;

        public bool passingThroughEntity;

        public float damageMultiplierOnDistance;

        public SpellComponent linkedSpellOnEnable;
        public SpellComponent linkedSpellOnDisable;
    }
}
