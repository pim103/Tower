using System;
using Networking.Client;
using UnityEngine;
using UnityEngine.UI;

namespace Menus
{
    public class MainMenu : MonoBehaviour, MenuInterface
    {
        [SerializeField]
        private MenuController mc;

        [SerializeField]
        private Button playButton;

        [SerializeField]
        private Button craftButton;

        [SerializeField]
        private Button deckButton;

        [SerializeField]
        private Button settingsButton;

        [SerializeField]
        private Button profilButton;

        [SerializeField]
        private Button shopButton;

        [SerializeField]
        private Button quitButton;

        private void Start()
        {
            playButton.onClick.AddListener(delegate {
                mc.ActivateMenu(MenuController.Menu.Play);
            });

            craftButton.onClick.AddListener(delegate {
                mc.ActivateMenu(MenuController.Menu.Craft);
            });

            deckButton.onClick.AddListener(delegate {
                mc.ActivateMenu(MenuController.Menu.DeckManagement);
            });

            settingsButton.onClick.AddListener(delegate {
                mc.ActivateMenu(MenuController.Menu.Settings);
            });
            /*
            profilButton.onClick.AddListener(delegate {
                mc.ActivateMenu(MenuController.Menu.Profile);
            });
            */
            shopButton.onClick.AddListener(delegate {
                mc.ActivateMenu(MenuController.Menu.Shop);
            });

            quitButton.onClick.AddListener(delegate
            {
                TowersWebSocket.CloseConnection();
                mc.QuitGame();
            });
        }

        public void InitMenu()
        {
            Debug.Log("Main Menu");
        }
    }
}