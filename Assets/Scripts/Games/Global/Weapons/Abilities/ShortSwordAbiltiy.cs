using System;
using System.Collections.Generic;
using UnityEngine;

namespace Games.Global.Weapons.Abilities
{
    public class ShortSwordAbiltiy
    {
        public Dictionary<string, Func<bool>> methodList;
        
        public void InitAbility()
        {
            methodList = new Dictionary<string, Func<bool>>();
            methodList.Add("ApplyFire", ApplyFire);
            methodList.Add("KillHim", KillHim);
        }
        
        public static bool ApplyFire()
        {
            Debug.Log("Burn !");
            return false;
        }

        public static bool KillHim()
        {
            Debug.Log("I just want to kill him");
            return false;
        }
    }
}
