using System;
using System.Collections.Generic;
using Games.Global.Entities;
using Games.Global.Weapons;
using UnityEngine;

namespace Utils
{
    [Serializable]
    public class GroupsMonsterList
    {
        public List<GroupsJsonObject> groups;
    }

    [Serializable]
    public class RawMonsterList
    {
        public List<MobJsonObject> monsters;
    }

    [Serializable]
    public class SpellList
    {
        public string id;
        public string name;
    }

    [Serializable]
    public class MobJsonObject
    {
        public string id { get; set; } = "";

        public string name { get; set; }
        public string att { get; set; } = "0";
        public string def { get; set; } = "0";
        public string physicalDef { get; set; } = "0";
        public string magicalDef { get; set; } = "0";
        public string hp { get; set; } = "0";
        public string attSpeed { get; set; } = "0";
        public string speed { get; set; } = "0";
        public string nbWeapon { get; set; } = "0";
        public string weaponId { get; set; } = "0";

        public string typeWeapon { get; set; } = "0";

        public List<SpellList> skillListId;

        public string onDamageDealt { get; set; }
        public string onDamageReceive { get; set; }
        public string model { get; set; }

        public string nbMonster { get; set; }

        public void PrintAttribute()
        {
            Debug.Log("Object id : " + id + " name : " + name);
            Debug.Log("Stats => dmg : " + att + " speed : " + speed + " hp : " + hp + " def : " + def + " nbWeapon : " + nbWeapon);
            Debug.Log("Ability => onDamageDealt : " + onDamageDealt + " onDamageReceive : " + onDamageReceive);
            Debug.Log("Model Name : " + model);
            Debug.Log("skills : ");
            foreach (SpellList skill in skillListId)
            {
                Debug.Log("Cast : " + skill.name);
            }
        }

        public Monster ConvertToMonster()
        {
            Monster monster = new Monster
            {
                id = Int32.Parse(id),
                initialAtt = Int32.Parse(att),
                initialDef = Int32.Parse(def),
                initialPhysicalDef = Int32.Parse(physicalDef),
                initialMagicalDef = Int32.Parse(magicalDef),
                initialHp = Int32.Parse(hp),
                initialSpeed = Int32.Parse(speed),
                initialAttSpeed = Int32.Parse(attSpeed),
                attSpeed = Int32.Parse(attSpeed),
                mobName = name,
                att = Int32.Parse(att),
                def = Int32.Parse(def),
                hp = Int32.Parse(hp),
                speed = Int32.Parse(speed),
                nbWeapon = Int32.Parse(nbWeapon),
                weaponOriginalId = Int32.Parse(weaponId),
                constraint = (TypeWeapon) Int32.Parse(typeWeapon),
                spellsName = skillListId,
                modelName = model
            };

            return monster;
        }
    }
    
    [Serializable]
    public class GroupsJsonObject: ObjectParsed
    {
        public string id { get; set; }
        public string family { get; set; }
        public string cost { get; set; }
        public string radius { get; set; }
        public string groupName { get; set; }

        public List<MobJsonObject> monsterList { get; set; }

        public string spritePath { get; set; }

        public override void InsertValue(string key, string value)
        {
            switch (key)
            {
                case "id":
                    id = value;
                    break;
                case "family":
                    family = value;
                    break;
                case "cost":
                    cost = value;
                    break;
                case "monster":
                    monsterList = new List<MobJsonObject>();
                    break;
                case "radius":
                    radius = value;
                    break;
                case "groupName":
                    groupName = value;
                    break;
                case "spritePath":
                    spritePath = value;
                    break;
                default:
                    //mob.InsertValue(key, value);
                    break;
            }
        }

        public override void DoSomething()
        {/*
            if (Int32.Parse(number) != 0 || mob != null)
            {
                mobs.Add(mob, Int32.Parse(number));
                mob = null;
                number = "0";
            }*/
        }

        public void PrintAttribute()
        {
            Debug.Log("Object id : " + id);
            Debug.Log("Family : " + (Family)Int32.Parse(family));

            foreach (MobJsonObject mob in monsterList)
            {
                mob.PrintAttribute();
            }
        }
        
        public GroupsMonster ConvertToMonsterGroups()
        {
            GroupsMonster groupsMonster = new GroupsMonster
            {
                id = Int32.Parse(id),
                cost = Int32.Parse(cost),
                family = Int32.Parse(family),
                radius = Int32.Parse(radius),
                name = groupName,
                sprite = Resources.Load<Texture2D>(spritePath),
                monstersInGroupList = new List<MonstersInGroup>()
            };

            foreach (MobJsonObject mob in monsterList)
            {
                MonstersInGroup monstersInGroup = new MonstersInGroup
                {
                    nbMonster = Int32.Parse(mob.nbMonster)
                };
                monstersInGroup.SetMonster(mob.ConvertToMonster());
                
                groupsMonster.monstersInGroupList.Add(monstersInGroup);
//                groupsMonster.monsterInGroups.Add(Int32.Parse(mob.id), Int32.Parse(mob.nbMonster));
            }

            return groupsMonster;
        }
    }
}
