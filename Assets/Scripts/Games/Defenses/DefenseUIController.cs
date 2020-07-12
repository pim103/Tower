using System;
using System.Collections.Generic;
using System.IO;
using DeckBuilding;
using Games.Global;
using Games.Global.Entities;
using Games.Global.Weapons;
using Games.Transitions;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;
using Utils;
using Random = UnityEngine.Random;

namespace Games.Defenses
{
public class DefenseUIController : MonoBehaviour
{
    [SerializeField]
    private Button wallButton;

    [SerializeField]
    private Button[] trapButtons;

    [SerializeField]
    public Button keyButton;

    [SerializeField]
    private Transform keySlot;

    [SerializeField]
    public Text wallButtonText;

    [SerializeField]
    public InitDefense initDefense;

    [SerializeField]
    private ObjectPooler defensePooler;

    [SerializeField]
    private HoverDetector hoverDetector;

    [SerializeField]
    private GameObject cardPrefab;

    public int currentWallNumber;
    private int currentWallType;
    public bool keyAlreadyPut;

    public MobDecklist mobDecklist;
    public List<GroupsMonster> mobDeckContent;
    public List<Equipement> equipmentDeckContent;

    [SerializeField]
    private GameObject[] mobCardContainers;

    [SerializeField]
    private GameObject[] equipementCardContainers;
    private List<Card> cardsInMonsterDeck;
    private List<Card> cardsInEquipmentDeck;

    [SerializeField]
    public Text maxResourceText;

    [SerializeField]
    public Text currentResourceText;

    [SerializeField]
    public GameObject keyObject;
    private void Start()
    {
        wallButton.onClick.AddListener(PutWallInHand);
        keyButton.onClick.AddListener(PutKeyInHand);
        foreach (var button in trapButtons)
        {
            button.transform.GetChild(0).GetComponent<Text>().text =
                button.gameObject.GetComponent<TrapBehavior>().mainType.ToString();
            button.onClick.AddListener(delegate
            {
                PutTrapInHand(button.gameObject.GetComponent<TrapBehavior>());
            });
        }

        keyObject.transform.position = keySlot.position;
    }

    void OnEnable()
    {
        keyAlreadyPut = false;
        keyObject.SetActive(true);
        currentWallNumber = initDefense.currentMapStats.wallNumber;
        currentWallType = initDefense.currentMapStats.wallType;
        wallButtonText.text = "Mur x" + currentWallNumber;

        if (cardsInMonsterDeck == null)
        {
            FetchMonsterDeckList(ChooseDeckAndClass.monsterDeckId);
        }

        if (cardsInEquipmentDeck == null)
        {
            FetchEquipmentDeckList(ChooseDeckAndClass.equipmentDeckId);
        }

        DrawCards();
        hoverDetector.maxResource = initDefense.currentLevel + 3;
        hoverDetector.currentResource = initDefense.currentLevel + 3;
        maxResourceText.text = "/ "+hoverDetector.maxResource;
        currentResourceText.text = hoverDetector.currentResource.ToString();
    }

    public void PutWallInHand()
    {
        if (currentWallNumber > 0 && !hoverDetector.objectInHand)
        {
            currentWallNumber -= 1;
            wallButtonText.text = "Mur x" + currentWallNumber;
            GameObject wall = defensePooler.GetPooledObject(currentWallType+1);
            hoverDetector.objectInHand = wall;
            wall.SetActive(true);
        }
    }

    public void PutKeyInHand()
    {
        if (!hoverDetector.objectInHand && !keyAlreadyPut)
        {
            keyObject.SetActive(true);
            hoverDetector.objectInHand = keyObject;
        }
    }

    public void PutKeyBackToSlot()
    {
        keyObject.SetActive(true);
        keyAlreadyPut = false;
        keyObject.transform.position = keySlot.position;
    }

    public void PutCardInHand(GameObject card)
    {
        card.transform.parent = null;
        hoverDetector.objectInHand = card;
        card.layer = LayerMask.NameToLayer("CardInHand");
    }

