using System;
using System.Collections.Generic;
using Games.Global;
using Games.Global.Armors;
using Games.Global.Weapons;
using UnityEngine;

namespace Utils
{
    [Serializable]
    public class EquipmentJsonList
    {
        public List<EquipmentJsonObject> equipment;
    }
    
    [Serializable]
    public class EquipmentJsonObject: ObjectParsed
    {
        public string id { get; set; }
        public string name { get; set; }
        public string category { get; set; }
        public string type { get; set; }

        public string rarity { get; set; }
        public string lootRate { get; set; }
        public string cost { get; set; }

//        private List<TypeEffect> effects;
        public string damage { get; set; }
        public string attSpeed { get; set; }

        public string onDamageDealt { get; set; }
        public string onDamageReceive { get; set; }
        public string model { get; set; }
        
        public string equipmentType { get; set; }
        public string spritePath { get; set; }

        public void PrintAttribute()
        {
            Debug.Log("Object id : " + id + " name : " + name + " type : " + category + " rarity : " + rarity);
            Debug.Log("Stats => dmg : " + damage + " att speed : " + attSpeed);
            Debug.Log("Ability => onDamageDealt : " + onDamageDealt + " onDamageReceive : " + onDamageReceive);
            Debug.Log("Model Name : " + model);
        }
        
        public override void InsertValue(string key, string value)
        {
            switch (key)
            {
                case "id":
                    id = value;
                    break;
                case "name":
                    name = value;
                    break;
                case "category":
                    category = value;
                    break;
                case "type":
                    type = value;
                    break;
                case "rarity":
                    rarity = value;
                    break;
                case "loot_rate":
                    lootRate = value;
                    break;
                case "cost":
                    cost = value;
                    break;
                case "damage":
                    damage = value;
                    break;
                case "att_speed":
                    attSpeed = value;
                    break;
                case "on_damage_dealt":
                    onDamageDealt = value;
                    break;
                case "on_damage_receive":
                    onDamageReceive = value;
                    break;
                case "model":
                    model = value;
                    break;
                case "equipmentType":
                    equipmentType = value;
                    break;
                case "spritePath":
                    spritePath = value;
                    break;
            }
        }

        public override void DoSomething()
        {
            throw new NotImplementedException();
        }

        public Weapon ConvertToWeapon()
        {
            Weapon weapon = new Weapon
            {
                id = Int32.Parse(id),
                damage = Int32.Parse(damage),
                category = DataObject.CategoryWeaponList?.GetCategoryFromId(category != null ? Int32.Parse(category) : -1),
                type = (TypeWeapon) Int32.Parse(type),
                rarity = (Rarity) Int32.Parse(rarity),
                lootRate = Int32.Parse(lootRate),
                equipmentName = name,
                cost = Int32.Parse(cost),
                attSpeed = Int32.Parse(attSpeed),
                modelName = model,
                sprite = Resources.Load<Texture2D>(spritePath),
                equipmentType = (EquipmentType) Int32.Parse(equipmentType)
            };

            return weapon;
        }

        public Armor ConvertToArmor()
        {
            Armor armor = new Armor
            {
                id = Int32.Parse(id),
                rarity = (Rarity) Int32.Parse(rarity),
                lootRate = Int32.Parse(lootRate),
                equipmentName = name,
                cost = Int32.Parse(cost),
                modelName = model,
                sprite = Resources.Load<Texture2D>(spritePath),
                equipmentType = (EquipmentType)Int32.Parse(equipmentType),
                armorCategory = category == null ? CategoryArmor.HELMET : (CategoryArmor)Int32.Parse(category),
                def = Int32.Parse(damage)
            };

            return armor;
        }
    }
}