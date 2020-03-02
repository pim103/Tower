using System.Collections.Generic;
using UnityEngine;

namespace Games.Global.Entities
{
    public enum Family
    {
        Demon,
        Human,
        Plant,
        Insect,
        Statue,
        Undead,
        Gobelin,
        Beast,
        Elementary,
        DivineCreature,
        Dwarf,
        Elf
    }
    
    public class GroupsMonster
    {
        public int id;
        public Family family;
        public int cost;

        public Dictionary<int, Monster> monsterInGroups;
    }
}