    public void PutCardBackToHand(GameObject equipementCard)
    {
        if (equipementCard)
        {
            CardBehaviorInGame equipementCardBehaviorInGame = equipementCard.GetComponent<CardBehaviorInGame>();
            equipementCardBehaviorInGame.groupParent.SetActive(false);
            equipementCardBehaviorInGame.groupParent.transform.localPosition = Vector3.zero;
            equipementCardBehaviorInGame.ownMeshRenderer.enabled = true;
            equipementCardBehaviorInGame.transform.SetParent(equipementCardBehaviorInGame.ownCardContainer);
            equipementCardBehaviorInGame.transform.localPosition = Vector3.zero;
            equipementCardBehaviorInGame.gameObject.layer = LayerMask.NameToLayer("Card");
            equipementCard.SetActive(true);
            hoverDetector.currentResource += equipementCardBehaviorInGame.equipement.cost;
            Debug.Log("return card");
        }
    }

    public void PutTrapInHand(TrapBehavior trapBehavior)
    {
        if (!hoverDetector.objectInHand)
        {
            GameObject trap = defensePooler.GetPooledObject(0);
            trap.GetComponent<TrapBehavior>().CopyBehavior(trapBehavior);
            hoverDetector.objectInHand = trap;
            trap.SetActive(true);
            hoverDetector.currentResource -= 1;
        }
    }

    public void DrawCards()
    {
        foreach (var cardContainer in mobCardContainers)
        {
            if (cardContainer.transform.childCount == 0 && cardsInMonsterDeck.Count>0)
            {
                Card selectedCard = cardsInMonsterDeck[Random.Range(0, cardsInMonsterDeck.Count-1)];
                InitCard(cardContainer, 0, selectedCard);
                selectedCard.copies--;
                if (selectedCard.copies < 1)
                {
                    cardsInMonsterDeck.Remove(selectedCard);
                }
            }
        }
        foreach (var cardContainer in equipementCardContainers)
        {
            if (cardContainer.transform.childCount == 0 && cardsInEquipmentDeck.Count>0)
            {
                Card selectedCard = cardsInEquipmentDeck[Random.Range(0, cardsInEquipmentDeck.Count-1)];
                InitCard(cardContainer, 1, selectedCard);
                selectedCard.copies--;
                if (selectedCard.copies < 1)
                {
                    cardsInEquipmentDeck.Remove(selectedCard);
                }
            }
        }
    }

    private void InitCard(GameObject cardContainer, int type, Card cardStats)
    {
        GameObject card = Instantiate(cardPrefab, cardContainer.transform, true);
        card.transform.localPosition = new Vector3(0,0,0);
        card.transform.localEulerAngles = new Vector3(0,0,0);
        card.GetComponent<CardBehaviorInGame>().SetCard(type,cardStats.id);
    }

    private void FetchMonsterDeckList(int deckId)
    {
        cardsInMonsterDeck = new List<Card>();

        List<DeckListJsonObject> dJsonObjects = new List<DeckListJsonObject>();

        foreach (string filePath in Directory.EnumerateFiles("Assets/Data/DeckListJson"))
        {
            StreamReader reader = new StreamReader(filePath, true);

            dJsonObjects.AddRange(ParserJson<DeckListJsonObject>.Parse(reader, "cards"));
        }

        foreach (DeckListJsonObject deckJson in dJsonObjects)
        {
            Card loadedCard = deckJson.ConvertToCard();
            if (loadedCard.deckId == deckId)
            {
                cardsInMonsterDeck.Add(loadedCard);
            }
        }
    }

    private void FetchEquipmentDeckList(int deckId)
    {
        cardsInEquipmentDeck = new List<Card>();

        List<DeckListJsonObject> dJsonObjects = new List<DeckListJsonObject>();

        foreach (string filePath in Directory.EnumerateFiles("Assets/Data/DeckListJson"))
        {
            StreamReader reader = new StreamReader(filePath, true);

            dJsonObjects.AddRange(ParserJson<DeckListJsonObject>.Parse(reader, "cards"));
        }

        foreach (DeckListJsonObject deckJson in dJsonObjects)
        {
            Card loadedCard = deckJson.ConvertToCard();
            if (loadedCard.deckId == deckId)
            {
                cardsInEquipmentDeck.Add(loadedCard);
            }
        }
    }

}
}
