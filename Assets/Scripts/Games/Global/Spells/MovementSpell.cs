﻿using UnityEngine;

namespace Games.Global.Spells
{
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

        public float damageOnHit;

        public Vector3 tpPosition;
        public Quaternion rotation;

        public Effect effectOnHit;

        public SpellComponent linkedSpellAtTheEnd;
    }
}