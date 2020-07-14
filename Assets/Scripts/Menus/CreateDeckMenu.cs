using System.Collections.Generic;
using System.IO;
using DeckBuilding;
using Games.Global;
using Games.Global.Entities;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace Menus
{
    public class CreateDeckMenu : MonoBehaviour, MenuInterface
    {
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

        private Deck selectedDeck;

        [SerializeField] private InputField deckName;
        private void Start()
        {
            saveDeckButton.onClick.AddListener(delegate {
                mc.ActivateMenu(MenuController.Menu.DeckManagement);
            });

            returnButton.onClick.AddListener(delegate
            {
                mc.ActivateMenu(MenuController.Menu.DeckManagement);
            });
        }

        public void InitMenu()
        {
            minBorder = 0;
            maxBorder = 15;
            
            if (deckManagementMenu.selectedDeck != 0)
            {
                Debug.Log(deckManagementMenu.selectedDeckName);
                deckName.text = deckManagementMenu.selectedDeckName;
                selectedDeck = DataObject.CardList.GetDeckById(deckManagementMenu.selectedDeck);
            }
            
            if (cardInCollButtonsList != null)
            {
                foreach (var button in cardInCollButtonsList)
                {
                    button.SetActive(false);
                }
            }
            if (cardInDeckButtonsList != null)
            {
                foreach (var button in cardInDeckButtonsList)
                {
                    button.SetActive(false);
                }
            }

            cardInDeckButtonsList = new List<GameObject>();
            cardInCollButtonsList = new List<GameObject>();

            if (!newDeck)
            {
                foreach (Card card in selectedDeck.GetCardsInDeck())
                {
                    int currentCount = cardInDeckButtonsList.Count;
                    GameObject currentCardInDeckButton = Instantiate(cardInDeckButton, transform);
                    cardInDeckButtonsList.Add(currentCardInDeckButton);
                    Vector3 currentPosition = currentCardInDeckButton.transform.position;
                    currentCardInDeckButton.transform.position = new Vector3(750, currentPosition.y - 15 * currentCount - 1, 0);
                    
                    CardInDeckButtonExposer currentButtonExposer = currentCardInDeckButton.GetComponent<CardInDeckButtonExposer>();

                    if (card.GroupsMonster != null)
                    {
                        currentButtonExposer.name.text = card.GroupsMonster.name;
                        currentButtonExposer.copies.text = "X" + selectedDeck.GetCardNumber(card.id);
                        currentCardInDeckButton.GetComponent<Button>().onClick.AddListener(delegate { });
                    } else if (card.Weapon != null)
                    {
                        currentButtonExposer.name.text = card.Weapon.equipementName;
                        currentButtonExposer.copies.text = "X" + selectedDeck.GetCardNumber(card.id);
                        currentCardInDeckButton.GetComponent<Button>().onClick.AddListener(delegate { });
                    }
                }
            }
            
            int ycount = 0;
            foreach (Card card in DataObject.CardList.GetCardsInCollection())
            {
                int currentCount = cardInCollButtonsList.Count;
                GameObject currentCardInCollButton = Instantiate(cardInCollButton, transform);
                cardInCollButtonsList.Add(currentCardInCollButton);
                if (currentCount % 5 == 0)
                {
                    ycount += 1;
                }
                Vector3 currentPosition = currentCardInCollButton.transform.position;
                currentCardInCollButton.transform.position = new Vector3(currentPosition.x+100*(currentCount%5),currentPosition.y-(150*(ycount-1))-20,0);
                CardInCollButtonExposer currentButtonExposer = currentCardInCollButton.GetComponent<CardInCollButtonExposer>();

                if (card.GroupsMonster != null)
                {
                    currentButtonExposer.name.text = card.GroupsMonster.name;
                    currentButtonExposer.copies.text = "X" + DataObject.CardList.GetNbSpecificCardInCollection(card.id);
                    currentButtonExposer.effect.text = "effet";
                    currentButtonExposer.cost.text = card.GroupsMonster.cost.ToString();
                    currentButtonExposer.family.text = card.GroupsMonster.family.ToString();
                    currentCardInCollButton.GetComponent<Button>().onClick.AddListener(delegate
                    {
                    
                    });
                } else if (card.Weapon != null)
                {
                    currentButtonExposer.name.text = card.Weapon.equipementName;
                    currentButtonExposer.copies.text = "X" + DataObject.CardList.GetNbSpecificCardInCollection(card.id);
                    currentButtonExposer.effect.text = "effet";
                    currentButtonExposer.cost.text = card.Weapon.cost.ToString();
                    currentButtonExposer.family.text = card.Weapon.type.ToString();
                    currentCardInCollButton.GetComponent<Button>().onClick.AddListener(delegate
                    {
                    
                    });
                }
                
            }
            ShowCards();
            Debug.Log("Create Deck Menu");
        }

        public void ShowCards()
        {
            if (cardInCollButtonsList != null)
            {
                foreach (var button in cardInCollButtonsList)
                {
                    button.SetActive(false);
                }
            }
            
            cardInCollButtonsList = new List<GameObject>();
            int ycount = 0;

            foreach (Card card in DataObject.CardList.GetCardsInCollection())
            {
                int currentCount = cardInCollButtonsList.Count;
                GameObject currentCardInCollButton = Instantiate(cardInCollButton, transform);
                cardInCollButtonsList.Add(currentCardInCollButton);
                if (currentCount % 5 == 0)
                {
                    ycount += 1;
                }
                Vector3 currentPosition = currentCardInCollButton.transform.position;
                currentCardInCollButton.transform.position = new Vector3(currentPosition.x+100*(currentCount%5),currentPosition.y-(150*(ycount-1))+20,0);
                
                CardInCollButtonExposer currentButtonExposer = currentCardInCollButton.GetComponent<CardInCollButtonExposer>();

                if (card.GroupsMonster != null)
                {
                    currentButtonExposer.name.text = card.GroupsMonster.name;
                    currentButtonExposer.copies.text = "X" + DataObject.CardList.GetNbSpecificCardInCollection(card.id);
                    currentButtonExposer.effect.text = "effet";
                    currentButtonExposer.cost.text = card.GroupsMonster.cost.ToString();
                    currentButtonExposer.family.text = card.GroupsMonster.family.ToString();
                    currentCardInCollButton.GetComponent<Button>().onClick.AddListener(delegate
                    {
                    
                    });
                } else if (card.Weapon != null)
                {
                    currentButtonExposer.name.text = card.Weapon.equipementName;
                    currentButtonExposer.copies.text = "X" + DataObject.CardList.GetNbSpecificCardInCollection(card.id);
                    currentButtonExposer.effect.text = "effet";
                    currentButtonExposer.cost.text = card.Weapon.cost.ToString();
                    currentButtonExposer.family.text = card.Weapon.type.ToString();
                    currentCardInCollButton.GetComponent<Button>().onClick.AddListener(delegate
                    {
                    
                    });
                }
            }
        }
    }
}