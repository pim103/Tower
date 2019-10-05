﻿using Photon.Pun;
using Scripts.Games;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Players
{
    public class PlayerMovement : MonoBehaviourPunCallbacks
    {
        private const int playerSpeed = 10;

        [SerializeField]
        private ObjectsInScene objectsInScene;

        public bool canMove;

        private bool wantToGoForward;
        private bool wantToGoBack;
        private bool wantToGoLeft;
        private bool wantToGoRight;

        private void Start()
        {
            canMove = false;

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
            if (Input.GetKeyUp(KeyCode.A))
            {
                wantToGoLeft = false;
            }
            if (Input.GetKeyUp(KeyCode.D))
            {
                wantToGoRight = false;
            }

            if(PhotonNetwork.IsConnected)
            {
                photonView.RPC("checkMovementRPC", RpcTarget.MasterClient, wantToGoForward, wantToGoBack, wantToGoLeft, wantToGoRight);
            }
        }

        [PunRPC]
        public void checkMovementRPC(bool wantToGoForward, bool wantToGoBack, bool wantToGoLeft, bool wantToGoRight)
        {

        }

        public void Movement()
        {
            Vector3 movement = objectsInScene.playerRigidbody.velocity;
            movement.x = Input.GetAxis("Horizontal") * 10;
            movement.z = Input.GetAxis("Vertical") * 10;

            objectsInScene.playerRigidbody.velocity = movement;
        }
    }
}