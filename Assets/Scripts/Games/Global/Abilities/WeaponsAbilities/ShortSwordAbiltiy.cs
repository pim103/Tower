using UnityEngine;

namespace Games.Global.Abilities.WeaponsAbilities
{
    public class ShortSwordAbiltiy
    {
        public static bool ApplyFire(AbilityParameters param)
        {
            Debug.Log("Burn !");
            return false;
        }

        public static bool KillHim(AbilityParameters param)
        {
            Debug.Log("I just want to kill him");
            return false;
        }
    }
}
