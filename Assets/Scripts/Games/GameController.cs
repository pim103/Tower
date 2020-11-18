using System;
using System.Collections;
using FullSerializer;
using Games.Defenses;
using Games.Global;
using Games.Transitions;
using Networking;
using Networking.Client;
using Networking.Client.Room;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Utils;

namespace Games {
    public class GameController : MonoBehaviour
    {
        [SerializeField]
        private ObjectsInScene objectsInScene;

        [SerializeField]
        private TransitionMenuGame transitionMenuGame;

        [SerializeField] private Text endGameText;

        [SerializeField]
        private ScriptsExposer se;
        [SerializeField] 
        private string endPoint;
        [SerializeField] 
        private string roomId;

        [SerializeField] private GameObject endGameMenu;
        [SerializeField] private Button backToMenu;

        public static GameObject mainCamera;

        public static string staticRoomId;
        
        private string canStart = null;

        public static int PlayerIndex;

        private bool idAssigned = false;

        private bool otherPlayerDie = false;
        private CallbackMessages callbackHandlers;

        public static GameGrid currentGameGrid;

        /*
         * Flag to skip defensePhase
         */
        public bool byPassDefense = true;

        public static string mapReceived;

        private IEnumerator CheckEndInit()
        {
            yield return new WaitForSeconds(0.1f);
            transitionMenuGame.WantToStartGame();
        }

        private IEnumerator WaitingDeathOtherPlayer()
        {
            while (!otherPlayerDie)
            {
                yield return new WaitForSeconds(1f);
            }

            Cursor.lockState = CursorLockMode.None;
            DataObject.playerInScene.Remove(PlayerIndex);
            objectsInScene.mainCamera.SetActive(true);
            endGameMenu.SetActive(true);
        }

        private void LoadMainMenu()
        {
            SceneManager.LoadScene("MenuScene");
        }
        
        // ================================== BASIC METHODS ======================================

        // Start is called before the first frame update
        void Start()
        {
            mainCamera = objectsInScene.mainCamera;
            endGameMenu.SetActive(false);
            backToMenu.onClick.AddListener(LoadMainMenu);
            staticRoomId = roomId;

            objectsInScene.mainCamera.SetActive(true);
            
            // TODO : change index
            PlayerIndex = 0;
            if (TowersWebSocket.wsGame != null)
            {
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
                            if (callbackMessage.callbackMessages.message == "WON")
                            {
                                CurrentRoom.loadGameDefense = false;
                                CurrentRoom.loadGameAttack = false;
                                Debug.Log("Un autre joueur a gagné");
                                otherPlayerDie = true;
                                endGameText.text = "Vous avez perdu...";
                            }
                            if (callbackMessage.callbackMessages.message == "DEATH")
                            {
                                CurrentRoom.loadGameDefense = false;
                                CurrentRoom.loadGameAttack = false;
                                Debug.Log("Vous avez gagné");
                                otherPlayerDie = true;
                                endGameText.text = "Vous avez gagné !";
                            }
                        }
                        catch (Exception e)
                        {
                            Debug.Log("Can't read callback : " + e.Message);
                        }
                    }
                    if (args.Data.Contains("GRID"))
                    {
                        mapReceived = args.Data;
                        Debug.Log(args.Data);
                    }
                };
                TowersWebSocket.wsGame.OnClose += (sender, args) =>
                {
                    if (args.Code != 1000)
                    {
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

            if (byPassDefense)
            {
                canStart = "{\"CanStartHandler\":[{\"message\":\"true\"}]}";
            }

            StartCoroutine(WaitingDeathOtherPlayer());
            StartCoroutine(CheckEndInit());
        }
        // TODO : Control player's movement here and not in PlayerMovement
    }
}