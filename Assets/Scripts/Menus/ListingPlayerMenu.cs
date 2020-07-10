using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using FullSerializer;
using Games.Transitions;
using Networking;
using Networking.Client;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace Menus
{
    public class ListingPlayerMenu : MonoBehaviour, MenuInterface
    {
        [SerializeField]
        private MenuController mc;

        [SerializeField]
        private Button readyButton;

        [SerializeField]
        private Button returnButton;

        [SerializeField]
        private GameObject playerCase;

        [SerializeField]
        private RectTransform contentListPlayer;

        private string canStart = null;

        private Dictionary<string, GameObject> listPlayerCase;
        private Dictionary<string, bool> listPlayerIsReady;
        private CallbackMessages callbackHandlers;

        private void Start()
        {
            returnButton.onClick.AddListener(ReturnAction);
            readyButton.onClick.AddListener(SetReadyAction);
        }

        private void ReturnAction()
        {
            mc.ActivateMenu(MenuController.Menu.PrivateMatch);
        }

        private void SetReadyAction()
        {
            
        }

        /* ============================== INTERFACE ============================== */

        public void InitMenu()
        {
        }
    }
}