﻿using System;
using System.Collections;
using System.Threading.Tasks;
using FullSerializer;
using Games.Attacks;
using Games.Defenses;
using Games.Global;
using Games.Transitions;
using Networking;
using Networking.Client;
using Networking.Client.Room;
using UnityEditorInternal;
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

        [SerializeField]
        private TransitionDefenseAttack transitionDefenseAttack;

        [SerializeField]
        private InitAttackPhase initAttackPhase;

        [SerializeField]
        private GameGridController gameGridController;

        [SerializeField]
        private EndCube endCube;

        [SerializeField] private GameObject endGameMenu;
        [SerializeField] private Text endGameText;

        [SerializeField]
        private ScriptsExposer se;
        [SerializeField] 
        private string endPoint;
        [SerializeField] 
        private string roomId;

        [SerializeField] private Button backToMenu;

        public static GameObject mainCamera;

        public static string staticRoomId;
        private string canStart = null;
        public static int PlayerIndex;
        private bool idAssigned = false;

        public static bool otherPlayerDie = false;

        public static GameGrid currentGameGrid;

        /*
         * Flag to skip defensePhase
         */
        public bool byPassDefense = true;

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
            
            GameControllerNetwork.InitGameControllerNetwork(this);

            if (byPassDefense)
            {
                canStart = "{\"CanStartHandler\":[{\"message\":\"true\"}]}";
            }

            StartWithSelectCharacter();
        }

        private async Task StartWithSelectCharacter()
        {
            while (!CurrentRoom.loadRoleAndDeck)
            {
                await Task.Delay(500);
            }

            await transitionMenuGame.SelectCharacter();
            GameControllerNetwork.SendRoleAndClasses();
            await PlayGame();
        }

        private async Task PlayGame()
        {
            while (!otherPlayerDie)
            {
                while (!CurrentRoom.loadGameDefense)
                {
                    await Task.Delay(500);
                }

                transitionMenuGame.StartGameWithDefense();
                await transitionDefenseAttack.PlayDefensePhase();
                GameControllerNetwork.SendGridData();

                while (!CurrentRoom.generateAttackGrid)
                {
                    await Task.Delay(500); 
                }

                await initAttackPhase.StartAttackPhase();
                gameGridController.InitGridData(currentGameGrid);
                GameControllerNetwork.SendSetAttackReady();

                while (!CurrentRoom.loadGameAttack)
                {
                    await Task.Delay(500);
                }

                await initAttackPhase.ActivePlayer();
                endCube.DesactiveAllGameObject();
            }
        }

        private void SetEndGameText(string text)
        {
            otherPlayerDie = true;
            endGameText.text = text;
        }

        public void EndGame(bool hasWon)
        {
            CurrentRoom.loadGameDefense = false;
            CurrentRoom.loadGameAttack = false;
            CurrentRoom.generateAttackGrid = false;

            Cursor.lockState = CursorLockMode.None;
            DataObject.playerInScene.Remove(PlayerIndex);
            objectsInScene.mainCamera.SetActive(true);
            endGameMenu.SetActive(true);

            if (hasWon)
            {
                Debug.Log("Vous avez gagné");
                SetEndGameText("Vous avez gagné !");
            }
            else
            {
                Debug.Log("Un autre joueur a gagné");
                SetEndGameText("Vous avez perdu...");
            }
        }
    }
}