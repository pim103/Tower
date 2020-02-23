using System;
using Photon.Pun.UtilityScripts;
using UnityEngine;

namespace Games.Global.Weapons.Abilities
{
    public static class AbilityManager
    {
        private static SpearAbility spearAbility;
        private static ShortSwordAbiltiy shortSwordAbility;
        
        public static void InitAbilities()
        {
            spearAbility = new SpearAbility();
            spearAbility.InitAbility();
            shortSwordAbility = new ShortSwordAbiltiy();
            shortSwordAbility.InitAbility();
        }
        
        public static Func<bool> GetAbility(TypeEquipement type, string methodName)
        {
            switch (type)
            {
                case TypeEquipement.SPEAR :
                    return spearAbility.methodList[methodName];
                case TypeEquipement.SHORT_SWORD:
                    return shortSwordAbility.methodList[methodName];
            }

            return null;
        }
    }
}
