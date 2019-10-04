using Photon.Pun;
using Scripts.Games.Transitions;
using Scripts.Players;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Games {
    public class GameController : MonoBehaviour
    {
        [SerializeField]
        private ObjectsInScene objectsInScene;

        [SerializeField]
        private TransitionMenuGame transitionMenuGame;

        [SerializeField]
        private PlayerMovement playerMovement;

        private IEnumerator CheckEndInit()
        {
            while (!PhotonNetwork.IsMessageQueueRunning)
            {
                yield return new WaitForSeconds(0.1f);
            }

            transitionMenuGame.WantToStartGame();
        }

        // Start is called before the first frame update
        void Start()
        {
            objectsInScene.mainCamera.SetActive(true);

            if (PhotonNetwork.IsConnected)
            {
                StartCoroutine(CheckEndInit());
            } else
            {
                transitionMenuGame.WantToStartGame();
            }
        }

        private void Update()
        {
            /*
            if (playerMovement.canMove)
            {
                playerMovement.GetIntentPlayer();
            }
            */
        }

        private void FixedUpdate()
        {
            /*
            if (playerMovement.canMove)
            {
                playerMovement.Movement();
            }
            */
        }
    }
}