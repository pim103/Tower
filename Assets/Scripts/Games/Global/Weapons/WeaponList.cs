using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Games.Global.Weapons.Abilities;
using UnityEngine;

namespace Games.Global.Weapons
{
    public class WeaponList : MonoBehaviour
    {
        [SerializeField]
        private GameObject[] weaponsGameObject;

        public List<Weapon> weapons;

        public Weapon GetWeaponWithName(string findName)
        {
            return weapons.First(we => we.equipementName == findName);
        }

        public Weapon GetWeaponWithId(int id)
        {
            return weapons.First(we => we.id == id);
        }

        private void InitWeaponDictionnary()
        {
            List<WeaponJsonObject> wJsonObjects = new List<WeaponJsonObject>();

            foreach (string filePath in Directory.EnumerateFiles("Assets/Data/WeaponsJson"))
            {
                StreamReader reader = new StreamReader(filePath, true);
            
                wJsonObjects.AddRange(WeaponParseJson.ParseWeapon(reader));
            }

            foreach (WeaponJsonObject weaponJson in wJsonObjects)
            {
                Weapon loadedWeapon = weaponJson.ConvertToWeapon();
                loadedWeapon.model = weaponsGameObject.First(go => go.name == loadedWeapon.modelName);
                weapons.Add(loadedWeapon);
            }
        }

        private void Start()
        {
            AbilityManager.InitAbilities();
            weapons = new List<Weapon>();
            InitWeaponDictionnary();
        }
    }
}