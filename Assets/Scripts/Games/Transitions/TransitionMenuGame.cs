using System;
using System.Collections;
using FullSerializer;
using Games.Defenses;
using Networking;
using Networking.Client;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils;

namespace Games.Transitions
{
    public class TransitionMenuGame : MonoBehaviour
    {
        private const int durationChooseDeckPhase = 100;

        [SerializeField]
        private ObjectsInScene objectsInScene;

        [SerializeField] 
        private InitDefense initDefense;

        private int waitingForStart;

        private string waitingGameStartText;

        [SerializeField] private GameObject chooseRoleAndDeckGameObject;

        private bool loadGame = false;
        private bool loadRoleAndDeck = false;
        private bool loadGameDefense = false;
        
        private void Start()
        {
            waitingGameStartText = "Waiting for another player";
        }


        public bool InitGame()
        {
            TowersWebSocket.TowerSender("SELF", NetworkingController.CurrentRoomToken,"null", "setGameLoaded", "null");
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
                            if (callbackMessage.callbackMessages.message == "LoadGame")
                            {
                                loadGame = true;
                            }
                            if (callbackMessage.callbackMessages.message == "setGameLoaded")
                            {
                                Debug.Log("En attente de l'adversaire");
                            }
                            if (callbackMessage.callbackMessages.timer != -1)
                            {
                                loadRoleAndDeck = true;
                                waitingForStart = callbackMessage.callbackMessages.timer;
                                Debug.Log(callbackMessage.callbackMessages.timer);
                                objectsInScene.waitingText.text = waitingGameStartText;
                                objectsInScene.counterText.text = waitingForStart.ToString();
                                if (waitingForStart >= 30)
                                {
                                    waitingForStart = 100;
                                    loadRoleAndDeck = false;
                                    loadGameDefense = true;
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            Debug.Log("Can't read callback : " + e.Message);
                        }
                    }
                };
            }
            return true;
        }

        private void ActiveChooseRoleAndDeack()
        {
            chooseRoleAndDeckGameObject.SetActive(true);
        }

        private void Update()
        {
            if (loadGame)
            {
                SceneManager.LoadScene("GameScene");
                if (loadRoleAndDeck)
                {
                    ActiveChooseRoleAndDeack();
                }
                if (loadGameDefense)
                {
                    StartGameWithDefense();
                }
            }
        }

        public IEnumerator WaitingForStart()
        {
            // TODO : Need rpc to synchro chrono
            objectsInScene.waitingText.text = waitingGameStartText;

            chooseRoleAndDeckGameObject.SetActive(true);

            while (waitingForStart > 0 && !ChooseDeckAndClass.isValidate)
            {
                objectsInScene.counterText.text = waitingForStart.ToString();

                yield return new WaitForSeconds(1);
                waitingForStart -= 1;
            }

            waitingForStart = durationChooseDeckPhase;
            

            chooseRoleAndDeckGameObject.SetActive(false);
            // TODO : Need RPC to launch game
            StartGameWithDefense();
        }

        public void WantToStartGame()
        {
            waitingGameStartText = "La partie commence dans     secondes";
            //waitingForStart = durationChooseDeckPhase;
            //StartCoroutine(WaitingForStart());
        }

        public void StartGameWithDefense()
        {
            chooseRoleAndDeckGameObject.SetActive(false);
            objectsInScene.mainCamera.SetActive(false);

            objectsInScene.containerAttack.SetActive(false);
            objectsInScene.containerDefense.SetActive(true);
            initDefense.Init();
        }
    }
}