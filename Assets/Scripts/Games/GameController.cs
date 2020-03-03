using Photon.Pun;
using Photon.Realtime;
using Scripts.Games.Attacks;
using Scripts.Games.Players;
using Scripts.Games.Transitions;
using System.Collections;
using System.Collections.Generic;
using Games.Players;
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
        private ScriptsExposer se;

        public static int PlayerIndex;

        private bool idAssigned = false;

        /*
         * Flag to skip defensePhase
         */
        public bool byPassDefense = true;

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

        private IEnumerator WaitForDataLoading()
        {
            while (se.dm.monsterList == null || se.dm.weaponList == null)
            {
                yield return new WaitForSeconds(0.5f);
            }

            se.initAttackPhase.StartAttackPhase();
        }

        private void ForceStartAttackPhase()
        {
            StartCoroutine(WaitForDataLoading());
        }

        // ================================== BASIC METHODS ======================================

        // Start is called before the first frame update
        void Start()
        {
            objectsInScene.mainCamera.SetActive(true);
            PlayerIndex = 0;

            if(byPassDefense)
            {
                ForceStartAttackPhase();
                return;
            }

            transitionMenuGame.WantToStartGame();
        }

        // TODO : Control player's movement here and not in PlayerMovement
    }
}