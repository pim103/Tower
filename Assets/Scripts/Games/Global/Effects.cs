﻿using UnityEngine;

namespace Games.Global
{
    public enum TypeEffect
    {
        Pierce,
        Aoe,
        Burn,
        Poison,
        Bleed,
        Weak,
        Stun,
        Sleep,
        Regen,
        Invisibility,

        MadeADash,
    }

    public struct Effect
    {
        public TypeEffect typeEffect;
        public int level;
        public float durationInSeconds;

        public Entity launcher;
        public float ressourceCost;

        public void UpdateEffect(Effect effect)
        {
            switch (typeEffect)
            {
                case TypeEffect.Burn:
                    durationInSeconds += effect.durationInSeconds;

                    if (durationInSeconds > 20)
                    {
                        durationInSeconds = 20;
                    }
                    break;
                case TypeEffect.Bleed:
                    if (durationInSeconds < effect.durationInSeconds)
                    {
                        durationInSeconds = effect.durationInSeconds;
                    }

                    if (level < 5)
                    {
                        level += effect.level;
                    }
                    break;
                case TypeEffect.Poison:
                    durationInSeconds += effect.durationInSeconds;

                    if (durationInSeconds > 20)
                    {
                        durationInSeconds = 20;
                    }
                    break;
                case TypeEffect.Regen:
                    if (durationInSeconds < effect.durationInSeconds)
                    {
                        durationInSeconds = effect.durationInSeconds;
                    }
                    break;
            }
        }
    }
}