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
        private static int idMobInit = 0;

        public int id;
        public Family family;
        public int cost;
        public int radius = DEFAULT_RADIUS;

        // first int : idMonster - Second int : numberMonster
        public Dictionary<int, int> monsterInGroups;

        private List<Monster> monsters;

        private void InitSpecificEquipment(Monster monster, List<int> equipment)
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

        public void InstantiateMonster(Vector3 position, List<int> equipment)
        {
            InstantiateParameters param;
            Monster monster;
            int nbMonsterInit = 0;

            Vector3 origPos = position;

            foreach (KeyValuePair<int, int> mobs in monsterInGroups)
            {
                for (int i = 0; i < mobs.Value; i++)
                {
                    monster = DataObject.MonsterList.GetMonsterById(mobs.Key);

                    param = new InstantiateParameters();
                    param.item = monster;
                    param.type = TypeItem.Monster;

                    monster.InstantiateModel(param, position);
                    monster.idInitialisation = idMobInit;
                    idMobInit++;
                    nbMonsterInit++;

                    InitSpecificEquipment(monster, equipment);

                    position = origPos + GroupsPosition.position[nbMonsterInit];
                    
                    DataObject.monsterInScene.Add(monster);
                }
            }
        }
    }
}
