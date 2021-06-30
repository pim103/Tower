using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DeckBuilding;
using Games.Global;
using Games.Players;
using Networking;
using Networking.Client;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Utils;

namespace Menus
{
    public class DeckManagementMenu : MonoBehaviour, MenuInterface
    {
        
        #region all parameters
        [SerializeField]
        private MenuController mc;
        [SerializeField]
        private Button returnButton;
        [SerializeField] 
        private CreateDeckMenu createDeckMenu;
        #endregion

        #region deck management parameters
        [SerializeField]
        private Button collectionButton;
        [SerializeField]
        private Button createDeckButton;
        [SerializeField]
        private Button editDeckButton;
        [SerializeField] 
        private GameObject deckPrefab;
        [SerializeField] 
        private Button deleteButton;
        [SerializeField] 
        private GameObject deckGrid;
        private List<GameObject> decks;
        [HideInInspector] public int selectedDeck;
        [HideInInspector] public string selectedDeckName;
        #endregion
       
        #region start and initialization functions
        private void Start()
        {
            collectionButton.onClick.AddListener(delegate {
                mc.ActivateMenu(MenuController.Menu.Collection);
            });

            createDeckButton.onClick.AddListener(delegate
            {
                createDeckMenu.newDeck = true;
                selectedDeck = 0;
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
            
            deleteButton.onClick.AddListener(delegate
            {
                if (selectedDeck != 0)
                {
                    StartCoroutine(DeleteDeck(selectedDeck));
                }
            });
        }

        public void InitMenu()
        {
            PlayerInMenu.isInMenu = true;
            Cursor.lockState = CursorLockMode.None;
            selectedDeck = 0;

            InitializeDecks();
            Debug.Log("Deck Management Menu");
        }

        private void InitializeDecks()
        {
            if (decks != null)
            {
                foreach (var deck in decks)
                {
                    deck.SetActive(false);
                }
            }
            
            selectedDeck = 0;
            decks = new List<GameObject>();
            
            foreach (var deck in DataObject.CardList.GetDecks())
            {
                GameObject currentDeck = Instantiate(deckPrefab, deckGrid.transform);
                decks.Add(currentDeck);
                
                DeckButtonExposer currentDeckExposer = currentDeck.GetComponent<DeckButtonExposer>();
                currentDeckExposer.deckName.text = deck.name;
                currentDeckExposer.typeImage.color = deck.type == Decktype.Monsters ? Color.red : Color.blue;
                currentDeckExposer.deckId = deck.id;
                currentDeckExposer.deckButton.onClick.AddListener(delegate
                {
                    selectedDeck = deck.id;
                    selectedDeckName = deck.name;
                });
            }
        }
        #endregion
        
        #region deck management functions
        public IEnumerator DeleteDeck(int deckId)
        {
            WWWForm form = new WWWForm();
            form.AddField("deckId", deckId);
            form.AddField("gameToken", NetworkingController.GameToken);
            var www = UnityWebRequest.Post(NetworkingController.PublicURL + "/services/game/deck/delete.php", form);
            www.certificateHandler = new AcceptCertificate();
            yield return www.SendWebRequest();
            yield return new WaitForSeconds(0.5f);
            if (www.responseCode == 201)
            {
                Debug.Log("Suppression des cartes effectuée");
                foreach (GameObject deck in decks)
                {
                    if (deck.GetComponent<DeckButtonExposer>().deckId == deckId)
                    {
                        deck.SetActive(false);
                    }
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
        #endregion
    }
}