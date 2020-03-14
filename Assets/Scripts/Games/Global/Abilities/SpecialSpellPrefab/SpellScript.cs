using UnityEngine;

namespace Games.Global.Abilities.SpecialSpellPrefab
{
    public abstract class SpellScript : MonoBehaviour
    {
        public int initalDamage = 0;
        
        public abstract void PlaySpecialEffect(Entity origin, Entity target);
    }
}
