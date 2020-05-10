using System.Collections.Generic;
using UnityEngine;

namespace Games.Global.Spells
{
    public enum Trajectory
    {
        OriginForward,
        AlreadySet
    }
    
    public class ProjectileSpell : SpellComponent
    {
        public ProjectileSpell()
        {
            typeSpell = TypeSpell.Projectile;
        }

        public int idPoolObject;

        public Trajectory trajectoryType;
        public Vector3 startPosition;
        public Vector3 initialRotation;
        public Vector3 trajectory;
        public float speed;
        
        public float damages;
        public List<Effect> effectsOnHit;

        public float duration;

        public bool passingThroughEntity;

        public float damageMultiplierOnDistance;

        public SpellComponent linkedSpellOnEnable;
        public SpellComponent linkedSpellOnDisable;

        /* Useless for instantiation of spell */
        public GameObject objectPooled;
        public GameObject prefabPooled;
    }
}
