using UnityEngine;

namespace Games.Global.Abilities.SpecialSpellPrefab
{
    public abstract class SpellScript : MonoBehaviour
    {
        public abstract void PlaySpecialEffect(Entity origin, Entity target);
    }
}
