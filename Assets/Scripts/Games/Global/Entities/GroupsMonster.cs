using System;
using System.Collections.Generic;
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

    [Serializable]
    public class MonstersInGroup
    {
        public Monster monster { get; set; }
        public int nbMonster { get; set; }

        public void SetMonster(Monster nMonster)
        {
            monster = nMonster;
        }

        public Monster GetMonster()
        {
            return Tools.Clone(monster);
        }

        public int GetMonsterId()
        {
            int id = -1;

            if (monster != null)
            {
                id = monster.id;
            }

            return id;
        }
    }

    [Serializable]
    public class GroupsMonster
    {
        public const int DEFAULT_RADIUS = 1;

        public int id { get; set; }
        public int family { get; set; }
        public int cost { get; set; }
        public string name { get; set; }
        public int radius { get; set; } = DEFAULT_RADIUS;
        public bool hasKey { get; set; }

        public List<MonstersInGroup> monstersInGroupList { get; set; } = new List<MonstersInGroup>();
        public Texture2D sprite { get; set; }

        public void AddEquipmentId(int meleeWeaponId, int rangeWeaponId)
        {
            foreach (MonstersInGroup monstersInGroup in monstersInGroupList)
            {
                Monster monster = monstersInGroup.GetMonster();

                if (monster.GetConstraint() == TypeWeapon.Cac && meleeWeaponId > 0)
                {
                    monster.weaponOriginalId = meleeWeaponId;
                    monstersInGroup.SetMonster(monster);
                } 
                else if (monster.GetConstraint() == TypeWeapon.Distance && rangeWeaponId > 0)
                {
                    monster.weaponOriginalId = rangeWeaponId;
                    monstersInGroup.SetMonster(monster);
                }
            }
        }
    }
}
