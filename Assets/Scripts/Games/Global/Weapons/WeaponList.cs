using System.Collections.Generic;
using UnityEngine;

namespace Games.Global.Weapons
{
    public class WeaponList : MonoBehaviour
    {
        [SerializeField]
        private GameObject[] weaponsGameObject;

        public Dictionary<string, GameObject> weaponsList;
        public Dictionary<int, string> idWeaponsList;

        public GameObject GetWeaponWithName(string findName)
        {
            return weaponsList[findName];
        }

        public GameObject GetWeaponWithId(int id)
        {
            return weaponsList[idWeaponsList[id]];
        }

        private void InitWeaponDictionnary()
        {
            int count = 0;

            foreach (var weapon in weaponsGameObject)
            {
                string weaponName = weapon.name;
                weaponsList.Add(weaponName, weapon);
                idWeaponsList.Add(count, weaponName);

                count++;
            }
        }

        private void Start()
        {
            weaponsList = new Dictionary<string, GameObject>();
            idWeaponsList = new Dictionary<int, string>();

            InitWeaponDictionnary();
        }
    }
}