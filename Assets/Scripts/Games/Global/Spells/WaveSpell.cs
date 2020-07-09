using System;
using System.Collections.Generic;
using UnityEngine;

namespace Games.Global.Spells
{
    [Serializable]
    public class WaveSpell : SpellComponent
    {
        public WaveSpell()
        {
            typeSpell = TypeSpell.Wave;
        }

        public Geometry geometryPropagation { get; set; }
        public int initialWidth { get; set; }
        public float duration { get; set; }
        public float damages { get; set; }

        public List<Effect> effectsOnHit { get; set; }
        public float incrementAmplitudeByTime { get; set; }
        public float speedPropagation { get; set; }

        /* Useless for instantiation of spell */
        public GameObject objectPooled;
    }
}