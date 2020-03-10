using UnityEngine;
using UnityEngine.UI;

namespace Menus
{
    public class ConnectionMenu : MonoBehaviour, MenuInterface
    {
        [SerializeField]
        private MenuController mc;

        [SerializeField]
        private InputField loginField;

        [SerializeField]
        private InputField passwordField;

        [SerializeField]
        private Button connectButton;

        [SerializeField]
        private Button createButton;

        [SerializeField]
        private Button quitButton;

        private void Start()
        {
            createButton.onClick.AddListener(delegate {
                mc.ActivateMenu(MenuController.Menu.Registration);
            });

            connectButton.onClick.AddListener(delegate {
                mc.ActivateMenu(MenuController.Menu.MainMenu);
                mc.networking.InitializeWebsocketEndpoint();
                mc.networking.StartConnection();
            });

            quitButton.onClick.AddListener(mc.QuitGame);
        }

        public void InitMenu()
        {
            Debug.Log("Connection Menu");
        }
    }
}