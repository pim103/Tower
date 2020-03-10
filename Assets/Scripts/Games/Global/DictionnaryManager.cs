using System.Collections.Generic;
using Games.Global.Abilities;
using Games.Global.Entities;
using Games.Global.Weapons;
using Games.Players;
using UnityEngine;
using Utils;

namespace Games.Global
{
    public class DictionnaryManager: MonoBehaviour
    {
        [SerializeField] private GameObject[] monsterGameObjects;
        [SerializeField] private GameObject[] weaponsGameObject;

        public void Start()
        {
            AbilityManager.InitAbilities();
            GroupsPosition.InitPosition();
            
            DataObject.MonsterList = new MonsterList(monsterGameObjects);
            DataObject.WeaponList = new WeaponList(weaponsGameObject);
        }
    }

    public static class DataObject
    {
        public static MonsterList MonsterList;
        public static WeaponList WeaponList;

        public static List<Monster> monsterInScene = new List<Monster>();
        public static Dictionary<int, PlayerPrefab> playerInScene = new Dictionary<int, PlayerPrefab>();
    }
}