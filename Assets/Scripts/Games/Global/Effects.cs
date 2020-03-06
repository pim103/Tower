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

    public struct Effect
    {
        public TypeEffect typeEffect;
        public int level;
        public float durationInSeconds;
    }
}