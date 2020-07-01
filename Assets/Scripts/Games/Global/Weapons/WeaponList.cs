using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.XR;
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
            switch (orig.category)
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

            weapon.id = orig.id;
            weapon.damage = orig.damage;
            weapon.category = orig.category;
            weapon.type = orig.type;
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
            Weapon findingWeapon = weapons.First(we => we.equipementName == findName);
            Weapon cloneWeapon = Tools.Clone(findingWeapon);
            return cloneWeapon;
        }

        public Weapon GetWeaponWithId(int id)
        {
            Weapon findingWeapon = weapons.First(we => we.id == id);
            Weapon cloneWeapon = Tools.Clone(findingWeapon);
            return cloneWeapon;
        }

        public void PrintDictionnary()
        {
            foreach (Weapon weapon in weapons)
            {
                Debug.Log(weapon);
                Debug.Log(weapon.category);
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