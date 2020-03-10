using System.Collections.Generic;
using UnityEngine;
using Utils;

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

        // first int : idMonster - Second int : numberMonster
        public Dictionary<int, int> monsterInGroups;

        private List<Monster> monsters;

        public void InitSpecificEquipment(Monster monster, List<int> equipment)
        {
            int nbWeaponFound = 0;
            
            foreach (int id in equipment)
            {
                // Is a weapon
                if (id < 1000)
                {
                    if (monster.InitWeapon(id))
                    {
                        nbWeaponFound++;   
                    }
                }
                // Is an armor
                else
                {
                    // TODO : init armor
                }
                
            }

            if (nbWeaponFound == 0)
            {
                monster.InitOriginalWeapon();
            }
        }
    }
}
