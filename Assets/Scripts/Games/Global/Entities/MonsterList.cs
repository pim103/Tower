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

        public GroupsMonster CloneGroups(GroupsMonster orig)
        {
            GroupsMonster groups = new GroupsMonster();
            groups.family = orig.family;
            groups.cost = orig.cost;
            groups.id = orig.id;
            groups.radius = orig.radius;
            groups.name = orig.name;
            groups.monsterInGroups = orig.monsterInGroups;

            return groups;
        }

        public Monster CloneMonster(Monster orig)
        {
            Monster clone = new Monster();

            clone.family = orig.family;
            clone.id = orig.id;
            clone.skills = orig.skills;
            clone.mobName = orig.mobName;
            clone.nbWeapon = orig.nbWeapon;
            clone.OnDamageDealt = orig.OnDamageDealt;
            clone.OnDamageReceive = orig.OnDamageReceive;
            clone.att = orig.att;
            clone.def = orig.def;
            clone.hp = orig.hp;
            clone.speed = orig.speed;
            clone.initialAtt = orig.att;
            clone.initialDef = orig.def;
            clone.initialHp = orig.hp;
            clone.initialSpeed = orig.speed;
            clone.modelName = orig.modelName;
            clone.model = orig.model;
            clone.weaponOriginalName = orig.weaponOriginalName;
            clone.constraint = orig.constraint;

            clone.typeEntity = TypeEntity.MOB;

            clone.InitEquipementArray(orig.nbWeapon);
            
            return clone;
        }

        public GroupsMonster GetGroupsMonsterById(int id)
        {
            return CloneGroups(groupsList.First(group => group.id == id));
        }

        public Monster GetMonsterById(int id)
        {
            return CloneMonster(monsterList.First(monster => monster.id == id));
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
