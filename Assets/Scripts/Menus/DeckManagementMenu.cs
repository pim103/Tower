using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DeckBuilding;
using Games.Global;
using Networking;
using Networking.Client;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Utils;

namespace Menus {
public class DeckManagementMenu : MonoBehaviour, MenuInterface {
  [SerializeField]
  private MenuController mc;

  [SerializeField]
  private Button collectionButton;

  [SerializeField]
  private Button createDeckButton;

  [SerializeField]
  private Button editDeckButton;

  [SerializeField]
  private Button returnButton;

  [SerializeField]
  private GameObject deckButton;

  [SerializeField]
  private CreateDeckMenu createDeckMenu;

  [SerializeField]
  private Button deleteButton;
  [SerializeField]
  private Transform BasePos;
  [SerializeField]
  private Transform BasePosXOffset;
  [SerializeField]
  private Transform BasePosYOffset;

  private List<GameObject> deckButtonList;

  public int selectedDeck;
  public string selectedDeckName;
  private void Start() {
    collectionButton.onClick.AddListener(
        delegate { mc.ActivateMenu(MenuController.Menu.Collection); });

    createDeckButton.onClick.AddListener(delegate {
      createDeckMenu.newDeck = true;
      selectedDeck = 0;
      mc.ActivateMenu(MenuController.Menu.CreateDeck);
    });

    editDeckButton.onClick.AddListener(delegate {
      if (selectedDeck != 0) {
        createDeckMenu.newDeck = false;
        mc.ActivateMenu(MenuController.Menu.CreateDeck);
      }
    });

    returnButton.onClick.AddListener(
        delegate { mc.ActivateMenu(MenuController.Menu.MainMenu); });

    deleteButton.onClick.AddListener(delegate {
      if (selectedDeck != 0) {
        StartCoroutine(DeleteDeck(selectedDeck));
      }
    });
  }

  public void InitMenu() {
    selectedDeck = 0;
    deckButtonList = new List<GameObject>();
    ShowDecks();
    Debug.Log("Deck Management Menu");
  }

  private void ShowDecks() {
    int ycount = 0;
    foreach (var deck in DataObject.CardList.GetDecks()) {
      int currentCount = deckButtonList.Count;
      GameObject currentDeckButton = Instantiate(deckButton, transform);
      deckButtonList.Add(currentDeckButton);
      if (currentCount % 3 == 0) {
        ycount += 1;
      }

      currentDeckButton.transform.position = new Vector3(
          BasePos.position.x +
              (BasePosXOffset.position.x - BasePos.position.x) *
                  (currentCount % 3),
          BasePos.position.y +
              ((BasePosYOffset.position.y - BasePos.position.y) * (ycount - 1)),
          0);
      DeckButtonExposer currentButtonExposer =
          currentDeckButton.GetComponent<DeckButtonExposer>();
      currentButtonExposer.deckName.text = deck.name;
      currentButtonExposer.typeImage.color =
          deck.type == Decktype.Monsters ? Color.red : Color.blue;
      currentButtonExposer.deckId = deck.id;
      currentDeckButton.GetComponent<Button>().onClick.AddListener(delegate {
        selectedDeck = deck.id;
        selectedDeckName = deck.name;
      });
    }
  }

  public IEnumerator DeleteDeck(int deckId) {
    WWWForm form = new WWWForm();
    form.AddField("deckId", deckId);
    form.AddField("gameToken", NetworkingController.GameToken);
    var www = UnityWebRequest.Post(
        "https://towers.heolia.eu/services/game/deck/delete.php", form);
    www.certificateHandler = new AcceptCertificate();
    yield return www.SendWebRequest();
    yield return new WaitForSeconds(0.5f);
    if (www.responseCode == 201) {
      Debug.Log("Suppression des cartes effectuée");
      foreach (GameObject deckButton in deckButtonList) {
        if (deckButton.GetComponent<DeckButtonExposer>().deckId == deckId) {
          deckButton.SetActive(false);
        }
      }
    } else if (www.responseCode == 406) {
      Debug.Log("Erreur dans la suppression des cartes");
    } else if (www.responseCode == 403) {
      Debug.Log("Erreur dans le formulaire");
    } else if (www.responseCode == 401) {
      Debug.Log("Vérifiez le GameToken");
    } else {
      Debug.Log(www.responseCode);
      Debug.Log(www.downloadHandler.text);
      Debug.Log("Serveur indisponible.");
    }
  }
}
}
