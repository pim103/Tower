using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Games.Transitions
{
    public class TransitionMenuGame : MonoBehaviour
    {
        [SerializeField]
        private GameObject gameContainer;

        public bool InitGame()
        {
            PhotonNetwork.LoadLevel("GameScene");
            Debug.Log(PhotonNetwork.IsMessageQueueRunning);
            return true;
        }

        public bool StartGame()
        {
            Debug.Log("GameStart");
            gameContainer.SetActive(true);
            return true;
        }
    }
}