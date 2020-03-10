using UnityEngine;
using UnityEngine.UI;

namespace Menus
{
    public class CreateDeckMenu : MonoBehaviour, MenuInterface
    {
        [SerializeField]
        private MenuController mc;

        [SerializeField]
        private Button saveDeckButton;

        [SerializeField]
        private Button returnButton;

        private void Start()
        {
            saveDeckButton.onClick.AddListener(delegate {
                mc.ActivateMenu(MenuController.Menu.DeckManagement);
            });

            returnButton.onClick.AddListener(delegate
            {
                mc.ActivateMenu(MenuController.Menu.DeckManagement);
            });
        }

        public void InitMenu()
        {
            Debug.Log("Create Deck Menu");
        }
    }
}