using UnityEngine;

namespace Games.Global.Abilities.MobAbilities
{
    public static class DemonSkill
    {
        public static bool ThrowPoison(AbilityParameters param)
        {
            Debug.Log("On lance du poison");
            return false;
        }

        public static bool EjectTarget(AbilityParameters param)
        {
            Debug.Log("Eject the target");
            return false;
        }
        
        public static bool Outch(AbilityParameters param)
        {
            Debug.Log("Outch");
            return false;
        }
        
        public static bool Aie(AbilityParameters param)
        {
            Debug.Log("Aie");
            return false;
        }
    }
}