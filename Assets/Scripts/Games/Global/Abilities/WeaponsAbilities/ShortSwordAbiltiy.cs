using UnityEngine;

namespace Games.Global.Abilities.WeaponsAbilities
{
    public class ShortSwordAbiltiy
    {
        public static bool ApplyFire(AbilityParameters param)
        {
//            param.directTarget.ApplyNewEffect(TypeEffect.Bleed, 5, 1);
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
