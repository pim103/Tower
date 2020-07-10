﻿using System.Collections.Generic;
using Games.Global.Weapons;
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

        public int id { get; set; }
        public Family family { get; set; }
        public int cost { get; set; }
        public string name { get; set; }
        public int radius { get; set; } = DEFAULT_RADIUS;

        // first int : idMonster - Second int : numberMonster
        public Dictionary<int, int> monsterInGroups { get; set; }

        private List<Monster> monsters { get; set; }

        public void InitSpecificEquipment(Monster monster, List<int> equipment)
        {
            int nbWeaponFound = 0;

            if (equipment != null)
            {
                if (monster.constraint == TypeWeapon.Cac)
                {
                    if (equipment[0] != 0)
                    {
                        if (monster.InitWeapon(equipment[0]))
                        {
                            nbWeaponFound++;
                        }
                    }
                }
                else if (monster.constraint == TypeWeapon.Distance)
                {
                    if (equipment[1] != 0)
                    {
                        if (monster.InitWeapon(equipment[1]))
                        {
                            nbWeaponFound++;
                        }
                    }
                
                    // TODO init armor with : equipment[2] / equipment[3] / equipment[4]
                }
            }

            if (nbWeaponFound == 0)
            {
                monster.InitOriginalWeapon();
            }
        }
    }
}
