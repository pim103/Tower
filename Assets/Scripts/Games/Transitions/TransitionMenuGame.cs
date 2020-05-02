﻿using System;
using System.Collections;
using Games.Defenses;
using Networking.Client;
using UnityEngine;
using UnityEngine.SceneManagement;

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
            objectsInScene.waitingCanvasGameObject.SetActive(true);
            // TODO : Need rpc to synchro chrono
            objectsInScene.waitingText.text = waitingGameStartText;

            chooseRoleAndDeckGameObject.SetActive(true);

            while (waitingForStart > 0 && !ChooseDeckAndClasse.isValidate)
            {
                objectsInScene.counterText.text = waitingForStart.ToString();

                yield return new WaitForSeconds(1);
                waitingForStart -= 1;
            }

            waitingForStart = durationChooseDeckPhase;

            objectsInScene.waitingCanvasGameObject.SetActive(false);

            chooseRoleAndDeckGameObject.SetActive(false);
            // TODO : Need RPC to launch game
            StartGameWithDefense();
        }

        public void WantToStartGame()
        {
            waitingGameStartText = "Waiting for game start";
            waitingForStart = durationChooseDeckPhase;
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