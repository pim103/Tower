using System;
using Games.Global;
using Menus;
using UnityEngine;
using UnityEngine.UI;

namespace DeckBuilding
{
    public class ScrollButtonController : MonoBehaviour
    {
        //IL FAUT FUSIONNER LES MENUS
        [SerializeField] private CollectionMenu collectionMenu;
        [SerializeField] private bool isPrevious;
        [SerializeField] private CreateDeckMenu createDeckMenu;
        [SerializeField] private bool onCollection;
        private void Start()
        {
            GetComponent<Button>().onClick.AddListener(delegate
            {
                if (isPrevious)
                {
                    if (onCollection)
                    {
                        if (collectionMenu.minBorder >= 15)
                        {
                            collectionMenu.minBorder -= 15;
                            collectionMenu.maxBorder -= 15;
                            collectionMenu.ShowCards();
                        }
                    }
                    else
                    {
                        if (createDeckMenu.minBorder >= 15)
                        {
                            createDeckMenu.minBorder -= 15;
                            createDeckMenu.maxBorder -= 15;
                            createDeckMenu.ShowCards();
                        }
                    }
                }
                else
                {
                    /*
                    if (onCollection)
                    {
                        if (collectionMenu.maxBorder < DataObject.CardList.GetTotalDistinctCardsInCollection())
                        {
                            collectionMenu.minBorder += 15;
                            collectionMenu.maxBorder += 15;
                            collectionMenu.ShowCards();
                        }
                    }
                    else
                    {
                        if (createDeckMenu.maxBorder < DataObject.playerCollection.Count)
                        {
                            createDeckMenu.minBorder += 15;
                            createDeckMenu.maxBorder += 15;
                            createDeckMenu.ShowCards();
                        }
                    }*/
                }
            });
        }
    }
}
