using System.Collections;
using System.Collections.Generic;
using DeckBuilding;
using Games.Global.Entities;
using Games.Global.Spells;
using Games.Global.Weapons;
using Games.Players;
using Networking;
using Networking.Client;
using UnityEngine;
using UnityEngine.Networking;
using Utils;

namespace Games.Global
{
    public class DictionaryManager: MonoBehaviour
    {
        public static bool hasSpellsLoad;
        public static bool hasWeaponsLoad;
        public static bool hasMonstersLoad;
        public static bool hasClassesLoad;
        public static bool hasCategoriesLoad;
        public static bool hasCardsLoad;
        public static bool wasConnected;

        private static bool wasInit;

        private static DictionaryManager instance;

        public void Awake()
        {
            if (!wasInit)
            {
                instance = this;
                GroupsPosition.InitPosition();
                
                DataObject.CardList = new CardList();

                StartCoroutine(InitializeDataObject());

                wasInit = true;
            }
        }

        public static IEnumerator InitializeDataObject()
        {
            instance.StartCoroutine(DatabaseManager.GetSpells());

            while (!hasSpellsLoad)
            {
                yield return new WaitForSeconds(0.5f);
            }
            
            instance.StartCoroutine(DatabaseManager.GetClasses());
            instance.StartCoroutine(DatabaseManager.GetCategoryWeapon());
            instance.StartCoroutine(DatabaseManager.GetGroupsMonster());

            while (!hasCategoriesLoad)
            {
                instance.StartCoroutine(DatabaseManager.GetWeapons());
            }
            
            while (!hasMonstersLoad || !hasWeaponsLoad)
            {
                yield return new WaitForSeconds(0.5f);
            }

            instance.StartCoroutine(DatabaseManager.GetCards());

            while (!hasCardsLoad || !wasConnected)
            {
                yield return new WaitForSeconds(0.5f);
            }

            instance.StartCoroutine(DatabaseManager.GetCardCollection());
            instance.StartCoroutine(DatabaseManager.GetDecks());
        }
    }

    public static class DataObject
    {
        public static int nbEntityInScene;
        
        public static MonsterList MonsterList;
        public static EquipmentList EquipmentList;
        public static CardList CardList;
        public static ClassesList ClassesList;
        public static SpellList SpellList;
        public static CategoryWeaponList CategoryWeaponList;
        
        public static List<Monster> monsterInScene = new List<Monster>();
        public static Dictionary<int, PlayerPrefab> playerInScene = new Dictionary<int, PlayerPrefab>();
        public static List<Entity> invocationsInScene = new List<Entity>();

        public static List<GameObject> objectInScene = new List<GameObject>();

        public static Dictionary<Ingredient, int> playerIngredients = new Dictionary<Ingredient, int>();
    }
}