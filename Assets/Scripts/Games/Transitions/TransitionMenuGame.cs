using System;
using System.Collections;
using Games.Defenses;
using Networking.Client;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Games.Transitions
{
    public class TransitionMenuGame : MonoBehaviour
    {
        private const int durationWaitingPhase = 3;

        [SerializeField]
        private ObjectsInScene objectsInScene;

        [SerializeField] 
        private InitDefense initDefense;

        private int waitingForStart;

        private string waitingGameStartText;

        private void Start()
        {
            waitingGameStartText = "Waiting for another player";
        }
        

        public bool InitGame()
        {
            SceneManager.LoadScene("GameScene");
            return true;
        }

        public IEnumerator WaitingForStart()
        {
            Debug.Log("Startplz2");
            objectsInScene.waitingCanvasGameObject.SetActive(true);
            // TODO : Need rpc to synchro chrono
            objectsInScene.waitingText.text = waitingGameStartText;

            while (waitingForStart > 0)
            {
                objectsInScene.counterText.text = waitingForStart.ToString();

                yield return new WaitForSeconds(1);
                waitingForStart -= 1;
            }

            waitingForStart = durationWaitingPhase;

            objectsInScene.waitingCanvasGameObject.SetActive(false);
            // TODO : Need RPC to launch game
            StartGameWithDefense();
        }

        public void WantToStartGame()
        {
            Debug.Log("Startplz");
            waitingGameStartText = "Waiting for game start";
            waitingForStart = durationWaitingPhase;
            StartCoroutine(WaitingForStart());
        }

        public void StartGameWithDefense()
        {
            objectsInScene.mainCamera.SetActive(false);

            objectsInScene.containerAttack.SetActive(false);
            objectsInScene.containerDefense.SetActive(true);
            initDefense.Init();
        }
    }
}