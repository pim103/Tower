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
        public List<GroupsMonster> groupsList;
        public List<Monster> monsterList;

        public MonsterList(string json)
        {
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
            cloneMonster.SetTypeEntity(TypeEntity.MOB);
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

                        GameObject monsterModel = Resources.Load(monster.modelName) as GameObject;
                        if (monsterModel)
                        {
                            monster.model = monsterModel;
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

                    GameObject monsterModel = Resources.Load(nMonster.modelName) as GameObject;
                    if (monsterModel)
                    {
                        nMonster.model = monsterModel;
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
