using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using Utils;

namespace Games.Global.Entities
{
    public class MonsterList
    {
        private GameObject[] monsterGameObjects;

        public List<GroupsMonster> groupsList;
        public List<Monster> monsterList;
        
        public MonsterList(GameObject[] list)
        {
            monsterGameObjects = list;

            groupsList = new List<GroupsMonster>();
            monsterList = new List<Monster>();

            InitMonsterList();
        }

        public GroupsMonster GetGroupsMonsterById(int id)
        {
            return Tools.Clone(groupsList.First(group => group.id == id));
        }

        public Monster GetMonsterById(int id)
        {
            Monster cloneMonster = Tools.Clone(monsterList.First(monster => monster.id == id));
            cloneMonster.InitEntityList();
            cloneMonster.InitSpells();
            cloneMonster.typeEntity = TypeEntity.MOB;
            return cloneMonster;
        }

        private void InitMonsterList()
        {
            List<GroupsJsonObject> wJsonObjects = new List<GroupsJsonObject>();

            foreach (string filePath in Directory.EnumerateFiles("Assets/Data/MonsterJson"))
            {
                StreamReader reader = new StreamReader(filePath, true);
            
                wJsonObjects.AddRange(ParserJson<GroupsJsonObject>.Parse(reader, "groups"));
            }

            foreach (GroupsJsonObject groupsJson in wJsonObjects)
            {
                groupsList.Add(groupsJson.ConvertToMonsterGroups());
                foreach (KeyValuePair<MobJsonObject, int> mob in groupsJson.mobs)
                {
                    Monster monster = mob.Key.ConvertToMonster(groupsJson.family);
                    monster.model = monsterGameObjects.First(model => model.name == monster.modelName);
                    monsterList.Add(monster);
                }
            }
        }
    }
}
