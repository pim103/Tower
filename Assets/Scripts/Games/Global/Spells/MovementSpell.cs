using System;
using UnityEngine;

namespace Games.Global.Spells
{
    public enum MovementSpellType
    {
        Dash,
        Charge,
        Tp,
        TpWithTarget
    }

    [Serializable]
    public class MovementSpell : SpellComponent
    {
        public MovementSpell()
        {
            typeSpell = TypeSpell.Movement;
        }

        public float duration { get; set; }
        public float speed { get; set; }
        public bool isFollowingMouse { get; set; }

        public MovementSpellType movementSpellType { get; set; }
        public Entity target { get; set; }

        public Vector3 tpPosition { get; set; }

        public SpellComponent linkedSpellAtTheStart { get; set; }
        public SpellComponent linkedSpellAtTheEnd { get; set; }
    }
}