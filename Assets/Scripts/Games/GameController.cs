using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using DefaultNamespace;
using Games.Attacks;
using Games.Defenses;
using Games.Global;
using Games.Transitions;
using Networking;
using Networking.Client;
using Networking.Client.Room;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Games {
    public enum Phase
    {
        RoleAndDeck,
        Defense,
        Attack
    }
    
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
        private InitDefense initDefensePhase;

        [SerializeField]
        private GameGridController gameGridController;

        [SerializeField]
        private EndCube endCube;

        [SerializeField] private GameObject endGameMenu;
        [SerializeField] private Text endGameText;

        [SerializeField] private GameObject waitingOpponentBeforeDefense;
        [SerializeField] private Text waitingOpponentEndTimer;

        [SerializeField] 
        private string roomId;

        [SerializeField] private Button backToMenu;
        [SerializeField] private Phase phase;

        public static GameObject mainCamera;

        public bool isWon = false;

        public static string staticRoomId;
        public static int PlayerIndex;
        private bool idAssigned = false;

        public static bool otherPlayerDie = false;

        public static GameGrid currentGameGrid;

        public static GameController instance;

        [SerializeField] private MapStats[] mapStatsList;

        public int level = 0;

        [SerializeField] private GameObject backGround;

        [SerializeField] private AudioClip defenseMusic;
        [SerializeField] private AudioClip attackMusic;
        [SerializeField] private AudioSource musicSource;
        
        /*
         * Flag to skip defensePhase
         */
        public bool byPassDefense = true;

        public static void LoadMainMenu()
        {
            SceneManager.LoadScene("MenuScene");
        }

        // ================================== BASIC METHODS ======================================

        // Start is called before the first frame update
        void Start()
        {
            instance = this;
            
            mainCamera = objectsInScene.mainCamera;
            endGameMenu.SetActive(false);
            backToMenu.onClick.AddListener(() => {
                var setSocket = new Dictionary<string, string>();
                setSocket.Add("tokenPlayer", NetworkingController.AuthToken);
                setSocket.Add("room", NetworkingController.CurrentRoomToken);
                TowersWebSocket.TowerSender("SELF", NetworkingController.CurrentRoomToken,"null", "quitMatchmaking", TowersWebSocket.FromDictToString(setSocket));
                LoadMainMenu();
            });
            staticRoomId = roomId;

            objectsInScene.mainCamera.SetActive(true);

            // TODO : change index
            PlayerIndex = 0;

            if (phase == Phase.Attack)
            {
                StartCoroutine(BypassToAttackPhase());
            }
            else if (phase == Phase.Defense)
            {
                mapStatsList[level].gameObject.SetActive(true);
                backGround.SetActive(true);
                musicSource.clip = defenseMusic;
                musicSource.Play();
                Debug.Log(mapStatsList[level].gameObject.name);
                Debug.Log(mapStatsList[level].mapSize);
                objectsInScene.startPos = mapStatsList[level].startPos;
                objectsInScene.endZone = mapStatsList[level].endZone;
                objectsInScene.endDoor = mapStatsList[level].endDoor;
                objectsInScene.endFx = mapStatsList[level].endFx;
                mapStatsList[level].roof.SetActive(false);
                gameGridController.GenerateAndInitFakeGrid(mapStatsList[level]);
                instance.waitingOpponentBeforeDefense.SetActive(false);
                ContainerController.ActiveContainerOfCurrentPhase(Phase.Defense);
                //gameGridController.InitGridData(currentGameGrid);
                initDefensePhase.Init(mapStatsList[level].mapSize, mapStatsList[level].floors);

                Debug.Log("PlayDef");
                try
                {
                    transitionDefenseAttack.PlayDefensePhase();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
            else
            {
                StartWithSelectCharacter();
            }
        }

        private IEnumerator BypassToAttackPhase()
        {
            yield return new WaitWhile(() => !DictionaryManager.hasCardsLoad);

            //gameGridController.GenerateAndInitFakeGrid();
            mapStatsList[level].gameObject.SetActive(true);
            AttackPhase();
        }
        
        private async Task StartWithSelectCharacter()
        {
            ContainerController.ActiveContainerOfCurrentPhase(Phase.RoleAndDeck);
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
                mapStatsList[level].gameObject.SetActive(true);
                backGround.SetActive(true);
                musicSource.clip = defenseMusic;
                musicSource.Play();
                Debug.Log(mapStatsList[level].gameObject.name);
                Debug.Log(mapStatsList[level].mapSize);
                objectsInScene.startPos = mapStatsList[level].startPos;
                objectsInScene.endZone = mapStatsList[level].endZone;
                objectsInScene.endDoor = mapStatsList[level].endDoor;
                objectsInScene.endFx = mapStatsList[level].endFx;
                mapStatsList[level].roof.SetActive(false);
                gameGridController.GenerateAndInitFakeGrid(mapStatsList[level]);
                instance.waitingOpponentBeforeDefense.SetActive(false);
                ContainerController.ActiveContainerOfCurrentPhase(Phase.Defense);
                //gameGridController.InitGridData(currentGameGrid);
                initDefensePhase.Init(mapStatsList[level].mapSize, mapStatsList[level].floors);

                Debug.Log("PlayDef");
                try
                {
                    await transitionDefenseAttack.PlayDefensePhase();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }

                Debug.Log("desactmap");
                gameGridController.DesactiveMap();
                backGround.SetActive(false);
                mapStatsList[level].roof.SetActive(true);

                // Send defense grid
                Debug.Log("SendGrid");
                GameControllerNetwork.SendGridData(initDefensePhase.defenseGrid);

                while (!CurrentRoom.generateAttackGrid)
                {
                    await Task.Delay(500); 
                }
                
                Debug.Log("BeforeInit");
                await initAttackPhase.Init();
                Debug.Log("BeforeInitGridData");
                gameGridController.InitGridData(currentGameGrid, mapStatsList[level].floors);
                Debug.Log("BeforeSendSet");
                GameControllerNetwork.SendSetAttackReady();

                while (!CurrentRoom.loadGameAttack)
                {
                    await Task.Delay(500);
                }
                musicSource.clip = attackMusic;
                musicSource.Play();
                Debug.Log("BeforeAwait");
                await AttackPhase();
                mapStatsList[level].gameObject.SetActive(false);
                if (level < mapStatsList.Length - 1)
                {
                    level += 1;
                }
            }

            EndGame(isWon);
        }

        public static void SetEndOfGame(bool isWon)
        {
            otherPlayerDie = true;
            instance.isWon = isWon;
            CurrentRoom.loadGameDefense = false;
            CurrentRoom.loadGameAttack = false;
            CurrentRoom.generateAttackGrid = false;
            CurrentRoom.loadGame = false;
            CurrentRoom.loadRoleAndDeck = false;
        }

        public static void WaitingOpponent()
        {
            int nbMin = TransitionMenuGame.timerAttack / 60;
            int nbSec = TransitionMenuGame.timerAttack % 60;

            instance.waitingOpponentBeforeDefense.SetActive(true);
            instance.waitingOpponentEndTimer.text = nbMin + (nbMin > 0 ? "min" : "") + nbSec + "sec";
        }

        private async Task AttackPhase()
        {
            ContainerController.ActiveContainerOfCurrentPhase(Phase.Attack);
            await initAttackPhase.PlayAttackPhase();

            endCube.DesactiveAllGameObject();
            gameGridController.DesactiveMap();
        }

        private void SetEndGameText(string text)
        {
            endGameText.text = text;
        }

        private void EndGame(bool hasWon)
        {
            Cursor.lockState = CursorLockMode.None;
            instance.objectsInScene.mainCamera.SetActive(true);
            instance.endGameMenu.SetActive(true);

            GameController.otherPlayerDie = false;

            if (hasWon)
            {
                Debug.Log("Vous avez gagné");
                instance.SetEndGameText("Vous avez gagné !");
            }
            else
            {
                Debug.Log("Un autre joueur a gagné");
                instance.SetEndGameText("Vous avez perdu...");
            }
        }
    }
}