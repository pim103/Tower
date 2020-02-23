using System;
using System.Collections.Generic;
using UnityEngine;

namespace Games.Global.Weapons.Abilities
{
    public class SpearAbility
    {
        public Dictionary<string, Func<bool>> methodList;
        
        public void InitAbility()
        {
            methodList = new Dictionary<string, Func<bool>>();
            methodList.Add("PierceHim", PierceHim);
            methodList.Add("Explode", Explode);
        }
        
        public static bool PierceHim()
        {
            Debug.Log("Un petit trou");
            return false;
        }

        public static bool Explode()
        {
            Debug.Log("Explosion");
            return false;
        }
    }
}
