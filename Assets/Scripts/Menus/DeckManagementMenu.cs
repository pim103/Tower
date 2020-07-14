using System.Collections.Generic;
using System.IO;
using System.Linq;
using DeckBuilding;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace Menus
{
    public class DeckManagementMenu : MonoBehaviour, MenuInterface
    {
        [SerializeField]
        private MenuController mc;

        [SerializeField]
        private Button collectionButton;

        [SerializeField]
        private Button createDeckButton;

        [SerializeField]
        private Button editDeckButton;

        [SerializeField]
        private Button returnButton;

        [SerializeField] 
        private GameObject deckButton;

        [SerializeField] 
        private CreateDeckMenu createDeckMenu;
        
        private List<GameObject> deckButtonList;
        public List<Deck> playerDecks;

        public int selectedDeck;
        public string selectedDeckName;
        private void Start()
        {
            collectionButton.onClick.AddListener(delegate {
                mc.ActivateMenu(MenuController.Menu.Collection);
            });

            createDeckButton.onClick.AddListener(delegate
            {
                createDeckMenu.newDeck = true;
                mc.ActivateMenu(MenuController.Menu.CreateDeck);
            });

            editDeckButton.onClick.AddListener(delegate {
                if (selectedDeck != 0)
                {
                    createDeckMenu.newDeck = false;
                    mc.ActivateMenu(MenuController.Menu.CreateDeck);
                }
            });

            returnButton.onClick.AddListener(delegate
            {
                mc.ActivateMenu(MenuController.Menu.MainMenu);
            });
        }

        public void InitMenu()
        {
            selectedDeck = 0;
            if (playerDecks == null)
            {
                playerDecks = new List<Deck>();
                deckButtonList = new List<GameObject>();
                FetchDecks();
                ShowDecks();
            }
            Debug.Log("Deck Management Menu");
        }

        private void FetchDecks()
        {
            List<DeckJsonObject> dJsonObjects = new List<DeckJsonObject>();

            foreach (string filePath in Directory.EnumerateFiles("Assets/Data/DeckJson"))
            {
                StreamReader reader = new StreamReader(filePath, true);
        
                //dJsonObjects.AddRange(ParserJson<DeckJsonObject>.Parse(reader, "decks"));
            }

            foreach (DeckJsonObject deckJson in dJsonObjects)
            {
                Deck loadedDeck = deckJson.ConvertToDeck();
                playerDecks.Add(loadedDeck);
            }
        }

        private void ShowDecks()
        {
            int ycount = 0;
            foreach (var deck in playerDecks)
            {
                int currentCount = deckButtonList.Count;
                GameObject currentDeckButton = Instantiate(deckButton, transform);
                deckButtonList.Add(currentDeckButton);
                if (currentCount % 4 == 0)
                {
                    ycount += 1;
                }

                Vector3 currentPosition = currentDeckButton.transform.position;
                currentDeckButton.transform.position = new Vector3(currentPosition.x + 150 * (currentCount % 4),
                    currentPosition.y - (120 * (ycount - 1)), 0);
                DeckButtonExposer currentButtonExposer = currentDeckButton.GetComponent<DeckButtonExposer>();
                currentButtonExposer.deckName.text = deck.name;
                currentButtonExposer.typeImage.color = deck.type == Decktype.Monsters ? Color.red : Color.blue;
                currentDeckButton.GetComponent<Button>().onClick.AddListener(delegate
                {
                    selectedDeck = deck.id;
                    selectedDeckName = deck.name;
                });
                //currentButtonExposer.deckImage a set
            }
        }
    }
}