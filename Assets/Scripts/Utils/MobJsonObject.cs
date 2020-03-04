using System;
using System.Collections.Generic;
using Games.Global;
using Games.Global.Abilities;
using Games.Global.Entities;
using UnityEngine;

namespace Utils
{
    public class MobJsonObject
    {
        private int id;
        
        private string mobName;
        private int att;
        private int def;
        private int hp;
        private int speed;
        private int nbWeapon;

        private List<string> skills;
        private string onDamageDealt;
        private string onDamageReceive;
        private string modelName;

        public void InsertValue(string key, string value)
        {
            switch (key)
            {
                case "idMonster":
                    id = Int32.Parse(value);
                    break;
                case "name":
                    mobName = value;
                    break;
                case "att":
                    att = Int32.Parse(value);
                    break;
                case "def":
                    def = Int32.Parse(value);
                    break;
                case "hp":
                    hp = Int32.Parse(value);
                    break;
                case "speed":
                    speed = Int32.Parse(value);
                    break;
                case "nbWeapon":
                    nbWeapon = Int32.Parse(value);
                    break;
                case "on_damage_dealt":
                    onDamageDealt = value;
                    break;
                case "on_damage_receive":
                    onDamageReceive = value;
                    break;
                case "model":
                    modelName = value;
                    break;
                case "skills":
                    if (skills == null)
                    {
                        skills = new List<string>();
                    }

                    skills.Add(value);
                    break;
            }
        }
        
        public void PrintAttribute()
        {
            Debug.Log("Object id : " + id + " name : " + mobName);
            Debug.Log("Stats => dmg : " + att + " speed : " + speed + " hp : " + hp + " def : " + def + " nbWeapon : " + nbWeapon);
            Debug.Log("Ability => onDamageDealt : " + onDamageDealt + " onDamageReceive : " + onDamageReceive);
            Debug.Log("Model Name : " + modelName);
            Debug.Log("skills : ");
            foreach (string skill in skills)
            {
                Debug.Log("Apply : " + skill);
            }
        }
        
        // Don't forget to clone fucking dumbass
        public Monster ConvertToMonster(Family family)
        {
            Monster monster = new Monster();
            monster.id = id;

            monster.initialAtt = att;
            monster.initialDef = def;
            monster.initialHp = hp;
            monster.initialSpeed = speed;
            
            monster.mobName = mobName;
            monster.att = att;
            monster.def = def;
            monster.hp = hp;
            monster.speed = speed;
            monster.nbWeapon = nbWeapon;
            monster.family = family;

            monster.OnDamageDealt = AbilityManager.GetAbility(onDamageDealt, AbilityDico.MOB);
            monster.OnDamageReceive = AbilityManager.GetAbility(onDamageReceive, AbilityDico.MOB);;
            monster.modelName = modelName;

            monster.skills = new List<Skill>();
            foreach (string skill in skills)
            {
                Skill newSkill = new Skill();
                newSkill.skill = AbilityManager.GetAbility(skill, AbilityDico.MOB);
                monster.skills.Add(newSkill);
            }

            return monster;
        }
    }
    
    public class GroupsJsonObject: ObjectParsed
    {
        private int id;
        private Dictionary<int, MobJsonObject> mobs = new Dictionary<int, MobJsonObject>();
        private Family family;
        private int cost;
        
        private MobJsonObject mob;
        private int number = 0;
        
        public override void InsertValue(string key, string value)
        {
            switch (key)
            {
                case "id":
                    id = Int32.Parse(value);
                    break;
                case "family":
                    family = (Family)Int32.Parse(value);
                    break;
                case "cost":
                    cost = Int32.Parse(value);
                    break;
                case "number":
                    number = Int32.Parse(value);
                    break;
                case "monster":
                    mob = new MobJsonObject();
                    break;
                default:
                    mob.InsertValue(key, value);
                    break;
            }
        }

        public override void DoSomething()
        {
            if (number != 0 || mob != null)
            {
                mobs.Add(number, mob);
                mob = null;
                number = 0;
            }
        }

        public void PrintAttribute()
        {
            Debug.Log("Object id : " + id);
            Debug.Log("Family : " + family);

            foreach (KeyValuePair<int, MobJsonObject> entry in mobs)
            {
                Debug.Log("Nombre de monstre : " + entry.Key);
                entry.Value.PrintAttribute();
            }
        }
        
        public GroupsMonster ConvertToMonsterGroups()
        {
            
            GroupsMonster groupsMonster = new GroupsMonster();
            groupsMonster.id = id;
            groupsMonster.cost = cost;
            groupsMonster.family = family;
            groupsMonster.monsterInGroups = new Dictionary<int, Monster>();

            foreach (KeyValuePair<int, MobJsonObject> mob in mobs)
            {
                groupsMonster.monsterInGroups.Add(mob.Key, mob.Value.ConvertToMonster(family));
            }

            return groupsMonster;
        }
    }
}
