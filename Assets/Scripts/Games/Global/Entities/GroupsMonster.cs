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
        public const int DEFAULT_RADIUS = 1;

        public int id;
        public Family family;
        public int cost;
        public int radius = DEFAULT_RADIUS;

        // first int : idMonster - Second int : numberMonster
        public Dictionary<int, int> monsterInGroups;

        private List<Monster> monsters;

        public void InstantiateMonster(Vector3 position, List<int> equipement)
        {
            InstantiateParameters param;
            Monster monster;

            foreach (KeyValuePair<int, int> mobs in monsterInGroups)
            {
                for (int i = 0; i < mobs.Value; i++)
                {
                    monster = DataObject.MonsterList.GetMonsterById(mobs.Key);

                    param = new InstantiateParameters();
                    param.item = monster;
                    param.type = TypeItem.Monster;

                    monster.InstantiateModel(param, position);
                    monster.InitOriginalWeapon();

                    // TODO : algo pour placement des monstres
                    position.x += 1;
                }
            }

            // Todo : init equipment
        }
    }
}
