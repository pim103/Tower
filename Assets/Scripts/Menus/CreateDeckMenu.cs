using System;
using System.Collections;
using System.Collections.Generic;
using DeckBuilding;
using Games.Global;
using Networking;
using Networking.Client;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace Menus {
public class CreateDeckMenu : MonoBehaviour, MenuInterface {
  [SerializeField]
  private MenuController mc;

  [SerializeField]
  private Button saveDeckButton;

  [SerializeField]
  private Button returnButton;

  [SerializeField]
  private DeckManagementMenu deckManagementMenu;

  private List<Card> cardsInDeck;
  private List<GameObject> cardInDeckButtonsList;
  private List<GameObject> cardInCollButtonsList;

  [SerializeField]
  private GameObject cardInDeckButton;
  [SerializeField]
  private GameObject cardInCollButton;

  public bool newDeck;
  private Deck selectedDeck;

  private int totalDistinctCardNumber;
  private int cardCounter;
  private bool reloadingDecks;

  [SerializeField]
  private InputField deckName;
  [SerializeField]
  private Transform deckPosition;
  private void Start() {
    saveDeckButton.onClick.AddListener(delegate { SaveDeck(); });

    returnButton.onClick.AddListener(
        delegate { mc.ActivateMenu(MenuController.Menu.DeckManagement); });
  }

  public void InitMenu() {

    if (deckManagementMenu.selectedDeck != 0) {
      Debug.Log(deckManagementMenu.selectedDeckName);
      deckName.text = deckManagementMenu.selectedDeckName;
      selectedDeck =
          DataObject.CardList.GetDeckById(deckManagementMenu.selectedDeck);
    } else {
      selectedDeck = null;
    }

    if (cardInCollButtonsList != null) {
      foreach (var button in cardInCollButtonsList) {
        button.SetActive(false);
      }
    }
    if (cardInDeckButtonsList != null) {
      foreach (var button in cardInDeckButtonsList) {
        button.SetActive(false);
      }
    }

    cardInDeckButtonsList = new List<GameObject>();
    cardInCollButtonsList = new List<GameObject>();

    if (!newDeck) {
      foreach (Card card in selectedDeck.GetCardsInDeck()) {
        int currentCount = cardInDeckButtonsList.Count;
        GameObject currentCardInDeckButton =
            Instantiate(cardInDeckButton, transform);
        cardInDeckButtonsList.Add(currentCardInDeckButton);
        currentCardInDeckButton.transform.position =
            new Vector3(deckPosition.position.x,
                        deckPosition.position.y - 15 * currentCount - 1, 0);

        CardInDeckButtonExposer currentButtonExposer =
            currentCardInDeckButton.GetComponent<CardInDeckButtonExposer>();

        if (card.GroupsMonster != null) {
          currentButtonExposer.card = card;
          currentButtonExposer.name.text = card.GroupsMonster.name;
          currentButtonExposer.copies.text =
              selectedDeck.GetCardNumber(card.id).ToString();
          currentCardInDeckButton.GetComponent<Button>().onClick.AddListener(
              delegate { RemoveCardFromDeck(currentButtonExposer); });
        } else if (card.Weapon != null) {
          currentButtonExposer.card = card;
          currentButtonExposer.name.text = card.Weapon.equipementName;
          currentButtonExposer.copies.text =
              selectedDeck.GetCardNumber(card.id).ToString();
          currentCardInDeckButton.GetComponent<Button>().onClick.AddListener(
              delegate { RemoveCardFromDeck(currentButtonExposer); });
        }
      }
    }
    ShowCards();
    Debug.Log("Create Deck Menu");
  }

  public void ShowCards() {
    if (cardInCollButtonsList != null) {
      foreach (var button in cardInCollButtonsList) {
        button.SetActive(false);
      }
    }

    cardInCollButtonsList = new List<GameObject>();
    int ycount = 0;

    foreach (Card card in DataObject.CardList.GetCardsInCollection()) {
      int currentCount = cardInCollButtonsList.Count;
      GameObject currentCardInCollButton =
          Instantiate(cardInCollButton, transform);
      cardInCollButtonsList.Add(currentCardInCollButton);
      if (currentCount % 5 == 0) {
        ycount += 1;
      }
      Vector3 currentPosition = currentCardInCollButton.transform.position;
      currentCardInCollButton.transform.position =
          new Vector3(currentPosition.x + 100 * (currentCount % 5),
                      currentPosition.y - (150 * (ycount - 1)) + 20, 0);

      CardInCollButtonExposer currentButtonExposer =
          currentCardInCollButton.GetComponent<CardInCollButtonExposer>();

      if (card.GroupsMonster != null) {
        currentButtonExposer.card = card;
        currentButtonExposer.name.text = card.GroupsMonster.name;
        currentButtonExposer.copies.text =
            DataObject.CardList.GetNbSpecificCardInCollection(card.id)
                .ToString();
        currentButtonExposer.effect.text = "effet";
        currentButtonExposer.cost.text = card.GroupsMonster.cost.ToString();
        currentButtonExposer.family.text = card.GroupsMonster.family.ToString();
        currentCardInCollButton.GetComponent<Button>().onClick.AddListener(
            delegate { PutCardInDeck(currentButtonExposer.card); });
      } else if (card.Weapon != null) {
        currentButtonExposer.card = card;
        currentButtonExposer.name.text = card.Weapon.equipementName;
        currentButtonExposer.copies.text =
            DataObject.CardList.GetNbSpecificCardInCollection(card.id)
                .ToString();
        currentButtonExposer.effect.text = "effet";
        currentButtonExposer.cost.text = card.Weapon.cost.ToString();
        currentButtonExposer.family.text = card.Weapon.type.ToString();
        currentCardInCollButton.GetComponent<Button>().onClick.AddListener(
            delegate { PutCardInDeck(currentButtonExposer.card); });
      }
    }
  }

  private void
  RemoveCardFromDeck(CardInDeckButtonExposer cardInDeckButtonExposer) {
    int cardNb = Int32.Parse(cardInDeckButtonExposer.copies.text);
    if (cardNb > 1) {
      cardInDeckButtonExposer.copies.text = (cardNb - 1).ToString();
    } else {
      cardInDeckButtonsList.Remove(cardInDeckButtonExposer.gameObject);
      cardInDeckButtonExposer.gameObject.SetActive(false);
      int posCount = 0;
      foreach (GameObject button in cardInDeckButtonsList) {
        button.transform.position =
            new Vector3(deckPosition.position.x,
                        deckPosition.position.y - 15 * posCount - 1,
                        button.transform.position.z);
        posCount++;
      }
    }
  }

  private void PutCardInDeck(Card card) {
    bool cardFound = false;
    foreach (GameObject cardButton in cardInDeckButtonsList) {
      CardInDeckButtonExposer currentButtonExposer =
          cardButton.GetComponent<CardInDeckButtonExposer>();
      if (currentButtonExposer.card.id == card.id) {
        cardFound = true;
        int nbCopies = Int32.Parse(currentButtonExposer.copies.text);
        if (nbCopies < 3) {
          currentButtonExposer.copies.text = (nbCopies + 1).ToString();
        }
      }
    }

    if (!cardFound) {
      int currentCount = cardInDeckButtonsList.Count;
      GameObject currentCardInDeckButton =
          Instantiate(cardInDeckButton, transform);
      cardInDeckButtonsList.Add(currentCardInDeckButton);
      currentCardInDeckButton.transform.position =
          new Vector3(deckPosition.position.x,
                      deckPosition.position.y - 15 * currentCount - 1, 0);

      CardInDeckButtonExposer currentButtonExposer =
          currentCardInDeckButton.GetComponent<CardInDeckButtonExposer>();

      if (card.GroupsMonster != null) {
        currentButtonExposer.card = card;
        currentButtonExposer.name.text = card.GroupsMonster.name;
        currentButtonExposer.copies.text = "1";
        currentCardInDeckButton.GetComponent<Button>().onClick.AddListener(
            delegate { RemoveCardFromDeck(currentButtonExposer); });
      } else if (card.Weapon != null) {
        currentButtonExposer.card = card;
        currentButtonExposer.name.text = card.Weapon.equipementName;
        currentButtonExposer.copies.text = "1";
        currentCardInDeckButton.GetComponent<Button>().onClick.AddListener(
            delegate { RemoveCardFromDeck(currentButtonExposer); });
      }
    }
  }

  public IEnumerator AddNewDeck(string deckName, int isMonsterDeck) {
    WWWForm form = new WWWForm();
    form.AddField("deckOwner", NetworkingController.AuthToken);
    form.AddField("deckName", deckName);
    form.AddField("isMonsterDeck", isMonsterDeck);
    form.AddField("gameToken", NetworkingController.GameToken);
    var www = UnityWebRequest.Post(
        "https://towers.heolia.eu/services/game/deck/add.php", form);
    www.certificateHandler = new AcceptCertificate();
    yield return www.SendWebRequest();
    yield return new WaitForSeconds(0.5f);
    if (www.responseCode == 201) {
      selectedDeck = new Deck();
      selectedDeck.id = Int32.Parse(www.downloadHandler.text);
      Debug.Log("Deck Created!");
      if (selectedDeck != null) {
        StartCoroutine(DeleteCardsInDeck(selectedDeck.id));
      }
    } else if (www.responseCode == 406) {
      Debug.Log("Erreur dans la création du deck");
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

  public IEnumerator DeleteCardsInDeck(int deckId) {
    WWWForm form = new WWWForm();
    form.AddField("deckId", deckId);
    form.AddField("gameToken", NetworkingController.GameToken);
    var www = UnityWebRequest.Post(
        "https://towers.heolia.eu/services/game/deck/deleteAllCardsInDeck.php",
        form);
    www.certificateHandler = new AcceptCertificate();
    yield return www.SendWebRequest();
    yield return new WaitForSeconds(0.5f);
    if (www.responseCode == 201) {
      Debug.Log("Suppression des cartes effectuée");
      foreach (GameObject cardButton in cardInDeckButtonsList) {
        CardInDeckButtonExposer currentExposer =
            cardButton.GetComponent<CardInDeckButtonExposer>();
        StartCoroutine(
            AddNewCardInDeck(selectedDeck.id, currentExposer.card.id,
                             Int32.Parse(currentExposer.copies.text)));
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

  public IEnumerator AddNewCardInDeck(int deckId, int cardId, int nbCards) {
    WWWForm form = new WWWForm();
    form.AddField("deckId", deckId);
    form.AddField("cardId", cardId);
    form.AddField("numberOfCards", nbCards);
    form.AddField("gameToken", NetworkingController.GameToken);
    var www = UnityWebRequest.Post(
        "https://towers.heolia.eu/services/game/deck/addCardInDeck.php", form);
    www.certificateHandler = new AcceptCertificate();
    yield return www.SendWebRequest();
    yield return new WaitForSeconds(0.1f);
    if (www.responseCode == 201) {
      Debug.Log("Carte ajoutée");
      cardCounter++;
    } else if (www.responseCode == 406) {
      Debug.Log("Erreur dans l'ajout de la carte");
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

  private void SaveDeck() {
    bool containsMonsters = false;
    bool containsEquipments = false;
    totalDistinctCardNumber = 0;
    cardCounter = 0;
    Debug.Log("allo");
    foreach (GameObject cardButton in cardInDeckButtonsList) {
      CardInDeckButtonExposer currentExposer =
          cardButton.GetComponent<CardInDeckButtonExposer>();
      if (currentExposer.card.GroupsMonster != null) {
        containsMonsters = true;
      } else {
        containsEquipments = true;
      }
      totalDistinctCardNumber++;
    }
    if (containsMonsters ^ containsEquipments) {
      Debug.Log(containsEquipments + " " + containsMonsters);
      if (selectedDeck == null) {
        StartCoroutine(containsMonsters ? AddNewDeck(deckName.text, 1)
                       : AddNewDeck(deckName.text, 0));
      } else {
        StartCoroutine(DeleteCardsInDeck(selectedDeck.id));
      }
      StartCoroutine(WaitEndCreation());
    }
  }

  private IEnumerator WaitEndCreation() {
    while (cardCounter < totalDistinctCardNumber) {
      Debug.Log("i wait");
      yield return new WaitForSeconds(0.1f);
    }

    DataObject.CardList.decks = new List<Deck>();
    reloadingDecks = true;
    StartCoroutine(ReloadDecks());
    while (reloadingDecks) {
      yield return new WaitForSeconds(0.1f);
    }
    mc.ActivateMenu(MenuController.Menu.DeckManagement);
  }

  private IEnumerator ReloadDecks() {
    WWWForm form = new WWWForm();
    if (NetworkingController.AuthToken != "") {
      form.AddField("deckOwner", NetworkingController.AuthToken);
    } else {
      form.AddField("deckOwner", "HZ0PUiJjDly8EDkyYUiP");
    }

    var www = UnityWebRequest.Post(
        "https://towers.heolia.eu/services/game/card/listCardDeck.php", form);
    www.certificateHandler = new AcceptCertificate();
    yield return www.SendWebRequest();
    yield return new WaitForSeconds(0.5f);
    if (www.responseCode == 200) {
      DataObject.CardList.InitDeck(www.downloadHandler.text);
      reloadingDecks = false;
    } else {
      Debug.Log("Can't get Cards decks...");
    }
  }
}
}
