using UnityEngine;
using UnityEngine.UI;

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

        private void Start()
        {
            collectionButton.onClick.AddListener(delegate {
                mc.ActivateMenu(MenuController.Menu.Collection);
            });

            createDeckButton.onClick.AddListener(delegate {
                mc.ActivateMenu(MenuController.Menu.CreateDeck);
            });

            editDeckButton.onClick.AddListener(delegate {
                mc.ActivateMenu(MenuController.Menu.CreateDeck);
            });

            returnButton.onClick.AddListener(delegate
            {
                mc.ActivateMenu(MenuController.Menu.MainMenu);
            });
        }

        public void InitMenu()
        {
            Debug.Log("Deck Management Menu");
        }
    }
}