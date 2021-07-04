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

        public BezierPath spellPath { get; set; }
        public float speed { get; set; }
        public EndOfPathInstruction endOfPathInstruction { get; set; } = EndOfPathInstruction.Stop;

        public bool disapearAtTheEndOfTrajectory { get; set; }

        // Set in game
        public Transform objectToFollow;
        public Transform initialParent;
    }
}