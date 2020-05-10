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
    
    public class MovementSpell : SpellComponent
    {
        public MovementSpell()
        {
            typeSpell = TypeSpell.Movement;
        }

        public float duration;
        public float speed;
        public Vector3 trajectory;
        public bool isFollowingMouse;

        public MovementSpellType movementSpellType;
        public Entity target;

        public Vector3 tpPosition;

        public SpellComponent linkedSpellAtTheStart;
        public SpellComponent linkedSpellAtTheEnd;
    }
}