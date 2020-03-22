using System;
using System.Collections;
using Games.Transitions;
using Networking.Client;
using UnityEngine;
using UnityEngine.SceneManagement;

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

        public TowersWebSocket networking;

        public static int PlayerIndex;

        private bool idAssigned = false;

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
        
        private IEnumerator WaitingForCanStart()
        {
            while (canStart == null)
            {
                yield return new WaitForSeconds(1f);
            }
            transitionMenuGame.WantToStartGame();
        }

        // ================================== BASIC METHODS ======================================

        // Start is called before the first frame update
        void Start()
        {
            staticRoomId = roomId;
            
            objectsInScene.mainCamera.SetActive(true);
            PlayerIndex = 0;
            networking = new TowersWebSocket(endPoint, staticRoomId);
            networking.InitializeWebsocketEndpoint();
            networking.StartConnection();
            
            TowersWebSocket.ws.OnMessage += (sender, args) =>
            {
                if (args.Data == "{\"CanStartHandler\":[{\"message\":\"true\"}]}")
                {
                    Debug.Log("Done!");
                    canStart = args.Data;
                }

                if (args.Data.Contains("GRID"))
                {
                    mapReceived = args.Data;
                }

                if (args.Data.Contains("DEATH"))
                {
                    Debug.Log("Vous avez gagné");
                    SceneManager.LoadScene("MenuScene");
                }
            };
            StartCoroutine(WaitingForCanStart());
        }
        // TODO : Control player's movement here and not in PlayerMovement
    }
}