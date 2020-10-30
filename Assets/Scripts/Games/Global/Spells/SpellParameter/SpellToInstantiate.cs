using UnityEngine;

namespace Games.Global.Spells.SpellParameter
{
    // Class used for instantiation of spell
    public class SpellToInstantiate
    {
        public Geometry geometry { get; set; }
        public Vector3 scale { get; set; }
        public float speed;
        public Trajectory trajectory;

        // If not null, set objetToPool at children of SpellPrefabController
        public GameObject objectToPool { get; set; }

        // Specific to Wave
        public int initialWidth { get; set; }
        public float incrementAmplitudeByTime { get; set; }

        // Specific to Area
    }
}