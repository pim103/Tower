using System;
using System.Collections.Generic;
using Games.Global;
using Games.Global.Abilities;
using Games.Global.Weapons;
using UnityEngine;

namespace Utils
{
    public class WeaponJsonObject: ObjectParsed
    {
        private CategoryWeapon category;
        private TypeWeapon type;
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
            Debug.Log("Object id : " + id + " name : " + nameWeapon + " type : " + category + " rarity : " + rarity);
            Debug.Log("Stats => dmg : " + damage + " att speed : " + attSpeed);
            Debug.Log("Ability => onDamageDealt : " + onDamageDealt + " onDamageReceive : " + onDamageReceive);
            Debug.Log("Model Name : " + modelName);
            Debug.Log("Effects : ");
            foreach (TypeEffect effect in effects)
            {
                Debug.Log("Apply : " + effect);
            }
        }
        
        public override void InsertValue(string key, string value)
        {
            switch (key)
            {
                case "id":
                    id = Int32.Parse(value);
                    break;
                case "name":
                    nameWeapon = value;
                    break;
                case "category":
                    category = (CategoryWeapon)Int32.Parse(value);
                    break;
                case "type":
                    type = (TypeWeapon)Int32.Parse(value);
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

        public override void DoSomething()
        {
            throw new NotImplementedException();
        }

        public Weapon ConvertToWeapon()
        {
            Weapon weapon = null;

            // If need specific attribute for weapon, init this after creation of weapon in the switch
            switch (category)
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

            weapon.id = id;
            weapon.damage = damage;
            weapon.category = category;
            weapon.type = type;
            weapon.effects = effects;
            weapon.rarity = rarity;
            weapon.lootRate = lootRate;
            weapon.equipementName = nameWeapon;
            weapon.cost = cost;
            weapon.attSpeed = attSpeed;
            weapon.modelName = modelName;

            weapon.OnDamageDealt = AbilityManager.GetAbility(onDamageDealt, AbilityDico.WEAPON);
            weapon.OnDamageReceive = AbilityManager.GetAbility(onDamageReceive, AbilityDico.WEAPON);

            return weapon;
        }
    }
}