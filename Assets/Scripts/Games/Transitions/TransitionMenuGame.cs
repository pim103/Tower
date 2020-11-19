using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using FullSerializer;
using Games.Defenses;
using Networking;
using Networking.Client;
using Networking.Client.Room;
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

        public static int waitingForStart;
        public static int timerAttack = Int32.MaxValue;

        private string waitingGameStartText;

        [SerializeField] private GameObject chooseRoleAndDeckGameObject;
        
        
        private void Start()
        {
            waitingForStart = durationChooseDeckPhase;
            waitingGameStartText = "Waiting for another player";
        }

        public bool InitGame()
        {
            CurrentRoom.loadGameDefense = false;
            CurrentRoom.loadGameAttack = false;
            CurrentRoom.generateAttackGrid = false;

            GameControllerNetwork.InitGameControllerNetwork();
            StartCoroutine(LoadGame());
            TowersWebSocket.TowerSender("SELF", NetworkingController.CurrentRoomToken,"null", "setGameLoaded", "null");

            return true;
        }

        public IEnumerator LoadGame()
        {
            while (!CurrentRoom.loadGame)
            {
                yield return new WaitForSeconds(0.1f);
            }
            SceneManager.LoadScene("GameScene");
        }

        public async Task SelectCharacter()
        {
            waitingGameStartText = "La partie commence dans     secondes";
            while (waitingForStart > 0 && !ChooseDeckAndClass.isValidate)
            {
                objectsInScene.waitingText.text = waitingGameStartText;
                objectsInScene.counterText.text = waitingForStart.ToString();

                await Task.Delay(500);
            }

            waitingForStart = durationChooseDeckPhase;
        }
    }
}