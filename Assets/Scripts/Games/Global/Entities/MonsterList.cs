using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FullSerializer;
using UnityEngine;
using Utils;

namespace Games.Global.Entities
{
    public class MonsterList
    {
        private GameObject[] monsterGameObjects;

        public List<GroupsMonster> groupsList;
        public List<Monster> monsterList;
        
        public MonsterList(GameObject[] list, string json)
        {
            monsterGameObjects = list;

            groupsList = new List<GroupsMonster>();
            monsterList = new List<Monster>();

            InitMonsterList(json);
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

        private void InitMonsterList(string json)
        {
            fsSerializer serializer = new fsSerializer();
            fsData data;

            try
            {
                GroupsMonsterList mobsList = null;
                data = fsJsonParser.Parse(json);
                serializer.TryDeserialize(data, ref mobsList);

                if (mobsList == null)
                {
                    return;
                }

                foreach (GroupsJsonObject groupsJson in mobsList.groups)
                {
                    groupsList.Add(groupsJson.ConvertToMonsterGroups());
                    foreach (MobJsonObject mob in groupsJson.monsterList)
                    {
                        Monster monster = mob.ConvertToMonster();

                        if (monsterGameObjects != null)
                        {
                            monster.model = monsterGameObjects.First(model => model.name == monster.modelName);
                        }

                        if (!monsterList.Exists(monsterAdded => monsterAdded.id == monster.id))
                        {
                            monsterList.Add(monster);
                        }
                    }
                }

                DictionaryManager.hasMonstersLoad = true;
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
                Debug.Log(e.Data);
            }
        }

        // Method used by editor
        public void InitSpecificMonsterList(string monsterListJson)
        {
            fsSerializer serializer = new fsSerializer();
            fsData data;

            try
            {
                RawMonsterList mobsList = null;
                data = fsJsonParser.Parse(monsterListJson);
                serializer.TryDeserialize(data, ref mobsList);

                if (mobsList == null)
                {
                    return;
                }

                monsterList.Clear();

                foreach (MobJsonObject mobs in mobsList.monsters)
                {
                    Monster nMonster = mobs.ConvertToMonster();

                    if (monsterGameObjects != null)
                    {
                        nMonster.model = monsterGameObjects.First(model => model.name == nMonster.modelName);
                    }

                    monsterList.Add(nMonster);
                }
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
                Debug.Log(e.Data);
            }
        }
    }
}
