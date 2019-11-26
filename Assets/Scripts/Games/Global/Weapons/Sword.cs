using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

using PA_INST = Scripts.Games.Global.PatternInstructions;

namespace Scripts.Games.Global.Weapons
{
    public class Sword : Weapon
    {
        //private Dictionary<PA_INST, int> pattern = new Dictionary<PA_INST, int>();

        private void Start()
        {
            pattern = new Pattern[2];

            pattern[0] = new Pattern(PA_INST.ROTATE_DOWN, 90, 0.1f, 0.01f);
            pattern[1] = new Pattern(PA_INST.ROTATE_UP, 90, 0.1f, 0.01f);
        }

        public override void BasicAttack()
        {

        }
    }
}
