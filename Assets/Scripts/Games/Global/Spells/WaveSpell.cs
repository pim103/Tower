using System.Collections.Generic;
using UnityEngine;

namespace Games.Global.Spells
{
    public class WaveSpell : SpellComponent
    {
        public Vector3 startPosition;
        public int number;
        public Geometry geometryPropagation;
        public float duration;
        public float damages;

        public List<Effect> effectsOnHit;
        public float incrementAmplitudeByTime;
        public float speedPropagation;
    }
}
