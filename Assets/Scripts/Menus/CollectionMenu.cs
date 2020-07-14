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
            for(int i = minBorder; i < maxBorder; i++){
                if(i>=DataObject.playerCollection.Count) return;
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
                GroupsMonster group = DataObject.MonsterList.GetGroupsMonsterById(DataObject.playerCollection[i].id);
                currentButtonExposer.name.text = group.name;
                //currentButtonExposer.copies.text = "X" + DataObject.playerCollection[i].copies;
                currentButtonExposer.effect.text = "effet";
                currentButtonExposer.cost.text = group.cost.ToString();
                currentButtonExposer.family.text = group.family.ToString();
                currentCardInCollButton.GetComponent<Button>().onClick.AddListener(delegate
                {
                    
                });
                
            }
        }
    }
}