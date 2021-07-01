using System;
using System.Collections;
using System.Collections.Generic;
using FullSerializer;
using Games.Defenses;
using Games.Players;
using Games.Transitions;
using Networking;
using Networking.Client;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Utils;

namespace Menus
{   
    public class MenuController : MonoBehaviour
    {
        [SerializeField]
        private GameObject[] menus;

        [SerializeField] 
        private string environnement;
        [SerializeField] 
        private string staticRoomId;

        [SerializeField]
        private TransitionMenuGame transitionMenuGame;

        [SerializeField] private GameObject searchingMatchCanvas;
        [SerializeField] private Button cancelResearchMatchButton;

        private string canStart;
        private Coroutine searchingMatch;
        
        public enum Menu
        {
            Connection,
            Registration,
            MainMenu,
            Play,
            PrivateMatch,
            CreateRoom,
            ListingPlayer,
            Craft,
            DeckManagement,
            Collection,
            CreateDeck,
            Settings,
            Shop
        };

        private void Start()
        {
            searchingMatchCanvas.active = false;
            if (NetworkingController.AuthToken == "")
            {
                ActivateMenu(Menu.Connection);
                NetworkingController.CurrentRoomToken = staticRoomId;
                NetworkingController.Environnement = environnement;
            }
            else
            {
                TowersWebSocket.StartConnection();
                //ActivateMenu(Menu.MainMenu);
            }

            StartCoroutine(CheckForDeconnection());

            cancelResearchMatchButton.onClick.AddListener(CancelResearchMatch);
        }

        public void ActivateMenu(Menu menuIndex)
        {
            int index = (int)menuIndex;

            DesactiveMenus();
            MenuInterface mi = menus[index].GetComponent<MenuInterface>();

            menus[index].SetActive(true);
            mi.InitMenu();
        }
        
        public IEnumerator CheckForDeconnection()
        {
            while (NetworkingController.ConnectionStart == false)
            {
                yield return new WaitForSeconds(0.1f);
            }
            TowersWebSocket.wsGame.OnMessage += (sender, args) =>
            {
                if (args.Data.Contains("callbackMessages"))
                {
                    fsSerializer serializer = new fsSerializer();
                    fsData data;
                    CallbackMessages callbackMessage = null; 
                    try
                    {
                        data = fsJsonParser.Parse(args.Data);
                        serializer.TryDeserialize(data, ref callbackMessage);
                        callbackMessage = Tools.Clone(callbackMessage);
                        
                        if (callbackMessage.callbackMessages.message == "Identity Set")
                        {
                            NetworkingController.IsConnected = true;
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.Log("Can't read callback : " + e.Message);
                    }
                }
            };
            while (NetworkingController.IsConnected == false)
            {
                yield return new WaitForSeconds(0.5f);
            }

            TowersWebSocket.wsGame.OnClose += (sender, args) =>
            {
                if (args.Code != 1000 || args.Code != 1005)
                {
                    Debug.Log("Deconnexion : " + args.Code);
                    NetworkingController.AuthToken = "";
                    NetworkingController.CurrentRoomToken = "";
                    NetworkingController.AuthRole = "";
                    NetworkingController.IsConnected = false;
                    NetworkingController.ConnectionClosed = args.Code;
                    NetworkingController.ConnectionStart = false;
                    SceneManager.LoadScene("MenuScene");
                }
            };
        }

        private void DesactiveMenus()
        {
            foreach (GameObject menu in menus)
            {
                menu.SetActive(false);
            }
        }

        public void QuitGame()
        {
            Application.Quit();
        }

        public void SearchMatch()
        {
            searchingMatchCanvas.active = true;
            NetworkingController.CurrentRoomToken = "MatchmakingWaitinglist";

            var setSocket = new Dictionary<string, string>();
            setSocket.Add("tokenPlayer", NetworkingController.AuthToken);
            setSocket.Add("room", NetworkingController.CurrentRoomToken);
            TowersWebSocket.TowerSender("SELF", NetworkingController.CurrentRoomToken,"null", "joinWaitingRanked", TowersWebSocket.FromDictToString(setSocket));
            TowersWebSocket.wsGame.OnMessage += (sender, args) =>
            {
                if (args.Data.Contains("callbackMessages"))
                {
                    fsSerializer serializer = new fsSerializer();
                    fsData data;
                    CallbackMessages callbackMessage = null; 
                    try
                    {
                        data = fsJsonParser.Parse(args.Data);
                        serializer.TryDeserialize(data, ref callbackMessage);
                        callbackMessage = Tools.Clone(callbackMessage);
                        if (callbackMessage.callbackMessages.room != null && callbackMessage.callbackMessages.message == "MatchStart")
                        {
                            NetworkingController.CurrentRoomToken = callbackMessage.callbackMessages.room;
                            canStart = args.Data;
                        } else if (callbackMessage.callbackMessages.message == "LeavingMatchmaking")
                        {
                            NetworkingController.CurrentRoomToken = "GENERAL";
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.Log("Can't read callback : " + e.Message);
                    }
                }
            };

            searchingMatch = StartCoroutine(WaitingForCanStart());
        }

        private IEnumerator WaitingForCanStart()
        {
            yield return new WaitForSeconds(2f);
            while (canStart == null)
            {
                Debug.Log("Waiting");
                TowersWebSocket.TowerSender("SELF", NetworkingController.CurrentRoomToken,"null", "getRankedMatch", "null");
                yield return new WaitForSeconds(5f);
            }

            transitionMenuGame.InitGame();
        }

        private void CancelResearchMatch()
        {
            PlayerInMenu.isInMenu = false;
            Cursor.lockState = CursorLockMode.Locked;
            searchingMatchCanvas.active = false;
            var setSocket = new Dictionary<string, string>();
            setSocket.Add("tokenPlayer", NetworkingController.AuthToken);
            setSocket.Add("room", NetworkingController.CurrentRoomToken);
            TowersWebSocket.TowerSender("SELF", NetworkingController.CurrentRoomToken,"null", "quitMatchmaking", TowersWebSocket.FromDictToString(setSocket));

            if (searchingMatchCanvas != null)
            {
                StopCoroutine(searchingMatch);
            }
        }
    }
}