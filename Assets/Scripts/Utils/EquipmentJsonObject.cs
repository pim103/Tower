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
            Weapon weapon = null;

            // If need specific attribute for weapon, init this after creation of weapon in the switch
            switch ((CategoryWeapon)Int32.Parse(category))
            {
                case CategoryWeapon.AXE:
                    weapon = new Axe();
                    break;
                case CategoryWeapon.BOW:
                    weapon = new Bow();
                    break;
                case CategoryWeapon.MACE:
                    weapon = new Mace();
                    break;
                case CategoryWeapon.RIFLE:
                    weapon = new Rifle();
                    break;
                case CategoryWeapon.SLING:
                    weapon = new Sling();
                    break;
                case CategoryWeapon.SPEAR:
                    weapon = new Spear();
                    break;
                case CategoryWeapon.STAFF:
                    weapon = new Staff();
                    break;
                case CategoryWeapon.DAGGER:
                    weapon = new Dagger();
                    break;
                case CategoryWeapon.HAMMER:
                    weapon = new Hammer();
                    break;
                case CategoryWeapon.HALBERD:
                    weapon = new Halberd();
                    break;
                case CategoryWeapon.HANDGUN:
                    weapon = new Handgun();
                    break;
                case CategoryWeapon.TRIDENT:
                    weapon = new Trident();
                    break;
                case CategoryWeapon.CROSSBOW:
                    weapon = new Crossbow();
                    break;
                case CategoryWeapon.LONG_SWORD:
                    weapon = new LongSword();
                    break;
                case CategoryWeapon.SHORT_SWORD:
                    weapon = new Sword();
                    break;
                case CategoryWeapon.TWO_HAND_AXE:
                    weapon = new TwoHandedAxe();
                    break;
            }

            weapon.id = Int32.Parse(id);
            weapon.damage = Int32.Parse(damage);
            weapon.category = (CategoryWeapon)Int32.Parse(category);
            weapon.type = (TypeWeapon)Int32.Parse(type);
            weapon.rarity = (Rarity)Int32.Parse(rarity);
            weapon.lootRate = Int32.Parse(lootRate);
            weapon.equipmentName = name;
            weapon.cost = Int32.Parse(cost);
            weapon.attSpeed = Int32.Parse(attSpeed);
            weapon.modelName = model;
            weapon.sprite = Resources.Load<Texture2D>(spritePath);
            weapon.equipmentType = (EquipmentType)Int32.Parse(equipmentType);

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
                armorCategory = (CategoryArmor)Int32.Parse(category),
                def = Int32.Parse(damage)
            };

            return armor;
        }
    }
}