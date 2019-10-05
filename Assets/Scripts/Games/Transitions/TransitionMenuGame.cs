using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Games.Transitions
{
    public class TransitionMenuGame : MonoBehaviour
    {
        private const int durationWaitingPhase = 3;

        [SerializeField]
        private ObjectsInScene objectsInScene;

        private int waitingForStart;

        private string waitingGameStartText;

        private void Start()
        {
            waitingForStart = durationWaitingPhase;
            waitingGameStartText = "Waiting for game start";
        }

        public bool InitGame()
        {
            PhotonNetwork.LoadLevel("GameScene");
            return true;
        }

        private IEnumerator WaitingForStart()
        {
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
            StartCoroutine(WaitingForStart());
        }

        public void StartGameWithDefense()
        {
            objectsInScene.mainCamera.SetActive(false);

            objectsInScene.containerAttack.SetActive(false);
            objectsInScene.containerDefense.SetActive(true);
        }
    }
}