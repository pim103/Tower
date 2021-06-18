using System.Threading.Tasks;
using DefaultNamespace;
using Games.Attacks;
using Games.Defenses;
using Games.Global;
using Games.Transitions;
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
            instance = this;
            
            mainCamera = objectsInScene.mainCamera;
            endGameMenu.SetActive(false);
            backToMenu.onClick.AddListener(LoadMainMenu);
            staticRoomId = roomId;

            objectsInScene.mainCamera.SetActive(true);
            
            // TODO : change index
            PlayerIndex = 0;

            if (phase == Phase.Attack)
            {
                gameGridController.GenerateAndInitFakeGrid();
                AttackPhase();
            } 
            else if (phase == Phase.Defense)
            {
                gameGridController.GenerateAndInitFakeGrid();
                StartCoroutine(GameControllerTest.CreateDefenseInstance(initDefensePhase));
                ContainerController.ActiveContainerOfCurrentPhase(Phase.Defense);
            }
            else
            {
                StartWithSelectCharacter();
            }
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

                ContainerController.ActiveContainerOfCurrentPhase(Phase.Defense);
                gameGridController.InitGridData(currentGameGrid);
                initDefensePhase.Init();
                await transitionDefenseAttack.PlayDefensePhase();

                gameGridController.DesactiveMap();

                // Send defense grid
                GameControllerNetwork.SendGridData(initDefensePhase.defenseGrid);

                while (!CurrentRoom.generateAttackGrid)
                {
                    await Task.Delay(500); 
                }

                await initAttackPhase.Init();
                gameGridController.InitGridData(currentGameGrid);
                GameControllerNetwork.SendSetAttackReady();

                while (!CurrentRoom.loadGameAttack)
                {
                    await Task.Delay(500);
                }

                await AttackPhase();
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