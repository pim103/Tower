﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Menus
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

            quitButton.onClick.AddListener(mc.QuitGame);
        }

        public void InitMenu()
        {
            mc.ConnectToPhoton();
            Debug.Log("Main Menu");
        }
    }
}