using System.Collections.Generic;
using DeckBuilding;
using Games.Global;
using UnityEngine;
using UnityEngine.UI;

//Author : Attika

namespace Menus
{
    public class CollectionAndCraftMenu : MonoBehaviour, MenuInterface
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
        
        #region craft parameters
        [SerializeField] 
        private GameObject craftVisualizer;
        [SerializeField] 
        private Button returnFromCraftButton;
        [SerializeField]
        private CardInCraftButtonExposer cardInCraftExposer;
        #endregion
        
        #region start and intialization functions
        private void Start()
        {
            returnButton.onClick.AddListener(delegate
            {
                mc.ActivateMenu(MenuController.Menu.MainMenu);
            });
        }
        
        public void InitMenu()
        {
            Debug.Log("CollectionAndCraft Menu");
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
                    cardExposer.cardCopies.text = "Copies possédées : " + DataObject.CardList.GetNbSpecificCardInCollection(card.id);
                    cardExposer.cardEffect.text = "Description de l'effet";
                    cardExposer.cardCost.text = card.GroupsMonster.cost.ToString();
                    cardExposer.cardFamily.text = card.GroupsMonster.family.ToString();
                    cardExposer.cardButton.onClick.RemoveAllListeners();
                    cardExposer.cardButton.onClick.AddListener(delegate { OpenCraft(card); });
                } 
                else if (card.Weapon != null)
                {
                    cardExposer.cardName.text = card.Weapon.equipmentName;
                    cardExposer.cardCopies.text = "Copies possédées : " + DataObject.CardList.GetNbSpecificCardInCollection(card.id);
                    cardExposer.cardEffect.text = "Description de l'effet";
                    cardExposer.cardCost.text = card.Weapon.cost.ToString();
                    cardExposer.cardFamily.text = card.Weapon.type.ToString();
                    cardExposer.cardButton.onClick.RemoveAllListeners();
                    cardExposer.cardButton.onClick.AddListener(delegate { OpenCraft(card); });
                }
            }
        }
        #endregion
        
        
        #region craft functions

        private void OpenCraft(Card card)
        {
            if (craftVisualizer) craftVisualizer.SetActive(true);
            
            returnFromCraftButton.onClick.RemoveAllListeners();
            returnFromCraftButton.onClick.AddListener(CloseCraft);
            
            if (card.GroupsMonster != null)
            {
                cardInCraftExposer.cardName.text = card.GroupsMonster.name;
                cardInCraftExposer.cardCost.text = card.GroupsMonster.cost.ToString();
                cardInCraftExposer.cardFamily.text = card.GroupsMonster.family.ToString();
            } 
            else if (card.Weapon != null)
            {
                cardInCraftExposer.cardName.text = card.Weapon.equipmentName;
                cardInCraftExposer.cardCost.text = card.Weapon.cost.ToString();
                cardInCraftExposer.cardFamily.text = card.Weapon.type.ToString();
            }
            
            cardInCraftExposer.cardCopies.text = "Copies possédées : " + DataObject.CardList.GetNbSpecificCardInCollection(card.id);
            cardInCraftExposer.cardEffect.text = "Description de l'effet";
            cardInCraftExposer.cardButton.onClick.RemoveAllListeners();
            cardInCraftExposer.cardButton.onClick.AddListener(CloseCraft);
            cardInCraftExposer.craftButton.onClick.RemoveAllListeners();
            cardInCraftExposer.craftButton.onClick.AddListener(delegate { Craft(card); });
        }

        private void Craft(Card card)
        {
            
        }

        private void CloseCraft()
        {
            if (craftVisualizer) craftVisualizer.SetActive(false);
        }
        
        #endregion
    }
}
