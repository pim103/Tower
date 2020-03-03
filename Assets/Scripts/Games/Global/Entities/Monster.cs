using System;
using System.Collections.Generic;
using Games.Global.Abilities;

namespace Games.Global.Entities
{
    public class Monster: Entity
    {
        public int id;
        public string mobName;
        public int nbWeapon;

        public Family family;
        
        public List<Skill> skills;
        
        public Func<AbilityParameters, bool> OnDamageReceive;

        public Func<AbilityParameters, bool> OnDamageDealt;
    }
}
