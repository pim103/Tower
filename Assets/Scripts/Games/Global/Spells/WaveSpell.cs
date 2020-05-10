using System.Collections.Generic;
using UnityEngine;

namespace Games.Global.Spells
{
    public class WaveSpell : SpellComponent
    {
        public WaveSpell()
        {
            typeSpell = TypeSpell.Wave;
        }

        public Vector3 startPosition;
        public Vector3 initialRotation;

        public Geometry geometryPropagation;
        public int initialWidth;
        public float duration;
        public float damages;

        public List<Effect> effectsOnHit;
        public float incrementAmplitudeByTime;
        public float speedPropagation;
        
        /* Useless for instantiation of spell */
        public GameObject objectPooled;
    }
}
