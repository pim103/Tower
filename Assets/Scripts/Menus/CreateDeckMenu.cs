using System.Collections.Generic;
using System.IO;
using DeckBuilding;
using Games.Global;
using Games.Global.Entities;
using UnityEngine;
using UnityEngine.UI;
using Utils;

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

  public int minBorder;
  public int maxBorder;

  [SerializeField]
  private InputField deckName;
  private void Start() {
    saveDeckButton.onClick.AddListener(
        delegate { mc.ActivateMenu(MenuController.Menu.DeckManagement); });

    returnButton.onClick.AddListener(
        delegate { mc.ActivateMenu(MenuController.Menu.DeckManagement); });
  }

  public void InitMenu() {
    minBorder = 0;
    maxBorder = 15;
    Debug.Log(deckManagementMenu.selectedDeckName);
    deckName.text = deckManagementMenu.selectedDeckName;
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

    cardsInDeck = new List<Card>();
    cardInDeckButtonsList = new List<GameObject>();
    cardInCollButtonsList = new List<GameObject>();
    if (!newDeck) {
      FetchDeckList();
      foreach (var card in cardsInDeck) {
        int currentCount = cardInDeckButtonsList.Count;
        GameObject currentCardInDeckButton =
            Instantiate(cardInDeckButton, transform);
        cardInDeckButtonsList.Add(currentCardInDeckButton);
        Vector3 currentPosition = currentCardInDeckButton.transform.position;
        currentCardInDeckButton.transform.position =
            new Vector3(750, currentPosition.y - 15 * currentCount - 1, 0);
        CardInDeckButtonExposer currentButtonExposer =
            currentCardInDeckButton.GetComponent<CardInDeckButtonExposer>();
        Debug.Log(card.id);
        Debug.Log(DataObject.MonsterList);
        GroupsMonster group =
            DataObject.MonsterList.GetGroupsMonsterById(card.id);
        currentButtonExposer.name.text = group.name;
        currentButtonExposer.copies.text = "X" + card.copies;
        currentCardInDeckButton.GetComponent<Button>().onClick.AddListener(
            delegate{});
      }
    }

    /*int ycount = 0;
    foreach (var card in DataObject.playerCollection)
    {
        int currentCount = cardInCollButtonsList.Count;
        GameObject currentCardInCollButton = Instantiate(cardInCollButton,
    transform); cardInCollButtonsList.Add(currentCardInCollButton); if
    (currentCount % 5 == 0)
        {
            ycount += 1;
        }
        Vector3 currentPosition = currentCardInCollButton.transform.position;
        currentCardInCollButton.transform.position = new
    Vector3(currentPosition.x+100*(currentCount%5),currentPosition.y-(150*(ycount-1))-20,0);
        CardInCollButtonExposer currentButtonExposer =
    currentCardInCollButton.GetComponent<CardInCollButtonExposer>();
        GroupsMonster group =
    DataObject.MonsterList.GetGroupsMonsterById(card.id);
        currentButtonExposer.name.text = group.name;
        currentButtonExposer.copies.text = "X" + card.copies;
        currentButtonExposer.effect.text = "effet";
        currentButtonExposer.cost.text = group.cost.ToString();
        currentButtonExposer.family.text = group.family.ToString();
        currentCardInCollButton.GetComponent<Button>().onClick.AddListener(delegate
        {

        });

    }*/
    ShowCards();
    Debug.Log("Create Deck Menu");
  }

  private void FetchDeckList() {
    List<DeckListJsonObject> dJsonObjects = new List<DeckListJsonObject>();

    foreach (string filePath in Directory.EnumerateFiles(
                 "Assets/Data/DeckListJson")) {
      StreamReader reader = new StreamReader(filePath, true);

      dJsonObjects.AddRange(
          ParserJson<DeckListJsonObject>.Parse(reader, "cards"));
    }

    foreach (DeckListJsonObject deckJson in dJsonObjects) {
      Card loadedCard = deckJson.ConvertToCard();
      cardsInDeck.Add(loadedCard);
    }
  }

  public void ShowCards() {
    if (cardInCollButtonsList != null) {
      foreach (var button in cardInCollButtonsList) {
        button.SetActive(false);
      }
    }
    cardInCollButtonsList = new List<GameObject>();
    int ycount = 0;
    for (int i = minBorder; i < maxBorder; i++) {
      if (i >= DataObject.playerCollection.Count)
        return;
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
      GroupsMonster group = DataObject.MonsterList.GetGroupsMonsterById(
          DataObject.playerCollection[i].id);
      currentButtonExposer.name.text = group.name;
      currentButtonExposer.copies.text =
          "X" + DataObject.playerCollection[i].copies;
      currentButtonExposer.effect.text = "effet";
      currentButtonExposer.cost.text = group.cost.ToString();
      currentButtonExposer.family.text = group.family.ToString();
      currentCardInCollButton.GetComponent<Button>().onClick.AddListener(
          delegate{

          });
    }
  }
}
}