using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Games.Global
{
    public enum TypeEffect
    {
        PIERCE,
        AOE,
        BURN,
        POISON,
        BLEED,
        WEAK,
        STUN
    }

    public enum Target
    {
        SELF,
        TARGET
    }

    public class Effect : MonoBehaviour
    {
        public TypeEffect typeEffect;
        public Target target;
    }
}