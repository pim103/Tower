using Photon.Pun;
using Photon.Realtime;
using Scripts.Games.Transitions;
using Scripts.Players;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Games {
    public class GameController : MonoBehaviourPunCallbacks
    {
        [SerializeField]
        private ObjectsInScene objectsInScene;

        [SerializeField]
        private TransitionMenuGame transitionMenuGame;

        [SerializeField]
        private PlayerMovementMaster playerMovementMaster;

        public string[] idToUserId;

        public int PlayerIndex;

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

            if(userId == PhotonNetwork.AuthValues.UserId)
            {
                PlayerIndex = id;
            }

            Debug.Log("Hote : " + PhotonNetwork.AuthValues.UserId + " userId " + userId + " id " + id);
        }

        // Start is called before the first frame update
        void Start()
        {
            objectsInScene.mainCamera.SetActive(true);
            idToUserId = new string[objectsInScene.playersMovement.Length];

            if (PhotonNetwork.IsConnected)
            {
                StartCoroutine(CheckEndInit());

                if(PhotonNetwork.IsMasterClient)
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
            if (objectsInScene.playersMovement[PlayerIndex].canMove)
            {
                objectsInScene.playersMovement[PlayerIndex].GetIntentPlayer();
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