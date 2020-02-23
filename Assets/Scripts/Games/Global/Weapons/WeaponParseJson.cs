using System;
using System.Collections.Generic;
using System.IO;
using Games.Global.Weapons.Abilities;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace Games.Global.Weapons
{
    public class WeaponJsonObject
    {
        private TypeEquipement type;
        private string nameWeapon;
        private int id;
        
        private Rarity rarity;
        private int lootRate;
        private int cost;
        
        private List<TypeEffect> effects;
        private int damage;
        private int attSpeed;

        private string onDamageDealt;
        private string onDamageReceive;
        private string modelName;

        public void PrintAttribute()
        {
            Debug.Log("Object id : " + id + " name : " + nameWeapon + " type : " + type + " rarity : " + rarity);
            Debug.Log("Stats => dmg : " + damage + " att speed : " + attSpeed);
            Debug.Log("Ability => onDamageDealt : " + onDamageDealt + " onDamageReceive : " + onDamageReceive);
            Debug.Log("Model Name : " + modelName);
            Debug.Log("Effects : ");
            foreach (TypeEffect effect in effects)
            {
                Debug.Log("Apply : " + effect);
            }
        }
        
        public void InsertValue(string key, string value)
        {
            switch (key)
            {
                case "id":
                    id = Int32.Parse(value);
                    break;
                case "name":
                    nameWeapon = value;
                    break;
                case "type":
                    type = (TypeEquipement)Int32.Parse(value);
                    break;
                case "rarity":
                    rarity = (Rarity)Int32.Parse(value);
                    break;
                case "loot_rate":
                    lootRate = Int32.Parse(value);
                    break;
                case "cost":
                    cost = Int32.Parse(value);
                    break;
                case "effects":
                    if (effects == null)
                    {
                        effects = new List<TypeEffect>();
                    }
                    effects.Add((TypeEffect)Int32.Parse(value));
                    break;
                case "damage":
                    damage = Int32.Parse(value);
                    break;
                case "att_speed":
                    attSpeed = Int32.Parse(value);
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
            }
        }

        public Weapon ConvertToWeapon()
        {
            Weapon weapon = null;

            // If need specific attribute for weapon, init this after creation of weapon in the switch
            switch (type)
            {
                case TypeEquipement.AXE:
                    break;
                case TypeEquipement.BOW:
                    break;
                case TypeEquipement.MACE:
                    break;
                case TypeEquipement.RIFLE:
                    break;
                case TypeEquipement.SLING:
                    break;
                case TypeEquipement.SPEAR:
                    weapon = new Spear();
                    break;
                case TypeEquipement.STAFF:
                    break;
                case TypeEquipement.DAGGER:
                    break;
                case TypeEquipement.HAMMER:
                    break;
                case TypeEquipement.HALBERD:
                    break;
                case TypeEquipement.HANDGUN:
                    break;
                case TypeEquipement.TRIDENT:
                    break;
                case TypeEquipement.CROSSBOW:
                    break;
                case TypeEquipement.LONG_SWORD:
                    break;
                case TypeEquipement.SHORT_SWORD:
                    weapon = new Sword();
                    break;
                case TypeEquipement.TWO_HAND_AXE:
                    break;   
            }

            weapon.id = id;
            weapon.damage = damage;
            weapon.type = type;
            weapon.effects = effects;
            weapon.rarity = rarity;
            weapon.lootRate = lootRate;
            weapon.equipementName = nameWeapon;
            weapon.cost = cost;
            weapon.attSpeed = attSpeed;
            weapon.modelName = modelName;

            weapon.OnDamageDealt = AbilityManager.GetAbility(type, onDamageDealt);
            weapon.OnDamageReceive = AbilityManager.GetAbility(type, onDamageReceive);

            return weapon;
        }
    }
    
    public static class WeaponParseJson
    {
        public static List<WeaponJsonObject> ParseWeapon(StreamReader file)
        {
            List<WeaponJsonObject> jsonObjects = new List<WeaponJsonObject>();
            bool fileIsValid = false;

            string line = "";
            string key = "";
            string value = "";
            int indexComma;
            int indexColon;
            int indexEndOfArray;

            bool isInArray = false;
            
            WeaponJsonObject jsonObject = new WeaponJsonObject();

            while (!file.EndOfStream)
            {
                line = file.ReadLine();

                // Check if file contain header name "weapons"
                if (!fileIsValid && line != null && line.Contains("weapons"))
                {
                    fileIsValid = true;
                    continue;
                }
                else if (!fileIsValid || line == null)
                {
                    continue;
                }

                indexComma = line.IndexOf(",");
                if (indexComma == -1)
                {
                    indexComma = line.Length;
                }
                
                if (!isInArray)
                {
                    if (line.Contains("}"))
                    {
                        jsonObjects.Add(jsonObject);
                        jsonObject = new WeaponJsonObject();
                    }

                    // If condition if verify, array closed is the array with key = "weapons" => EndOfFile
                    if (line.Contains("]") && !isInArray)
                    {
                        break;
                    }
                    
                    //Get information from file
                    indexColon = line.IndexOf(":");
                
                    if (indexColon == -1)
                    {
                        continue;
                    }

                    key = line.Substring(0, indexColon).Replace('"', ' ').Trim();
                    value = line.Substring(indexColon + 1, indexComma - indexColon - 1).Replace('"', ' ').Trim();

                    if (value.Contains("["))
                    {
                        isInArray = true;
                        value = value.Replace("[", " ").Trim();
                    }
                }
                else
                {
                    indexEndOfArray = line.IndexOf("]");
                    if (indexEndOfArray == -1)
                    {
                        indexEndOfArray = indexComma;
                    }
                    else
                    {
                        isInArray = false;
                    }
                    
                    value = line.Substring(0, indexEndOfArray).Replace('"', ' ').Trim();
                }

                if (value == "")
                {
                    continue;
                }
                
                jsonObject.InsertValue(key, value);
            }

            return jsonObjects;
        }
    }
}
