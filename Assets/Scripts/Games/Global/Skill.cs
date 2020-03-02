using System;
using Games.Global.Abilities;

namespace Games.Global
{
    public class Skill
    {
        public int cost;
        public float startCooldownTimer;
        public float castTime;
        public int cooldown;
        
        public Func<AbilityParameters, bool> skill;
    }
}