using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Games.Global.Abilities;
using UnityEngine;
using Utils;

namespace Games.Global.Weapons
{
    public class WeaponList
    {
        private GameObject[] weaponsGameObject;

        public List<Weapon> weapons;

        public WeaponList(GameObject[] list)
        {
            weaponsGameObject = list;

            weapons = new List<Weapon>();
            InitWeaponDictionnary();
        }

        private Weapon CloneWeapon(Weapon orig)
        {
            Weapon weapon = null;

            // If need specific attribute for weapon, init this after creation of weapon in the switch
            switch (orig.type)
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

            weapon.id = orig.id;
            weapon.damage = orig.damage;
            weapon.type = orig.type;
            weapon.effects = orig.effects;
            weapon.rarity = orig.rarity;
            weapon.lootRate = orig.lootRate;
            weapon.equipementName = orig.equipementName;
            weapon.cost = orig.cost;
            weapon.attSpeed = orig.attSpeed;
            weapon.modelName = orig.modelName;
            weapon.model = orig.model;

            weapon.OnDamageDealt = orig.OnDamageDealt;
            weapon.OnDamageReceive = orig.OnDamageReceive;

            return weapon;
        }

        public Weapon GetWeaponWithName(string findName)
        {
            return CloneWeapon(weapons.First(we => we.equipementName == findName));
        }

        public Weapon GetWeaponWithId(int id)
        {
            return CloneWeapon(weapons.First(we => we.id == id));
        }

        public void PrintDictionnary()
        {
            foreach (Weapon weapon in weapons)
            {
                Debug.Log(weapon);
                Debug.Log(weapon.type);
                Debug.Log(weapon.damage);
            }
        }

        private void InitWeaponDictionnary()
        {
            List<WeaponJsonObject> wJsonObjects = new List<WeaponJsonObject>();

            foreach (string filePath in Directory.EnumerateFiles("Assets/Data/WeaponsJson"))
            {
                StreamReader reader = new StreamReader(filePath, true);
            
                wJsonObjects.AddRange(ParserJson<WeaponJsonObject>.Parse(reader, "weapons"));
            }

            foreach (WeaponJsonObject weaponJson in wJsonObjects)
            {
                Weapon loadedWeapon = weaponJson.ConvertToWeapon();
                loadedWeapon.model = weaponsGameObject.First(go => go.name == loadedWeapon.modelName);
                weapons.Add(loadedWeapon);
            }
        }
    }
}