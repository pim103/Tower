using UnityEngine;

namespace Games.Global.Abilities.WeaponsAbilities
{
    public class SpearAbility
    {
        public static bool PierceHim(AbilityParameters param)
        {
            Debug.Log("Un petit trou");
            return false;
        }

        public static bool Explode(AbilityParameters param)
        {
            Debug.Log("Explosion");
            return false;
        }
    }
}
