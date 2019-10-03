﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Players
{

    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField]
        private Rigidbody players;

        private bool wantToGoForward;
        private bool wantToGoBack;
        private bool wantToGoLeft;
        private bool wantToGoRight;

        private void Start()
        {
            wantToGoBack = false;
            wantToGoForward = false;
            wantToGoLeft = false;
            wantToGoRight = false;
        }

        private void Update()
        {
            GetIntentPlayer();
        }

        private void FixedUpdate()
        {
            Movement();
        }

        private void GetIntentPlayer()
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
        }

        private void Movement()
        {
            Vector3 movement = players.velocity;
            movement.x = Input.GetAxis("Horizontal") * 10;
            movement.z = Input.GetAxis("Vertical") * 10;
            
            players.velocity = movement;
        }
    }
}