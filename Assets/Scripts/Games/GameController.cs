using Photon.Pun;
using Scripts.Games.Transitions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Games {
    public class GameController : MonoBehaviour
    {
        [SerializeField]
        private TransitionMenuGame transitionMenuGame;

        // Start is called before the first frame update
        void Start()
        {
            StartCoroutine(CheckEndInit());
        }

        private IEnumerator CheckEndInit()
        {
            Debug.Log(PhotonNetwork.IsMessageQueueRunning);
            while(!PhotonNetwork.IsMessageQueueRunning)
            {
                yield return new WaitForSeconds(0.1f);
            }

            transitionMenuGame.StartGame();
        }
    }
}