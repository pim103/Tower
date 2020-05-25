using System;
using System.Collections.Generic;
using UnityEngine;

namespace Games.Global.Spells
{
    [Serializable]
    public class TransformationSpell : SpellComponent
    {
        public TransformationSpell()
        {
            typeSpell = TypeSpell.Transformation;
        }

        public int idPoolPrefab { get; set; }
        public List<Spell> newSpells { get; set; }
        public float duration { get; set; }
    }
}