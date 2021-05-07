using UnityEngine;

namespace Games.Global.Spells.SpellParameter
{
    // Class used for instantiation of spell
    public class SpellToInstantiate
    {
        public Geometry geometry { get; set; }
        public Vector3 scale { get; set; }
        public Vector3 offsetStartPosition { get; set; }
        public Vector3 offsetObjectToInstantiate { get; set; }

        public string pathGameObjectToInstantiate { get; set; }
        public Vector3 incrementAmplitudeByTime { get; set; }

        public bool passingThroughEntity { get; set; } = true;
    }
}