using System.Collections.Generic;

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
        public const int DEFAULT_RADIUS = 1;
        
        public int id;
        public Family family;
        public int cost;
        public int radius = DEFAULT_RADIUS;

        public Dictionary<int, Monster> monsterInGroups;
    }
}
