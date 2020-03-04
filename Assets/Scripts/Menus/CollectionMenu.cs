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

        private void Start()
        {
            returnButton.onClick.AddListener(delegate
            {
                mc.ActivateMenu(MenuController.Menu.DeckManagement);
            });
        }

        public void InitMenu()
        {
            Debug.Log("Collection Menu");
        }
    }
}