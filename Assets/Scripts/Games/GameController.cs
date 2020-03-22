using System.Collections;
using Games.Transitions;
using Networking.Client;
using UnityEngine;

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
        private string staticRoomId;

        public TowersWebSocket networking;

        public static int PlayerIndex;

        private bool idAssigned = false;

        /*
         * Flag to skip defensePhase
         */
        public bool byPassDefense = true;

        private IEnumerator CheckEndInit()
        {
            yield return new WaitForSeconds(0.1f);
            transitionMenuGame.WantToStartGame();
        }

        // ================================== BASIC METHODS ======================================

        // Start is called before the first frame update
        void Start()
        {
            objectsInScene.mainCamera.SetActive(true);
            PlayerIndex = 0;
            networking = new TowersWebSocket(endPoint, staticRoomId);
            networking.InitializeWebsocketEndpoint();
            networking.StartConnection();

            transitionMenuGame.WantToStartGame();
        }

        // TODO : Control player's movement here and not in PlayerMovement
    }
}