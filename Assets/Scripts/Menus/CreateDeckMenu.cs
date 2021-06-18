﻿using System;
using System.Collections;
using System.Collections.Generic;
using DeckBuilding;
using Games.Global;
using Networking;
using Networking.Client;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace Menus
{
    //TODO : refacto like CollectionAndCraftMenu.cs + the menu prefab asociated at this script
    
    public class CreateDeckMenu : MonoBehaviour, MenuInterface
    {
        
        #region all menus parameters
        [SerializeField]
        private MenuController mc;
        [SerializeField]
        private Button returnButton;
        #endregion

        #region collection parameters
        [SerializeField] 
        private GridLayoutGroup collectionGrid;
        [SerializeField] 
        private GameObject cardPrefab;
        private List<GameObject> cards;
        #endregion
        
        #region deck edition parameters
        [SerializeField]
        private Button saveDeckButton;
        [SerializeField] 
        private DeckManagementMenu deckManagementMenu;
        [SerializeField] 
        private GridLayoutGroup deckGrid;
        [SerializeField] 
        private GameObject cardInDeckPrefab;
        private Deck currentDeck;
        private List<GameObject> deckCards;
        public bool newDeck;
        [SerializeField] private GameObject deckNameStatic; 
        [SerializeField] private GameObject deckNameDynamic; 
        [SerializeField] private TextMeshProUGUI deckName;
        [SerializeField] private Button editDeckNameButton;
        [SerializeField] private Button saveDeckNameButton;
        #endregion

        private int totalDistinctCardNumber;
        private int cardCounter;
        private bool reloadingDecks;

        #region start and initialization functions
        private void Start()
        {
            saveDeckButton.onClick.AddListener(SaveDeck);
            
            editDeckNameButton.onClick.AddListener(EditDeckName);
            saveDeckNameButton.onClick.AddListener(SaveDeckName);

            returnButton.onClick.AddListener(delegate
            {
                mc.ActivateMenu(MenuController.Menu.DeckManagement);
            });
        }

        public void InitMenu()
        {
            InitializeCollection();
        }

        private void InitializeCollection()
        {
            if (cards != null)
            {
                foreach (GameObject card in cards)
                {
                    card.SetActive(false);
                }
            }

            cards = new List<GameObject>();

            foreach (Card card in DataObject.CardList.GetCardsInCollection())
            {
                GameObject instantiateCard = Instantiate(cardPrefab, collectionGrid.transform);
                cards.Add(instantiateCard);
                
                CardInCollButtonExposer cardExposer = instantiateCard.GetComponent<CardInCollButtonExposer>();
                
                if (card.GroupsMonster != null)
                {
                    cardExposer.cardName.text = card.GroupsMonster.name;
                    cardExposer.cardCost.text = card.GroupsMonster.cost.ToString();
                    cardExposer.cardFamily.text = card.GroupsMonster.family.ToString();
                } 
                else if (card.Weapon != null)
                {
                    cardExposer.cardName.text = card.Weapon.equipmentName;
                    cardExposer.cardCost.text = card.Weapon.cost.ToString();
                    cardExposer.cardFamily.text = card.Weapon.type.ToString();
                }
                cardExposer.cardCopies.text = "Copies possédées : " + DataObject.CardList.GetNbSpecificCardInCollection(card.id);
                cardExposer.cardEffect.text = "Description de l'effet";
                cardExposer.cardButton.onClick.RemoveAllListeners();
                cardExposer.cardButton.onClick.AddListener(delegate
                {
                    PutCardInDeck(cardExposer.card);
                });
                cardExposer.card = card;
                
            }
            
            InitializeDeck();
        }
        
        private void InitializeDeck()
        {
            if (deckCards != null)
            {
                foreach (GameObject card in deckCards)
                {
                    card.SetActive(false);
                }
            }
            
            deckCards = new List<GameObject>();
            
            if (deckManagementMenu.selectedDeck != 0)
            {
                currentDeck = DataObject.CardList.GetDeckById(deckManagementMenu.selectedDeck);
                deckName.text = deckManagementMenu.selectedDeckName;
            }
            else
            {
                currentDeck = null;
                deckName.text = "";
            }

            if (!newDeck)
            {
                foreach (Card card in currentDeck.GetCardsInDeck())
                {
                    GameObject instantiateCard = Instantiate(cardInDeckPrefab, deckGrid.transform);
                    deckCards.Add(instantiateCard);
                    
                    CardInDeckButtonExposer cardExposer = instantiateCard.GetComponent<CardInDeckButtonExposer>();

                    if (card.GroupsMonster != null)
                    {
                        cardExposer.cardName.text = card.GroupsMonster.name;
                    } 
                    else if (card.Weapon != null)
                    {
                        cardExposer.cardName.text = card.Weapon.equipmentName;
                    }
                    cardExposer.card = card;
                    cardExposer.cardCopies.text = currentDeck.GetCardNumber(card.id).ToString();
                    cardExposer.cardButton.onClick.RemoveAllListeners();
                    cardExposer.cardButton.onClick.AddListener(delegate
                    {
                        RemoveCardFromDeck(cardExposer);
                    });
                }
            }
        }

        #endregion
        
        #region deck management functions
        private void RemoveCardFromDeck(CardInDeckButtonExposer cardExposer)
        {
            int cardCopies = Int32.Parse(cardExposer.cardCopies.text);
            if (cardCopies > 1)
            {
                cardExposer.cardCopies.text = (cardCopies - 1).ToString();
            }
            else
            {
                cards.Remove(cardExposer.gameObject);
                cardExposer.gameObject.SetActive(false);
            }
        }

        private void PutCardInDeck(Card card)
        {
            bool cardFound = false;
            foreach (GameObject cardButton in cards)
            {
                CardInDeckButtonExposer cardExposer = cardButton.GetComponent<CardInDeckButtonExposer>();
                if (cardExposer.card.id == card.id)
                {
                    cardFound = true;
                    int cardCopies = Int32.Parse(cardExposer.cardCopies.text);
                    if (cardCopies < 3)
                    {
                        cardExposer.cardCopies.text = (cardCopies + 1).ToString();
                    }
                }
            }

            //TODO : faire un scroll rect pour les cartes  à l'intérieur du deck 
            //en gros refaire la partie droite dans le (prefab du) menu
            if (!cardFound)
            {
                GameObject currentDeckCard = Instantiate(cardInDeckPrefab, deckGrid.transform);
                deckCards.Add(currentDeckCard);
                CardInDeckButtonExposer currentButtonExposer = currentDeckCard.GetComponent<CardInDeckButtonExposer>();

                if (card.GroupsMonster != null)
                {
                    currentButtonExposer.card = card;
                    currentButtonExposer.cardName.text = card.GroupsMonster.name;
                    currentButtonExposer.cardCopies.text = "1";
                    currentButtonExposer.cardButton.onClick.AddListener(delegate
                    {
                        RemoveCardFromDeck(currentButtonExposer);
                    });
                } else if (card.Weapon != null)
                {
                    currentButtonExposer.card = card;
                    currentButtonExposer.cardName.text = card.Weapon.equipmentName;
                    currentButtonExposer.cardCopies.text = "1";
                    currentButtonExposer.cardButton.onClick.AddListener(delegate
                    {
                        RemoveCardFromDeck(currentButtonExposer);
                    });
                }
            }
        }
        
        public IEnumerator AddNewDeck(string deckName, int isMonsterDeck)
        {
            WWWForm form = new WWWForm();
            form.AddField("deckOwner", NetworkingController.AuthToken);
            form.AddField("deckName", deckName);
            form.AddField("isMonsterDeck", isMonsterDeck);
            form.AddField("gameToken", NetworkingController.GameToken);
            var www = UnityWebRequest.Post(NetworkingController.PublicURL + "/services/game/deck/add.php", form);
            www.certificateHandler = new AcceptCertificate();
            yield return www.SendWebRequest();
            yield return new WaitForSeconds(0.5f);
            if (www.responseCode == 201)
            {
                currentDeck = new Deck();
                currentDeck.id = Int32.Parse(www.downloadHandler.text);
                Debug.Log("Deck Created!");
                if (currentDeck != null)
                {
                    StartCoroutine(DeleteCardsInDeck(currentDeck.id));
                }
            }
            else if (www.responseCode == 406)
            {
                Debug.Log("Erreur dans la création du deck");
            }
            else if (www.responseCode == 403)
            {
                Debug.Log("Erreur dans le formulaire");
            }
            else if (www.responseCode == 401)
            {
                Debug.Log("Vérifiez le GameToken");
            }
            else
            {
                Debug.Log(www.responseCode);
                Debug.Log(www.downloadHandler.text);
                Debug.Log("Serveur indisponible.");
            }
        }
        
        public IEnumerator DeleteCardsInDeck(int deckId)
        {
            WWWForm form = new WWWForm();
            form.AddField("deckId", deckId);
            form.AddField("gameToken", NetworkingController.GameToken);
            var www = UnityWebRequest.Post(NetworkingController.PublicURL + "/services/game/deck/deleteAllCardsInDeck.php", form);
            www.certificateHandler = new AcceptCertificate();
            yield return www.SendWebRequest();
            yield return new WaitForSeconds(0.5f);
            if (www.responseCode == 201)
            {
                Debug.Log("Suppression des cartes effectuée");
                foreach (GameObject cardButton in cards)
                {
                    CardInDeckButtonExposer currentExposer = cardButton.GetComponent<CardInDeckButtonExposer>();
                    StartCoroutine(AddNewCardInDeck(currentDeck.id,currentExposer.card.id,Int32.Parse(currentExposer.cardCopies.text)));
                }
            }
            else if (www.responseCode == 406)
            {
                Debug.Log("Erreur dans la suppression des cartes");
            }
            else if (www.responseCode == 403)
            {
                Debug.Log("Erreur dans le formulaire");
            }
            else if (www.responseCode == 401)
            {
                Debug.Log("Vérifiez le GameToken");
            }
            else
            {
                Debug.Log(www.responseCode);
                Debug.Log(www.downloadHandler.text);
                Debug.Log("Serveur indisponible.");
            }
        }
        
        public IEnumerator AddNewCardInDeck(int deckId, int cardId, int nbCards)
        {
            WWWForm form = new WWWForm();
            form.AddField("deckId", deckId);
            form.AddField("cardId", cardId);
            form.AddField("numberOfCards", nbCards);
            form.AddField("gameToken", NetworkingController.GameToken);
            var www = UnityWebRequest.Post(NetworkingController.PublicURL + "/services/game/deck/addCardInDeck.php", form);
            www.certificateHandler = new AcceptCertificate();
            yield return www.SendWebRequest();
            yield return new WaitForSeconds(0.1f);
            if (www.responseCode == 201)
            {
                Debug.Log("Carte ajoutée");
                cardCounter++;
            }
            else if (www.responseCode == 406)
            {
                Debug.Log("Erreur dans l'ajout de la carte");
            }
            else if (www.responseCode == 403)
            {
                Debug.Log("Erreur dans le formulaire");
            }
            else if (www.responseCode == 401)
            {
                Debug.Log("Vérifiez le GameToken");
            }
            else
            {
                Debug.Log(www.responseCode);
                Debug.Log(www.downloadHandler.text);
                Debug.Log("Serveur indisponible.");
            }
        }

        private void SaveDeck()
        {
            bool containsMonsters = false;
            bool containsEquipments = false;
            totalDistinctCardNumber = 0;
            cardCounter = 0;
            foreach (GameObject cardButton in cards)
            {
                CardInDeckButtonExposer currentExposer = cardButton.GetComponent<CardInDeckButtonExposer>();
                if (currentExposer.card.GroupsMonster != null)
                {
                    containsMonsters = true;
                }
                else
                {
                    containsEquipments = true;
                }
                totalDistinctCardNumber++;
            }
            if (containsMonsters ^ containsEquipments)
            {
                Debug.Log(containsEquipments+ " "+containsMonsters);
                if (currentDeck == null)
                {
                    StartCoroutine(containsMonsters ? AddNewDeck(deckName.text, 1) : AddNewDeck(deckName.text, 0));
                }
                else
                {
                    StartCoroutine(DeleteCardsInDeck(currentDeck.id));
                }
                StartCoroutine(WaitEndCreation());
            }
        }

        private IEnumerator WaitEndCreation()
        {
            while (cardCounter < totalDistinctCardNumber)
            {
                Debug.Log("i wait");
                yield return new WaitForSeconds(0.1f);
            }

            DataObject.CardList.decks = new List<Deck>();
            reloadingDecks = true;
            StartCoroutine(ReloadDecks());
            while (reloadingDecks)
            {
                yield return new WaitForSeconds(0.1f);
            }
            mc.ActivateMenu(MenuController.Menu.DeckManagement);
        }

        private IEnumerator ReloadDecks()
        {
            WWWForm form = new WWWForm();
            if (NetworkingController.AuthToken != "")
            {
                form.AddField("deckOwner", NetworkingController.AuthToken);
            }
            else
            {
                form.AddField("deckOwner", "HZ0PUiJjDly8EDkyYUiP");
            }
            
            var www = UnityWebRequest.Post(NetworkingController.PublicURL + "/services/game/card/listCardDeck.php", form);
            www.certificateHandler = new AcceptCertificate();
            yield return www.SendWebRequest();
            yield return new WaitForSeconds(0.5f);
            if (www.responseCode == 200)
            {
                DataObject.CardList.InitDeck(www.downloadHandler.text);
                reloadingDecks = false;
            }
            else
            {
                Debug.Log("Can't get Cards decks...");
            }
        }

        private void EditDeckName()
        {
            deckNameStatic.SetActive(false);
            deckNameDynamic.SetActive(true);
        }

        public void EditDeckName(string newName)
        {
            deckName.text = newName;
        }

        private void SaveDeckName()
        {
            deckNameStatic.SetActive(true);
            deckNameDynamic.SetActive(false);
        }
        #endregion
    }
}