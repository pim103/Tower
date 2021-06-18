using Games.Players;
using UnityEngine;
using UnityEngine.UI;

namespace Menus
{
    public class CraftMenu : MonoBehaviour, MenuInterface
    {
        [SerializeField]
        private MenuController mc;

        [SerializeField]
        private Button returnButton;

        private void Start()
        {
            returnButton.onClick.AddListener(delegate
            {
                mc.ActivateMenu(MenuController.Menu.MainMenu);
            });
        }

        public void InitMenu()
        {
            PlayerInMenu.isInMenu = true;
            Cursor.lockState = CursorLockMode.None;
            Debug.Log("Craft Menu");
        }
    }
}