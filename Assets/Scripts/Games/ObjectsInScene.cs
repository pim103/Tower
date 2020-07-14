using Games.Players;
using UnityEngine;
using UnityEngine.UI;

namespace Games
{
    public class ObjectsInScene : MonoBehaviour
    {
        // ============================ MAIN CAMERA ========================

        [SerializeField]
        public GameObject mainCamera;

        // ============================ CANVAS AND COUNTER ACCESS =============

        [SerializeField]
        public Text waitingText;

        [SerializeField]
        public Text counterText;

        // ============================ PRINCIPAL ATTACK AND DEFENSE CONTAINER =============

        [SerializeField]
        public GameObject containerDefense;

        [SerializeField]
        public GameObject containerAttack;

        // =========================== PLAYER CHARACTER ========================

        [SerializeField]
        public PlayerPrefab[] playerPrefab;
    }
}