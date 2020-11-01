using UnityEngine;

namespace Games.Global.Spells.SpellParameter
{
    // Class used for instantiation of spell
    public class SpellToInstantiate
    {
        public Geometry geometry { get; set; }
        public Vector3 scale { get; set; }
        public float height { get; set; }

        // If not null, set objetToPool at children of SpellPrefabController
        public int idPoolObject { get; set; } = -1;

        // Specific to Wave
        public Vector3 incrementAmplitudeByTime { get; set; }

        public bool passingThroughEntity { get; set; } = true;
    }
}