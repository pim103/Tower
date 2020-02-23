using Scripts.Games.Global;
using UnityEngine;

namespace Games.Global
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

    public static class Effect
    {
        public static void TriggerEffect(TypeEffect type, Entity target)
        {
            switch (type)
            {
                case TypeEffect.AOE:
                    break;
                case TypeEffect.BURN:
                    break;
                case TypeEffect.STUN:
                    break;
                case TypeEffect.WEAK:
                    break;
                case TypeEffect.BLEED:
                    break;
                case TypeEffect.PIERCE:
                    break;
                case TypeEffect.POISON:
                    break;
            }
        }
    }
}