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

        public void Start()
        {
            AbilityManager.InitAbilities();
            
            DataObject.MonsterList = new MonsterList(monsterGameObjects);
            DataObject.WeaponList = new WeaponList(weaponsGameObject);
        }
    }

    public static class DataObject
    {
        public static MonsterList MonsterList;
        public static WeaponList WeaponList;
    }
}