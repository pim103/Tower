using System.Threading.Tasks;
using Games.Attacks;
using Games.Defenses;
using Games.Global;
using Games.Transitions;
using Networking.Client.Room;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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

                gameGridController.InitGridData(currentGameGrid);
                transitionMenuGame.StartGameWithDefense();
                await transitionDefenseAttack.PlayDefensePhase();
                
                gameGridController.DesactiveMap();
                
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
                gameGridController.DesactiveMap();
            }
        }

        private void SetEndGameText(string text)
        {
            otherPlayerDie = true;
            endGameText.text = text;
        }

        public static void EndGame(bool hasWon)
        {
            CurrentRoom.loadGameDefense = false;
            CurrentRoom.loadGameAttack = false;
            CurrentRoom.generateAttackGrid = false;

            Cursor.lockState = CursorLockMode.None;
            DataObject.playerInScene.Remove(PlayerIndex);
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