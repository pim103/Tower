using System;
using System.Collections;
using FullSerializer;
using Games.Transitions;
using Networking;
using Networking.Client;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils;

namespace Games {
    public class GameController : MonoBehaviour
    {
        [SerializeField]
        private ObjectsInScene objectsInScene;

        [SerializeField]
        private TransitionMenuGame transitionMenuGame;

        [SerializeField]
        private ScriptsExposer se;
        [SerializeField] 
        private string endPoint;
        [SerializeField] 
        private string roomId;

        public static string staticRoomId;
        
        private string canStart = null;

        public static int PlayerIndex;

        private bool idAssigned = false;

        private bool otherPlayerDie = false;
        private CallbackMessages callbackHandlers;

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
            SceneManager.LoadScene("MenuScene");
        }

        // ================================== BASIC METHODS ======================================

        // Start is called before the first frame update
        void Start()
        {
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
                            Debug.Log(data);
                            serializer.TryDeserialize(data, ref callbackMessage);
                            callbackMessage = Tools.Clone(callbackMessage);
                            if (callbackMessage.callbackMessages.message == "WON")
                            {
                                Debug.Log("Un autre joueur a gagné");
                                otherPlayerDie = true;
                            }
                            if (callbackMessage.callbackMessages.message == "DEATH")
                            {
                                Debug.Log("Vous avez gagné");
                                otherPlayerDie = true;
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