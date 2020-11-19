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

        [SerializeField]
        public Text maxResourceText;

        [SerializeField] 
        public Text currentResourceText;

        [SerializeField] 
        public GameObject keyObject;

        [SerializeField] 
        private GameObject[] mobCardContainers;
        
        [SerializeField] 
        private GameObject[] equipementCardContainers;

        public int currentWallNumber;
        private int currentWallType;
        public bool keyAlreadyPut;

        private Deck monsterDeck;
        private Deck weaponDeck;

        private void Start()
        {
            wallButton.onClick.AddListener(PutWallInHand);
            keyButton.onClick.AddListener(PutKeyInHand);
            foreach (var button in trapButtons)
            {
                button.transform.GetChild(0).GetComponent<Text>().text =
                    button.gameObject.GetComponent<TrapBehavior>().trapData.mainType.ToString();
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
            
            // TODO : set currentWallNumber (in GameGrid)
            currentWallNumber = 30;
            wallButtonText.text = "Mur x" + currentWallNumber;

            if (monsterDeck == null)
            {
                monsterDeck = DataObject.CardList.GetDeckById(ChooseDeckAndClass.monsterDeckId);
            }

            if (weaponDeck == null)
            {
                weaponDeck = DataObject.CardList.GetDeckById(ChooseDeckAndClass.equipmentDeckId);
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
                trap.GetComponent<TrapBehavior>().SetAndActiveTraps(trapBehavior.trapData);
                hoverDetector.objectInHand = trap;
                trap.SetActive(true);
                hoverDetector.currentResource -= 1;
            }
        }

        public void DrawCards()
        {
            foreach (var cardContainer in mobCardContainers)
            {
                if (cardContainer.transform.childCount == 0)
                {
                    Card selectedCard = monsterDeck.DrawRandomCard();

                    if (selectedCard == null)
                    {
                        Debug.Log("Pas plus de cartes");
                    }
                    
                    InitCard(cardContainer, selectedCard);
                }
            }
            foreach (var cardContainer in equipementCardContainers)
            {
                if (cardContainer.transform.childCount == 0)
                {
                    Card selectedCard = weaponDeck.DrawRandomCard();

                    if (selectedCard == null)
                    {
                        Debug.Log("Pas plus de cartes");
                    }
                    Debug.Log(selectedCard.id);
                    Debug.Log(selectedCard.GroupsMonster);
                    Debug.Log(selectedCard.Weapon);

                    InitCard(cardContainer, selectedCard);
                }
            }
        }

        private void InitCard(GameObject cardContainer, Card cardStats)
        {
            GameObject card = Instantiate(cardPrefab, cardContainer.transform, true);
            card.transform.localPosition = new Vector3(0,0,0);
            card.transform.localEulerAngles = new Vector3(0,0,0);
            card.GetComponent<CardBehaviorInGame>().SetCard(cardStats);
        }
    }
}
