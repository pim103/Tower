using System;
using Games.Global.Abilities;
using Games.Global.Entities;
using Games.Global.Weapons;
using UnityEngine;

namespace Games.Global
{
    public class DictionnaryManager: MonoBehaviour
    {
        [SerializeField] private GameObject[] monsterGameObjects;
        [SerializeField] private GameObject[] weaponsGameObject;

        public MonsterList monsterList;
        public WeaponList weaponList;

        public void Start()
        {
            AbilityManager.InitAbilities();
            
            monsterList = new MonsterList(monsterGameObjects);
            weaponList = new WeaponList(weaponsGameObject);
        }
    }
}