using Scripts.Games.Players;
using UnityEngine;

namespace Games.Players
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