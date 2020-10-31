using System;
using PathCreation;
using UnityEngine;

namespace Games.Global.Spells.SpellParameter
{
    public enum FollowCategory
    {
        NONE,
        FOLLOW_TARGET,
        FOLLOW_LAST_SPELL
    }
    
    [Serializable]
    public class Trajectory
    {
        public FollowCategory followCategory { get; set; }
        
        public Transform objectToFollow;
        public VertexPath spellPath { get; set; }
        public float speed { get; set; }
    }
}