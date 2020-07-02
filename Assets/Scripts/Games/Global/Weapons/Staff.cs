using System;
using Games.Players;

namespace Games.Global.Weapons
{
    [Serializable]
    public class Staff : Weapon
    {
        public Staff()
        {
           animationToPlay = "ShortSwordAttack";
        }
    }
}