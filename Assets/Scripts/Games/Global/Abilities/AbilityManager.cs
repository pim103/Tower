using System;
using System.Collections.Generic;
using Games.Global.Abilities.WeaponsAbilities;
using Games.Global.Weapons;
using Scripts.Games.Global;

namespace Games.Global.Abilities
{
    public struct AbilityParameters
    {
        private List<Entity> origin;
        private List<Entity> target;
    }
    
    public class AbilityManager
    {
        private static Dictionary<string, Func<AbilityParameters, bool>> methodList;

        public static void InitAbilities()
        {
            // Add short sword abilities
            methodList.Add("ApplyFire", ShortSwordAbiltiy.ApplyFire);
            methodList.Add("KillHim", ShortSwordAbiltiy.KillHim);
            
            // Add spear abilities
            methodList.Add("PierceHim", SpearAbility.PierceHim);
            methodList.Add("Explode", SpearAbility.Explode);
        }
        
        public static Func<AbilityParameters, bool> GetAbility(string methodName)
        {
            return methodList[methodName];
        }
    }
}
