using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Menus
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
            });

            quitButton.onClick.AddListener(mc.QuitGame);
        }

        public void InitMenu()
        {
            mc.DisconnectToPhoton();
            Debug.Log("Connection Menu");
            //throw new System.NotImplementedException();
        }
    }
}