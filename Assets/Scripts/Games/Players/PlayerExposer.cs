using Scripts.Games.Players;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Games.Players
{
    public class PlayerExposer : MonoBehaviour
    {
        [SerializeField]
        public GameObject playerGameObject;

        [SerializeField]
        public PlayerMovement playerMovement;

        [SerializeField]
        public Rigidbody playerRigidbody;

        [SerializeField]
        public GameObject playerCamera;

        [SerializeField]
        public GameObject playerHand;

        [SerializeField]
        public Player player;
    }
}