using System;
using System.Collections.Generic;
using Games.Global;
using Games.Global.Abilities;
using Games.Global.Entities;
using Games.Global.Weapons;
using UnityEngine;

namespace Utils
{
    public class MobJsonObject
    {
        public int id;
        
        private string mobName;
        private int att;
        private int def;
        private int physicalDef;
        private int magicalDef;
        private int hp;
        private float attSpeed;
        private float speed;
        private int nbWeapon;
        private string weapon;

        private TypeWeapon constraint;

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
                case "magicalDef":
                    magicalDef = Int32.Parse(value);
                    break;
                case "physicalDef":
                    physicalDef = Int32.Parse(value);
                    break;
                case "hp":
                    hp = Int32.Parse(value);
                    break;
                case "att_speed":
                    attSpeed = Int32.Parse(value);
                    break;
                case "speed":
                    speed = Int32.Parse(value);
                    break;
                case "nbWeapon":
                    nbWeapon = Int32.Parse(value);
                    break;
                case "weapon":
                    weapon = value;
                    break;
                case "constraint":
                    constraint = (TypeWeapon)Int32.Parse(value);
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
        
        // Don't forget to clone
        public Monster ConvertToMonster(Family family)
        {
            Monster monster = new Monster();
            monster.id = id;

            monster.initialAtt = att;
            monster.initialDef = def;
            monster.initialPhysicalDef = physicalDef;
            monster.initialMagicalDef = magicalDef;
            monster.initialHp = hp;
            monster.initialSpeed = speed;
            monster.initialAttSpeed = attSpeed;
            monster.mobName = mobName;
            monster.att = att;
            monster.def = def;
            monster.hp = hp;
            monster.speed = speed;
            monster.nbWeapon = nbWeapon;
            monster.family = family;
            monster.weaponOriginalName = weapon;
            monster.constraint = constraint;

            monster.OnDamageDealt = AbilityManager.GetAbility(onDamageDealt, AbilityDico.MOB);
            monster.OnDamageReceive = AbilityManager.GetAbility(onDamageReceive, AbilityDico.MOB);;
            monster.modelName = modelName;

            // TODO : Implement spell with new system
//            foreach (string skill in skills)
//            {
//                Spell newSpell = new Spell();
//                newSpell.skill = AbilityManager.GetAbility(skill, AbilityDico.MOB);
//                monster.skills.Add(newSpell);
//            }

            return monster;
        }
    }
    
    public class GroupsJsonObject: ObjectParsed
    {
        public int id;
        public Dictionary<MobJsonObject, int> mobs = new Dictionary<MobJsonObject, int>();
        public Family family;
        private int cost;
        private int radius = GroupsMonster.DEFAULT_RADIUS;
        private string name;

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
                case "radius":
                    radius = Int32.Parse(value);
                    break;
                case "groupName":
                    name = value;
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
                mobs.Add(mob, number);
                mob = null;
                number = 0;
            }
        }

        public void PrintAttribute()
        {
            Debug.Log("Object id : " + id);
            Debug.Log("Family : " + family);

            foreach (KeyValuePair<MobJsonObject, int> entry in mobs)
            {
                Debug.Log("Nombre de monstre : " + entry.Value);
                entry.Key.PrintAttribute();
            }
        }
        
        public GroupsMonster ConvertToMonsterGroups()
        {
            
            GroupsMonster groupsMonster = new GroupsMonster();
            groupsMonster.id = id;
            groupsMonster.cost = cost;
            groupsMonster.family = family;
            groupsMonster.radius = radius;
            groupsMonster.name = name;
            groupsMonster.monsterInGroups = new Dictionary<int, int>();

            foreach (KeyValuePair<MobJsonObject, int> mob in mobs)
            {
                groupsMonster.monsterInGroups.Add(mob.Key.id, mob.Value);
            }

            return groupsMonster;
        }
    }
}
