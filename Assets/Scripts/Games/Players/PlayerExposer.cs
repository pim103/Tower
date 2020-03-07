using UnityEngine;
using UnityEngine.Serialization;

namespace Games.Players
{
    public class PlayerExposer : MonoBehaviour
    {
        [SerializeField]
        public GameObject playerGameObject;

        [SerializeField]
        public PlayerPrefab playerPrefab;

        [SerializeField]
        public Rigidbody playerRigidbody;

        [SerializeField]
        public GameObject playerCamera;

        [SerializeField]
        public GameObject playerHand;
        
        [SerializeField]
        public Transform playerTransform;
    }
}