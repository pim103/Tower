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
using Networking;
using Networking.Client;
using UnityEngine;
using UnityEngine.Networking;
using Utils;

namespace Games.Global {
  public class DictionaryManager : MonoBehaviour {
    [SerializeField]
    private GameObject[] monsterGameObjects;
    [SerializeField]
    private GameObject[] weaponsGameObject;
    [SerializeField]
    private Material[] effectMaterials;

    public static bool hasWeaponsLoad;
    public static bool hasMonstersLoad;
    public static bool hasCardsLoad;
    public static bool wasConnected;

    private static bool wasInit;

    public void Awake() {
      if (!wasInit) {
        InitAbility();

        DataObject.CardList = new CardList();

        StartCoroutine(GetWeapons());
        StartCoroutine(GetGroupsMonster());
        StartCoroutine(GetCards());
        StartCoroutine(GetCardCollection());
        StartCoroutine(GetDecks());

        DataObject.MaterialsList = new List<Material>();
        DataObject.MaterialsList.AddRange(effectMaterials.ToList());

        wasInit = true;
      }
    }

    public static void InitAbility() {
      AbilityManager.InitAbilities();
      GroupsPosition.InitPosition();
    }

    public IEnumerator GetWeapons() {
      var www = UnityWebRequest.Get(NetworkingController.PublicURL +
                                    "/services/game/weapons/list.php");
      www.certificateHandler = new AcceptCertificate();
      yield return www.SendWebRequest();
      yield return new WaitForSeconds(0.5f);

      if (www.responseCode == 200) {
        DataObject.EquipmentList =
            new EquipmentList(weaponsGameObject, www.downloadHandler.text);
      } else {
        Debug.Log("Can't get weapons...");
      }
    }

    public IEnumerator GetGroupsMonster() {
      var www = UnityWebRequest.Get(NetworkingController.PublicURL +
                                    "/services/game/group/list.php");
      www.certificateHandler = new AcceptCertificate();
      yield return www.SendWebRequest();
      yield return new WaitForSeconds(0.5f);
      if (www.responseCode == 200) {
        Debug.Log(www.downloadHandler.text);
        DataObject.MonsterList =
            new MonsterList(monsterGameObjects, www.downloadHandler.text);
      } else {
        Debug.Log("Can't get Monsters...");
      }
    }

    public IEnumerator GetCards() {
      while (!hasMonstersLoad || !hasWeaponsLoad) {
        yield return new WaitForSeconds(0.5f);
      }

      var www = UnityWebRequest.Get(NetworkingController.PublicURL +
                                    "/services/game/card/list.php");
      www.certificateHandler = new AcceptCertificate();
      yield return www.SendWebRequest();
      yield return new WaitForSeconds(0.5f);
      if (www.responseCode == 200) {
        Debug.Log(www.downloadHandler.text);
        DataObject.CardList.InitCards(www.downloadHandler.text);
      } else {
        Debug.Log("Can't get Cards...");
      }
    }

    public IEnumerator GetCardCollection() {
      while (!hasCardsLoad || !wasConnected) {
        yield return new WaitForSeconds(0.5f);
      }

      WWWForm form = new WWWForm();
      if (NetworkingController.AuthToken != "") {
        form.AddField("collectionOwner", NetworkingController.AuthToken);
      } else {
        form.AddField("collectionOwner", "HZ0PUiJjDly8EDkyYUiP");
      }

      var www = UnityWebRequest.Post(
          NetworkingController.PublicURL +
              "/services/game/card/listAccountCollection.php",
          form);
      www.certificateHandler = new AcceptCertificate();
      yield return www.SendWebRequest();
      yield return new WaitForSeconds(0.5f);
      if (www.responseCode == 200) {
        DataObject.CardList.InitCardCollection(www.downloadHandler.text);
      } else {
        Debug.Log("Can't get Cards collection...");
      }
    }

    public IEnumerator GetDecks() {
      while (!hasCardsLoad || !wasConnected) {
        yield return new WaitForSeconds(0.5f);
      }

      WWWForm form = new WWWForm();
      if (NetworkingController.AuthToken != "") {
        form.AddField("deckOwner", NetworkingController.AuthToken);
      } else {
        form.AddField("deckOwner", "HZ0PUiJjDly8EDkyYUiP");
      }

      var www = UnityWebRequest.Post(NetworkingController.PublicURL +
                                         "/services/game/card/listCardDeck.php",
                                     form);
      www.certificateHandler = new AcceptCertificate();
      yield return www.SendWebRequest();
      yield return new WaitForSeconds(0.5f);
      if (www.responseCode == 200) {
        DataObject.CardList.InitDeck(www.downloadHandler.text);
      } else {
        Debug.Log("Can't get Cards decks...");
      }
    }
  }

  public static class DataObject {
    public static int nbEntityInScene;

    public static MonsterList MonsterList;
    public static EquipmentList EquipmentList;
    public static CardList CardList;
    public static List<Material> MaterialsList;

    public static List<Monster> monsterInScene = new List<Monster>();
    public static Dictionary<int, PlayerPrefab> playerInScene =
        new Dictionary<int, PlayerPrefab>();
    public static List<Entity> invocationsInScene = new List<Entity>();

    public static List<GameObject> objectInScene = new List<GameObject>();
  }
}