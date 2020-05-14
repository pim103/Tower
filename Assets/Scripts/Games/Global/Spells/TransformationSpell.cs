using System.Collections.Generic;
using UnityEngine;

namespace Games.Global.Spells
{
    public class TransformationSpell : SpellComponent
    {
        public TransformationSpell()
        {
            typeSpell = TypeSpell.Transformation;
        }

        public GameObject prefab;
        public List<Spell> newSpells;
        public float duration;
    }
}
