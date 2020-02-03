using Photon.Pun;
using Photon.Realtime;
using Scripts.Games.Attacks;
using Scripts.Games.Players;
using Scripts.Games.Transitions;
using System.Collections;
using System.Collections.Generic;
using Scripts;
using Scripts.Games;
using UnityEngine;

namespace Games {
    public class GameController : MonoBehaviourPunCallbacks
    {
        [SerializeField]
        private ObjectsInScene objectsInScene;

        [SerializeField]
        private TransitionMenuGame transitionMenuGame;

        [SerializeField]
        private PlayerMovementMaster playerMovementMaster;

        [SerializeField]
        private ScriptsExposer se;

        public string[] idToUserId;

        public int PlayerIndex;

        private bool idAssigned = false;

        /*
         * Flag to skip defensePhase
         */
        private bool byPassDefense = true;

        private IEnumerator CheckEndInit()
        {
            while (!PhotonNetwork.IsMessageQueueRunning)
            {
                yield return new WaitForSeconds(0.1f);
            }

            transitionMenuGame.WantToStartGame();
        }

        private void AssignId()
        {
            var count = 0;

            foreach (var player in PhotonNetwork.PlayerList)
            {
                photonView.RPC("ReturnId", RpcTarget.All, player.UserId, count);
                count++;
            }
        }

        [PunRPC]
        private void ReturnId(string userId, int id)
        {
            idToUserId[id] = userId;

            if (userId == PhotonNetwork.AuthValues.UserId)
            {
                PlayerIndex = id;
            }

            idAssigned = true;
        }

        // =================================== BYPASS DEFENSE METHOD ================================

        private IEnumerator WaitForConnection()
        {
            while (!se.photonController.isInGame && PhotonNetwork.PlayerList.Length == 0)
            {
                yield return new WaitForSeconds(0.5f);
            }

            AssignId();

            while (!idAssigned)
            {
                yield return new WaitForSeconds(0.5f);
            }

            se.initAttackPhase.StartAttackPhase();
        }

        private void ForceStartAttackPhase()
        {
            se.photonController.setupPhotonController(TypeLobby.BYPASS_DEFENSE);
            se.photonController.ConnectToPhoton();

            StartCoroutine(WaitForConnection());
        }

        // ================================== BASIC METHODS ======================================

        // Start is called before the first frame update
        void Start()
        {
            objectsInScene.mainCamera.SetActive(true);
            idToUserId = new string[objectsInScene.playerExposer.Length];

            if(byPassDefense)
            {
                ForceStartAttackPhase();
                return;
            }

            if (PhotonNetwork.IsConnected)
            {
                StartCoroutine(CheckEndInit());

                if (PhotonNetwork.IsMasterClient)
                {
                    AssignId();
                }
            }
            else
            {
                transitionMenuGame.WantToStartGame();
            }
        }

        private void Update()
        {
            if (objectsInScene.playerExposer[PlayerIndex].playerMovement.canMove)
            {
                objectsInScene.playerExposer[PlayerIndex].playerMovement.GetIntentPlayer();
            }
        }

        private void FixedUpdate()
        {
            if(PhotonNetwork.IsMasterClient)
            {
                playerMovementMaster.ApplyMovement();
            }
            else
            {
                playerMovementMaster.Movement(PlayerIndex);
            }
        }
    }
}