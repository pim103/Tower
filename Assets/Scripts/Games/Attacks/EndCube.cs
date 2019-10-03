using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Games.Attacks
{
    public class EndCube : MonoBehaviourPunCallbacks
    {
        private void OnTriggerEnter(Collider other)
        {
            photonView.RPC("CheckEndOfGame", RpcTarget.All);
        }

        [PunRPC]
        private void CheckEndOfGame()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                photonView.RPC("EndGameForAll", RpcTarget.All);
            }
        }

        [PunRPC]
        private void EndGameForAll()
        {
            Debug.Log("End of Game");
            if(PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.LoadLevel("MenuScene");
            }
        }
    }
}