using System.Collections.Generic;
using Games.Global;
using UnityEngine;
using UnityEngine.Serialization;
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
            //if (cards != null)
            //{
            //    foreach (GameObject card in cards)
            //    {
            //        card.SetActive(false);
            //    }
            //}

            cards = new List<GameObject>();

            foreach (Card card in DataObject.CardList.GetCardsInCollection())
            {
                GameObject instantiateCard = Instantiate(cardPrefab, collectionGrid.transform);
                cards.Add(instantiateCard);
            }
        }
        #endregion
    }
}
