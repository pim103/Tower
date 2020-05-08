using UnityEngine;

namespace Games.Global.Spells
{
    public class TransformationSpell : SpellComponent
    {
        private void Start()
        {
            typeSpell = TypeSpell.Transformation;
        }

        public GameObject prefab;
        public SpellComponent spell1;
        public SpellComponent spell2;
        public SpellComponent spell3;

        public float duration;
    }
}
