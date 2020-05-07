using System;
using System.Diagnostics;
//using Games.Global.Patterns;
using Games.Players;
//using PA_INST = Games.Global.Patterns.PatternInstructions;

namespace Games.Global.Weapons
{
    [Serializable]
    public class Sword : Weapon
    {
        
        
        public Sword()
        {
            animationToPlay = "ShortSwordAttack";
        }

        public override void InitPlayerSkill(Classes classe)
        {
            
        }
    }
}