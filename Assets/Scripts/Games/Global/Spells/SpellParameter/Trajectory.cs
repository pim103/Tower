using System.Collections.Generic;
using UnityEngine;

namespace Games.Global.Spells.SpellParameter
{
    public class Trajectory
    {
        public GameObject objectToFollow { get; set; }
        public List<Vector3> points;
    }
}