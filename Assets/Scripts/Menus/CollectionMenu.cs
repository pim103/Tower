using System.Collections.Generic;
using DeckBuilding;
using Games.Global;
using Games.Global.Entities;
using Networking.Client;
using UnityEngine;
using UnityEngine.UI;

namespace Menus
{
    public class CollectionMenu : MonoBehaviour, MenuInterface
    {
        [SerializeField]
        private MenuController mc;

        [SerializeField]
        private Button returnButton;

        [SerializeField] 
        private GameObject cardInCollButton;
        
        private List<GameObject> cardInCollButtonsList;

        public int minBorder;
        public int maxBorder;
        private void Start()
        {
            returnButton.onClick.AddListener(delegate
            {
                mc.ActivateMenu(MenuController.Menu.DeckManagement);
            });
        }

        public void InitMenu()
        {
            minBorder = 0;
            maxBorder = 15;
            ShowCards();
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
                    currentButtonExposer.cardName.text = card.GroupsMonster.name;
                    currentButtonExposer.cardCopies.text = "X" + DataObject.CardList.GetNbSpecificCardInCollection(card.id);
                    currentButtonExposer.cardEffect.text = "effet";
                    currentButtonExposer.cardCost.text = card.GroupsMonster.cost.ToString();
                    currentButtonExposer.cardFamily.text = card.GroupsMonster.family.ToString();
                    currentCardInCollButton.GetComponent<Button>().onClick.AddListener(delegate
                    {
                    
                    });
                } else if (card.Weapon != null)
                {
                    currentButtonExposer.cardName.text = card.Weapon.equipmentName;
                    currentButtonExposer.cardCopies.text = "X" + DataObject.CardList.GetNbSpecificCardInCollection(card.id);
                    currentButtonExposer.cardEffect.text = "effet";
                    currentButtonExposer.cardCost.text = card.Weapon.cost.ToString();
                    currentButtonExposer.cardFamily.text = card.Weapon.type.ToString();
                    currentCardInCollButton.GetComponent<Button>().onClick.AddListener(delegate
                    {
                    
                    });
                }
            }
        }
    }
}