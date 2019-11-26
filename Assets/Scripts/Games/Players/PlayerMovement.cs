using Photon.Pun;
using Scripts.Games;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Games.Players
{
    public class PlayerMovement : PlayerIntent
    {
        [SerializeField]
        private PhotonView photonView;

        [SerializeField]
        private Player player;

        public int playerIndex;
        public bool canMove;


        private void Start()
        {
            wantToGoBack = false;
            wantToGoForward = false;
            wantToGoLeft = false;
            wantToGoRight = false;
        }

        public void GetIntentPlayer()
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                wantToGoForward = true;
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                wantToGoBack = true;
            }
            if (Input.GetKeyDown(KeyCode.Q))
            {
                wantToGoLeft = true;
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                wantToGoRight = true;
            }

            if (Input.GetKeyUp(KeyCode.Z))
            {
                wantToGoForward = false;
            }
            if (Input.GetKeyUp(KeyCode.S))
            {
                wantToGoBack = false;
            }
            if (Input.GetKeyUp(KeyCode.Q))
            {
                wantToGoLeft = false;
            }
            if (Input.GetKeyUp(KeyCode.D))
            {
                wantToGoRight = false;
            }

            if(Input.GetMouseButtonDown(0))
            {
                player.weapons[0].PlayMovement(player.movementPattern, 
                    player.
                    objectsInScene.
                    playerExposer[playerIndex].
                    playerHand);
            }

            if(PhotonNetwork.IsConnected)
            {
                photonView.RPC("CheckMovementRPC", RpcTarget.MasterClient, wantToGoForward, wantToGoBack, wantToGoLeft, wantToGoRight);
            }
        }

        [PunRPC]
        public void CheckMovementRPC(bool wantToGoForward, bool wantToGoBack, bool wantToGoLeft, bool wantToGoRight)
        {
            this.wantToGoBack = wantToGoBack;
            this.wantToGoForward = wantToGoForward;
            this.wantToGoLeft = wantToGoLeft;
            this.wantToGoRight = wantToGoRight;
        }
    }
}