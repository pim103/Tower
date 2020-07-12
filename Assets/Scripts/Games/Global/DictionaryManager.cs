using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DeckBuilding;
using Games.Global.Abilities;
using Games.Global.Entities;
using Games.Global.Spells;
using Games.Global.Weapons;
using Games.Players;
using Networking.Client;
using UnityEngine;
using UnityEngine.Networking;
using Utils;

namespace Games.Global
{
    public class DictionaryManager: MonoBehaviour
    {
        [SerializeField] private GameObject[] monsterGameObjects;
        [SerializeField] private GameObject[] weaponsGameObject;
        [SerializeField] private Material[] effectMaterials;

        public void Awake()
        {
            AbilityManager.InitAbilities();
            GroupsPosition.InitPosition();
            FetchCollection();

            StartCoroutine(GetWeapons());
            StartCoroutine(GetGroupsMonster());

            DataObject.MaterialsList = new List<Material>();
            DataObject.MaterialsList.AddRange(effectMaterials.ToList());
        }

        public IEnumerator GetWeapons()
        {
            var www = UnityWebRequest.Get("https://towers.heolia.eu/services/game/weapons/list.php");
            www.certificateHandler = new AcceptCertificate();
            yield return www.SendWebRequest();
            yield return new WaitForSeconds(0.5f);
            if (www.responseCode == 200)
            {
                DataObject.WeaponList = new WeaponList(weaponsGameObject, www.downloadHandler.text);
            }
            else
            {
                Debug.Log("Can't get weapons...");
            }
        }
        
        public IEnumerator GetGroupsMonster()
        {
            var www = UnityWebRequest.Get("https://towers.heolia.eu/services/game/group/list.php");
            www.certificateHandler = new AcceptCertificate();
            yield return www.SendWebRequest();
            yield return new WaitForSeconds(0.5f);
            if (www.responseCode == 200)
            {
                DataObject.MonsterList = new MonsterList(monsterGameObjects, www.downloadHandler.text);
            }
            else
            {
                Debug.Log("Can't get Monsters...");
            }
        }
        
        private void FetchCollection()
        {
            List<CollectionJsonObject> dJsonObjects = new List<CollectionJsonObject>();

            foreach (string filePath in Directory.EnumerateFiles("Assets/Data/CollectionJson"))
            {
                StreamReader reader = new StreamReader(filePath, true);
        
                dJsonObjects.AddRange(ParserJson<CollectionJsonObject>.Parse(reader, "cards"));
            }

            foreach (CollectionJsonObject deckJson in dJsonObjects)
            {
                Card loadedCard = deckJson.ConvertToCard();
                DataObject.playerCollection.Add(loadedCard);
            }
        }
    }

    public static class DataObject
    {
        public static int nbEntityInScene;
        
        public static MonsterList MonsterList;
        public static WeaponList WeaponList;
        public static List<Material> MaterialsList;
        
        public static List<Monster> monsterInScene = new List<Monster>();
        public static Dictionary<int, PlayerPrefab> playerInScene = new Dictionary<int, PlayerPrefab>();
        public static List<Entity> invocationsInScene = new List<Entity>();

        public static List<GameObject> objectInScene = new List<GameObject>();
        
        public static List<Card> playerCollection = new List<Card>();
    }
}